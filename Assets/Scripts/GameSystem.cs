using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public List<IngameCharacterMover> GetPlayerList() { return players; }

    public void AddPlayer(IngameCharacterMover player)
    {
        //player(자신) 미포함 시
        if (!players.Contains(player)) players.Add(player);
    }

    
    private IEnumerator GameReady()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;

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
}
