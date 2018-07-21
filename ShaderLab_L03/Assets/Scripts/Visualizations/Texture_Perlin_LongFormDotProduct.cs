using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture_Perlin_LongFormDotProduct : MonoBehaviour {

    public Vector3[,] cornerOrigins;
    public Vector3[,] gradientVectorDirections;

    public Vector3 cursor;

    public Texture2D myTex;

    void Start()
    {
        Init();
        CornerVectorSetup();
        MakeATexture();
        DisplayTexture();
    }

    void Init()
    {
        cornerOrigins = new Vector3[101, 101];
        gradientVectorDirections = new Vector3[cornerOrigins.GetLength(0), cornerOrigins.GetLength(1)];
        cursor = Vector3.zero;
    }

    #region Generate Gradient Vector Field
    // Goes to whole int positions and calculates a gradient vector
    // Gradient vector is a 2D vector along the x-z plane
    // The direction of the vector will be FR, FL, RR, or RL
    // The direction is determined by the hash operation of the x an z coordinate
    // A "randomization" from the hex occurs based on it's x position first
    // That hash value gets increased by its y position
    // The resultant is limited and looped ("&") to a max inclusive value of 255
    // That value is used as a second hash lookup index
    // The gradient values are determined the way that Perlin does
    // Gradient X is evaluated based on an even/odd bitwise operation
    // Gradient Z is evaluated based on a "& 2" bitwise operation
    // Those gradient vectors are stored in a 2D array for later evaluation
    void CornerVectorSetup()
    {
        for (int i = 0; i < cornerOrigins.GetLength(0); i++)
        {
            for (int j = 0; j < cornerOrigins.GetLength(1); j++)
            {
                cornerOrigins[i, j] = new Vector3(i, 0, j);

                int hashIndex = perm[i] + j & 0xff;

                int gradientX = (perm[hashIndex] & 1) == 0 ? 1 : -1;
                int gradientZ = (perm[hashIndex] & 2) == 0 ? 1 : -1;
                gradientVectorDirections[i, j] = new Vector3(gradientX, 0, gradientZ);
            }
        }
    }
    #endregion

    #region Calculations to Texture
    void MakeATexture()
    {
        myTex = new Texture2D(1000, 1000);
        for (int i = 0; i < (myTex.width - 1); i++)
        {
            for (int j = 0; j < (myTex.height - 1); j++)
            {
                Vector3 pointOfAnalysis = new Vector3((float)i / 10, 0, (float)j / 10);
                //Debug.Log(pointOfAnalysis);
                myTex.SetPixel(i, j, PointToColor(pointOfAnalysis));             
            }
        }
        myTex.Apply();
        myTex.filterMode = FilterMode.Point;
    }
    #endregion

    #region Make Generated Texture Visible
    void DisplayTexture()
    {
        GameObject quad = Instantiate(Resources.Load<GameObject>("Quad"), new Vector3(50, 0, 50), Quaternion.Euler(new Vector3(90, 0, 0)));
        quad.transform.localScale = Vector3.one * 100;

        Material mat = Resources.Load<Material>("EmptyMat");

        quad.GetComponent<MeshRenderer>().material = mat;
        mat.SetTexture("_MainTex", myTex);
    }
    #endregion

    #region Calcuations
    Color PointToColor(Vector3 poa)
    {
        // Name change for legibility
        // The poa is the position that is being analyzed for a color value
        Vector3 floatPointOfAnalysis = poa; 

        // Establishing the bounds
        // Taking each axis position and casting it to an int makes it a whole number
        // Done for each axis, the result is the lower bound whole number reference point
        // These points correspond to the position and the index for gradient vector lookups
        Vector3 corner_index0_lowerLeft;
        corner_index0_lowerLeft.x = (int)floatPointOfAnalysis.x;
        corner_index0_lowerLeft.y = (int)floatPointOfAnalysis.y;
        corner_index0_lowerLeft.z = (int)floatPointOfAnalysis.z;

        // Adding 1 to the x value gives the lower right bound
        Vector3 corner_index1_lowerRight = corner_index0_lowerLeft;
        corner_index1_lowerRight.x++;

        // Adding 1 to the z value gives the upper left bound
        Vector3 corner_index2_upperLeft = corner_index0_lowerLeft;
        corner_index2_upperLeft.z++;

        // Adding 1 to the x and z gives the upper right bound
        Vector3 corner_index3_upperRight = corner_index0_lowerLeft;
        corner_index3_upperRight.x++;
        corner_index3_upperRight.z++;

        // Calculates and stores the relative vectors from the corners to the POA
        Vector3 relativeVector_index0_POA_LL = (floatPointOfAnalysis - corner_index0_lowerLeft);
        Vector3 relativeVector_index1_POA_LR = (floatPointOfAnalysis - corner_index1_lowerRight);
        Vector3 relativeVector_index2_POA_UL = (floatPointOfAnalysis - corner_index2_upperLeft);
        Vector3 relativeVector_index3_POA_UR = (floatPointOfAnalysis - corner_index3_upperRight);

        // Takes the dot product of the relative vector and gradients at the respective corners
        // Dot product is commutative so order does not matter
        // Because Perlin noise doesn't wan't dead spots, vectors are not normalized to preserve coverage
        float dot0 = Vector3.Dot(relativeVector_index0_POA_LL, 
            gradientVectorDirections[(int)(corner_index0_lowerLeft.x), (int)(corner_index0_lowerLeft.z)]);
        float dot1 = Vector3.Dot(relativeVector_index1_POA_LR, 
            gradientVectorDirections[(int)(corner_index1_lowerRight.x), (int)(corner_index1_lowerRight.z)]);
        float dot2 = Vector3.Dot(relativeVector_index2_POA_UL, 
            gradientVectorDirections[(int)(corner_index2_upperLeft.x), (int)(corner_index2_upperLeft.z)]);
        float dot3 = Vector3.Dot(relativeVector_index3_POA_UR, 
            gradientVectorDirections[(int)(corner_index3_upperRight.x), (int)(corner_index3_upperRight.z)]);

        // Take a lerp across the bottom and top along the x-axis based on the x position
        // Take a lerp across from the bottom to top along the z-axis based on the z position
        float dotAcrossTop = Mathf.Lerp(dot2, dot3, Fade(floatPointOfAnalysis.x - (int)floatPointOfAnalysis.x));
        float dotAcrossBottom = Mathf.Lerp(dot0, dot1, Fade(floatPointOfAnalysis.x - (int)floatPointOfAnalysis.x));
        float dotUpAndDown = Mathf.Lerp(dotAcrossBottom, dotAcrossTop, Fade(floatPointOfAnalysis.z - (int)floatPointOfAnalysis.z));

        // Takes the computed float value and applies to color
        // Desired output is gray-scale, so same value is applied to RGB
        return new Color(DotFloatToColorFloat(dotUpAndDown), DotFloatToColorFloat(dotUpAndDown), DotFloatToColorFloat(dotUpAndDown));
    }

    float DotFloatToColorFloat(float dotVal)
    {
        // Perlin noise using Dot Product results is a output -1 to 1
        // Colors, in Unity, take fractions from 0 to 1
        // Extrapolates a value from -1 to 1 and converts it to a corresponding value between 0 to 1
        return (dotVal + 1) / 2;
    }
    #endregion

    #region Additional Info in Scene View
    void Update()
    {
        DrawField();
        CalcAndDrawToCursor();
    }

    void DrawField()
    {
        for (int i = 0; i < cornerOrigins.GetLength(0); i++)
        {
            for (int j = 0; j < cornerOrigins.GetLength(1); j++)
            {
                Debug.DrawLine(cornerOrigins[i, j], cornerOrigins[i, j] + Vector3.up * .1f, Color.blue);
                Debug.DrawLine(cornerOrigins[i, j], cornerOrigins[i, j] + gradientVectorDirections[i, j] * .2f, Color.blue);
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

        Vector3 line0 = Vector3.Normalize(cursor - new Vector3((int)cursor.x, 0, (int)cursor.z));
        Vector3 line1 = Vector3.Normalize(cursor - new Vector3((int)cursor.x + 1, 0, (int)cursor.z));
        Vector3 line2 = Vector3.Normalize(cursor - new Vector3((int)cursor.x, 0, (int)cursor.z + 1));
        Vector3 line3 = Vector3.Normalize(cursor - new Vector3((int)cursor.x + 1, 0, (int)cursor.z + 1));

        float dot0 = Vector3.Dot(gradientVectorDirections[(int)cursor.x, (int)cursor.z], line0);
        float dot1 = Vector3.Dot(gradientVectorDirections[(int)cursor.x + 1, (int)cursor.z], line1);
        float dot2 = Vector3.Dot(gradientVectorDirections[(int)cursor.x, (int)cursor.z + 1], line2);
        float dot3 = Vector3.Dot(gradientVectorDirections[(int)cursor.x + 1, (int)cursor.z + 1], line3);

        Debug.DrawLine(corner0, corner0 + Vector3.up * dot0);
        Debug.DrawLine(corner1, corner1 + Vector3.up * dot1);
        Debug.DrawLine(corner2, corner2 + Vector3.up * dot2);
        Debug.DrawLine(corner3, corner3 + Vector3.up * dot3);

        float dotAcrossTop = Mathf.Lerp(dot2, dot3, cursor.x - (int)cursor.x);
        float dotAcrossBottom = Mathf.Lerp(dot0, dot1, cursor.x - (int)cursor.x);
        float dotUpAndDown = Mathf.Lerp(dotAcrossBottom, dotAcrossTop, cursor.z - (int)cursor.z);

        Color lineColor = new Color(DotFloatToColorFloat(dotUpAndDown), DotFloatToColorFloat(dotUpAndDown), DotFloatToColorFloat(dotUpAndDown));

        Debug.DrawLine(cursor, cursor + Vector3.up * DotFloatToColorFloat(dotUpAndDown) * 5, lineColor);
    }
    #endregion

    #region Functionality from Perlin for Symmetry

    //Fade takes a float "t" and returns 6t^5 - 15t^4 - 10t^3
    static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    static int[] perm = {
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        151
    };

    #endregion
}
