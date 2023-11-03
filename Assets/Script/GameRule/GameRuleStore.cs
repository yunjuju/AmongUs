using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Text;

//ų ����
public enum EKillRange
{
    Short, Normal, Long
}

//����ũ�� ������Ʈ �ֱ�
public enum ETaskBarUpdates
{
    Always, Meetings, Never
}

//����ü 
public struct GameRuleData
{
    public bool confirmEjects;
    public int emergencyMeetings;
    public int emergencyMeetingsCooldown;
    public int meetingsTime;
    public int voteTime;
    public bool anonymousvotes;
    public float moveSpeed;
    public float crewSight;
    public float imposterSight;
    public float killCooldown;
    public EKillRange killRange;
    public bool visualTasks;
    public ETaskBarUpdates taskBarupdates;
    public int commonTask;
    public int complextTask;
    public int simpleTask;

}

//���� ��Ģ���� �������ְ� �����鼭 �ٸ� Ŭ���̾�Ʈ���� �˷���
public class GameRuleStore : NetworkBehaviour
{

    //�� �������� ����Ǹ� Ŭ���̾�Ʈ���� UI�� ����Ǿ�� ��
    //���� ��Ģ �������� ���� �Ǿ��� �� Ŭ���̾�Ʈ���� UI�� ����ǰ� �ϱ� ���ؼ� hook �Լ��� ����� ���

    //�̶� bool Ÿ���� ��� ������ Ŭ���̾�Ʈ Customize UI���� ������ ���� ���̱� ������ ��Ģ ���丸 ������Ʈ ��Ŵ
    // text�� ǥ���Ǵ� ������ �� Text UI�� ������Ʈ �� ���� �� ������ �Լ��� ȣ���ϵ��� ����


    //���� ����� �� Ŭ���̾�Ʈ �鿡�� �����ǰ� ����

    [SyncVar(hook = nameof(SetIsRecommendRule_Hook))]
    private bool isRecommedRule;
    [SerializeField]
    private Toggle isRecommendRuleToggle;

