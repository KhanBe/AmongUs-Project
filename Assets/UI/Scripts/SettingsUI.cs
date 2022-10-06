using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    private Button MouseControlButton;

    [SerializeField]
    private Button KeyboardMouseControlButton;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    //게임오브젝트가 활성화 될 때 호출되는 함수
    private void OnEnable()
    {
        //null error
        //AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = false;

        switch (PlayerSettings.controlType)
        {
            case EControlType.Mouse:
                MouseControlButton.image.color = Color.green;
                KeyboardMouseControlButton.image.color = Color.white;
                break;

            case EControlType.KeyboardMouse:
                MouseControlButton.image.color = Color.white;
                KeyboardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    //매개변수 숫자 값에 따라 플레이어셋팅s의 컨트롤타입을 변경해주는 함수
    public void SetControlMode(int controlType)
    {
        PlayerSettings.controlType = (EControlType)controlType;
        switch (PlayerSettings.controlType)
        {
            case EControlType.Mouse:
                MouseControlButton.image.color = Color.green;
                KeyboardMouseControlButton.image.color = Color.white;
                break;

            case EControlType.KeyboardMouse:
                MouseControlButton.image.color = Color.white;
                KeyboardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    //닫히는 애니 재생후 오브젝트 비활성화
    public virtual void Close()
    {
        //null error
        //AmongUsRoomPlayer.MyRoomPlayer.myCharacter.IsMoveable = true;
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        animator.ResetTrigger("close");//트리거 종료함수
    }
}
