using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

//NetworkBehaviour 상속
public class CharacterMover : NetworkBehaviour
{
    public bool isMoveable;
    
    //SyncVar 어트리뷰트를 붙여 네트워크로 동기화 가능하게
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
        //클라이언트가 오브젝트에 대해 권한이 있으면
        if (hasAuthority && isMoveable)
        {
            Vector3 dir = new Vector3(0f, 0f);
            
            if (PlayerSettings.controlType == EControlType.KeyboardMouse)
            {
                //Vector3.ClampMagnitude(Vector3 vector, float maxLength);
                //만약 Vector3 값이 (30f, 10f, 3f)이고 maxLength값이 5이면 ClampMagnitude로 인해
                //Vector3 값은 (5f, 5f, 3f)로 바뀌게 된다.
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
