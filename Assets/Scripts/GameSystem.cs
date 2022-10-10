using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Mirror;

//플레이어들 객체를 찾아 GameSystem에 데이터 저장해야함
public class GameSystem : NetworkBehaviour
{
    public static GameSystem Instance;

    private List<IngameCharacterMover> players = new List<IngameCharacterMover>();

    [SerializeField]
    private Transform spawnTransform;

    [SerializeField]
    private float spawnDistance;

    [SyncVar]
    public float killCooldown;

    [SyncVar]
    public int killRange;

    //skip한 플레이어 카운트
    [SyncVar]
    public int skipVotePlayerCount;

    //남은 회의 시간
    [SyncVar]
    public float remainTime;

    [SerializeField]
    private Light2D shadowLight;

    [SerializeField]
    private Light2D lightMapLight;

    [SerializeField]
    private Light2D globalLight;

    //전체 플레이어 리스트 가져오는 함수
    public List<IngameCharacterMover> GetPlayerList() { return players; }

    public void AddPlayer(IngameCharacterMover player)
    {
        //player(자신) 미포함 시
        if (!players.Contains(player)) players.Add(player);
    }

    
    private IEnumerator GameReady()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;

        //값 초기화
        killCooldown = manager.gameRuleData.killCooldown;
        killRange = (int)(manager.gameRuleData.killRange);

        //players에 IngameCharacterMover객체들이 다 들어갔는지 기다림
        while (manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }

        //임포스터 뽑기
        for (int i = 0; i < manager.imposterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];

            if (player.playerType != EPlayerType.Imposter)
                player.playerType = EPlayerType.Imposter;
            else
                i--;
        }

        //캐릭터 배치
        for (int i = 0; i <players.Count; i++)
        {
            float radian = (2f * Mathf.PI) / players.Count;
            radian *= i;

            //transform.position으로 변경하면 동기화가 안됨
            players[i].RpcTeleport(spawnTransform.position + (new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * spawnDistance));
        }

        yield return new WaitForSeconds(2f);
        RpcStartGame();

        foreach (var player in players)
        {
            player.SetKillCooldown();
        }
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    //ShowIntroSequence함수는 호스트와 클라이언트에서 모두 실행되어야한다
    private IEnumerator StartGameCoroutine()
    {
        yield return StartCoroutine(IngameUIManager.Instance.IngameIntroUI.ShowIntroSequence());

        IngameCharacterMover myCharacter = null;
        foreach (var player in players)
        {
            if (player.hasAuthority)
            {
                myCharacter = player;
                break;
            }
        }

        foreach (var player in players)
        {
            player.SetNicknameColor(myCharacter.playerType);
        }

        yield return new WaitForSeconds(3f);
        IngameUIManager.Instance.IngameIntroUI.Close();
    }

    private void Awake()
    {
        Instance = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //호스트일때만 실행해야한다
        if (isServer) StartCoroutine(GameReady());
    }

    //캐릭터타입에 따른 게임 밝기 변경 함수
    public void ChangeLightMode(EPlayerType type)
    {
        if (type == EPlayerType.Ghost)
        {
            lightMapLight.lightType = Light2D.LightType.Global;
            shadowLight.intensity = 0f;
            globalLight.intensity = 1f;
        }
        else
        {
            lightMapLight.lightType = Light2D.LightType.Point;
            shadowLight.intensity = 0.5f;
            globalLight.intensity = 0.5f;
        }
    }

    //리포트 미팅 시작함수
    public void StartReportMeeting(EPlayerColor deadbodyColor)
    {
        RpcSendReportSign(deadbodyColor);
        StartCoroutine(MeetingProcess_Coroutine());
    }

    private IEnumerator StartMeeting_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        IngameUIManager.Instance.ReportUI.Close();
        IngameUIManager.Instance.MeetingUI.Open();
        
        //회의상태 Meeting으로 변경
        IngameUIManager.Instance.MeetingUI.ChangeMeetingState(EMeetingState.Meeting);
    }

    private IEnumerator MeetingProcess_Coroutine()
    {
        //회의 시간에 투표 못하게
        var players = FindObjectsOfType<IngameCharacterMover>();
        foreach (var player in players) player.isVote = true;

        yield return new WaitForSeconds(3f);

        var manager = NetworkManager.singleton as AmongUsRoomManager;
        //미팅시간
        remainTime = manager.gameRuleData.meetingsTime;
        while (true)
        {
            remainTime -= Time.deltaTime;
            yield return null;
            if (remainTime <= 0f) break;
        }

        //미팅 후 값 초기화
        skipVotePlayerCount = 0;
        foreach (var player in players)
        {
            if ((player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
            {
                player.isVote = false;
            }
            player.vote = 0;
        }

        //투표시간
        RpcStartVoteTime();
        remainTime = manager.gameRuleData.voteTime;
        while (true)
        {
            remainTime -= Time.deltaTime;
            yield return null;
            if (remainTime <= 0f) break;
        }

        //투표안한 플레이어 강제 스킵
        foreach (var player in players)
        {
            //투표하지않았고 살아있는 플레이어라면
            if (!player.isVote && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
            {
                player.isVote = true;
                skipVotePlayerCount += 1;
                RpcSignSkipVote(player.playerColor);
            }
        }

        RpcEndVoteTime();
    }

    //투표시작알림함수
    [ClientRpc]
    public void RpcStartVoteTime()
    {
        //회의상태 Vote으로 변경
        IngameUIManager.Instance.MeetingUI.ChangeMeetingState(EMeetingState.Vote);
    }

    //투표시작종료함수
    [ClientRpc]
    public void RpcEndVoteTime()
    {
        IngameUIManager.Instance.MeetingUI.CompleteVote();
    }

    [ClientRpc]
    private void RpcSendReportSign(EPlayerColor deadbodyColor)
    {
        IngameUIManager.Instance.ReportUI.Open(deadbodyColor);

        StartCoroutine(StartMeeting_Coroutine());
    }

    //
    [ClientRpc]
    public void RpcSignVoteEject(EPlayerColor voterColor, EPlayerColor ejectColor)
    {
        IngameUIManager.Instance.MeetingUI.UpdateVote(voterColor, ejectColor);
    }

    //투표스킵한 플레이어를 클라이언트MeetingUI에 알려주고 업데이트 기능
    [ClientRpc]
    public void RpcSignSkipVote(EPlayerColor skipVotePlayerColor)
    {
        IngameUIManager.Instance.MeetingUI.UpdateSkipVotePlayer(skipVotePlayerColor);
    }
}
