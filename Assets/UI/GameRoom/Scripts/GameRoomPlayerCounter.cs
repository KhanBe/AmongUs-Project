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

    //화면 중앙 아래 플레이어 인원 텍스트
    [SerializeField]
    private Text playerCountText;

    //이 함수 업데이트는 AmongUsRoomPlayer의 Start()와 OnDestroy()에서 해준다.
    public void UpdatePlayerCount()
    {
        var players = FindObjectsOfType<AmongUsRoomPlayer>();

        bool isStartable = players.Length >= minPlayer;
        playerCountText.color = isStartable ? Color.white : Color.red;
        //players.Length : 크기
        playerCountText.text = string.Format("{0}/{1}", players.Length, maxPlayer);
        LobbyUIManager.Instance.SetInteractableButton(isStartable);
    }

    // Start is called before the first frame update
    void Start()
    {
        //서버에 있는 상태라면
        if (isServer)
        {
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            minPlayer = manager.minPlayerCount;
            maxPlayer = manager.maxConnections;
        }
    }
}
