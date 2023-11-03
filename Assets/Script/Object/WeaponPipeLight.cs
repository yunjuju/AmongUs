using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPipeLight : MonoBehaviour
{

    private Animator animator;

    //���� ����Ʈ �����µ����� ������
    private WaitForSeconds wait = new WaitForSeconds(0.15f);

    //���� ����Ʈ�� ��ȣ�� ��������� ���� ����Ʈ���� ��� ���� ����Ʈ ���� ����
    private List<WeaponPipeLight> lights = new List<WeaponPipeLight>();

    void Start()
    {
        animator = GetComponent<Animator>();

        //�ڽ� ���ӿ�����Ʈ �߿� WeaponPipeLight ������Ʈ�� ������ �ִ� ���� ������Ʈ�� ã�Ƽ� ����Ʈ�� ����
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
        //animator Ʈ���� �ߵ����Ѽ� �ڽ��� ����Ʈ�� �� ����
        animator.SetTrigger("On");

        //�ڽ� ������Ʈ�� ���� �Ѱ� ����� �ڷ�ƾ ����
        StartCoroutine(TurnOnLightAtChild());
    }

    private IEnumerator TurnOnLightAtChild()
    {
        //��� ������ ��
        yield return wait;

        //�ڽ� ������Ʈ�鿡�� TurnOnLight �Լ� �̾ �����ϵ��� �������
        foreach(var child in lights)
        {
            child.TurnOnLight();
        }
    }
}
