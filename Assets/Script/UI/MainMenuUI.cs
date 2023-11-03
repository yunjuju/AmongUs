using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void OnClickOnlineButton() //온라인 클릭 버튼
    {
        Debug.Log("Click Online");
    }
    
    public void OnClickQuitButton()
    {
#if UNITY_EDITOR //에디터에서 시작된 상태면 플레이를 중단 시킴
        UnityEditor.EditorApplication.isPlaying = false;

#else //빌드된 상태라면 어플리케이션을 종료 
        Application.Quit();
#endif
    }
}
