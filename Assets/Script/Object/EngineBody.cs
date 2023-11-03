using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EngineBody : MonoBehaviour
{
    //���� ���ӿ�����Ʈ ����
    [SerializeField]
    private List<GameObject> steams = new List<GameObject>();

    //����ũ ��������Ʈ ����
    [SerializeField]
    private List<SpriteRenderer> sparks = new List<SpriteRenderer>();

    //���� ������ ����ũ ��������Ʈ ����
    [SerializeField]
    private List<Sprite> sparkSprites = new List<Sprite>();

    //���� ����ũ�� �߻��ϰ� �ִ� ��ġ ����
    private int nowIndex = 0;


    void Start()
    {
        //����Ʈ�� ����ִ� steam ���ӿ�����Ʈ���� ������� �ڷ�ƾ �Լ��� �����ϵ��� ����
        foreach (var steam in steams)
        {
            StartCoroutine(RandomSteam(steam));
        }

        StartCoroutine(SparkEngine());
    }


    //�ڷ�ƾ �Լ� ���� �Ű������� ���� steam ���� ������Ʈ�� ������ �ð��� �帥 �ڿ� Ȱ��ȭ �Ǿ��ٰ�
    //�ִϸ��̼��� ������ �ٽ� ��Ȱ��ȭ �ǵ���

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

    //���� ����ũ�� ǥ���ž��ϴ� ��ġ�� ������Ʈ�� ������ �����ֵ��� �����
    //����ũ ǥ�� �ε����� �������� �ѱ�� �۾��� ���� ���� �ݺ��ϵ��� �ڵ� �ۼ�
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
