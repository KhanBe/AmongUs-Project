using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomManager : NetworkRoomManager
{
    //서버에 새로 접속한 클라이언트를 감지했을때 동작하는 함수
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        // conn : 방금 서버에 접속한 플레이어의 정보
        base.OnRoomServerConnect(conn);

        //Room Manager 인스펙터의 Registered Spawnable Prefabs에 추가된 프리펩을
        //인스턴스화 한다
        var player = Instantiate(spawnPrefabs[0]);

        //클라이언트들에게 게임오브젝트가 소환됨을 알림
        //방금 소환된 오브젝트 (player)가 새로 접속한 플레이어 (conn)의 소유임을 알 수 있음
        NetworkServer.Spawn(player, conn);
    }
}
