using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualization1D_Perlin : MonoBehaviour {

    public GameObject visualizationPrefab;

    public uint distanceInUnits;
    public uint intervalsPerUnit;

    [Range(0, 10)]
    public float numberLinePoint;

    public float slope; 

    private Vector3 originLineIntersect;
    private Vector3 intersect;

    private Vector3 magnitude;
    private Vector3 tangent;
    private Vector3 backTangent;

    private Vector3 smallStepBack;

	void Start () {
		for (int i = 0; i < (distanceInUnits * intervalsPerUnit); i++)
        {
            Instantiate(visualizationPrefab, 
                new Vector3((float)i / intervalsPerUnit, Perlin.Noise((float)i/ intervalsPerUnit), 0), 
                Quaternion.identity, 
                transform);
        }

        for (int i = 0; i <= distanceInUnits; i++)
        {
            for (int j = -10; j <= 10; j++)
            {
                Instantiate(visualizationPrefab,
                    new Vector3(i, (float)j/10, 0),
                    Quaternion.identity,
                    transform);
            }
        }

	}

    private void Update()
    {
        originLineIntersect.x = numberLinePoint;

        intersect.x = numberLinePoint;
        intersect.y = Perlin.Noise(intersect.x);

        magnitude.x = numberLinePoint;
        magnitude.y = intersect.y;

        smallStepBack.x = numberLinePoint - .01f;
        smallStepBack.y = Perlin.Noise(smallStepBack.x);

        tangent = Vector3.Normalize(intersect -  smallStepBack)  + intersect;
        backTangent = Vector3.Normalize(smallStepBack - intersect) + intersect;

        Debug.DrawLine(originLineIntersect, magnitude);
        Debug.DrawLine(backTangent, tangent);

        slope = tangent.y / tangent.x;
    }
}
