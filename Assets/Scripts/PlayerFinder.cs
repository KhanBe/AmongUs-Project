using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFinder : MonoBehaviour
{
    private CircleCollider2D circleCollider;

    //킬범위에 들어왔을 때 캐릭터 List
    public List<IngameCharacterMover> targets = new List<IngameCharacterMover>();

    //킬범위 설정 함수
    public void SetKillRange(float range)
    {
        circleCollider.radius = range;
    }

    private void Awake()
    {
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();
        if (player && player.playerType == EPlayerType.Crew)
        {
            //Contains(포함)기능을 통해 List 중복여부
            if (!targets.Contains(player))
            {
                targets.Add(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<IngameCharacterMover>();
        if (player && player.playerType == EPlayerType.Crew)
        {
            //Contains(포함)기능을 통해 List 중복여부
            if (targets.Contains(player))
            {
                targets.Remove(player);
            }
        }
    }

    //가장 가까운 (IngameCharacterMover)타켓을 반환하는 함수
    public IngameCharacterMover GetFirstTarget()
    {
        float dist = float.MaxValue;
        IngameCharacterMover closeTarget = null;

        foreach (var target in targets)
        {
            float newDist = Vector3.Distance(transform.position, target.transform.position);
            if (newDist < dist)
            {
                dist = newDist;
                closeTarget = target;
            }
        }

        targets.Remove(closeTarget);
        return closeTarget;
    }
}
