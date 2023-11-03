using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnClickOnlineButton() //�¶��� Ŭ�� ��ư
    {
        Debug.Log("Click Online");
    }
    
    public void OnClickQuitButton()
    {
#if UNITY_EDITOR //�����Ϳ��� ���۵� ���¸� �÷��̸� �ߴ� ��Ŵ
        UnityEditor.EditorApplication.isPlaying = false;

#else //����� ���¶�� ���ø����̼��� ���� 
        Application.Quit();
#endif
    }
}
