using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Button colorButton;
    [SerializeField]
    private GameObject colorPanel;
    [SerializeField]
    private Button gameRuleButton;
    [SerializeField]
    private GameObject gameRulePanel;

    [SerializeField]
    private Image characterPreview;

    [SerializeField]//UI색상Buttons
    private List<ColorSelectButton> colorSelectButtons;
    
    void Start()
    {
        //material 인스턴스화(복제품)
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }
    //색상 버튼 눌렀을때 호출 함수 Event
    public void ActiveColorPanel()
    {
        colorButton.image.color = new Color(0f, 0f, 0f, 0.75f);
        gameRuleButton.image.color = new Color(0f, 0f, 0f, 0.25f);

        colorPanel.SetActive(true);
        gameRulePanel.SetActive(false);
    }

    //게임 버튼 눌렀을때 호출 함수 Event
    public void ActiveGameRulePanel()
    {
        colorButton.image.color = new Color(0f, 0f, 0f, 0.25f);
        gameRuleButton.image.color = new Color(0f, 0f, 0f, 0.75f);

        colorPanel.SetActive(false);
        gameRulePanel.SetActive(true);
    }

    //활성화 시 호출되는 함수
    private void OnEnable()
    {
        UpdateColorButton();//초기값
        ActiveColorPanel();//초기값
        //프리뷰는 이미 정해진 자기 색을 가지고있어야하기때문에 roomSlot으로 색을 미리 지정
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        foreach (var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            if (aPlayer.isLocalPlayer)//자신 Player일 때
            {
                UpdatePreviewColor(aPlayer.playerColor);
                break;
            }
        }
    }

    //Player상태에 따라 색상버튼 업데이트 기능 함수
    public void UpdateColorButton()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        //true로 초기값
        for (int i = 0; i < colorSelectButtons.Count; i++)
            colorSelectButtons[i].SetInteractable(true);

        //서버에접속한 player의 색상들은 SetInteractable(false)
        foreach (var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            colorSelectButtons[(int)aPlayer.playerColor].SetInteractable(false);
        }
    }

    //버튼 비활성화 
    public void UpdateSelectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(false);
    }

    //버튼 활성화 
    public void UpdateUnselectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(true);
    }

    //Preview캐릭터 색을 넣어주는 함수
    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    //색상 버튼을 클릭했을 경우 Event
    //Inspector에 지정한 index
    public void OnClickColorButton(int index)
    {
        //색상의 버튼이 상호작용 가능하면
        if (colorSelectButtons[index].isInteractable)
        {
            AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = false;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = true;
        gameObject.SetActive(false);
    }
}
