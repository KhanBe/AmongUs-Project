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

        //SpawnPositions Ŭ������ GetSpawnPosition�Լ��� ��ȯ Vector3 ������ ���� �����´�
        Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        //Room Manager �ν������� Registered Spawnable Prefabs�� �߰��� ��������
        //�ν��Ͻ�ȭ �Ѵ� 
        var player = Instantiate(spawnPrefabs[0], spawnPos, Quaternion.identity);

        //Ŭ���̾�Ʈ�鿡�� ���ӿ�����Ʈ�� ��ȯ���� �˸�
        //��� ��ȯ�� ������Ʈ (player)�� ���� ������ �÷��̾� (conn)�� �������� �� �� ����
        NetworkServer.Spawn(player, conn);
    }
}
