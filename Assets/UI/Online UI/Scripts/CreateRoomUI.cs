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
        // ���� Material�� ����Ǵ� ���� �����ϱ� ���� Material Instantiating(
        for (int i = 0; i < crewImgs.Count; i++)
        {
            Material materialInstance = Instantiate(crewImgs[i].material);
            crewImgs[i].material = materialInstance;
        }
        //�ʱ� �⺻ ������
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };
        UpdateCrewImages();
    }

    //�� ����� ũ��� �̹��� ������Ʈ�Լ�
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

        //ũ��� �� �̹����� ǥ��
        for (i = 0; i<crewImgs.Count; i++)
        {
            if (i < roomData.maxPlayerCount) crewImgs[i].gameObject.SetActive(true);
            else crewImgs[i].gameObject.SetActive(false);
        }
    }
}

//������ ���� �� ���� ���
public class CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}
