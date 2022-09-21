using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    private Button useButton;
    [SerializeField]
    private Sprite originUseButtonSprite;

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
}
