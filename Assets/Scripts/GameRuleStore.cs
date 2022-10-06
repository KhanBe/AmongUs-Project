using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Text;

public enum EKillRange
{
    Short, Normal, Long
}

public enum ETaskBarUpdates
{
    Always, Meetings, Never
}

//����ü
public struct GameRuleData
{
    public bool confirmEjects;
    public int emergencyMeetings;
    public int emergencyMeeingsCooldown;
    public int meetingsTime;
    public int voteTime;
    public bool anonymousVotes;
    public float moveSpeed;
    public float crewSight;
    public float imposterSight;
    public EKillRange killRange;
    public float killCooldown;
    public bool visualTasks;
    public ETaskBarUpdates taskBarUpdates;
    public int commonTask;
    public int complexTask;
    public int simpleTask;
}

public class GameRuleStore : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetIsRecommendRule_Hook))]
    private bool isRecommendRule;
    [SerializeField]
    private Toggle isRecommendRuleToggle;
    public void SetIsRecommendRule_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnRecommendToggle(bool value)
    {
        isRecommendRule = value;
        if (isRecommendRule) SetRecommendGameRule();
    }

    [SyncVar(hook = nameof(SetConfirmEjects_Hook))]
    private bool confirmEjects;
    [SerializeField]
    private Toggle confirmEjectsToggle;
    public void SetConfirmEjects_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnConfirmEjectsToggle(bool value)
    {
        isRecommendRule = false;
        //��� üũ ����
        isRecommendRuleToggle.isOn = false;
        confirmEjects = value;
    }

    [SyncVar(hook = nameof(SetEmergencyMeetings_Hook))]
    private int emergencyMeetings;
    [SerializeField]
    private Text emergencyMeetingsText;
    public void SetEmergencyMeetings_Hook(int _, int value)
    {
        emergencyMeetingsText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeEmergencyMeetings(bool isPlus)
    {
        emergencyMeetings = Mathf.Clamp(emergencyMeetings + (isPlus ? 1 : -1), 0, 9);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetEmergencyMeetingCooldown_Hook))]
    private int emergencyMeetingsCooldown;
    [SerializeField]
    private Text emergencyMeetingsCooldownText;
    public void SetEmergencyMeetingCooldown_Hook(int _, int value)
    {
        emergencyMeetingsCooldownText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeEmergencyMeetingCooldown(bool isPlus)
    {
        emergencyMeetingsCooldown = Mathf.Clamp(emergencyMeetingsCooldown + (isPlus ? 5 : -5), 0, 60);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetMeeingsTime_Hook))]
    private int meetingsTime;
    [SerializeField]
    private Text meetingsTimeText;
    public void SetMeeingsTime_Hook(int _, int value)
    {
        meetingsTimeText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeMeetingsTime(bool isPlus)
    {
        meetingsTime = Mathf.Clamp(meetingsTime + (isPlus ? 5 : -5), 0, 120);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetVoteTime_Hook))]
    private int voteTime;
    [SerializeField]
    private Text voteTimeText;
    public void SetVoteTime_Hook(int _, int value)
    {
        voteTimeText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeVoteTime(bool isPlus)
    {
        voteTime = Mathf.Clamp(voteTime + (isPlus ? 5 : -5), 0, 300);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetAnonymousVotes_Hook))]
    private bool anonymousVotes;
    [SerializeField]
    private Toggle anonymousVotesToggle;
    public void SetAnonymousVotes_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnAnonymousVotesToggle(bool value)
    {
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
        anonymousVotes = value;
    }

    [SyncVar(hook = nameof(SetMoveSpeed_Hook))]
    private float moveSpeed;
    [SerializeField]
    private Text moveSpeedText;
    public void SetMoveSpeed_Hook(float _, float value)
    {
        moveSpeedText.text = string.Format("{0:0.0}x", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeMoveSpeed(bool isPlus)
    {
        moveSpeed = Mathf.Clamp(moveSpeed + (isPlus ? 0.25f : -0.25f), 0.5f, 3f);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetCrewSight_Hook))]
    private float crewSight;
    [SerializeField]
    private Text crewSightText;
    public void SetCrewSight_Hook(float _, float value)
    {
        crewSightText.text = string.Format("{0:0.0}x", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeCrewSight(bool isPlus)
    {
        crewSight = Mathf.Clamp(crewSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof (SetImposterSight_Hook))]
    private float imposterSight;
    [SerializeField]
    private Text imposterSightText;
    public void SetImposterSight_Hook(float _, float value)
    {
        imposterSightText.text = string.Format("{0:0.0}x", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeImposterSight(bool isPlus)
    {
        imposterSight = Mathf.Clamp(imposterSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetKillCooldown_Hook))]
    private float killCooldown;
    [SerializeField]
    private Text killCooldownText;
    public void SetKillCooldown_Hook(float _, float value)
    {
        killCooldownText.text = string.Format("{0:0.0}", value);
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeKillCooldown(bool isPlus)
    {
        killCooldown = Mathf.Clamp(killCooldown + (isPlus ? 2.5f : -2.5f), 10f, 60f);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetKillRange_Hook))]
    private EKillRange killRange;
    [SerializeField]
    private Text killRangeText;
    public void SetKillRange_Hook(EKillRange _, EKillRange value)
    {
        killRangeText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeKillRange(bool isPlus)
    {
        killRange = (EKillRange)Mathf.Clamp((int)killRange + (isPlus ? 1 : -1), 0, 2);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }
    [SyncVar(hook = nameof(SetVisualTasks_Hook))]
    private bool visualTasks;
    [SerializeField]
    private Toggle visualTasksToggle;
    public void SetVisualTasks_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnVisualTasksToggle(bool value)
    {
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
        visualTasks = value;
    }

    [SyncVar(hook = nameof(SetTaskBarUpdates_Hook))]
    private ETaskBarUpdates taskBarUpdates;
    [SerializeField]
    private Text taskBarUpdatesText;
    public void SetTaskBarUpdates_Hook(ETaskBarUpdates _, ETaskBarUpdates value)
    {
        taskBarUpdatesText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeTaskBarUpdates(bool isPlus)
    {
        taskBarUpdates = (ETaskBarUpdates)Mathf.Clamp((int)taskBarUpdates + (isPlus ? 1 : -1), 0, 2);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetCommonTask_Hook))]
    private int commonTask;
    [SerializeField]
    private Text commonTaskText;
    public void SetCommonTask_Hook(int _, int value)
    {
        commonTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeCommonTask(bool isPlus)
    {
        commonTask = Mathf.Clamp(commonTask + (isPlus ? 1 : -1), 0, 2);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetComplexTask_Hook))]
    private int complexTask;
    [SerializeField]
    private Text complexTaskText;
    public void SetComplexTask_Hook(int _, int value)
    {
        complexTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeComplexTask(bool isPlus)
    {
        complexTask = Mathf.Clamp(complexTask + (isPlus ? 1 : -1), 0, 3);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetSimpleTask_Hook))]
    private int simpleTask;
    [SerializeField]
    private Text simpleTaskText;
    public void SetSimpleTask_Hook(int _, int value)
    {
        simpleTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //�� UI�� ���� ��ȣ�ۿ� �ݹ� �Լ�
    public void OnChangeSimpleTask(bool isPlus)
    {
        simpleTask = Mathf.Clamp(simpleTask + (isPlus ? 1 : -1), 0, 5);
        isRecommendRule = false;
        isRecommendRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetImposterCount_Hook))]
    private int imposterCount;
    public void SetImposterCount_Hook(int _, int value)
    {
        UpdateGameRuleOverview();
    }

    [SerializeField]//���� ��Ģ ��� UI ����
    private Text gameRuleOverview;

    [SerializeField]//��Ģ�������� ������ �� gameRuleOverview�� ������Ʈ���ִ� �Լ�
    public void UpdateGameRuleOverview()
    {
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        StringBuilder sb = new StringBuilder(isRecommendRule ? "��õ ����\n" : "Ŀ���� ����\n");
        sb.Append("�� : The Skeld\n");
        sb.Append($"#�������� : {imposterCount}\n");
        sb.Append(string.Format("Confirm Ejects : {0}\n", confirmEjects ? "����" : "����"));
        sb.Append($"��� ȸ�� : {emergencyMeetings}\n");
        sb.Append(string.Format("Anonymous Votes : {0}\n", anonymousVotes ? "����" : "����"));
        sb.Append($"��� ȸ�� ��Ÿ�� : {emergencyMeetingsCooldown}\n");
        sb.Append($"ȸ�� ���� �ð� : {meetingsTime}\n");
        sb.Append($"��ǥ ���� �ð� : {voteTime}\n");
        sb.Append($"�̵� �ӵ� : {moveSpeed}\n");
        sb.Append($"ũ��� �þ� : {crewSight}\n");
        sb.Append($"�������� �þ� : {imposterSight}\n");
        sb.Append($"ų ��Ÿ�� : {killCooldown}\n");
        sb.Append($"ų ���� : {killRange}\n");
        sb.Append(string.Format("Visual Tasks : {0}\n", visualTasks ? "����" : "����"));
        sb.Append($"���� �ӹ� : {commonTask}\n");
        sb.Append($"������ �ӹ� : {complexTask}\n");
        sb.Append($"������ �ӹ� : {simpleTask}\n");
        gameRuleOverview.text = sb.ToString();
    }

    private void SetRecommendGameRule()
    {
        isRecommendRule = true;
        confirmEjects = true;
        emergencyMeetings = 1;
        emergencyMeetingsCooldown = 15;
        meetingsTime = 15;
        voteTime = 120;
        moveSpeed = 1f;
        crewSight = 1f;
        imposterSight = 1.5f;
        killCooldown = 45f;
        killRange = EKillRange.Normal;
        visualTasks = true;
        commonTask = 1;
        complexTask = 1;
        simpleTask = 2;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            //�ʱⰪ
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            imposterCount = manager.imposterCount;
            anonymousVotes = false;
            taskBarUpdates = ETaskBarUpdates.Always;

            SetRecommendGameRule();
        }
    }

    //����ü ��ȯ
    public GameRuleData GetGameRuleData()
    {
        return new GameRuleData()
        {
            anonymousVotes = anonymousVotes,
            confirmEjects = confirmEjects,
            crewSight = crewSight,
            imposterSight = imposterSight,
            emergencyMeetings = emergencyMeetings,
            emergencyMeeingsCooldown = emergencyMeetingsCooldown,
            killRange = killRange,
            killCooldown = killCooldown,
            moveSpeed = moveSpeed,
            taskBarUpdates = taskBarUpdates,
            commonTask = commonTask,
            complexTask = complexTask,
            simpleTask = simpleTask,
            visualTasks = visualTasks,
            meetingsTime = meetingsTime,
            voteTime = voteTime
        };
    }
}