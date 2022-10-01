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
    [SyncVar]
    public EPlayerType playerType;

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
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }

        //IngameCharacter��ü���� ������ GameSystemŬ������ �ڽ��� ����ϵ��� �����.
        GameSystem.Instance.AddPlayer(this);
    }

    [Command]
    private void CmdSetPlayerCharacter(string nickname, EPlayerColor color)
    {
        this.nickname = nickname;
        playerColor = color;
    }
}
