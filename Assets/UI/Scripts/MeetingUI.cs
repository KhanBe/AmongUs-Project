using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    //Player Panel���� ��� Parent������Ʈ
    [SerializeField]
    private Transform playerPanelsParent;

    //������ �÷��̾� �г��� ������ List
    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();

    public void Open()
    {
        //�ڽ� �÷��̾� ���� �߰�
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
        var myPanel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
        myPanel.SetPlayer(myCharacter);
        meetingPlayerPanels.Add(myPanel);

        gameObject.SetActive(true);

        //������ �÷��̾� �߰�
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

    //�ٸ� �ǳ� ��Ȱ��ȭ �Լ�
    public void SelectPlayerPanel()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            panel.Unselect();
        }
    }
}
