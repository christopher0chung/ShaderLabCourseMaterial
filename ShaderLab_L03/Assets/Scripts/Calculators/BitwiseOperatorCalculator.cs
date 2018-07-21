using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BitwiseOperatorCalculator : MonoBehaviour {

    [Header("Bitwise Operator: A & B = Output1")]
    [Header("Input Ints")]
    public int A;
    public int B;
    public int Output1;

    private string sA;
    private string sB;
    private string sOutput1;

    [Header("Values in Binary")]
    public string BinaryA;
    public string BinaryB;
    public string BinaryOutput1;

    [Header("Bitwise Operator: FloorToInt(C) & 0xff = Output2")]
    [Header("Inputs (C - Float; D - Int")]
    public float C;
    public float D;
    public int Output2;

    private string sC;
    private string sD;
    private string sOutput2;

    [Header("Values in Binary")]
    public string BinaryC;
    public string BinaryD;
    public string BinaryOutput2;

    private int counter;
    private int pseudoHash;
    private int hashAndOne;
    private int hashAndTwo;
    private int hashAndVar;
    private int _v;
    private int _andVarValue
    {
        get
        {
            return _v;
        }
        set
        {
            if (value != _v)
            {
                _v = value;
                counter = 0;
                cumulativeHashAndVar = 0;
                cumulativeHashAndOne = 0;
                cumulativeHashAndTwo = 0;
}
        }
    }
    [Header("& 2 Test")]
    [Header("Output of &2")]
    public int andVarValue;
    public float cumulativeHashAndVar;
    public float cumulativeHashAndOne;
    public float cumulativeHashAndTwo;
    public float hashAndVarRate;
    public float hashAndOneRate;
    public float hashAndTwoRate;

	void Update () {
        Output1 = A & B;

        sA = Convert.ToString(A);
        sB = Convert.ToString(B);
        sOutput1 = Convert.ToString(Output1);

        BinaryA = Convert.ToString(Convert.ToInt32(sA, 10), 2);
        BinaryB = Convert.ToString(Convert.ToInt32(sB, 10), 2);
        BinaryOutput1 = Convert.ToString(Convert.ToInt32(sOutput1, 10), 2);

        D = 0xff;

        Output2 = Mathf.FloorToInt(C) & (int)D;

        sC = Convert.ToString(Mathf.FloorToInt(C));
        sD = Convert.ToString(D);
        sOutput2 = Convert.ToString(Output2);

        BinaryC = Convert.ToString(Convert.ToInt32(sC, 10), 2);
        BinaryD = Convert.ToString(Convert.ToInt32(sD, 10), 2);
        BinaryOutput2 = Convert.ToString(Convert.ToInt32(sOutput2, 10), 2);

        counter++;
        _andVarValue = andVarValue;

        pseudoHash = UnityEngine.Random.Range(0, 256);

        hashAndOne = pseudoHash & 1;
        hashAndTwo = pseudoHash & 2;
        hashAndVar = pseudoHash & _v;

        if (hashAndOne == 0)
            cumulativeHashAndOne++;
        if (hashAndTwo == 0)
            cumulativeHashAndTwo++;
        if (hashAndVar == 0)
            cumulativeHashAndVar++;

        hashAndOneRate = (float)cumulativeHashAndOne / counter;
        hashAndTwoRate = (float)cumulativeHashAndTwo / counter;
        hashAndVarRate = (float)cumulativeHashAndVar / counter;
    }
}
