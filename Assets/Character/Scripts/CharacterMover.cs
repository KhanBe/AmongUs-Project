using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//NetworkBehaviour ���
public class CharacterMover : NetworkBehaviour
{
    public bool isMoveable;
    
    //SyncVar ��Ʈ����Ʈ�� �ٿ� ��Ʈ��ũ�� ����ȭ �����ϰ�
    [SyncVar]
    public float speed = 2f;

    void Start()
    {
        if (hasAuthority)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = 2.5f;
        }
    }
    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //Ŭ���̾�Ʈ�� ������Ʈ�� ���� ������ ������
        if (hasAuthority && isMoveable)
        {
            Vector3 dir = new Vector3(0f, 0f);
            
            if (PlayerSettings.controlType == EControlType.KeyboardMouse)
            {
                //Vector3.ClampMagnitude(Vector3 vector, float maxLength);
                //���� Vector3 ���� (30f, 10f, 3f)�̰� maxLength���� 5�̸� ClampMagnitude�� ����
                //Vector3 ���� (5f, 5f, 3f)�� �ٲ�� �ȴ�.
                dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
            }
            else if (PlayerSettings.controlType == EControlType.Mouse)
            {
                if (Input.GetMouseButton(0))
                {
                    dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;
                }
            }
            if (dir.x < 0f) transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            else if (dir.x > 0f) transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            transform.position += dir * speed * Time.deltaTime;
        }
    }
}
