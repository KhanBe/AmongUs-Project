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

    [SerializeField]
    private ReportButtonUI reportButtonUI;
    public ReportButtonUI ReportButtonUI { get { return reportButtonUI; } }

    [SerializeField]
    private ReportUI reportUI;
    public ReportUI ReportUI { get { return reportUI; } }

    [SerializeField]
    private MeetingUI meetingUI;
    public MeetingUI MeetingUI { get { return meetingUI; } }

    private void Awake()
    {
        Instance = this;
    }   
}
