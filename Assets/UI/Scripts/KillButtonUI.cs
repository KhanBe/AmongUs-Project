using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillButtonUI : MonoBehaviour
{
    [SerializeField]
    private Button killButton;

    [SerializeField]
    private Text cooldownText;

    private IngameCharacterMover targetPlayer;

    //킬버튼 활성화 함수
    public void Show(IngameCharacterMover player)
    {
        gameObject.SetActive(true);
        targetPlayer = player;
    }

    private void Update()
    {
        //null error
        if (targetPlayer != null)
        {
            //킬 불가능 시
            if (!targetPlayer.isKillable)
            {
                cooldownText.text = targetPlayer.KillCooldown > 0 ? ((int)targetPlayer.KillCooldown).ToString() : "";
                killButton.interactable = false;
            }
            //킬 가능 시
            else
            {
                cooldownText.text = "";
                killButton.interactable = true;
            }
        }
    }

    public void OnClickKillButton()
    {
        targetPlayer.Kill();
    }
}
