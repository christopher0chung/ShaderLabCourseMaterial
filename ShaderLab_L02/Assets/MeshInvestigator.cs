using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshInvestigator : MonoBehaviour {

    public Vector3[] verts;
    public int[] tris;
    public Vector3[] norms;
    public Vector2[] uvs;

    private MeshFilter _mF;

	void Start () {
        Debug.Assert(GetComponent<MeshFilter>(), "No MeshFilter");

        _mF = GetComponent<MeshFilter>();
        verts = _mF.mesh.vertices;
        tris = _mF.mesh.triangles;
        norms = _mF.mesh.normals;
        uvs = _mF.mesh.uv;
	}
}
