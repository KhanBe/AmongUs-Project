using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWiringTaskObject : MonoBehaviour
{
    [SerializeField]
    private Sprite _UseButtonSprite;

    private SpriteRenderer _SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        _SpriteRenderer = GetComponent<SpriteRenderer>();
        _SpriteRenderer.material = Instantiate(_SpriteRenderer.material);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<IngameCharacterMover>();

        if (character != null && character.hasAuthority)
        {
            _SpriteRenderer.material.SetFloat("_Highlighted", 1f);
            IngameUIManager.Instance.SetUseButton(_UseButtonSprite, OnClickUse);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<IngameCharacterMover>();

        if (character != null && character.hasAuthority)
        {
            _SpriteRenderer.material.SetFloat("_Highlighted", 0f);
            IngameUIManager.Instance.UnsetUseButton();
        }
    }

    public void OnClickUse()
    {
        IngameUIManager.Instance.FixWiringTaskUI.Open();
    }
}
