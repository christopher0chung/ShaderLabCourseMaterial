using UnityEngine;

[ExecuteInEditMode]
public class BasicColorCalculator : MonoBehaviour {

    [Header("Input Colors")]
    public Color InputColor1;
    public Color InputColor2;

    [Header("Output Colors")]
    public Color Color1PlusColor2;
    public Color Color1TimesColor2;
    public Color DoubleColor1;
    public Color HalfColor1;
    public Color Color1_90Percent;
    public Color Color1HalfAlpha;

    private void Update()
    {
        Color1PlusColor2 = InputColor1 + InputColor2;

        Color1TimesColor2 = InputColor1 * InputColor2;

        DoubleColor1 = InputColor1 * 2;

        HalfColor1.r = InputColor1.r / 2;
        HalfColor1.g = InputColor1.g / 2;
        HalfColor1.b = InputColor1.b / 2;

        Color1_90Percent = InputColor1 * .9f;

        Color temp = InputColor1;
        temp.a *= 0.5f;
        Color1HalfAlpha = temp;
    }
}
