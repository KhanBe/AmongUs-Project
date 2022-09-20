using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sprite를 정렬기능을 가진 스크립트
public class SpriteSorter : MonoBehaviour
{
    [SerializeField]
    private Transform Back;

    [SerializeField]
    private Transform Front;

    public int GetSortingOrder(GameObject obj)
    {
        //Back - obj 의 y 거리
        float objDist = Mathf.Abs(Back.position.y - obj.transform.position.y);
        //Back - Front 의 y거리
        float totalDist = Mathf.Abs(Back.position.y - Front.position.y);

        // 예시로 Back.y : 2, obj.y : 1, Front.y : 0으로 하면 쉽게 이해 된다.
        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));
    }
}
