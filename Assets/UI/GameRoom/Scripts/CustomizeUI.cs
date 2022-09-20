using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CustomizeUI : MonoBehaviour
{
    [SerializeField]
    private Image characterPreview;

    [SerializeField]
    private List<ColorSelectButton> colorSelectButtons;
    // Start is called before the first frame update
    void Start()
    {
        //material 인스턴스화(복제품)
        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }

    //활성화 시 호출되는 함수
    private void OnEnable()
    {
        UpdateColorButton();

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

    public void UpdateColorButton()
    {
        var roomSlots = (NetworkManager.singleton as AmongUsRoomManager).roomSlots;

        for (int i = 0; i < colorSelectButtons.Count; i++)
            colorSelectButtons[i].SetInteractable(true);

        foreach(var player in roomSlots)
        {
            var aPlayer = player as AmongUsRoomPlayer;
            colorSelectButtons[(int)aPlayer.playerColor].SetInteractable(false);
        }
    }

    //Preview캐릭터 색을 넣어주는 함수
    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    public void OnClickColorButton(int index)
    {
        if (colorSelectButtons[index].isInteractable)
        {
            AmongUsRoomPlayer.MyRoomPlayer.CmdSetPlayerColor((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }
}
