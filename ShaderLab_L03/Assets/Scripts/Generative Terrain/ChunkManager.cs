using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour {

    public List<Chunk> myChunk;

	void Start () {
        myChunk = new List<Chunk>();

        for (int j = 0; j < 20; j++)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 origin = new Vector3(i * 8, 0, j * 8);
                myChunk.Add(new Chunk(origin, j * 20 + i));
            }
        }
	}

}
