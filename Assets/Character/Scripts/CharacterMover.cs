using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

//NetworkBehaviour ���
public class CharacterMover : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Animator animator;

    private bool isMoveable;

    //property
    //customize�� ���콺�� Ŭ���� �޸��� �ִϸ��̼� ������
    public bool IsMoveable
    {
        get { return isMoveable; }
        set
        {
            //�޾ƿ� �� : value
            // isMoveable�� false�� ����
            if (!value)
            {
                animator.SetBool("isMove", false);
            }
            isMoveable = value;
        }
    }

    //SyncVar ��Ʈ����Ʈ�� �ٿ� ��Ʈ��ũ�� ����ȭ �����ϰ�
    [SyncVar]
    public float speed = 2f;

    //hook SyncVar�� ����ȭ�� ������ �������� ����Ǹ� hook���� ����� �Լ��� Ŭ���̾�Ʈ���� ȣ���ϴ� ���
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

    //�г���
    [SyncVar(hook = nameof(SetNickname_Hook))]
    public string nickname;
    [SerializeField]
    private Text nicknameText;
    public void SetNickname_Hook(string _, string value)
    {
        nicknameText.text = value;
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));//�ʱⰪ

        animator = GetComponent<Animator>();

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
            Vector3 dir = new Vector3(0f, 0f, 0f);
            bool isMove = false;

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
            //Flip
            if (dir.x < 0f) transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            else if (dir.x > 0f) transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            //�̰ɷδ� ����ȭ �ȵ�
            //if (dir.x < 0f) spriteRenderer.flipX = true;
            //else if (dir.x > 0f) spriteRenderer.flipX = false;

            transform.position += dir * speed * Time.deltaTime;

            // dir �̵��Ÿ��� 0�� �ƴϸ� true (magnitude : ũ��)
            isMove = dir.magnitude != 0f;
            animator.SetBool("isMove", isMove);
        }

        //localScale�� �������� �г��ӵ� ���������� �ٽ� ��������
        if (transform.localScale.x < 0)
            nicknameText.transform.localScale = new Vector3(-1f, 1f, 1f);
        else if (transform.localScale.x > 0)
            nicknameText.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
