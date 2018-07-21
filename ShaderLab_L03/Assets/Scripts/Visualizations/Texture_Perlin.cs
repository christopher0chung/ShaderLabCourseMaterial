using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture_Perlin : MonoBehaviour {

    public Vector3 cursor;
    public Texture2D myTex;

    void Start()
    {
        MakeATexture();
    }

    void MakeATexture()
    {
        myTex = new Texture2D(1000, 1000);
        for (int i = 0; i < (myTex.width - 1); i++)
        {
            for (int j = 0; j < (myTex.height - 1); j++)
            {
                Vector3 pointOfAnalysis = new Vector3((float)i / 10, 0, (float)j / 10);
                float valOfNoise = DotFloatToColorFloat(Perlin.Noise(pointOfAnalysis.x, pointOfAnalysis.z));
                Color colorOfNoise = new Color(valOfNoise, valOfNoise, valOfNoise);
                myTex.SetPixel(i, j, colorOfNoise);
            }
        }
        myTex.Apply();
        myTex.filterMode = FilterMode.Point;

        GameObject quad = Instantiate(Resources.Load<GameObject>("Quad"), new Vector3(50, 0, 50), Quaternion.Euler(new Vector3(90, 0, 0)));
        quad.transform.localScale = Vector3.one * 100;

        Material mat = Resources.Load<Material>("EmptyMat 1");

        quad.GetComponent<MeshRenderer>().material = mat;
        mat.SetTexture("_MainTex", myTex);
    }

    float DotFloatToColorFloat(float dotVal)
    {
        return (dotVal + 1) / 2;
    }

    #region Additional Info in Scene View
    void Update()
    {
        DrawField();
        CalcAndDrawToCursor();
    }

    void DrawField()
    {
        for (int i = 0; i <= myTex.width / 10; i++)
        {
            for (int j = 0; j <= myTex.height / 10; j++)
            {
                Vector3 corner = new Vector3(i, 0, j);
                Debug.DrawLine(corner, corner + Vector3.up * .1f, Color.blue);
            }
        }
    }

    void CalcAndDrawToCursor()
    {
        cursor.x = Mathf.Clamp(cursor.x, 0, 100);
        cursor.y = 0;
        cursor.z = Mathf.Clamp(cursor.z, 0, 100);

        //Debug.DrawRay(cursor, Vector3.up);

        Vector3 corner0 = new Vector3((int)cursor.x, 0, (int)cursor.z);
        Vector3 corner1 = new Vector3((int)cursor.x + 1, 0, (int)cursor.z);
        Vector3 corner2 = new Vector3((int)cursor.x, 0, (int)cursor.z + 1);
        Vector3 corner3 = new Vector3((int)cursor.x + 1, 0, (int)cursor.z + 1);

        Debug.DrawLine(corner0, cursor);
        Debug.DrawLine(corner1, cursor);
        Debug.DrawLine(corner2, cursor);
        Debug.DrawLine(corner3, cursor);

        float noise = Perlin.Noise(cursor.x, cursor.y);

        Color lineColor = new Color(noise, noise, noise);

        Debug.DrawLine(cursor, cursor + Vector3.up * noise * 5, lineColor);
    }
    #endregion
}
