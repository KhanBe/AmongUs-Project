using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> crewImgs;

    [SerializeField]
    private List<Button> imposterCountButtons;

    [SerializeField]
    private List<Button> maxPlayerCountButtons;

    private CreateGameRoomData roomData;

    private void Start()
    {
        // 원본 Material이 변경되는 것을 방지하기 위해 Material Instantiating(
        for (int i = 0; i < crewImgs.Count; i++)
        {
            Material materialInstance = Instantiate(crewImgs[i].material);
            crewImgs[i].material = materialInstance;
        }
        //초기 기본 설정값
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };
        UpdateCrewImages();
    }

    //맵 배너의 크루원 이미지 업데이트함수
    private void UpdateCrewImages()
    {
        int imposterCount = roomData.imposterCount;
        int i = 0;
        while (imposterCount != 0)
        {
            if (i >= roomData.maxPlayerCount) i = 0;

            if (crewImgs[i].material.GetColor("_PlayerColor") != Color.red && Random.Range(0, 5) == 0)
            {
                crewImgs[i].material.SetColor("_PlayerColor", Color.red);
                imposterCount--;
            }
            i++;
        }

        //크루원 수 이미지로 표시
        for (i = 0; i<crewImgs.Count; i++)
        {
            if (i < roomData.maxPlayerCount) crewImgs[i].gameObject.SetActive(true);
            else crewImgs[i].gameObject.SetActive(false);
        }
    }
}

//데이터 저장 및 전달 기능
public class CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}