    public void SetIsRecommendRule_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }

    public void OnRecommendToggle(bool value)
    {
        isRecommedRule = value;
        if (isRecommedRule)
        {
            //�ٸ� ��� ��Ģ�� ������ �� isRecommendRule�� false�� �����ϰ� ����� üũ�� �����ϵ��� �ؾ���
            //���� ��Ģ�� ��õ �������� �������
            SetRecommendGameRule();
        }
    }


    [SyncVar(hook = nameof(SetConfirmEjects_Hook))]
    private bool confirmEjects;
    [SerializeField]
    private Toggle confirmEjectsToggle;

    public void SetConfirmEjects_Hook(bool _, bool value)
    {
        UpdateGameRuleOverview();
    }

    public void OnConfirmEjectsToggle(bool value)
    {
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
        confirmEjects = value;

        if (!isRecommedRule)
        {
            SetRecommendGameRule();
        }
    }

    //��� ȸ��
    [SyncVar(hook = nameof(SetEmergencyMeetings_Hook))]
    private int emergencyMeetings;
    [SerializeField]
    private Text emergencyMeetingsText;
    public void SetEmergencyMeetings_Hook(int _, int value)
    {
        emergencyMeetingsText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    // -, +��ư���� �� �����ϴ� ��Ģ���� �Ű������� �޴� bool ������ +��ư����  -��ư���� ���� ��,
    //�� ���� ���ؾ� �ϴ� �ܰ�� �ּҰ��� �ִ밪�� ���缭 ���� �����ϵ��� �������
    public void OnChangeEmergencyMeetings(bool isPlus)
    {
        //���� 1�� 0 ~ 9����
        emergencyMeetings = Mathf.Clamp(emergencyMeetings + (isPlus ? 1 : -1), 0, 9);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //��� ȸ�� ��Ÿ��
    [SyncVar(hook = nameof(emergencyMeetingCooldown_Hook))]
    private int emergencyMeetingsCooldown;
    [SerializeField]
    private Text emergencyMeetingsCooldownText;

    public void emergencyMeetingCooldown_Hook(int _, int value)
    {
        emergencyMeetingsCooldownText.text = string.Format("{0}s", emergencyMeetingsCooldown);
        UpdateGameRuleOverview();
    }

    public void OnChangeEmergencyMeetingCooldown(bool isPlus)
    {
        //���� 5�� 0 ~ 60����
        emergencyMeetingsCooldown = Mathf.Clamp(emergencyMeetingsCooldown + (isPlus ? 5 : -5), 0, 60);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //ȸ�� ���� �ð�
    [SyncVar(hook = nameof(SetMeetingsTime_Hook))]
    private int meetingsTime;
    [SerializeField]
    private Text meetingsTimeText;

    public void SetMeetingsTime_Hook(int _, int value)
    {
        meetingsTimeText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }

    public void OnChangeMeetingsTime(bool isPlus)
    {
        //���� 5�� 0 ~ 120����
        meetingsTime = Mathf.Clamp(meetingsTime + (isPlus ? 5 : -5), 0, 120);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //��ǥ ���� �ð�
    [SyncVar(hook = nameof(SetvoteTime_Hook))]
    private int voteTime;
    [SerializeField]
    private Text voteTimeText;

    public void SetvoteTime_Hook(int _, int value)
    {
        voteTimeText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }

    public void OnChangeVoteTime(bool isPlus)
    {
        //���� 5�� 0 ~ 300����
        voteTime = Mathf.Clamp(voteTime + (isPlus ? 5 : -5), 0, 300);
        isRecommedRule = false;
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

    public void OnAnonymousVotesToggle(bool value)
    {
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
        anonymousVotes = value;
    }


    //�̵� �ӵ�
    [SyncVar(hook = nameof(SetmoveSpeed_Hook))]
    private float moveSpeed;
    [SerializeField]
    private Text moveSpeedText;

    public void SetmoveSpeed_Hook(float _, float value)
    {
        moveSpeedText.text = string.Format("{0:0.0}x", value);
        UpdateGameRuleOverview();
    }

    public void OnChangeMoveSpeed(bool isPlus)
    {
        //���� 0.25�� 0.5 ~ 3.0����
        moveSpeed = Mathf.Clamp(moveSpeed + (isPlus ? 0.25f : -0.25f), 0.5f, 3f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //ũ��� �þ�
    [SyncVar(hook = nameof(SetCrewSight_Hook))]
    private float crewSight;
    [SerializeField]
    private Text crewSightText;

    public void SetCrewSight_Hook(float _, float value)
    {
        crewSightText.text = string.Format("{0:0.0}x", value);
        UpdateGameRuleOverview();
    }

    public void OnChangCrewSight(bool isPlus)
    {
        //���� 0.25�� 0.25 ~ 5.0����
        crewSight = Mathf.Clamp(crewSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //�������� �þ�
    [SyncVar(hook = nameof(SetImposterSight_Hook))]
    private float imposterSight;
    [SerializeField]
    private Text imposterSightText;

    public void SetImposterSight_Hook(float _, float value)
    {
        imposterSightText.text = string.Format("{0:0.0}x", value);
        UpdateGameRuleOverview();
    }

    public void OnChangImposterSight(bool isPlus)
    {
        //���� 0.25�� 0.25 ~ 5.0����
        imposterSight = Mathf.Clamp(imposterSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //ų ��Ÿ��
    [SyncVar(hook = nameof(SetKillCooldown_Hook))]
    private float killCooldown;
    [SerializeField]
    private Text killCooldownText;

    public void SetKillCooldown_Hook(float _, float value)
    {
        killCooldownText.text = string.Format("{0:0.0}s", value);
        UpdateGameRuleOverview();
    }

    public void OnChangKillCooldown(bool isPlus)
    {
        //���� 2.5�� 10 ~ 60����
        killCooldown = Mathf.Clamp(killCooldown + (isPlus ? 2.5f : -2.5f), 10f, 60f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //ų ����
    [SyncVar(hook = nameof(SetKillRange_Hook))]
    private EKillRange killRange;
    [SerializeField]
    private Text killRangeText;

    public void SetKillRange_Hook(EKillRange _, EKillRange value)
    {
        killRangeText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangKillRange(bool isPlus)
    {
        //���� 1�� 0 ~ 2.0����
        killRange = (EKillRange)Mathf.Clamp((int)killRange + (isPlus ? 1 : -1), 0, 2);
        isRecommedRule = false;
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

    public void OnVisualTasksToggle(bool value)
    {
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
        visualTasks = value;
    }



    [SyncVar(hook = nameof(SetTaskBarUpdates_Hook))]
    private ETaskBarUpdates taskBarupdates;
    [SerializeField]
    private Text taskBarupdatesText;

    public void SetTaskBarUpdates_Hook(ETaskBarUpdates _, ETaskBarUpdates value)
    {
        taskBarupdatesText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangTaskBarUpdates(bool isPlus)
    {
        //���� 1�� 0 ~ 2.0����
        taskBarupdates = (ETaskBarUpdates)Mathf.Clamp((int)taskBarupdates + (isPlus ? 1 : -1), 0, 2);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //���� �ӹ�
    [SyncVar(hook = nameof(SetCommonTask_Hook))]
    private int commonTask;
    [SerializeField]
    private Text commonTaskText;

    public void SetCommonTask_Hook(int _, int value)
    {
        commonTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangCommonTask(bool isPlus)
    {
        //���� 1�� 0 ~ 2.0����
        commonTask = Mathf.Clamp(commonTask + (isPlus ? 1 : -1), 0, 2);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //������ �ӹ�
    [SyncVar(hook = nameof(SetComplexTask_Hook))]
    private int complexTask;
    [SerializeField]
    private Text complexTaskText;

    public void SetComplexTask_Hook(int _, int value)
    {
        complexTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangComplexTask(bool isPlus)
    {
        //���� 1�� 0 ~ 3.0����
        complexTask = Mathf.Clamp(complexTask + (isPlus ? 1 : -1), 0, 3);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //������ �ӹ�
    [SyncVar(hook = nameof(SetSimpleTask_Hook))]
    private int simpleTask;
    [SerializeField]
    private Text simpleTaskText;

    public void SetSimpleTask_Hook(int _, int value)
    {
        simpleTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    public void OnChangSimpleTask(bool isPlus)
    {
        //���� 1�� 0 ~ 5.0����
        simpleTask = Mathf.Clamp(simpleTask + (isPlus ? 1 : -1), 0, 5);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }
    //�߰�
    [SyncVar(hook = nameof(SetImposterCount_Hook))]
    private int imposterCount;

    public void SetImposterCount_Hook(int _, int value)
    {
        UpdateGameRuleOverview();
    }


    //��Ģ ����ؼ� �����ִ� ����
    [SerializeField]
    private Text gameRuleOverview;

    //���� ��Ģ �������� ������ �� ���� ��Ģ�� ����� UI ������Ʈ ���� �Լ� ����
    public void UpdateGameRuleOverview()
    {
        //���� ��Ģ�� ������� ���ڿ��� ����� gameRuleOverview Text�� �־��ִ� �۾�
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        StringBuilder sb = new StringBuilder(isRecommedRule ? "��õ���� \n" : "Ŀ���� ����\n");
        sb.Append("�� : The Skeld\n");
        sb.Append($"#�������� : {imposterCount}\n");
        sb.Append(string.Format("Confirm Ejects : {0} \n", confirmEjects ? "����" : "����"));
        sb.Append($"���ȸ�� : {emergencyMeetings}\n");
        sb.Append(string.Format("Anonymous Votes : {0} \n", anonymousVotes ? "����" : "����"));
        sb.Append($"��� ȸ�� ��Ÿ�� : {emergencyMeetingsCooldown}\n");
        sb.Append($"ȸ�� ���� �ð� : {meetingsTime}\n");
        sb.Append($"��ǥ ���� �ð� : {voteTime}\n");
        sb.Append($"�̵� �ӵ� : {moveSpeed}\n");
        sb.Append($"ũ��� �þ� : {crewSight}\n");
        sb.Append($"�������� �þ� : {imposterSight}\n");
        sb.Append($"ų ��Ÿ�� : {killCooldown}\n");
        sb.Append($"ų ���� : {killRange}\n");
        sb.Append($"Task Bar Updates : {taskBarupdates}\n");
        sb.Append(string.Format("Visual Tasks : {0}\n", visualTasks ? "����" : "����"));
        sb.Append($"���� �ӹ� : {commonTask}\n");
        sb.Append($"������ �ӹ� : {complexTask}\n");
        sb.Append($"������ �ӹ� : {simpleTask}\n");
        gameRuleOverview.text = sb.ToString();
    }

    //��� ������ ��õ �������� ������
    private void SetRecommendGameRule()
    {
        isRecommedRule = true;
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

    void Start()
    {
        
        
        if (isServer)
        {

            //�ʱ�ȭ ���� �ʴ� ��Ģ �������� �ʱ�ȭ �ϵ��� �������
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            imposterCount = manager.imposterCount;
            anonymousVotes = false;
            taskBarupdates = ETaskBarUpdates.Always;

            //������ �� SetRecommendGameRule �Լ��� ȣ���ϵ��� �������
            SetRecommendGameRule();
        }

    }

    public GameRuleData GetGameRuleData()
    {
        //���� �� ������ ����ü�� �����ϰ� �� ���� �������� ���� �� ���� ������ �ִ� 
        //���� ��Ģ ���� ������ ä���� ��ȯ
        return new GameRuleData()
        {
            anonymousvotes = anonymousVotes,
            commonTask = commonTask,
            complextTask = commonTask,
            confirmEjects = confirmEjects,
            crewSight = crewSight,
            emergencyMeetings = emergencyMeetings,
            emergencyMeetingsCooldown = emergencyMeetingsCooldown,
            imposterSight = imposterSight,
            killCooldown = killCooldown,
            killRange = killRange,
            meetingsTime = meetingsTime,
            moveSpeed = moveSpeed,
            simpleTask = simpleTask,
            taskBarupdates = taskBarupdates,
            visualTasks = visualTasks,
            voteTime = voteTime,
        };
    }
}