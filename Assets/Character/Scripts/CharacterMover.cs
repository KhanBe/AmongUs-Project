using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

//NetworkBehaviour 상속
public class CharacterMover : NetworkBehaviour
{
    protected SpriteRenderer spriteRenderer;

    protected Animator animator;

    private bool isMoveable;

    //property
    //customize를 마우스로 클릭시 달리는 애니메이션 때문에
    public bool IsMoveable
    {
        get { return isMoveable; }
        set
        {
            //받아온 값 : value
            // isMoveable에 false가 들어가면
            if (!value)
            {
                animator.SetBool("isMove", false);
            }
            isMoveable = value;
        }
    }

    //SyncVar 어트리뷰트를 붙여 네트워크로 동기화 가능하게
    [SyncVar]
    public float speed = 2f;

    [SerializeField]
    private float characterSize = 0.5f;

    [SerializeField]
    private float cameraSize = 2.5f;

    //hook SyncVar로 동기화된 변수가 서버에서 변경되면 hook으로 등록한 함수를 클라이언트에서 호출하는 기능
    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    //닉네임
    [SyncVar(hook = nameof(SetNickname_Hook))]
    public string nickname;
    [SerializeField]
    protected Text nicknameText;
    public void SetNickname_Hook(string _, string value)
    {
        nicknameText.text = value;
    }

    //virtual : 상속에서 재정의 가능하게
    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));//초기값

        animator = GetComponent<Animator>();

        if (hasAuthority)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = cameraSize;
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
            Vector3 dir = new Vector3(0f, 0f, 0f);
            bool isMove = false;

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
            //Flip
            if (dir.x < 0f) transform.localScale = new Vector3(-characterSize, characterSize, 1f);
            else if (dir.x > 0f) transform.localScale = new Vector3(characterSize, characterSize, 1f);
            //이걸로는 동기화 안됨
            //if (dir.x < 0f) spriteRenderer.flipX = true;
            //else if (dir.x > 0f) spriteRenderer.flipX = false;

            transform.position += dir * speed * Time.deltaTime;

            // dir 이동거리가 0이 아니면 true (magnitude : 크기)
            isMove = dir.magnitude != 0f;
            animator.SetBool("isMove", isMove);
        }

        //localScale을 뒤집으면 닉네임도 뒤집어져서 다시 뒤집어줌
        if (transform.localScale.x < 0)
            nicknameText.transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (transform.localScale.x > 0)
            nicknameText.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
