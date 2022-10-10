using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//회의 상태
public enum EMeetingState
{
    None,
    Meeting,
    Vote
}

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    //Player Panel들을 담는 Parent오브젝트
    [SerializeField]
    private Transform playerPanelsParent;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private GameObject skipVoteButton;

    [SerializeField]
    private GameObject skipVotePlayers;

    [SerializeField]
    private Transform skipVoteParentTransform;

    //시간 출력 Text
    [SerializeField]
    private Text meetingTimeText;

    private EMeetingState meetingState;

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

    //미팅 상태 변경 함수
    public void ChangeMeetingState(EMeetingState state)
    {
        meetingState = state;
    }

    //다른 판넬 비활성화 함수
    public void SelectPlayerPanel()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            panel.Unselect();
        }
    }

    public void UpdateVote(EPlayerColor voterColor, EPlayerColor ejectColor)
    {
        foreach (var panel in meetingPlayerPanels)
        {
            //투표받은 플레이어면
            if (panel.targetPlayer.playerColor == ejectColor)
            {
                panel.UpdatePanel(voterColor);
            }

            //투표한 플레이어면
            if (panel.targetPlayer.playerColor == voterColor)
            {
                panel.UpdateVoteSign(true);
            }
        }
    }

    public void UpdateSkipVotePlayer(EPlayerColor skipVotePlayerColor)
    {
        //skip하면 sign 표시
        foreach (var panel in meetingPlayerPanels)
        {
            if (panel.targetPlayer.playerColor == skipVotePlayerColor)
            {
                panel.UpdateVoteSign(true);
            }
        }

        //
        var voter = Instantiate(voterPrefab, skipVoteParentTransform).GetComponent<Image>();
        voter.material = Instantiate(voter.material);
        voter.material.SetColor("_PlayerColor", PlayerColor.GetColor(skipVotePlayerColor));

        //버튼 비활성화
        skipVoteButton.SetActive(false);
    }

    public void OnClickSkipVoteButton()
    {
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;

        //투표를 안했고 죽지않았으면 스킵 가능하게
        if (!myCharacter.isVote && !((myCharacter.playerType & EPlayerType.Ghost) == EPlayerType.Ghost))
        {
            myCharacter.CmdSkipVote();
            SelectPlayerPanel();
        }
    }

    //투표 끝난 후
    public void CompleteVote()
    {
        foreach (var panel in meetingPlayerPanels) panel.OpenResult();

        skipVoteButton.SetActive(false);
        skipVotePlayers.SetActive(true);
    }

    private void Update()
    {
        //Mathf.Clamp(value, min, max) : value가 범위보다 작으면 min반환, max보다 크면 max반환 
        if (meetingState == EMeetingState.Meeting)
        {
            meetingTimeText.text = string.Format("회의시간 : {0}s", (int)Mathf.Clamp(GameSystem.Instance.remainTime, 0f, float.MaxValue));
        }

        else if (meetingState == EMeetingState.Vote)
        {
            meetingTimeText.text = string.Format("투표시간 : {0}s", (int)Mathf.Clamp(GameSystem.Instance.remainTime, 0f, float.MaxValue));
        }
    }
}
