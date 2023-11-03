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

    private void OnEnable() // ���ӿ�����Ʈ Ȱ��ȭ�� ȣ��
    {
        switch(PlayerSettings.controlType)
        {
            //Ȱ��ȭ �� ���� ����� ��ư�� �ʷϻ�����
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
        //�Ű������� �޾ƿ� ���ڿ� ���� �÷��̾� ������ ��Ʈ�� Ÿ���� �����ϰ�, ��ư �� ����
        PlayerSettings.controlType = (EControlType)controlType;

        switch (PlayerSettings.controlType)
        {
            //Ȱ��ȭ �� ���� ����� ��ư�� �ʷϻ�����
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

    public virtual void Close() //���� â UI ����
    {
        StartCoroutine(CloseAfterDelay());
    }

    private IEnumerator CloseAfterDelay() //������ �ִϸ��̼� �� UI ��Ȱ��ȭ ��Ű�� �Լ�
    {
        anim.SetTrigger("Close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        anim.ResetTrigger("Close");
    }
}
