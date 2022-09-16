using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));
    }
}
