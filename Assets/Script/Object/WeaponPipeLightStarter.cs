using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPipeLightStarter : MonoBehaviour
{
    //1초 딜레이
    private WaitForSeconds wait = new WaitForSeconds(1f);

    //제일 처음 신호를 받을 라이트 담음 
    List<WeaponPipeLight> lights = new List<WeaponPipeLight>();

    //자식 게임오브젝트 중에 WeaponPipeLight 컴포넌트를 가진 게임오브젝트 찾아서
    //리스트에 저장한 뒤 TurnOnPipeLight 코루틴 실행하게 해줌

    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i).GetComponent<WeaponPipeLight>();
            if (child)
            {
                lights.Add(child);
            }
        }

        StartCoroutine(TurnOnPipeLight());
    }

    private IEnumerator TurnOnPipeLight()
    {
        while (true)
        {
            yield return wait;

            foreach (var child in lights)
            {
                child.TurnOnLight();
            }
        }
    }
}
