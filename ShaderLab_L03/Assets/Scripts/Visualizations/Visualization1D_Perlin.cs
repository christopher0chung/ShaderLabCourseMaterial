using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualization1D_Perlin : MonoBehaviour {

    public GameObject visualizationPrefab;

    public uint distanceInUnits;
    public uint intervalsPerUnit;

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
}
