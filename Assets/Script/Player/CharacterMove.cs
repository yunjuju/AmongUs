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
            //�޾ƿ� value ���� false�� ���� �ִϸ������� isMove�� false�� �����ϰ� �������
            if(!value)
            {
                anim.SetBool("isMove",false); 
            }
            isMoveable = value;
        }
    }

    //��Ʈ��ũ�� ����ȭ
    [SyncVar]
    public float speed = 2f;

    //ĳ���� ������ �ʱⰪ
    [SerializeField]
    private float characterSize = 0.5f;

    //ī�޶� ������ �⺻��
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

    //�г��� ���� ����
    [SyncVar(hook = nameof(SetNickname_Hook))]
    public string nickname;

    //�г��� ������ SetNickname_Hook �Լ� ����
    [SerializeField]
    private Text nicknameText;

    public void SetNickname_Hook(string _, string value)
    {
        //�Ű������� ���� value�� nicknameText�� ����ϵ��� �������
        nicknameText.text = value;
    }


    public virtual void Start()
    {
        anim = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));


        //���ǿ��� �⺻���� ��ġ���ִ� ī�޶� ã�Ƽ� Ŭ���̾�Ʈ�� ������ ĳ���Ϳ� ���̵��� ����
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
        //hasAuthority = Ŭ���̾�Ʈ�� ������Ʈ�� ���� ������ �������ִٸ�
        //�Է��� �޾Ƽ� �̵� ����� ó���ϵ��� ������ ��

        if (hasAuthority && isMoveable)
        {
            bool isMove = false;

            //������Ʈ�� �ڽ��� ���̰� ������ �� �ִٸ�
            if (PlayerSettings.controlType == EControlType.KeyboardMouse)
            {
                //Ű���� ���콺 ��Ʈ���̶�� Ű���� �Է��� ���� �� �̵��ϵ���
                Vector3 dir = Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);

                // ���� ��ȯ�� �ø�ȿ��
                if (dir.x < 0f)
                    transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                else if (dir.x > 0f)
                    transform.localScale = new Vector3(characterSize, characterSize, 1f);

                transform.position += dir * speed * Time.deltaTime;

                //�̵� �Է¿� ���� �ִϸ������� isMove �Ķ���� ���� �ٲ� �ֵ��� �ۼ�
                isMove = dir.magnitude != 0f;
            }
            else
            {
                //���콺 ��Ʈ���̶�� ���콺 �Է��� ���� �� �̵��ϵ���
                if (Input.GetMouseButton(0))
                {
                    Vector3 dir = (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;

                    // ���� ��ȯ�� �ø�ȿ��
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
        //�ø����� ���� �г����� �������� �� �ִ� �� ����
        if(transform.localScale.x < 0)
        {
            // ĳ������ ������ ���������� �̸��� ������ �ѹ� �� ��������
            nicknameText.transform.localScale = new Vector3(-1f,1f,1f);
        }
        else if(transform.localScale.x > 0)
        {
            nicknameText.transform.localScale = new Vector3(1f,1f,1f);
        }
    }
}
