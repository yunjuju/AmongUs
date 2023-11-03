using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeLaptop : MonoBehaviour
{

    [SerializeField]
    private Sprite useButtonSprite; //Use ��ư �̹��� ����
    private SpriteRenderer spriteRenderer; //��Ʈ�� �̹��� ��Բ�

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        //���͸��� �ν��Ͻ�ȭ
        var inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = inst;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMove>();
        if (character != null && character.hasAuthority)
        {
            // ��������Ʈ ���� �������� ����
            spriteRenderer.material.SetFloat("_Highlighted", 1f);

            // ��ü�� sprite�� OnclickUse �Լ��� USE ��ư�� ����
            LobbyUIManager.Instance.SetUseButton(useButtonSprite, OnClickUse);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMove>();
        if (character != null && character.hasAuthority)
        {
            // ��������Ʈ ���� �������� ����
            spriteRenderer.material.SetFloat("Highlighted", 0f);

            LobbyUIManager.Instance.UnsetUseButton(); // Use ��ư ���� �ʱ�ȭ
        }

    }

    public void OnClickUse()
    { // Customize UI Open �Լ� ȣ��
        LobbyUIManager.Instance.CustomizeUI.Open();
    }
}
