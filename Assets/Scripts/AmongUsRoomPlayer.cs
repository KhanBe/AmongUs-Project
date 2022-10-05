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
        LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(oldColor);//��Ȱ��
        LobbyUIManager.Instance.CustomizeUI.UpdateSelectColorButton(newColor);//Ȱ��
    }

    [SyncVar]
    public string nickname;

    public CharacterMover myCharacter;

    public void Start()
    {
        //�̹� ��ӵ� �θ� Ŭ���� NetworkRoomPlayer���� Start()�Լ��� ȣ��Ǿ��ֱ� ������
        //base.Start()�� Start�Լ��� ȣ���Ѵ�.
        base.Start();

        //���������� ���
        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
            LobbyUIManager.Instance.ActiveStartButton();
        }

        if (isLocalPlayer) CmdSetNickname(PlayerSettings.nickname);

        //�߾� �ϴ� �÷��̾� ��
        LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
    }

    //�濡�� ���� ������Ʈ �ı� �� �����ù�ư Ȱ��ȭ
    private void OnDestroy()
    {
        if (LobbyUIManager.Instance != null)
        {
            LobbyUIManager.Instance.CustomizeUI.UpdateUnselectColorButton(playerColor);

            //�߾� �ϴ� �÷��̾� ��
            LobbyUIManager.Instance.GameRoomPlayerCounter.UpdatePlayerCount();
        }   
    }

    //Command Attribute (Mirror API ���� : Ŭ���̾�Ʈ���� �Լ� ���� �� �������� �Լ� ����)
    //Ŭ���̾�Ʈ���� ������ ��ȣ�� ������ ���
    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        myCharacter.playerColor = color;
    }

    [Command]
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        myCharacter.nickname = nick;
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

        var spawnPositions = FindObjectOfType<SpawnPositions>();
        int index = spawnPositions.Index;

        //SpawnPositions Ŭ������ GetSpawnPosition�Լ��� ��ȯ Vector3 ������ ���� �����´�
        Vector3 spawnPos = spawnPositions.GetSpawnPosition();
        
        //Room Manager �ν������� Registered Spawnable Prefabs�� �߰��� ��������
        //�ν��Ͻ�ȭ �Ѵ� 
        var playerChracter = Instantiate(AmongUsRoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();

        //6��° Ŭ����ʹ� flip�ؼ� ��ȯ
        playerChracter.transform.localScale = index < 5 ? new Vector3(0.5f, 0.5f, 1f) : new Vector3(-0.5f, 0.5f, 1f);

        //Ŭ���̾�Ʈ�鿡�� ���ӿ�����Ʈ�� ��ȯ���� �˸�
        //connectionToClient�� connectionToServer�� ������ Ŭ���̾�Ʈ �� ��� ������ ȣ���ϴ��� ����
        //���� ������ connection�� Ȯ���Ϸ��� �ϸ� conntectionToServer�� ����ϰ� Ŭ���̾�Ʈ ������ connection�� Ȯ���� ���� connectionToClient�� ����ϸ� �ȴ�.
        NetworkServer.Spawn(playerChracter.gameObject, connectionToClient);

        playerChracter.ownerNetId = netId;
        playerChracter.playerColor = color;
    }
}
