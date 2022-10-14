using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EWireColor
{
    None = -1,
    Red,
    Blue,
    Yellow,
    Magenta
}

public class FixWiringTask : MonoBehaviour
{
    [SerializeField]
    private List<LeftWire> mLeftWires;

    [SerializeField]
    private List<RightWire> mRightWires;

    private LeftWire mSelectedWire;

    //활성화시 작동함수
    private void OnEnable()
    {
        List<int> numberPool = new List<int>();

        //왼쪽 선 색상 랜덤 지정
        for (int i = 0; i < 4; i++) numberPool.Add(i);

        int index = 0;
        while (numberPool.Count != 0)
        {
            var number = numberPool[Random.Range(0, numberPool.Count)];

            mLeftWires[index++].SetWireColor((EWireColor)number);
            numberPool.Remove(number);
        }

        //오른쪽 선 색상 랜덤 지정
        for (int i = 0; i < 4; i++) numberPool.Add(i);

        index = 0;
        while (numberPool.Count != 0)
        {
            var number = numberPool[Random.Range(0, numberPool.Count)];

            mRightWires[index++].SetWireColor((EWireColor)number);
            numberPool.Remove(number);
        }
    }

    void Update()
    {
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

        if (Input.GetMouseButtonUp(0))
        {
            if (mSelectedWire != null)
            {
                RaycastHit2D[] hits = Physics2D.RaycastAll(Input.mousePosition, Vector2.right, 1f);

                foreach (var hit in hits)
                {
                    if (hit.collider != null)
                    {
                        var right = hit.collider.GetComponentInParent<RightWire>();

                        if (right != null)
                        {
                            mSelectedWire.SetTarget(hit.transform.position, 50f);
                            mSelectedWire.ConnectWire(right);//left
                            right.ConnectWire(mSelectedWire);//right
                            mSelectedWire = null;
                            return;
                        }
                    }
                }
                mSelectedWire.ResetTarget();
                mSelectedWire.DisconnectWire();
                mSelectedWire = null;
            }
        }

        if (mSelectedWire != null)
        {
            mSelectedWire.SetTarget(Input.mousePosition, 20f);
        }
    }
}
