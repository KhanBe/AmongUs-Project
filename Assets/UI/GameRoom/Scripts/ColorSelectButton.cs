using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ���� ���� ��ư�� �޾��� ��ũ��Ʈ
public class ColorSelectButton : MonoBehaviour
{
    [SerializeField]
    private GameObject x;

    public bool isInteractable = true;

    public void SetInteractable(bool isInteractable)
    {
        this.isInteractable = isInteractable;
        x.SetActive(!isInteractable);
    }
}
