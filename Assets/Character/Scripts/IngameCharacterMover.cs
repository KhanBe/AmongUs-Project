using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//자기 타입
// 첫 자리 : 0 - 크루원 / 1 - 임포스터
// 두 자리 : 0 - 살아있음 / 1 - 죽음
// 00 - 살아있는 크루원 / 01 살아있는 임포스터
// 10 - 죽은 크루원 / 11 죽은 임포스터
public enum EPlayerType
{
    Crew = 0,
    Imposter = 1,
    Ghost= 2,
    Crew_Alive = 0,
    Imposter_Alive = 1,
    Crew_Ghost = 2,
    Imposter_Ghost = 3,
}

public class IngameCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetPlayerType_Hook))]
    public EPlayerType playerType;
    private void SetPlayerType_Hook(EPlayerType _, EPlayerType type)//playerType변경시 작동되는 훅함수
    {
        //자기 캐릭터이면서 임포스터일 경우
        if (hasAuthority && type == EPlayerType.Imposter)
        {
            IngameUIManager.Instance.KillButtonUI.Show(this);//this : 자신 캐릭터
            playerFinder.SetKillRange(GameSystem.Instance.killRange + 1f);
        }
    }

    [SerializeField]
    private PlayerFinder playerFinder;

    [SyncVar]
    private float killCooldown;
    public float KillCooldown { get { return killCooldown; } }

    public bool isKillable { get { return killCooldown < 0f && playerFinder.targets.Count != 0; } }//bool타입 킬가능여부 함수

    //킬 쿨타임 설정하는 함수
    public void SetKillCooldown()
    {
        //내가 서버(방장)이면
        if (isServer)
        {
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }

    [ClientRpc]//서버에서 클라이언트로 호출하는 어트리뷰트
    public void RpcTeleport(Vector3 position)
    {
        //서버의position값을 변경해 동기화
        transform.position = position;
    }

    //인게임 색상설정 함수
    public void SetNicknameColor(EPlayerType type)
    {
        //내가임포스터이면서 다른플레이어가 임포스터이면
        if (playerType == EPlayerType.Imposter && type == EPlayerType.Imposter)
        {
            nicknameText.color = Color.red;
        }
    }

    private void Awake()
    {
        
    }

    public override void Start()
    {
        // CharacterMover의 Start()함수 호출
        base.Start();

        if (hasAuthority)
        {
            IsMoveable = true;

            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
            myRoomPlayer.myCharacter = this;
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        //IngameCharacter객체들이 스스로 GameSystem클래스에 자신을 등록하도록 만든다.
        GameSystem.Instance.AddPlayer(this);
    }

    private void Update()
    {
        if (isServer && playerType == EPlayerType.Imposter)
        {
            killCooldown -= Time.deltaTime;
        }
    }

    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor color)
    {
        this.nickname = nickname;
        playerColor = color;
    }

    public void Kill()
    {
        //GetFirstTarget().netId : 첫 타겟의 netId
        CmdKill(playerFinder.GetFirstTarget().netId);
    }

    [Command]
    private void CmdKill(uint targetNetId)
    {
        IngameCharacterMover target = null;

        foreach (var player in GameSystem.Instance.GetPlayerList())
        {
            if (player.netId == targetNetId)
            {
                target = player;
            }
        }

        //null error
        if (target != null)
        {
            //킬시 임포스터가 시체쪽으로 이동
            RpcTeleport(target.transform.position);
            target.Dead(playerColor);
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }

    //타깃 크루원 죽는 기능처리 함수
    public void Dead(EPlayerColor imposterColor)
    {
        playerType |= EPlayerType.Ghost;
        RpcDead(imposterColor, playerColor);
        var manager = NetworkRoomManager.singleton as AmongUsRoomManager;

        //인스턴스화
        var deadbody = Instantiate(manager.spawnPrefabs[1], transform.position, transform.rotation).GetComponent<Deadbody>();
        NetworkServer.Spawn(deadbody.gameObject);//시체 생성
        deadbody.RpcSetColor(playerColor);
    }

    [ClientRpc]
    private void RpcDead(EPlayerColor imposterColor, EPlayerColor crewColor)
    {
        //죽은 크루원이 자신이면
        if (hasAuthority)
        {
            animator.SetBool("isGhost", true);
            IngameUIManager.Instance.KillUI.Open(imposterColor, crewColor);

            var players = GameSystem.Instance.GetPlayerList();
            
            //이미 죽어있는 고스트들을 볼 수 있게
            foreach (var player in players)
            {
                if ((player.playerType & EPlayerType.Ghost) == EPlayerType.Ghost)
                {
                    player.SetVisibility(true);
                }
            }
            GameSystem.Instance.ChangeLightMode(EPlayerType.Ghost);
        }
        else // 죽은 크루원이 내가 아니면
        {
            var myPlayer = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            if (((int)myPlayer.playerType & 0x02) != (int)EPlayerType.Ghost)
            {
                SetVisibility(false);
            }
        }

        //죽으면 자기캐릭터의 BoxCollider 비활성
        var collider = GetComponent<BoxCollider2D>();
        if (collider) collider.enabled = false;
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            var color = PlayerColor.GetColor(playerColor);
            //color.a = 0f;기본 값
            //최신버전에서 material의 컬러 알파값 오류때문에 바꿔봄 (문제가 될 수 있음)
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            spriteRenderer.material.SetColor("_PlayerColor", color);
            nicknameText.text = nickname;
        }
        else
        {
            var color = PlayerColor.GetColor(playerColor);

            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            spriteRenderer.material.SetColor("_PlayerColor", color);
            nicknameText.text = "";
        }
    }
}
