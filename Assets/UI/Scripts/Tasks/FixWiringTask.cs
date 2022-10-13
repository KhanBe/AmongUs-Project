using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWiringTask : MonoBehaviour
{
    [SerializeField]
    private List<LeftWire> mLeftWires;

    [SerializeField]
    private List<RightWire> mRightWires;

    private LeftWire mSelectedWire;

    void FixedUpdate()
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

                            mSelectedWire = null;
                            return;
                        }
                    }
                }
                mSelectedWire.ResetTarget();
                mSelectedWire = null;
            }
        }

        if (mSelectedWire != null)
        {
            mSelectedWire.SetTarget(Input.mousePosition, 20f);
        }
    }
}
