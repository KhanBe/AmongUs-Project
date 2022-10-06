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

    //�������� ȣ���Ͽ� Ŭ���̾�Ʈ���� �����ϴ� ��Ʈ����Ʈ
    [ClientRpc]
    public void RpcSetColor(EPlayerColor color)
    {
        //�Ű������� �������� ����
        deadbodyColor = color;

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }

    //ũ����� ��ü collider ���ٽ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();

        //�ڱ� �ڽ��̸鼭 ������ �ƴѻ��� �� ��
        if (player != null && player.hasAuthority && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.ReportButtonUI.SetInteractable(true);

            //�߰��� ĳ������ ����foundDeadbodyColor�� ����
            var myCharacter = AmongUsRoomPlayer.MyRoomPlayer.myCharacter as IngameCharacterMover;
            myCharacter.foundDeadbodyColor = deadbodyColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();

        //�ڱ� �ڽ��̸鼭 ������ �ƴѻ��� �� ��
        if (player != null && player.hasAuthority && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            IngameUIManager.Instance.ReportButtonUI.SetInteractable(false);
        }
    }
}
