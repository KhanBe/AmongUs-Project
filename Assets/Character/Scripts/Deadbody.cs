using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Deadbody : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //서버에서 호출하여 클라이언트에서 동작하는 어트리뷰트
    [ClientRpc]
    public void RpcSetColor(EPlayerColor color)
    {
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }
}
