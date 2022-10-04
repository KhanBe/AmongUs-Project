using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    [SerializeField]
    private Transform left;
    [SerializeField]
    private Transform right;
    [SerializeField]
    private Transform top;
    [SerializeField]
    private Transform bottom;

    [SerializeField]
    private Image minimapImage;
    [SerializeField]
    private Image minimapPlayerImage;

    private CharacterMover targetPlayer;

    private void Start()
    {
        var inst = Instantiate(minimapImage.material);
        minimapImage.material = inst;

        targetPlayer = AmongUsRoomPlayer.MyRoomPlayer.myCharacter;
    }

    private void Update()
    {
        //left, right, top, bottom 기준으로 normalized(정규화)하여 미니맵 구상
        if (targetPlayer != null)
        {
            Vector2 mapArea = new Vector2(Vector3.Distance(left.position, right.position), Vector3.Distance(bottom.position, top.position));

            //앵커값을 left,bottom으로 기준을 잡음
            Vector2 charPos = new Vector2(Vector3.Distance(left.position, new Vector3(targetPlayer.transform.position.x, 0f, 0f)),
                Vector3.Distance(bottom.position, new Vector3(0f, targetPlayer.transform.position.y, 0f)));

            //비율
            Vector2 normalPos = new Vector2(charPos.x / mapArea.x, charPos.y / mapArea.y);

            minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
        }
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
