using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrew : MonoBehaviour
{
    public EPlayerColor playerColor; //크루원 색상 저장 변수

    private SpriteRenderer spriteRenderer; //스프라이트 색상 변경
    private Vector3 direction; //크루원 떠다니는 방향
    private float floatingSpeed;
    private float rotateSpeed;

    private void Awake()
    {
        //게임 오브젝트에 붙어있는 스프라이트 렌더러 컴포넌트를 가져옴
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetFloatingCrew(Sprite sprite, EPlayerColor playerColor, Vector3 direction, float floatingSpeed,
        float rotateSpeed, float size)
    {
        //내부 프로퍼티에 저장
        this.playerColor = playerColor;
        this.direction = direction;
        this.floatingSpeed = floatingSpeed;
        this.rotateSpeed = rotateSpeed;
        
        //이미지 변경
        spriteRenderer.sprite = sprite;
        //색상을 가져와서 material에 넣어줌
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));

        //사이즈를 이용해서 크루원의 크기 정함
        transform.localScale = new Vector3(size, size, size);

        //크기가 작은 크루원이 뒤로가게함
        spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size);
    }


    void Update()
    {
        transform.position += direction * floatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
    }
}
