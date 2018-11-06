using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ApplyShaderTest : MonoBehaviour
{
    public Material testSSEffect;

    [Range(0, 1)]
    public float lerpyGuy;

    void OnRenderImage(RenderTexture sourceImage, RenderTexture outputTexture)
    {
        Graphics.Blit(sourceImage, outputTexture, testSSEffect);
        testSSEffect.SetFloat("_Float2", lerpyGuy);
    }
}