using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingPlayerPanel : MonoBehaviour
{

    [SerializeField]
    private Image characterImg;

    [SerializeField]
    private Text nicknameText;

    [SerializeField]
    private GameObject deadPlayerBlock;

    [SerializeField]
    private GameObject reportSign;

    [SerializeField]
    private GameObject voteButtons;

    public IngameCharacterMover targetPlayer;

    [SerializeField]
    private GameObject voteSign;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private Transform voterParentTransform;

    public void UpdatePanel(EPlayerColor voterColor)
    {
        var voter = Instantiate(voterPrefab, voterParentTransform).GetComponent<Image>();

        voter.material = Instantiate(voter.material);
        voter.material.SetColor("_PlayerColor", PlayerColor.GetColor(voterColor));
    }

    //투표자 결과 활성화
    public void OpenResult()
    {
        voterParentTransform.gameObject.SetActive(true);
    }

    //투표했다는 Sign 표시해주는 함수
    public void UpdateVoteSign(bool isVoted)
    {
        voteSign.SetActive(isVoted);
    }

    //플레이어 판넬 세팅
    public void SetPlayer(IngameCharacterMover target)
    {
        Material inst = Instantiate(characterImg.material);
        characterImg.material = inst;

        targetPlayer = target;
        characterImg.material.SetColor("_PlayerColor", PlayerColor.GetColor(targetPlayer.playerColor));
        nicknameText.text = target.nickname;

        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;

        if (((myCharacter.playerType & EPlayerType.Imposter) == EPlayerType.Imposter)
            && ((targetPlayer.playerType & EPlayerType.Imposter) == EPlayerType.Imposter))
        {
            nicknameText.color = Color.red;
        }

        bool isDead = (targetPlayer.playerType & EPlayerType.Ghost) == EPlayerType.Ghost;

        //죽으면 블라인드 처리 img
        deadPlayerBlock.SetActive(isDead);
        //죽으면 버튼 상호작용 불가능하게
        GetComponent<Button>().interactable = !isDead;
        reportSign.SetActive(targetPlayer.isReporter);
    }

    public void OnClickPlayerPanel()
    {
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;

        if (myCharacter.isVote) return;

        //고스트가 아닐경우에만 Panel버튼작동
        if ((myCharacter.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.MeetingUI.SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
    }

    public void Select()
    {
        var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
        myCharacter.CmdVoteEjectPlayer(targetPlayer.playerColor);
        Unselect();
    }

    public void Unselect()
    {
        voteButtons.SetActive(false);
    }
}
