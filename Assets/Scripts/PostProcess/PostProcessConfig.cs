using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PostProcessConfig", menuName = "ScriptableObjects/PostProcessConfig", order = 1)]
[Serializable] public class PostProcessConfig : ScriptableObject
{
    public Color vignetteColor;
    public float vignetteIntensity;
    public float vignetteSmoothness;
    public float filmGrainIntensity;
    public float gausianEnd;
}
