using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrew : MonoBehaviour
{
    public EPlayerColor playerColor; //ũ��� ���� ���� ����

    private SpriteRenderer spriteRenderer; //��������Ʈ ���� ����
    private Vector3 direction; //ũ��� ���ٴϴ� ����
    private float floatingSpeed;
    private float rotateSpeed;

    private void Awake()
    {
        //���� ������Ʈ�� �پ��ִ� ��������Ʈ ������ ������Ʈ�� ������
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFloatingCrew(Sprite sprite, EPlayerColor playerColor, Vector3 direction, float floatingSpeed,
        float rotateSpeed, float size)
    {
        //���� ������Ƽ�� ����
        this.playerColor = playerColor;
        this.direction = direction;
        this.floatingSpeed = floatingSpeed;
        this.rotateSpeed = rotateSpeed;
        
        //�̹��� ����
        spriteRenderer.sprite = sprite;
        //������ �����ͼ� material�� �־���
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));

        //����� �̿��ؼ� ũ����� ũ�� ����
        transform.localScale = new Vector3(size, size, size);

        //ũ�Ⱑ ���� ũ����� �ڷΰ�����
        spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size);
    }


    void Update()
    {
        transform.position += direction * floatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
    }
}
