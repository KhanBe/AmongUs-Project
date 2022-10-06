using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameUIManager : MonoBehaviour
{
    public static IngameUIManager Instance;

    [SerializeField]
    private IngameIntroUI ingameIntroUI;

    public IngameIntroUI IngameIntroUI { get { return ingameIntroUI; } }

    [SerializeField]
    private KillButtonUI killButtonUI;
    public KillButtonUI KillButtonUI { get { return killButtonUI; } }

    [SerializeField]
    private KillUI killUI;

    public KillUI KillUI { get { return killUI; } }

    private void Awake()
    {
        Instance = this;
    }   
}