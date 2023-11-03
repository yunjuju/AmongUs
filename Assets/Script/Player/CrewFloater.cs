using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CrewFloater : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab; //프리팹 프로퍼티
    [SerializeField]
    private List<Sprite> sprites; //크루원 이미지 바꿔줄 때 사용할 스프라이트 배열 프로퍼티 

    //떠다니는 크루원의 색이 중복되지 않게 해줄 프로퍼티
    private bool[] crewStates = new bool[12];
    private float timer = 0.01f; //크루원 소환 간격 프로퍼티
    private float distance = 11f; //중심으로부터 소환 될 위치를 지정할 distance 프로퍼티


    void Start()
    {
        for(int i = 0; i < 12;  i++)
        {
            SpawnFloatingCrew((EPlayerColor)i, Random.Range(0f, distance));
        }
    }


    void Update()
    {
        //일정시간마다 SpawnFloatingCrew 함수를 호출하게 만들어줌
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // 무작위로 색상 생성, 생성될 위치와 거리를 구함
            SpawnFloatingCrew((EPlayerColor)Random.Range(0, 12), distance);

            timer = 0.1f;
        }

    }

    public void SpawnFloatingCrew(EPlayerColor playerColor, float dist) //크루원 소환
    {

        //crewStates 배열에서 해당 번호의 크루원이 소환되지 않은 상태인지 확인, 소환되지 않은 상태라면 소환되도록 해줌
        if (!crewStates[(int)playerColor])
        {
            crewStates[(int)playerColor] = true;
        

        //크루원 소환 시 카메라 영역을 벗어나서 원형으로 생성되게 만들어야 함

        //0 - 360도 사이의 랜덤한 숫자를 뽑아낸 다음 sin 함수와 cos 함수를 이용해 vector를 만듦
        //-> 중심으로부터 우넣영의 방향을 돌아가며 가리키는 벡터를 구할 수 있음

        float angle = Random.Range(0f, 360f);
        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * dist; 

        //크루원이 날아갈 방향
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

        //속도
        float floatingSPeed = Random.Range(1f, 2f);

        //회전 속도
        float rotateSpeed = Random.Range(-3f,2f);

        var crew = Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<FloatingCrew>();

        crew.SetFloatingCrew(sprites[Random.Range(0, sprites.Count)], playerColor, direction, floatingSPeed, rotateSpeed,
            Random.Range(0.5f, 1f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //크루원이 crew Floater의 영역을 벗어 난 경우 해당 색상의 크루원 상태를 false로 바꾸고 크루원을 파괴
        var crew = collision.GetComponent<FloatingCrew>();

        if (crew != null)
        {
            crewStates[(int)crew.playerColor] = false;
            Destroy(crew.gameObject);
        }
    }
}
