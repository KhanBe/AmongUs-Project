using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//정렬될 Sprite들을 담는 스크립트
public class SortingSprite : MonoBehaviour
{
    //Static : 고정된 object
    //Update : 움직이는 object
    public enum ESortingType
    {
        Static, Update
    }

    [SerializeField]
    private ESortingType sortingType;

    private SpriteSorter sorter;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        //Sorter class
        sorter = FindObjectOfType<SpriteSorter>();
        //SpriteRenderer Component
        spriteRenderer = GetComponent<SpriteRenderer>();
        //sortingOrder는 SpriteRenderer의 Order in Layer 값을 의미하는거 같다.
        spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }

    private void FixedUpdate()
    {
        //Start함수에서 GetSortingOrder함수를 한번 호출하고
        //여기서 Update를 가진(움직이는 object) 만 주기적 Update를 한다.
        if (sortingType == ESortingType.Update)
            spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }
}
