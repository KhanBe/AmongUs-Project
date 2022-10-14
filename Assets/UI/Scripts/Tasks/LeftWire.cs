using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftWire : MonoBehaviour
{
    public EWireColor WireColor { get; private set; }

    public bool IsConnected { get; private set; }

    [SerializeField]
    private List<Image> mWireImages;

    [SerializeField]//불빛 들어올 이미지
    private Image mLightImage;

    [SerializeField]//연결된 오른쪽 선 변수
    private RightWire mConnectedWire;

    [SerializeField]
    private RectTransform mWireBody;

    private Canvas mGameCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        mGameCanvas = FindObjectOfType<Canvas>();
    }

    public void SetTarget(Vector3 targetPosition, float offset)
    {
        float angle = Vector2.SignedAngle(transform.position + Vector3.right - transform.position,
                targetPosition - transform.position);

        float distance = (Vector2.Distance(mWireBody.transform.position, targetPosition) - offset);

        mWireBody.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        //게임 해상도에 따라 distance가 달라지므로 캔버스크기의 역수를 가져와 곱한다.
        mWireBody.sizeDelta = new Vector2(distance * (1 / mGameCanvas.transform.localScale.x), mWireBody.sizeDelta.y);
    }

    public void ResetTarget()
    {
        mWireBody.localRotation = Quaternion.Euler(Vector3.zero);
        mWireBody.sizeDelta = new Vector2(0f, mWireBody.sizeDelta.y);
    }

    public void SetWireColor(EWireColor wireColor)
    {
        WireColor = wireColor;
        Color color = Color.black;

        switch (WireColor)
        {
            case EWireColor.Red:
                color = Color.red;
                break;

            case EWireColor.Blue:
                color = Color.blue;
                break;

            case EWireColor.Yellow:
                color = Color.yellow;
                break;

            case EWireColor.Magenta:
                color = Color.magenta;
                break;
        }

        foreach (var image in mWireImages)
        {
            image.color = color;
        }
    }

    public void ConnectWire(RightWire rightWire)
    {
        //연결된 오른쪽와이어가 있고 && 연결된 오른쪽와이어가 다른오른쪽 와이어에 연결하면
        if (mConnectedWire != null && mConnectedWire != rightWire)
        {
            mConnectedWire.DisconnectWire(this);
            mConnectedWire = null;
        }

        mConnectedWire = rightWire;

        if (mConnectedWire.WireColor == WireColor)
        {
            mLightImage.color = Color.yellow;
            IsConnected = true;
        }
    }

    public void DisconnectWire()
    {
        if (mConnectedWire != null)
        {
            mConnectedWire.DisconnectWire(this);
            mConnectedWire = null;
        }

        mLightImage.color = Color.gray;
        IsConnected = false;
    }
}
