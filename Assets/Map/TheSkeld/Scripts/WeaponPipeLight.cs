using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//무기실 파이프 애니메이션 스크립트
public class WeaponPipeLight : MonoBehaviour
{
    private Animator animator;

    private WaitForSeconds wait = new WaitForSeconds(0.15f);

    private List<WeaponPipeLight> lights = new List<WeaponPipeLight>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //자식 오브젝트 중 WeaponPipeLight컴포넌트를 가진 오브젝트를 찾아 List에 저장
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<WeaponPipeLight>();
            if (child) lights.Add(child);
        }
    }

    public void TurnOnLight()
    {
        animator.SetTrigger("on");
        StartCoroutine(TurnOnLightAtChild());
    }

    private IEnumerator TurnOnLightAtChild()
    {
        yield return wait;

        foreach (var child in lights) child.TurnOnLight();
    }
}