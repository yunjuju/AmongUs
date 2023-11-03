using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField]
    private Transform Back;

    [SerializeField]
    private Transform Front;

    public int GetSortingOrder(GameObject obj)
    {
        //�Ű������� �޾ƿ� ������Ʈ�� Back�� �Ÿ�,
        //Back�� Front�� �Ÿ��� �̿��ؼ� SortingOrder�� ���ؼ� ��ȯ
        float objDist = Mathf.Abs(Back.position.y - obj.transform.position.y);
        float totalDist = Mathf.Abs(Back.position.y - Front.transform.position.y);

        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));

    }
}
