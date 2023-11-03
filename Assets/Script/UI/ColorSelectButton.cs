using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSelectButton : MonoBehaviour
{

    [SerializeField]
    private GameObject x;

    public bool isInteractable = true;

    //inInteractable의 상태와 x 이미지의 활성화 상태를 변경함
    public void SetInteractable(bool inInteractable)
    {
        this.isInteractable = inInteractable;

        //x자 표시는 interactable 불가할 때 활성화 되어야 함
        x.SetActive(!isInteractable); 
    }
}
