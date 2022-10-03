using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//IPointerClickHandler 클릭 입력을 받아줄 인터페이스 상속
public class TaskListUI : MonoBehaviour, IPointerClickHandler
{
    //TaskListUI가 열릴 때 transform.x의 기본값
    [SerializeField]
    private float offset;

    [SerializeField]
    private RectTransform TaskListUITransform;

    private bool isOpen = true;

    private float timer;

    public void OnPointerClick(PointerEventData eventData)
    {
        //동작중인 코루틴함수 정지
        StopAllCoroutines();
        StartCoroutine(OpenAndHideUI());
    }

    private IEnumerator OpenAndHideUI()
    {
        isOpen = !isOpen;
        if (timer != 0f) timer = 1f - timer;

        while (timer <= 1f)
        {
            timer += Time.deltaTime * 2f;

            float start = isOpen ? -TaskListUITransform.sizeDelta.x : offset;
            float dest = isOpen ? offset : -TaskListUITransform.sizeDelta.x;
            TaskListUITransform.anchoredPosition = new Vector2(Mathf.Lerp(start, dest, timer), TaskListUITransform.anchoredPosition.y);
            yield return null;
        }
    }
}
