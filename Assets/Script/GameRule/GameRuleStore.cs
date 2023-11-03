using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System.Text;

//킬 범위
public enum EKillRange
{
    Short, Normal, Long
}

//데스크바 업데이트 주기
public enum ETaskBarUpdates
{
    Always, Meetings, Never
}

//구조체 
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

//게임 규칙들을 저장해주고 있으면서 다른 클라이언트에게 알려줌
public class GameRuleStore : NetworkBehaviour
{

    //각 변수들이 변경되면 클라이언트에서 UI가 변경되어야 함
    //게임 규칙 변수들이 변경 되었을 때 클라이언트에서 UI가 변경되게 하기 위해선 hook 함수를 만들어 등록

    //이때 bool 타입의 토글 변수는 클라이언트 Customize UI에서 보이지 않을 것이기 때문에 규칙 개요만 업데이트 시킴
    // text로 표현되는 변수는 각 Text UI를 업데이트 한 다음 룰 오버뷰 함수를 호출하도록 만듦


    //값이 변경될 때 클라이언트 들에게 통지되게 만듦

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
            //다른 모든 규칙은 수정될 때 isRecommendRule을 false로 변경하고 토글의 체크를 해제하도록 해야함
            //게임 규칙을 추천 설정으로 만들어줌
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

    //긴급 회의
    [SyncVar(hook = nameof(SetEmergencyMeetings_Hook))]
    private int emergencyMeetings;
    [SerializeField]
    private Text emergencyMeetingsText;
    public void SetEmergencyMeetings_Hook(int _, int value)
    {
        emergencyMeetingsText.text = value.ToString();
        UpdateGameRuleOverview();
    }

    // -, +버튼으로 값 조절하는 규칙들은 매개변수로 받는 bool 값으로 +버튼인지  -버튼인지 판정 후,
    //각 값이 변해야 하는 단계와 최소값과 최대값에 맞춰서 값을 변경하도록 만들어줌
    public void OnChangeEmergencyMeetings(bool isPlus)
    {
        //단위 1로 0 ~ 9까지
        emergencyMeetings = Mathf.Clamp(emergencyMeetings + (isPlus ? 1 : -1), 0, 9);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //긴급 회의 쿨타임
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
        //단위 5로 0 ~ 60까지
        emergencyMeetingsCooldown = Mathf.Clamp(emergencyMeetingsCooldown + (isPlus ? 5 : -5), 0, 60);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //회의 제한 시간
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
        //단위 5로 0 ~ 120까지
        meetingsTime = Mathf.Clamp(meetingsTime + (isPlus ? 5 : -5), 0, 120);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //투표 제한 시간
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
        //단위 5로 0 ~ 300까지
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


    //이동 속도
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
        //단위 0.25로 0.5 ~ 3.0까지
        moveSpeed = Mathf.Clamp(moveSpeed + (isPlus ? 0.25f : -0.25f), 0.5f, 3f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //크루원 시야
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
        //단위 0.25로 0.25 ~ 5.0까지
        crewSight = Mathf.Clamp(crewSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //임포스터 시야
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
        //단위 0.25로 0.25 ~ 5.0까지
        imposterSight = Mathf.Clamp(imposterSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //킬 쿨타임
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
        //단위 2.5로 10 ~ 60까지
        killCooldown = Mathf.Clamp(killCooldown + (isPlus ? 2.5f : -2.5f), 10f, 60f);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }


    //킬 범위
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
        //단위 1로 0 ~ 2.0까지
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
        //단위 1로 0 ~ 2.0까지
        taskBarupdates = (ETaskBarUpdates)Mathf.Clamp((int)taskBarupdates + (isPlus ? 1 : -1), 0, 2);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //공통 임무
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
        //단위 1로 0 ~ 2.0까지
        commonTask = Mathf.Clamp(commonTask + (isPlus ? 1 : -1), 0, 2);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //복잡한 임무
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
        //단위 1로 0 ~ 3.0까지
        complexTask = Mathf.Clamp(complexTask + (isPlus ? 1 : -1), 0, 3);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }



    //간단한 임무
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
        //단위 1로 0 ~ 5.0까지
        simpleTask = Mathf.Clamp(simpleTask + (isPlus ? 1 : -1), 0, 5);
        isRecommedRule = false;
        isRecommendRuleToggle.isOn = false;
    }
    //추가
    [SyncVar(hook = nameof(SetImposterCount_Hook))]
    private int imposterCount;

    public void SetImposterCount_Hook(int _, int value)
    {
        UpdateGameRuleOverview();
    }


    //규칙 요약해서 보여주는 변수
    [SerializeField]
    private Text gameRuleOverview;

    //게임 규칙 변수들이 수정될 때 게임 규칙을 요약한 UI 업데이트 해줄 함수 구현
    public void UpdateGameRuleOverview()
    {
        //게임 규칙을 기반으로 문자열을 만들고 gameRuleOverview Text에 넣어주는 작업
        var manager = NetworkManager.singleton as AmongUsRoomManager;
        StringBuilder sb = new StringBuilder(isRecommedRule ? "추천설정 \n" : "커스텀 설정\n");
        sb.Append("맵 : The Skeld\n");
        sb.Append($"#임포스터 : {imposterCount}\n");
        sb.Append(string.Format("Confirm Ejects : {0} \n", confirmEjects ? "켜짐" : "꺼짐"));
        sb.Append($"긴급회의 : {emergencyMeetings}\n");
        sb.Append(string.Format("Anonymous Votes : {0} \n", anonymousVotes ? "켜짐" : "꺼짐"));
        sb.Append($"긴급 회의 쿨타임 : {emergencyMeetingsCooldown}\n");
        sb.Append($"회의 제한 시간 : {meetingsTime}\n");
        sb.Append($"투표 제한 시간 : {voteTime}\n");
        sb.Append($"이동 속도 : {moveSpeed}\n");
        sb.Append($"크루원 시야 : {crewSight}\n");
        sb.Append($"임포스터 시야 : {imposterSight}\n");
        sb.Append($"킬 쿨타임 : {killCooldown}\n");
        sb.Append($"킬 범위 : {killRange}\n");
        sb.Append($"Task Bar Updates : {taskBarupdates}\n");
        sb.Append(string.Format("Visual Tasks : {0}\n", visualTasks ? "켜짐" : "꺼짐"));
        sb.Append($"공통 임무 : {commonTask}\n");
        sb.Append($"복잡한 임무 : {complexTask}\n");
        sb.Append($"간단한 임무 : {simpleTask}\n");
        gameRuleOverview.text = sb.ToString();
    }

    //모든 세팅을 추천 설정으로 맞춰줌
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

            //초기화 되지 않는 규칙 변수들을 초기화 하도록 만들어줌
            var manager = NetworkManager.singleton as AmongUsRoomManager;
            imposterCount = manager.imposterCount;
            anonymousVotes = false;
            taskBarupdates = ETaskBarUpdates.Always;

            //서버일 때 SetRecommendGameRule 함수를 호출하도록 만들어줌
            SetRecommendGameRule();
        }

    }

    public GameRuleData GetGameRuleData()
    {
        //게임 룰 데이터 구조체를 생성하고 그 안의 변수들을 게임 룰 스토어가 가지고 있는 
        //게임 규칙 변수 값으로 채워서 반환
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