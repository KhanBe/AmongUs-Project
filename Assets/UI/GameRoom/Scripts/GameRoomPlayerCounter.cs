using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameRoomPlayerCounter : NetworkBehaviour 
{
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    //ȭ�� �߾� �Ʒ� �÷��̾� �ο� �ؽ�Ʈ
    [SerializeField]
    private Text playerCountText;

    //�� �Լ� ������Ʈ�� AmongUsRoomPlayer�� Start()�� OnDestroy()���� ���ش�.
    public void UpdatePlayerCount()
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();

        bool isStartable = players.Length >= minPlayer;
        playerCountText.color = isStartable ? Color.white : Color.red;
        //players.Length : ũ��
        playerCountText.text = string.Format("{0}/{1}", players.Length, maxPlayer);
        LobbyUIManager.Instance.SetInteractableButton(isStartable);
    }

    // Start is called before the first frame update
    void Start()
    {
        //������ �ִ� ���¶��
        if (isServer)
        {
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            minPlayer = manager.minPlayerCount;
            maxPlayer = manager.maxConnections;
        }
    }
}
