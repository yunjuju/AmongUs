using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPositions : MonoBehaviour
{
    [SerializeField]
    private Transform[] positions;

    //스폰위치 겹치지 않게 해줄 idex변수 선언
    private int index;


    public int Index { get { return index; } }
    
    public Vector3 GetSpawnPosition()
    {
        //index에 따라서 위치를 반환하도록 만들어줌
        Vector3 pos = positions[index++].position;

        if(index >= positions.Length)
        {
            index = 0;
        }
        return pos;
    }
}
