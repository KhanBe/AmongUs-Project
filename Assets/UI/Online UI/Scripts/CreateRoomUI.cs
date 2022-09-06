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

    public void UpdateImposterCount(int count)
    {
        roomData.imposterCount = count;

        //Ŭ���� ��ư�� ���İ� �־��ִ� for��
        for (int i = 0; i < imposterCountButtons.Count; i++)
        {
            //�ν����Ϳ� imposterCountButtons����Ʈ���� 1,2,3���� ����
            if (i == count - 1)
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            else
                imposterCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
        }

        //���� �� ) 
        // ����1�̸� �ִ��ο� 4�̻�, ����2�̸� 7�̻�, ����3�̸� 9�̻� �������
        int limitMaxPlayer = count == 1 ? 4 : count == 2 ? 7 : 9;

        if (roomData.maxPlayerCount < limitMaxPlayer)
            UpdateMaxPlayerCount(limitMaxPlayer);
        else
            UpdateMaxPlayerCount(roomData.maxPlayerCount);

        //��ư ��Ȱ��ȭ
        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            var text = maxPlayerCountButtons[i].GetComponentInChildren<Text>();
            if (i < limitMaxPlayer - 4)//�ε������̶� -4
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

    //�ִ� �ο� �� ���� �Լ�
    public void UpdateMaxPlayerCount(int count)
    {
        roomData.maxPlayerCount = count;

        //Ŭ���� ��ư�� ���İ� �־��ִ� for��
        for (int i = 0; i < maxPlayerCountButtons.Count; i++)
        {
            //�ν����Ϳ� maxPlayerCountButtons����Ʈ���� 4,5,6,7,8,9,10���� ����
            if (i == count - 4)
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 1f);
            else
                maxPlayerCountButtons[i].image.color = new Color(1f, 1f, 1f, 0f);
        }

        UpdateCrewImages();
    }

    //�� ����� ũ��� �̹��� ������Ʈ�Լ�
    private void UpdateCrewImages()
    {
        int i = 0;

        for (i = 0; i < crewImgs.Count; i++)//ũ��� �⺻���� �ʱ�ȭ
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

        //ũ��� �� �̹����� ǥ��
        for (i = 0; i<crewImgs.Count; i++)
        {
            if (i < roomData.maxPlayerCount) crewImgs[i].gameObject.SetActive(true);
            else crewImgs[i].gameObject.SetActive(false);
        }
    }

    public void CreateRoom()
    {
        //���� �ִ� ��Ʈ��ũ �Ŵ����� �����´�.
        var manager = AmongUsRoomManager.singleton;

        // StartHost : ������ ���� ���ÿ� Ŭ���̾�Ʈ�ν� ���ӿ� �����ϰ� �Ѵ�
        manager.StartHost();
        //�⺻������ ���� ���� Room Manager�� ���� �����ϰ� �����Ѵ�.
    }
}

//������ ���� �� ���� ���
public class CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}
