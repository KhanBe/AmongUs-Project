using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mirror;

public class LobbyUIManager : MonoBehaviour
{
    //Singleton
    //LobbyUIManager instance
    public static LobbyUIManager Instance;

    [SerializeField]
    private CustomizeUI customizeUI;

    //CustomizeUI 호출 Property
    public CustomizeUI CustomizeUI { get { return customizeUI; } }

    [SerializeField]
    private GameRoomPlayerCounter gameRoomPlayerCounter;
    public GameRoomPlayerCounter GameRoomPlayerCounter { get { return gameRoomPlayerCounter; } }

    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Sprite originUseButtonSprite;

    [SerializeField]
    private Button startButton;

    private void Awake()
    {
        Instance = this;
    }

    //action 
    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        useButton.image.sprite = sprite;

        useButton.onClick.AddListener(action);

        //상호작용
        useButton.interactable = true;
    }

    public void UnsetUseButton()
    {
        useButton.image.sprite = originUseButtonSprite;
        useButton.onClick.RemoveAllListeners();
        useButton.interactable = false;
    }

    //AmongUsRoomPlayer의 start()에서 호출
    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    //GameRoomPlayerCounter의 UpdatePlayerCount()에서 호출
    public void SetInteractableButton(bool isInteractable)
    {
        startButton.interactable = isInteractable;
    }

    //Start버튼 클릭 시 이벤트 함수
    public void OnClickStartButton()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;

        //AmongUsRoomManager의 gameRuleData에 저장
        manager.gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();

        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        //플레이어들을 준비상태로
        foreach (var player in players) player.readyToBegin = true;

        //Scene 전환
        manager.ServerChangeScene(manager.GameplayScene);
    }
}
