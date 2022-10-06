using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//�ڱ� Ÿ��
// ù �ڸ� : 0 - ũ��� / 1 - ��������
// �� �ڸ� : 0 - ������� / 1 - ����
// 00 - ����ִ� ũ��� / 01 ����ִ� ��������
// 10 - ���� ũ��� / 11 ���� ��������
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

    private void Awake()
    {
        
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
            //ų�� �������Ͱ� ��ü������ �̵�
            RpcTeleport(target.transform.position);
            target.Dead(playerColor);
            killCooldown = GameSystem.Instance.killCooldown;
        }
    }

    //Ÿ�� ũ��� �״� ���ó�� �Լ�
    public void Dead(EPlayerColor imposterColor)
    {
        playerType |= EPlayerType.Ghost;
        RpcDead(imposterColor, playerColor);
        var manager = NetworkRoomManager.singleton as AmongUsRoomManager;

        //�ν��Ͻ�ȭ
        var deadbody = Instantiate(manager.spawnPrefabs[1], transform.position, transform.rotation).GetComponent<Deadbody>();
        NetworkServer.Spawn(deadbody.gameObject);//��ü ����
        deadbody.RpcSetColor(playerColor);
    }

    [ClientRpc]
    private void RpcDead(EPlayerColor imposterColor, EPlayerColor crewColor)
    {
        //���� ũ����� �ڽ��̸�
        if (hasAuthority)
        {
            animator.SetBool("isGhost", true);
            IngameUIManager.Instance.KillUI.Open(imposterColor, crewColor);

            var players = GameSystem.Instance.GetPlayerList();
            
            //�̹� �׾��ִ� ��Ʈ���� �� �� �ְ�
            foreach (var player in players)
            {
                if ((player.playerType & EPlayerType.Ghost) == EPlayerType.Ghost)
                {
                    player.SetVisibility(true);
                }
            }
            GameSystem.Instance.ChangeLightMode(EPlayerType.Ghost);
        }
        else // ���� ũ����� ���� �ƴϸ�
        {
            var myPlayer = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            if (((int)myPlayer.playerType & 0x02) != (int)EPlayerType.Ghost)
            {
                SetVisibility(false);
            }
        }

        //������ �ڱ�ĳ������ BoxCollider ��Ȱ��
        var collider = GetComponent<BoxCollider2D>();
        if (collider) collider.enabled = false;
    }

    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            var color = PlayerColor.GetColor(playerColor);
            //color.a = 0f;�⺻ ��
            //�ֽŹ������� material�� �÷� ���İ� ���������� �ٲ㺽 (������ �� �� ����)
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
