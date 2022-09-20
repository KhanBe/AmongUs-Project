using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Sprite�� ���ı���� ���� ��ũ��Ʈ
public class SpriteSorter : MonoBehaviour
{
    [SerializeField]
    private Transform Back;

    [SerializeField]
    private Transform Front;

    public int GetSortingOrder(GameObject obj)
    {
        //Back - obj �� y �Ÿ�
        float objDist = Mathf.Abs(Back.position.y - obj.transform.position.y);
        //Back - Front �� y�Ÿ�
        float totalDist = Mathf.Abs(Back.position.y - Front.position.y);

        // ���÷� Back.y : 2, obj.y : 1, Front.y : 0���� �ϸ� ���� ���� �ȴ�.
        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));
    }
}
