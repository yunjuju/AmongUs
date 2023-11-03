using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;

    //������ġ ��ġ�� �ʰ� ���� idex���� ����
    private int index;


    public int Index { get { return index; } }
    
    public Vector3 GetSpawnPosition()
    {
        //index�� ���� ��ġ�� ��ȯ�ϵ��� �������
        Vector3 pos = positions[index++].position;

        if(index >= positions.Length)
        {
            index = 0;
        }
        return pos;
    }
}
