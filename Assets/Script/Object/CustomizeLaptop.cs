using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeLaptop : MonoBehaviour
{

    [SerializeField]
    private Sprite useButtonSprite; //Use 버튼 이미지 변경
    private SpriteRenderer spriteRenderer; //노트북 이미지 밝게끔

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //머터리얼 인스턴스화
        var inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = inst;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMove>();
        if (character != null && character.hasAuthority)
        {
            // 스프라이트 밝은 색상으로 변경
            spriteRenderer.material.SetFloat("_Highlighted", 1f);

            // 교체할 sprite와 OnclickUse 함수를 USE 버튼에 세팅
            LobbyUIManager.Instance.SetUseButton(useButtonSprite, OnClickUse);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMove>();
        if (character != null && character.hasAuthority)
        {
            // 스프라이트 원래 색상으로 변경
            spriteRenderer.material.SetFloat("Highlighted", 0f);

            LobbyUIManager.Instance.UnsetUseButton(); // Use 버튼 세팅 초기화
        }

    }

    public void OnClickUse()
    { // Customize UI Open 함수 호출
        LobbyUIManager.Instance.CustomizeUI.Open();
    }
}
