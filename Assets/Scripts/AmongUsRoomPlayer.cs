using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomPlayer : NetworkRoomPlayer
{
    private static AmongUsRoomPlayer myRoomPlayer;

    //null ���̸� �ڱ� �ڽ��÷��̾ ��ȯ�ϴ� Property
    public static AmongUsRoomPlayer MyRoomPlayer
    {
        get
        {
            if (myRoomPlayer == null)
            {
                var players = FindObjectsOfType<AmongUsRoomPlayer>();
                foreach (var player in players)
                {
                    if (player.hasAuthority)
                        myRoomPlayer = player;
                }
            }
            return myRoomPlayer;
        }
    }

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    //playerColor�� ����� �� ȣ�� Hook
    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        LobbyUIManager.Instance.CustomizeUI.UpdateColorButton();
    }

    public CharacterMover lobbyPlayerCharacter;

    public void Start()
    {
        //�̹� ��ӵ� �θ� Ŭ���� NetworkRoomPlayer���� Start()�Լ��� ȣ��Ǿ��ֱ� ������
        //base.Start()�� Start�Լ��� ȣ���Ѵ�.
        base.Start();
        
        if (isServer) SpawnLobbyPlayerCharacter();
    }

    //Command Attribute (Mirror API ���� : Ŭ���̾�Ʈ���� �Լ� ���� �� �������� �Լ� ����)
    //Ŭ���̾�Ʈ���� ������ ��ȣ�� ������ ���
    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        lobbyPlayerCharacter.playerColor = color;
    }

    private void SpawnLobbyPlayerCharacter()
    {
        // as ������ : ĳ������ �ȵǸ� null��ȯ �Ǹ� ĳ����
        //���ǿ� ���� ���� �÷��̾ �������� ���� RoomManager�� roomSlots
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;
        for (int i = 0; i < (int)EPlayerColor.Lime + 1; i++)
        {
            bool isFindSameColor = false;
            foreach (var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomPlayer;
                //netId �����ĺ� network id
                if (amongUsRoomPlayer.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }

            if (!isFindSameColor)
            {
                color = (EPlayerColor)i;
                break;
            }
        }
        playerColor = color;

        //SpawnPositions Ŭ������ GetSpawnPosition�Լ��� ��ȯ Vector3 ������ ���� �����´�
        Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        //Room Manager �ν������� Registered Spawnable Prefabs�� �߰��� ��������
        //�ν��Ͻ�ȭ �Ѵ� 
        var playerChracter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();

        //Ŭ���̾�Ʈ�鿡�� ���ӿ�����Ʈ�� ��ȯ���� �˸�
        //connectionToClient�� connectionToServer�� ������ Ŭ���̾�Ʈ �� ��� ������ ȣ���ϴ��� ����
        //���� ������ connection�� Ȯ���Ϸ��� �ϸ� conntectionToServer�� ����ϰ� Ŭ���̾�Ʈ ������ connection�� Ȯ���� ���� connectionToClient�� ����ϸ� �ȴ�.
        NetworkServer.Spawn(playerChracter.gameObject, connectionToClient);

        playerChracter.ownerNetId = netId;

        playerChracter.playerColor = color;
    }
}
