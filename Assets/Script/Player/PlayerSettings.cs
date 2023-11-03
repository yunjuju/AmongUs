using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용자의 컨트롤 방식 정의
public enum EControlType
{
    Mouse,
    KeyboardMouse
}



public class PlayerSettings 
{
    public static EControlType controlType; //정적 변수 선언
    public static string nickname;

}
