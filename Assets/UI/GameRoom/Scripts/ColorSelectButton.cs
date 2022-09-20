using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 색상 선택 버튼에 달아줄 스크립트
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
