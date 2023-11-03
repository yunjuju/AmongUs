using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CrewFloater : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab; //������ ������Ƽ
    [SerializeField]
    private List<Sprite> sprites; //ũ��� �̹��� �ٲ��� �� ����� ��������Ʈ �迭 ������Ƽ 

    //���ٴϴ� ũ����� ���� �ߺ����� �ʰ� ���� ������Ƽ
    private bool[] crewStates = new bool[12];
    private float timer = 0.01f; //ũ��� ��ȯ ���� ������Ƽ
    private float distance = 11f; //�߽����κ��� ��ȯ �� ��ġ�� ������ distance ������Ƽ


    void Start()
    {
        for(int i = 0; i < 12;  i++)
        {
            SpawnFloatingCrew((EPlayerColor)i, Random.Range(0f, distance));
        }
    }


    void Update()
    {
        //�����ð����� SpawnFloatingCrew �Լ��� ȣ���ϰ� �������
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            // �������� ���� ����, ������ ��ġ�� �Ÿ��� ����
            SpawnFloatingCrew((EPlayerColor)Random.Range(0, 12), distance);

            timer = 0.1f;
        }

    }

    public void SpawnFloatingCrew(EPlayerColor playerColor, float dist) //ũ��� ��ȯ
    {

        //crewStates �迭���� �ش� ��ȣ�� ũ����� ��ȯ���� ���� �������� Ȯ��, ��ȯ���� ���� ���¶�� ��ȯ�ǵ��� ����
        if (!crewStates[(int)playerColor])
        {
            crewStates[(int)playerColor] = true;
        

        //ũ��� ��ȯ �� ī�޶� ������ ����� �������� �����ǰ� ������ ��

        //0 - 360�� ������ ������ ���ڸ� �̾Ƴ� ���� sin �Լ��� cos �Լ��� �̿��� vector�� ����
        //-> �߽����κ��� ��ֿ��� ������ ���ư��� ����Ű�� ���͸� ���� �� ����

        float angle = Random.Range(0f, 360f);
        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * dist; 

        //ũ����� ���ư� ����
        Vector3 direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

        //�ӵ�
        float floatingSPeed = Random.Range(1f, 2f);

        //ȸ�� �ӵ�
        float rotateSpeed = Random.Range(-3f,2f);

        var crew = Instantiate(prefab, spawnPos, Quaternion.identity).GetComponent<FloatingCrew>();

        crew.SetFloatingCrew(sprites[Random.Range(0, sprites.Count)], playerColor, direction, floatingSPeed, rotateSpeed,
            Random.Range(0.5f, 1f));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //ũ����� crew Floater�� ������ ���� �� ��� �ش� ������ ũ��� ���¸� false�� �ٲٰ� ũ����� �ı�
        var crew = collision.GetComponent<FloatingCrew>();

        if (crew != null)
        {
            crewStates[(int)crew.playerColor] = false;
            Destroy(crew.gameObject);
        }
    }
}
