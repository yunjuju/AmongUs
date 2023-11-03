using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPipeLightStarter : MonoBehaviour
{
    //1�� ������
    private WaitForSeconds wait = new WaitForSeconds(1f);

    //���� ó�� ��ȣ�� ���� ����Ʈ ���� 
    List<WeaponPipeLight> lights = new List<WeaponPipeLight>();

    //�ڽ� ���ӿ�����Ʈ �߿� WeaponPipeLight ������Ʈ�� ���� ���ӿ�����Ʈ ã�Ƽ�
    //����Ʈ�� ������ �� TurnOnPipeLight �ڷ�ƾ �����ϰ� ����

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
