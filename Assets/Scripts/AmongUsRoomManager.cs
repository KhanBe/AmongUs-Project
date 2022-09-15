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
        base.OnRoomServerConnect(conn);

        //Room Manager �ν������� Registered Spawnable Prefabs�� �߰��� ��������
        //�ν��Ͻ�ȭ �Ѵ�
        var player = Instantiate(spawnPrefabs[0]);

        //Ŭ���̾�Ʈ�鿡�� ���ӿ�����Ʈ�� ��ȯ���� �˸�
        //��� ��ȯ�� ������Ʈ (player)�� ���� ������ �÷��̾� (conn)�� �������� �� �� ����
        NetworkServer.Spawn(player, conn);
    }
}
