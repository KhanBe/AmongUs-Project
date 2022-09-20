using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ĵ� Sprite���� ��� ��ũ��Ʈ
public class SortingSprite : MonoBehaviour
{
    //Static : ������ object
    //Update : �����̴� object
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
        //sortingOrder�� SpriteRenderer�� Order in Layer ���� �ǹ��ϴ°� ����.
        spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }

    private void FixedUpdate()
    {
        //Start�Լ����� GetSortingOrder�Լ��� �ѹ� ȣ���ϰ�
        //���⼭ Update�� ����(�����̴� object) �� �ֱ��� Update�� �Ѵ�.
        if (sortingType == ESortingType.Update)
            spriteRenderer.sortingOrder = sorter.GetSortingOrder(gameObject);
    }
}
