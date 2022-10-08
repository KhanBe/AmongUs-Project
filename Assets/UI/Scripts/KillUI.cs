using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillUI : MonoBehaviour
{
    [SerializeField]
    private Image imposterImg;

    [SerializeField]
    private Image crewmateImg;

    [SerializeField]
    private Material material;

    public void Open(EPlayerColor imposter, EPlayerColor crewmate)
    {
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = false;

        Material inst1 = Instantiate(material);
        imposterImg.material = inst1;

        Material inst2 = Instantiate(material);
        crewmateImg.material = inst2;

        gameObject.SetActive(true);//KillUI 활성화

        imposterImg.material.SetColor("_PlayerColor", PlayerColor.GetColor(imposter));
        crewmateImg.material.SetColor("_PlayerColor", PlayerColor.GetColor(crewmate));

        //지연 함수
        Invoke("Close", 3f);
    }

    public void Close()
    {
        gameObject.SetActive(false);


        //killUI가 닫히기 전 Report시 Ghost가 회의 중 움직이는 현상이 발생
        //AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = true;

        //UI가 하나라도 켜져있으면 false
        AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable =
            !(IngameUIManager.Instance.ReportUI.gameObject.activeSelf || 
            IngameUIManager.Instance.MeetingUI.gameObject.activeSelf);
    }
}
