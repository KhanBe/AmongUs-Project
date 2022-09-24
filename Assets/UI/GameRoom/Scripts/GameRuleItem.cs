using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//호스트가 아닌 클라이언트가 게임 규칙 설정을 못 바꾸도록 설정 스크립트
public class GameRuleItem : MonoBehaviour
{
    [SerializeField]
    private GameObject inactiveObject;

    void Start()
    {
        //호스트 이면
        if (!AmongUsRoomPlayer.MyRoomPlayer.isServer)
            inactiveObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
