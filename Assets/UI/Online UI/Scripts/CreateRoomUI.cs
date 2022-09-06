using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

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

    public void UpdateImposterCount(int count)
    {
        roomData.imposterCount = count;

        //클릭한 버튼만 알파값 넣어주는 for문
        for (int i = 0; i < imposterCountButtons.Count; i++)
        {
            //인스펙터에 imposterCountButtons리스트값을 1,2,3으로 넣음
            if (i == count - 1)
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            else
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
        }

        //게임 룰 ) 
        // 임포1이면 최대인원 4이상, 임포2이면 7이상, 임포3이면 9이상 제약사항
        int limitMaxPlayer = count == 1 ? 4 : count == 2 ? 7 : 9;

        if (roomData.maxPlayerCount < limitMaxPlayer)
            UpdateMaxPlayerCount(limitMaxPlayer);
        else
            UpdateMaxPlayerCount(roomData.maxPlayerCount);

        //버튼 비활성화
        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            var text = maxPlayerCountButtons[i].GetComponentInChildren<Text>();
            if (i < limitMaxPlayer - 4)//인덱스값이라 -4
            {
                maxPlayerCountButtons[i].interactable = false;
                text.color = Color.gray;
            }
            else
            {
                maxPlayerCountButtons[i].interactable = true;
                text.color = Color.white;
            }
        }
    }

    //최대 인원 수 변경 함수
    public void UpdateMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        //클릭한 버튼만 알파값 넣어주는 for문
        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            //인스펙터에 maxPlayerCountButtons리스트값을 4,5,6,7,8,9,10으로 넣음
            if (i == count - 4)
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            else
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
        }

        UpdateCrewImages();
    }

    //맵 배너의 크루원 이미지 업데이트함수
    private void UpdateCrewImages()
    {
        int i = 0;

        for (i = 0; i < crewImgs.Count; i++)//크루원 기본색상 초기화
            crewImgs[i].material.SetColor("_PlayerColor", Color.white);

        int imposterCount = roomData.imposterCount;
        i = 0;
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

    public void CreateRoom()
    {
        //씬에 있는 네트워크 매니저를 가져온다.
        var manager = AmongUsRoomManager.singleton;

        // StartHost : 서버를 여는 동시에 클라이언트로써 게임에 참가하게 한다
        manager.StartHost();
        //기본적으로 열기 전에 Room Manager에 방을 설정하고 세팅한다.
    }
}

//데이터 저장 및 전달 기능
public class CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}
