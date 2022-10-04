using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//�ڱ� Ÿ��
public enum EPlayerType
{
    Crew,
    Imposter
}

public class IngameCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetPlayerType_Hook))]
    public EPlayerType playerType;
    private void SetPlayerType_Hook(EPlayerType _, EPlayerType type)//playerType����� �۵��Ǵ� ���Լ�
    {
        //�ڱ� ĳ�����̸鼭 ���������� ���
        if (hasAuthority && type == EPlayerType.Imposter)
        {
            IngameUIManager.Instance.KillButtonUI.Show(this);//this : �ڽ� ĳ����
            playerFinder.SetKillRange(GameSystem.Instance.killRange + 1f);
        }
    }

    [SerializeField]
    private PlayerFinder playerFinder;

    [SyncVar]
    private float killCooldown;
    public float KillCooldown { get { return killCooldown; } }

    public bool isKillable { get { return killCooldown < 0f && playerFinder.targets.Count != 0; } }//boolŸ�� ų���ɿ��� �Լ�

    //ų ��Ÿ�� �����ϴ� �Լ�
    public void SetKillCooldown()
    {
        //���� ����(����)�̸�
        if (isServer)
        {
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }

    [ClientRpc]//�������� Ŭ���̾�Ʈ�� ȣ���ϴ� ��Ʈ����Ʈ
    public void RpcTeleport(Vector3 position)
    {
        //������position���� ������ ����ȭ
        transform.position = position;
    }

    //�ΰ��� ������ �Լ�
    public void SetNicknameColor(EPlayerType type)
    {
        //�������������̸鼭 �ٸ��÷��̾ ���������̸�
        if (playerType == EPlayerType.Imposter && type == EPlayerType.Imposter)
        {
            nicknameText.color = Color.red;
        }
    }

    public override void Start()
    {
        // CharacterMover�� Start()�Լ� ȣ��
        base.Start();

        if (hasAuthority)
        {
            IsMoveable = true;

            var myRoomPlayer = AmongUsRoomPlayer.MyRoomPlayer;
            myRoomPlayer.myCharacter = this;
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        //IngameCharacter��ü���� ������ GameSystemŬ������ �ڽ��� ����ϵ��� �����.
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
        //GetFirstTarget().netId : ù Ÿ���� netId
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

            //�ν��Ͻ�ȭ
            var deadbody = Instantiate(manager.spawnPrefabs[1], target.transform.position, target.transform.rotation).GetComponent<Deadbody>();
            NetworkServer.Spawn(deadbody.gameObject);
            deadbody.RpcSetColor(target.playerColor);
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }
}
