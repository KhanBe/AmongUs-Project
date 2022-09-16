using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;

    private int index;

    //스폰될 포지션Vector3 값 반환 함수
    public Vector3 GetSpawnPosition()
    {
        Vector3 pos = positions[index++].position;

        if (index >= positions.Length) index = 0;

        return pos;
    }
}
