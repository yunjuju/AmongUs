using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineObject : MonoBehaviour
{

    private SpriteRenderer spriterenderer;

    [SerializeField]
    private Color OutlineColor;

    void Start()
    {
        spriterenderer = GetComponent<SpriteRenderer>();

        //material 인스턴스화
        var inst = Instantiate(spriterenderer.material);
        spriterenderer.material = inst;

        // OutlineColor 설정
        spriterenderer.material.SetColor("_OutlineColor", OutlineColor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //trigger에 부딪힌 object가 LobbyPlayerCharacter이고 
        var character = collision.GetComponent<CharacterMove>();

        // 해당 캐릭터가 클라이언트 권한 가지고 있다면 
        if (character != null && character.hasAuthority)
        {
            spriterenderer.enabled = true; //outline 보이게 만들기
        }
    }

    // 영역 벗어나면 outline 없앰
    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent <CharacterMove>();
        if(character !=null && character.hasAuthority)
        {
            spriterenderer.enabled= false; //SpriteRenderer 끄기
        }
    }
}
