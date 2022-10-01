using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameIntroUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shhh;

    [SerializeField]
    private GameObject crewmate;

    [SerializeField]
    private Text playerType;

    [SerializeField]
    private Image gradientImg;

    [SerializeField]
    private IntroCharacter myCharacter;

    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    [SerializeField]
    private Color crewColor;

    [SerializeField]
    private Color imposterColor;

    [SerializeField]//오브젝트 아래UI들을 다루는 변수
    private CanvasGroup canvasGroup;

    //인트로 시퀀스 보여주는 함수
    public IEnumerator ShowIntroSequence()
    {
        shhh.SetActive(true);
        yield return new WaitForSeconds(3f);
        shhh.SetActive(false);

        ShowPlayerType();
        crewmate.SetActive(true);
    }

    //인트로 타입 선정 함수
    public void ShowPlayerType()
    {
        var players = GameSystem.Instance.GetPlayerList();
        
        IngameCharacterMover myPlayer = null;

        foreach (var player in players)
        {
            if (player.hasAuthority)
            {
                myPlayer = player;
                break;
            }
        }

        myCharacter.SetIntroCharacter(myPlayer.nickname, myPlayer.playerColor);

        //자신이 임포스터고
        if (myPlayer.playerType == EPlayerType.Imposter)
        {
            playerType.text = "임포스터";
            playerType.color = gradientImg.color = imposterColor;

            int i = 0;

            foreach (var player in players)
            {
                //다른사람이 임포스터이면
                if (!player.hasAuthority && player.playerType == EPlayerType.Imposter)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }

        else //자신이 크루원일 경우
        {
            playerType.text = "크루원";
            playerType.color = gradientImg.color = crewColor;

            int i = 0;

            foreach (var player in players)
            {
                //자신 제외 
                if (!player.hasAuthority)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playerColor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }

    public void Close()
    {
        StartCoroutine(FadeOut());
    }

    //canvas가 점점 지워지는 함수
    private IEnumerator FadeOut()
    {
        float timer = 0f;

        while (timer <= 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer);
        }

        gameObject.SetActive(false);
    }
}
