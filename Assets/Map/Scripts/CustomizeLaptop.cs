using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeLaptop : MonoBehaviour
{
    [SerializeField]
    private Sprite useButtonSprite;//use Button
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        var inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = inst;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMover>();
        if (character != null && character.hasAuthority)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 1f);

            LobbyUIManager.Instance.SetUseButton(useButtonSprite, OnClickUse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMover>();
        if (character != null && character.hasAuthority)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 0f);

            LobbyUIManager.Instance.UnsetUseButton();
        }
    }

    //Use눌렀을 때 함수
    public void OnClickUse()
    {
        LobbyUIManager.Instance.CustomizeUI.Open();
    }
}
