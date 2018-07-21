using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualization2DInterstitial_Perlin : MonoBehaviour {

    public GameObject visualizationPrefab;

    public uint distanceInUnits;
    public uint intervalsPerUnit;

    void Start()
    {
        //Draw down X & Z
        for (int j = 0; j <= (distanceInUnits * intervalsPerUnit); j++)
        {
            for (int i = 0; i < (distanceInUnits * intervalsPerUnit); i++)
            {
                Instantiate(visualizationPrefab,
                    new Vector3((float)i / intervalsPerUnit, Perlin.Noise((float)i / intervalsPerUnit, (float)j / intervalsPerUnit), (float)j / intervalsPerUnit),
                    Quaternion.identity,
                    transform);
            }
        }

        for (int i = 0; i <= distanceInUnits; i++)
        {
            for (int k = 0; k <= distanceInUnits; k++)
            {
                for (int j = -10; j <= 10; j++)
                {
                    GameObject g = (GameObject)Instantiate(visualizationPrefab,
                        new Vector3(i, (float)j / 10, k),
                        Quaternion.identity,
                        transform);
                    g.transform.localScale = Vector3.one / 50;
                }
            }
        }

    }
}
