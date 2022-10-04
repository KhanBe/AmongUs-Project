using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//자기 타입
public enum EPlayerType
{
    Crew,
    Imposter
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
            var manager = NetworkRoomManager.singleton as AmongUsRoomManager;

            //인스턴스화
            var deadbody = Instantiate(manager.spawnPrefabs[1], target.transform.position, target.transform.rotation).GetComponent<Deadbody>();
            NetworkServer.Spawn(deadbody.gameObject);
            deadbody.RpcSetColor(target.playerColor);
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }
}
