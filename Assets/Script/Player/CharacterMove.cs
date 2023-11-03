using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class CharacterMove : NetworkBehaviour
{
    private Animator anim;

    private bool isMoveable;
    public bool IsMoveable
    {
        get { return isMoveable; }
        set
        {
            //받아온 value 값이 false일 때만 애니메이터의 isMove를 false로 변경하게 만들어줌
            if(!value)
            {
                anim.SetBool("isMove",false); 
            }
            isMoveable = value;
        }
    }

    //네트워크로 동기화
    [SyncVar]
    public float speed = 2f;

    //캐릭터 사이즈 초기값
    [SerializeField]
    private float characterSize = 0.5f;

    //카메라 사이즈 기본값
    [SerializeField]
    private float cameraSize = 2.5f;


    private SpriteRenderer spriteRenderer;

    [SyncVar(hook = nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor) 
    {
        if(spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    //닉네임 변수 선언
    [SyncVar(hook = nameof(SetNickname_Hook))]
    public string nickname;

    //닉네임 보여줄 SetNickname_Hook 함수 선언
    [SerializeField]
    private Text nicknameText;

    public void SetNickname_Hook(string _, string value)
    {
        //매개변수로 받은 value를 nicknameText에 출력하도록 만들어줌
        nicknameText.text = value;
    }


    public virtual void Start()
    {
        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));


        //대기실에서 기본으로 배치돼있는 카메라를 찾아서 클라이언트가 소유한 캐릭터에 붙이도록 해줌
        if (hasAuthority)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0, 0, -10f);
            cam.orthographicSize = cameraSize;
        }
    }


    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        //hasAuthority = 클라이언트가 오브젝트에 대한 권한을 가지고있다면
        //입력을 받아서 이동 기능을 처리하도록 만들어야 함

        if (hasAuthority && isMoveable)
        {
            bool isMove = false;

            //오브젝트가 자신의 것이고 움직일 수 있다면
            if (PlayerSettings.controlType == EControlType.KeyboardMouse)
            {
                //키보드 마우스 컨트롤이라면 키보드 입력이 있을 때 이동하도록
                Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);

                // 방향 전환시 플립효과
                if (dir.x < 0f)
                    transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                else if (dir.x > 0f)
                    transform.localScale = new Vector3(characterSize, characterSize, 1f);

                transform.position += dir * speed * Time.deltaTime;

                //이동 입력에 따라서 애니메이터의 isMove 파라미터 값을 바꿔 주도록 작성
                isMove = dir.magnitude != 0f;
            }
            else
            {
                //마우스 컨트롤이라면 마우스 입력이 있을 때 이동하도록
                if (Input.GetMouseButton(0))
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;

                    // 방향 전환시 플립효과
                    if (dir.x < 0f)
                        transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                    else if (dir.x > 0f)
                        transform.localScale = new Vector3(characterSize, characterSize, 1f);

                    transform.position += dir * speed * Time.deltaTime;

                    isMove = dir.magnitude != 0f;
                }
            }
            anim.SetBool("isMove", isMove);
        }
        //플립으로 인해 닉네임이 뒤집어질 수 있는 것 방지
        if(transform.localScale.x < 0)
        {
            // 캐릭터의 방향이 뒤집어지면 이름의 방향을 한번 더 뒤집어줌
            nicknameText.transform.localScale = new Vector3(-1f,1f,1f);
        }
        else if(transform.localScale.x > 0)
        {
            nicknameText.transform.localScale = new Vector3(1f,1f,1f);
        }
    }
}
