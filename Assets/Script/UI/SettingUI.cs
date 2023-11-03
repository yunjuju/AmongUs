using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{

    [SerializeField]
    private Button MouseControlButton;
    [SerializeField]
    private Button KeyboardMouseControlButton;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable() // 게임오브젝트 활성화시 호출
    {
        switch(PlayerSettings.controlType)
        {
            //활성화 된 조작 모드의 버튼을 초록색으로
            case EControlType.Mouse:
                MouseControlButton.image.color = Color.green; 
                KeyboardMouseControlButton.image.color = Color.white;
                break;
            case EControlType.KeyboardMouse:
                MouseControlButton.image.color = Color.white;
                KeyboardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    public void SetControlMode(int controlType)
    {
        //매개변수로 받아온 숫자에 따라 플레이어 셋팅의 컨트롤 타입을 변경하고, 버튼 색 변경
        PlayerSettings.controlType = (EControlType)controlType;

        switch (PlayerSettings.controlType)
        {
            //활성화 된 조작 모드의 버튼을 초록색으로
            case EControlType.Mouse:
                MouseControlButton.image.color = Color.green;
                KeyboardMouseControlButton.image.color = Color.white;
                break;
            case EControlType.KeyboardMouse:
                MouseControlButton.image.color = Color.white;
                KeyboardMouseControlButton.image.color = Color.green;
                break;
        }
    }

    public virtual void Close() //설정 창 UI 닫음
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay() //닫히는 애니메이션 후 UI 비활성화 시키는 함수
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        anim.ResetTrigger("Close");
    }
}
