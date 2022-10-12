using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftWire : MonoBehaviour
{
    [SerializeField]
    private RectTransform mWireBody;

    private LeftWire mSelectedWire;

    [SerializeField]
    private float offset = 15f;

    private Canvas mGameCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        mGameCanvas = FindObjectOfType<Canvas>();
    }


    void FixedUpdate()
    {
        //버튼을 누른순간
        if (Input.GetMouseButtonDown(0))
        {
            //레이캐스트에맞은 콜라이더 부모의 LeftWire를 mSelectWire로 설정
            RaycastHit2D hit = Physics2D.Raycast(Input.mousePosition, Vector2.right, 1f);

            if (hit.collider != null)
            {
                var left = hit.collider.GetComponentInParent<LeftWire>();

                if (left != null) mSelectedWire = left;
            }
        }

        //마우스 떼면 원상복구
        if (Input.GetMouseButtonUp(0))
        {
            if (mSelectedWire != null)
            {
                mWireBody.localRotation = Quaternion.Euler(Vector3.zero);
                mWireBody.sizeDelta = new Vector2(0f, mWireBody.sizeDelta.y);
                mSelectedWire = null;
            }
        }

        if (mSelectedWire != null)
        {
            float angle = Vector2.SignedAngle(transform.position + Vector3.right - transform.position,
                Input.mousePosition - transform.position);

            float distance = (Vector2.Distance(mWireBody.transform.position, Input.mousePosition) - offset);

            mWireBody.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            //게임 해상도에 따라 distance가 달라지므로 캔버스크기의 역수를 가져와 곱한다.
            mWireBody.sizeDelta = new Vector2(distance * (1 / mGameCanvas.transform.localScale.x), mWireBody.sizeDelta.y);
        }
    }
}
