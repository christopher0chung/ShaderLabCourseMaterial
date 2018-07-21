using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriScrambler : MonoBehaviour {

    public int[] tris;

    private MeshFilter _mF;

    void Start()
    {
        Debug.Assert(GetComponent<MeshFilter>(), "No MeshFilter");

        _mF = GetComponent<MeshFilter>();
        tris = _mF.mesh.triangles;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
            for (int i = 0; i < tris.Length; i++)
            {
                tris[i] = Random.Range(0, tris.Length);
            }
            _mF.mesh.triangles = tris;
        //}
    }
}
