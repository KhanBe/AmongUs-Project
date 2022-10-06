using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Deadbody : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;

    private EPlayerColor deadbodyColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //서버에서 호출하여 클라이언트에서 동작하는 어트리뷰트
    [ClientRpc]
    public void RpcSetColor(EPlayerColor color)
    {
        //매개변수로 받은값을 저장
        deadbodyColor = color;

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    //크루원과 시체 collider 접근시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();

        //자기 자신이면서 유령이 아닌상태 일 때
        if (player != null && player.hasAuthority && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.ReportButtonUI.SetInteractable(true);

            //발견자 캐릭터의 변수foundDeadbodyColor값 지정
            var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            myCharacter.foundDeadbodyColor = deadbodyColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();

        //자기 자신이면서 유령이 아닌상태 일 때
        if (player != null && player.hasAuthority && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.ReportButtonUI.SetInteractable(false);
        }
    }
}
