using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetOwnerNetId_Hook))]
    public uint ownerNetId;//unit : (0 ~ 4,294,967,295)

    //ownerNetId 변경시 클라이언트에서 호출될 함수
    public void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        foreach (var player in players)
        {
            //netId를 이용해 자신의 RoomPlayer찾기
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
