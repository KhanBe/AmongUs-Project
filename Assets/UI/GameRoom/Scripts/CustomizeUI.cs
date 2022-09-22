using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Image characterPreview;

    [SerializeField]//UI����Buttons
    private List<ColorSelectButton> colorSelectButtons;
    
    void Start()
    {
        //material �ν��Ͻ�ȭ(����ǰ)
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }

    //Ȱ��ȭ �� ȣ��Ǵ� �Լ�
    private void OnEnable()
    {
        UpdateColorButton();

        //������� �̹� ������ �ڱ� ���� �������־���ϱ⶧���� roomSlot���� ���� �̸� ����
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;
        foreach (var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            if (aPlayer.isLocalPlayer)//�ڽ� Player�� ��
            {
                UpdatePreviewColor(aPlayer.playerColor);
                break;
            }
        }
    }

    //Player���¿� ���� �����ư ������Ʈ ��� �Լ�
    public void UpdateColorButton()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        //true�� �ʱⰪ
        for (int i = 0; i < colorSelectButtons.Count; i++)
            colorSelectButtons[i].SetInteractable(true);

        //������������ player�� ������� SetInteractable(false)
        foreach (var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            colorSelectButtons[(int)aPlayer.playerColor].SetInteractable(false);
        }
    }

    //��ư ��Ȱ��ȭ 
    public void UpdateSelectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(false);
    }

    //��ư Ȱ��ȭ 
    public void UpdateUnselectColorButton(EPlayerColor color)
    {
        colorSelectButtons[(int)color].SetInteractable(true);
    }

    //Previewĳ���� ���� �־��ִ� �Լ�
    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    //���� ��ư�� Ŭ������ ��� Event
    //Inspector�� ������ index
    public void OnClickColorButton(int index)
    {
        //������ ��ư�� ��ȣ�ۿ� �����ϸ�
        if (colorSelectButtons[index].isInteractable)
        {
            AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }

    public void Open()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = false;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        AmongUsRoomPlayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = true;
        gameObject.SetActive(false);
    }
}
