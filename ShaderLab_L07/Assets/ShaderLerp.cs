using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderLerp : MonoBehaviour {

    private Color _color;

    private bool _t;
    public bool toVis
    {
        get { return _t; }
        set
        {
            if (value != _t)
            {
                _t = value;
                _color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            }
        }
    }

    private float _normalizedTime = 2;

    private float _lerpTimer;

    public Material myOpacityCutoutMaterial;

	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            toVis = !toVis;

        if (toVis)
            _lerpTimer += Time.deltaTime / _normalizedTime;
        else
            _lerpTimer -= Time.deltaTime / _normalizedTime;

        _lerpTimer = Mathf.Clamp01(_lerpTimer);

        myOpacityCutoutMaterial.SetFloat("_Cutoff", _lerpTimer);
	}
}
