using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPipeLight : MonoBehaviour
{

    private Animator animator;

    //다음 라이트 켜지는데까지 딜레이
    private WaitForSeconds wait = new WaitForSeconds(0.15f);

    //현재 라이트가 신호를 전해줘야할 다음 라이트들을 담고 있을 리스트 변수 선언
    private List<WeaponPipeLight> lights = new List<WeaponPipeLight>();

    void Start()
    {
        animator = GetComponent<Animator>();

        //자식 게임오브젝트 중에 WeaponPipeLight 컴포넌트를 가지고 있는 게임 오브젝트를 찾아서 리스트에 저장
        for(int i = 0; i < lights.Count; i++)
        {
            var child = transform.GetChild(i).GetComponent<WeaponPipeLight>();
            if (child)
            {
                lights.Add(child);
            }
        }
    }

    public void TurnOnLight()
    {
        //animator 트리거 발동시켜서 자신의 라이트를 켠 다음
        animator.SetTrigger("On");

        //자식 오브젝트의 불을 켜게 만드는 코루틴 실행
        StartCoroutine(TurnOnLightAtChild());
    }

    private IEnumerator TurnOnLightAtChild()
    {
        //잠시 딜레이 줌
        yield return wait;

        //자식 오브젝트들에세 TurnOnLight 함수 이어서 실행하도록 만들어줌
        foreach(var child in lights)
        {
            child.TurnOnLight();
        }
    }
}
