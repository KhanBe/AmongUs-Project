using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint ownerNetId;//unit : (0 ~ 4,294,967,295)

    //ownerNetId ����� Ŭ���̾�Ʈ���� ȣ��� �Լ�
    public void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        foreach (var player in players)
        {
            //netId�� �̿��� �ڽ��� RoomPlayerã��
            if (newOwnerId == player.netId)
            {
                player.lobbyPlayerCharacter = this;
                break;
            }
        }
    }

    public void CompleteSpawn()
    {
        if (hasAuthority)
        {
            isMoveable = true;
        }
    }
}
