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

    //CustomizeUI ȣ�� Property
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

        //��ȣ�ۿ�
        useButton.interactable = true;
    }

    public void UnsetUseButton()
    {
        useButton.image.sprite = originUseButtonSprite;
        useButton.onClick.RemoveAllListeners();
        useButton.interactable = false;
    }

    //AmongUsRoomPlayer�� start()���� ȣ��
    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }

    //GameRoomPlayerCounter�� UpdatePlayerCount()���� ȣ��
    public void SetInteractableButton(bool isInteractable)
    {
        startButton.interactable = isInteractable;
    }

    //Start��ư Ŭ�� �� �̺�Ʈ �Լ�
    public void OnClickStartButton()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;

        //AmongUsRoomManager�� gameRuleData�� ����
        manager.gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();

        var players = FindObjectsOfType<AmongUsRoomPlayer>();
        //�÷��̾���� �غ���·�
        foreach (var player in players) player.readyToBegin = true;

        //Scene ��ȯ
        manager.ServerChangeScene(manager.GameplayScene);
    }
}
