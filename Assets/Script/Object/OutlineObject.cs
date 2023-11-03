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

        //material �ν��Ͻ�ȭ
        var inst = Instantiate(spriterenderer.material);
        spriterenderer.material = inst;

        // OutlineColor ����
        spriterenderer.material.SetColor("_OutlineColor", OutlineColor);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //trigger�� �ε��� object�� LobbyPlayerCharacter�̰� 
        var character = collision.GetComponent<CharacterMove>();

        // �ش� ĳ���Ͱ� Ŭ���̾�Ʈ ���� ������ �ִٸ� 
        if (character != null && character.hasAuthority)
        {
            spriterenderer.enabled = true; //outline ���̰� �����
        }
    }

    // ���� ����� outline ����
    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent <CharacterMove>();
        if(character !=null && character.hasAuthority)
        {
            spriterenderer.enabled= false; //SpriteRenderer ����
        }
    }
}
