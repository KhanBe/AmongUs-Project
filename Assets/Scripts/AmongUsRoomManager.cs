using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AmongUsRoomManager : NetworkRoomManager
{
    //������ ���� ������ Ŭ���̾�Ʈ�� ���������� �����ϴ� �Լ�
    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        // conn : ��� ������ ������ �÷��̾��� ����
        // OnRoomServerConnect : ������ ������ Ŭ���̾�Ʈ ���� ���� �Լ�
        base.OnRoomServerConnect(conn);
    }
}
