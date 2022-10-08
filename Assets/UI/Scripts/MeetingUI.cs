using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    //Player Panel들을 담는 Parent오브젝트
    [SerializeField]
    private Transform playerPanelsParent;

    //생성된 플레이어 패널을 저장할 List
    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();

    public void Open()
    {
        //자신 플레이어 먼저 추가
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
        var myPanel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
        myPanel.SetPlayer(myCharacter);
        meetingPlayerPanels.Add(myPanel);

        gameObject.SetActive(true);

        //나머지 플레이어 추가
        var players = FindObjectsOfType<IngameCharacterMover>();

        foreach (var player in players)
        {
            if (player != myCharacter)
            {
                var panel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
                panel.SetPlayer(player);
                meetingPlayerPanels.Add(panel);
            }
        }
    }

    //다른 판넬 비활성화 함수
    public void SelectPlayerPanel()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            panel.Unselect();
        }
    }
}
