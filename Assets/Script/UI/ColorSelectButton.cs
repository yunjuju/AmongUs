using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectButton : MonoBehaviour
{

    [SerializeField]
    private GameObject x;

    public bool isInteractable = true;

    //inInteractable�� ���¿� x �̹����� Ȱ��ȭ ���¸� ������
    public void SetInteractable(bool inInteractable)
    {
        this.isInteractable = inInteractable;

        //x�� ǥ�ô� interactable �Ұ��� �� Ȱ��ȭ �Ǿ�� ��
        x.SetActive(!isInteractable); 
    }
}
