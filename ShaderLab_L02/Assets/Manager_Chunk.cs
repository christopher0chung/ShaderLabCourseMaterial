using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Chunk : MonoBehaviour {

    List<Chunk_Nondeforming> myChunks = new List<Chunk_Nondeforming>();

    void Start () {

        myChunks.Add(new Chunk_Nondeforming(Vector3.zero, 0));
        _CheckOneRingAndPopulate(Vector3.zero);
	}


    void _CheckOneRingAndPopulate(Vector3 currentPos)
    {
        Vector3 bracketedPos = _BracketV3(currentPos);

        bool[] checkRingRegions = new bool[8];

        foreach (Chunk_Nondeforming c in myChunks)
        {
            if (c.originPoint.z == bracketedPos.z + 8)
            {
                if (c.originPoint.x == bracketedPos.x - 8)
                    checkRingRegions[0] = true;
                else if (c.originPoint.x == bracketedPos.x)
                    checkRingRegions[1] = true;
                else if(c.originPoint.x == bracketedPos.x + 8)
                    checkRingRegions[2] = true;
            }
            else if(c.originPoint.z == bracketedPos.z)
            {
                if (c.originPoint.x == bracketedPos.x - 8)
                    checkRingRegions[3] = true;
                else if(c.originPoint.x == bracketedPos.x + 8)
                    checkRingRegions[4] = true;
            }
            else if(c.originPoint.z == bracketedPos.z - 8)
            {
                if (c.originPoint.x == bracketedPos.x - 8)
                    checkRingRegions[5] = true;
                else if(c.originPoint.x == bracketedPos.x)
                    checkRingRegions[6] = true;
                else if(c.originPoint.x == bracketedPos.x + 8)
                    checkRingRegions[7] = true;
            }
        }

        if (!checkRingRegions[0])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(-8, 0, 8), 1));
        if (!checkRingRegions[1])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(0, 0, 8), 1));
        if (!checkRingRegions[2])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(8, 0, 8), 1));
        if (!checkRingRegions[3])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(-8, 0, 0), 1));
        if (!checkRingRegions[4])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(8, 0, 0), 1));
        if (!checkRingRegions[5])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(-8, 0, -8), 1));
        if (!checkRingRegions[6])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(0, 0, -8), 1));
        if (!checkRingRegions[7])
            myChunks.Add(new Chunk_Nondeforming(new Vector3(8, 0, -8), 1));
    }

    private Vector3 _BracketV3 (Vector3 inputVector)
    {
        inputVector.x /= 8;
        inputVector.y /= 8;
        inputVector.z /= 8;

        inputVector.x = Mathf.FloorToInt(inputVector.x);
        inputVector.y = Mathf.FloorToInt(inputVector.y);
        inputVector.z = Mathf.FloorToInt(inputVector.z);

        inputVector.x *= 8;
        inputVector.y *= 8;
        inputVector.z *= 8;

        return inputVector;
    }
}
