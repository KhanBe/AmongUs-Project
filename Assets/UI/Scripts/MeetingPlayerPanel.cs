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

    public IngameCharacterMover targetPlayer;

    //미팅 시 플레이어들 세팅
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

        deadPlayerBlock.SetActive((targetPlayer.playerType & EPlayerType.Ghost) == EPlayerType.Ghost);
        reportSign.SetActive(targetPlayer.isReporter);
    }

}
