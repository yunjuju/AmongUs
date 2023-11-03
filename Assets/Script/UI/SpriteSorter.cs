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
        //매개변수로 받아온 오브젝트와 Back의 거리,
        //Back과 Front의 거리를 이용해서 SortingOrder를 구해서 반환
        float objDist = Mathf.Abs(Back.position.y - obj.transform.position.y);
        float totalDist = Mathf.Abs(Back.position.y - Front.transform.position.y);

        return (int)(Mathf.Lerp(System.Int16.MinValue, System.Int16.MaxValue, objDist / totalDist));

    }
}
