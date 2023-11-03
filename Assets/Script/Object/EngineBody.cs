using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EngineBody : MonoBehaviour
{
    //스팀 게임오브젝트 저장
    [SerializeField]
    private List<GameObject> steams = new List<GameObject>();

    //스파크 스프라이트 저장
    [SerializeField]
    private List<SpriteRenderer> sparks = new List<SpriteRenderer>();

    //여러 종류의 스파크 스프라이트 저장
    [SerializeField]
    private List<Sprite> sparkSprites = new List<Sprite>();

    //현재 스파크가 발생하고 있는 위치 저장
    private int nowIndex = 0;


    void Start()
    {
        //리스트에 들어있는 steam 게임오브젝트들을 대상으로 코루틴 함수를 시작하도록 만듦
        foreach (var steam in steams)
        {
            StartCoroutine(RandomSteam(steam));
        }

        StartCoroutine(SparkEngine());
    }


    //코루틴 함수 만들어서 매개변수로 받은 steam 게임 오브젝트가 랜덤한 시간이 흐른 뒤에 활성화 되었다가
    //애니메이션이 끝나면 다시 비활성화 되도록

    private IEnumerator RandomSteam(GameObject steam)
    {
        while (true)
        {
            float timer = Random.Range(0.5f, 1.5f);

            while (timer >= 0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }

            steam.SetActive(true);
            timer = 0f;
            while (timer <= 0.6f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            steam.SetActive(false);
        }
    }

    //현재 스파크가 표현돼야하는 위치의 오브젝트에 빠르게 보여주도록 만들고
    //스파크 표현 인덱스를 다음으로 넘기는 작업을 게임 내내 반복하도록 코드 작성
    private IEnumerator SparkEngine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.05f);
        while (true)
        {
            float timer = Random.Range(0.2f, 1.5f);

            while (timer >= 0f)
            {
                yield return null;
                timer -= Time.deltaTime;
            }

            int[] indices = new int[Random.Range(2, 7)];

            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = Random.Range(0, sparkSprites.Count);
            }

            for (int i = 0; i < indices.Length; i++)
            {
                yield return wait;
                sparks[nowIndex].sprite = sparkSprites[indices[i]];
            }

            sparks[nowIndex++].sprite = null;

            if(nowIndex >= sparks.Count)
            {
                nowIndex = 0;
            }
        }
    }
}
