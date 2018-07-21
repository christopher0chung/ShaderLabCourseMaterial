using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMesh : MonoBehaviour {

    private MeshRenderer _mR;
    private MeshFilter _mF;

    private Mesh _mesh;

    private Vector3[] _vertices;
    private int[] _triangles;
    private Vector2[] _uvs;
    private Vector3[] _normals;

    public Vector3 norm;

	void Start () {
        _mR = gameObject.AddComponent<MeshRenderer>();
        _mF = gameObject.AddComponent<MeshFilter>();

        _mesh = new Mesh();
        _mesh.name = "Steve";

        _CalcVerts();
        _CalcTris();
        _CalcUVs();
        _CalcNorms();

        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;

        _mF.mesh = _mesh;

        _mR.material = Resources.Load<Material>("MyMat");
	}

    private void _CalcVerts()
    {
        _vertices = new Vector3[3];
        _vertices[0] = Vector3.zero;
        _vertices[1] = new Vector3(0, 1, 0);
        _vertices[2] = new Vector3(1, 0, 0);
    }

    private void _CalcTris()
    {
        _triangles = new int[3];
        _triangles[0] = 0;
        _triangles[1] = 1;
        _triangles[2] = 2;
    }

    private void _CalcUVs()
    {
        _uvs = new Vector2[3];
        _uvs[0] = new Vector2(0, 0);
        _uvs[1] = new Vector2(0, 1);
        _uvs[2] = new Vector2(1, 0);
    }

    private void _CalcNorms()
    {
        _normals = new Vector3[3];
        for (int i = 0; i < _normals.Length; i++)
        {
            _normals[i] = Vector3.up;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            for (int i = 0; i < _normals.Length; i++)
            {
                _normals[i] = Random.insideUnitSphere;
            }
            _mF.mesh.normals = _normals;
        }
    }
}
