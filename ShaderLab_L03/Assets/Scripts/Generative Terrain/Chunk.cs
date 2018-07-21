using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk {

    public Vector3 originPoint;
    public int index;

    public GameObject _gO;
    public MeshRenderer _mR;
    public MeshFilter _mF;

    private Mesh _mesh;

    public Vector3[] _vertices;
    public int[] _triangles;
    public Vector2[] _uvs;
    public Vector3[] _normals;

    public Chunk(Vector3 origin, int index)
    {
        originPoint = origin;

        _Initialize(index);
        _CalcVerts();
        _CalcTris();
        _CalcNorms();
        _Apply();
    }

    private void _Initialize(int index)
    {
        _gO = new GameObject();
        _gO.name = "Chunk_" + index;

        _mR = _gO.AddComponent<MeshRenderer>();
        _mF = _gO.AddComponent<MeshFilter>();

        _mesh = new Mesh();
        _mesh.name = "Mesh_" + index;
    }

    private void _CalcVerts()
    {
        List<Vector3> _tempVerts = new List<Vector3>();
        List<Vector2> _tempUVs = new List<Vector2>();

        for (int i = 0; i <= 8; i++)
        {
            for (int j = 0; j <= 8; j++)
            {
                float X = (float)(originPoint.x + j);
                float Z = (float)(originPoint.z + i);

                Vector3 pos = new Vector3(X, Mathf.Abs(Perlin.Noise(X/64, Z/64) * 30 * Perlin.Noise(X/8, Z/8)) + Perlin.Noise(X / 16, Z / 16), Z);
                //Debug.Log(pos);
                _tempVerts.Add(pos);
                _tempUVs.Add(new Vector2((float)i / 8, (float)j / 8));
            }
        }

        _vertices = new Vector3[_tempVerts.Count];
        for (int i = 0; i < _vertices.Length; i++)
        {
            _vertices[i] = _tempVerts[i];
        }

        _uvs = new Vector2[_tempUVs.Count];
        for (int i = 0; i < _uvs.Length; i++)
        {
            _uvs[i] = _tempUVs[i];
        }
    }

    private void _CalcTris()
    {
        List<int> _tempTris = new List<int>();

        for (int z = 0; z < 8; z++)
        {
            for (int x = 0; x < 8; x++)
            {
                _tempTris.Add(x + z * 9);
                _tempTris.Add(x + z * 9 + 10);
                _tempTris.Add(x + z * 9 + 1);

                _tempTris.Add(x + z * 9);
                _tempTris.Add(x + z * 9 + 9);
                _tempTris.Add(x + z * 9 + 10);
            }
        }

        _triangles = new int[_tempTris.Count];
        for (int i = 0; i < _triangles.Length; i++)
        {
            _triangles[i] = _tempTris[i];
        }
    }

    private void _CalcNorms()
    {
        _normals = new Vector3[_vertices.Length];
        //for (int i = 0; i < _normals.Length; i++)
        //{
        //    _normals[i] = Vector3.up;
        //    //Debug.Log(_normals[i]);
        //}
    }

    private void _Apply()
    {
        _mesh.vertices = _vertices;
        _mesh.triangles = _triangles;
        _mesh.uv = _uvs;
        _mesh.normals = _normals;
        _mesh.RecalculateNormals();


        _mF.mesh = _mesh;

        _mR.material = Resources.Load<Material>("MyMat");
    }
}