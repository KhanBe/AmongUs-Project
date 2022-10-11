using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EjectionUI : MonoBehaviour
{
    [SerializeField]
    private Text ejectionResultText;

    [SerializeField]
    private Image ejectionPlayer;

    [SerializeField]
    private RectTransform left;

    [SerializeField]
    private RectTransform Right;

    // Start is called before the first frame update
    void Start()
    {
        ejectionPlayer.material = Instantiate(ejectionPlayer.material);
    }

    //(추방당할 플레이어가 있는지, 추방자 색상, 추방자가 임포스터인지, 남은임포스터 수)
    public void Open(bool isEjection, EPlayerColor ejectionPlayerColor, bool isImposter, int remainImposterCount)
    {
        string text = "";
        IngameCharacterMover ejectPlayer = null;

        if (isEjection)
        {
            IngameCharacterMover[] players = FindObjectsOfType<IngameCharacterMover>();

            foreach (var player in players)
            {
                if (player.playerColor == ejectionPlayerColor)
                {
                    ejectPlayer = player;
                    break;
                }
            }
            text = string.Format("{0}은 임포스터{1}\n임포스터가 {2}명 남았습니다.",
                ejectPlayer.nickname, isImposter ? "입니다." : "가 아니었습니다.", remainImposterCount);
        }
        else
        {
            text = string.Format("아무도 퇴출되지 않았습니다.\n임포스터가 {0}명 남았습니다.", remainImposterCount);
        }

        gameObject.SetActive(true);

        StartCoroutine(ShowEjectionResult_Coroutine(ejectPlayer, text));
    }

    private IEnumerator ShowEjectionResult_Coroutine(IngameCharacterMover ejectionPlayerMover, string text)
    {
        ejectionResultText.text = "";

        string forwardText = "";
        string backText = "";

        if (ejectionPlayerMover != null)
        {
            //색 지정
            ejectionPlayer.material.SetColor("_PlayerColor", PlayerColor.GetColor(ejectionPlayerMover.playerColor));

            float timer = 0f;

            //추방 모션
            while (timer <= 1f)
            {
                yield return null;
                timer += Time.deltaTime * 0.3f;

                ejectionPlayer.rectTransform.anchoredPosition = Vector2.Lerp(left.anchoredPosition, Right.anchoredPosition, timer);
                ejectionPlayer.rectTransform.rotation = Quaternion.Euler(ejectionPlayer.rectTransform.rotation.eulerAngles +
                    new Vector3(0f, 0f, -360 * Time.deltaTime));
            }
        }

        //글자 하나씩 출력되는 모션
        backText = text;
        while (backText.Length != 0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            ejectionResultText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
