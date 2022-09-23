using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomManager : NetworkRoomManager
{
    public int minPlayerCount;
    public int imposterCount;

    //서버에 새로 접속한 클라이언트를 감지했을때 동작하는 함수
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        // conn : 방금 서버에 접속한 플레이어의 정보
        // OnRoomServerConnect : 서버에 접속한 클라이언트 감지 역할 함수
        base.OnRoomServerConnect(conn);
    }
}
