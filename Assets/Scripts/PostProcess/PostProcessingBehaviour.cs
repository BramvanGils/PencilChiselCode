using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine;
using System;

public class PostProcessingBehaviour : MonoBehaviour
{
    private Vignette vignette;
    private FilmGrain filmGrain;
    private DepthOfField depthOfField;

    [SerializeField] private PostProcessConfig[] configs;

    private int intensity;

    public float transitionTime = 1f;

    private void Awake()
    {
        Volume volume = GetComponent<Volume>();

        volume.sharedProfile.TryGet<Vignette>(out vignette);
        volume.sharedProfile.TryGet<FilmGrain>(out filmGrain);
        volume.sharedProfile.TryGet<DepthOfField>(out depthOfField);
    }

    private void Start()
    {
        vignette.color.SetValue(new NoInterpColorParameter(configs[0].vignetteColor, true));
        vignette.intensity.SetValue(new NoInterpFloatParameter(configs[0].vignetteIntensity, true));
        vignette.smoothness.SetValue(new NoInterpFloatParameter(configs[0].vignetteSmoothness, true));
        depthOfField.gaussianEnd.SetValue(new NoInterpFloatParameter(configs[0].gausianEnd, true));
        filmGrain.intensity.SetValue(new NoInterpFloatParameter(configs[0].filmGrainIntensity, true));
    }

    public void IncreaseIntensity()
    {
        if (intensity < configs.Length)
        {
            StartCoroutine(Transistion(configs[intensity], configs[intensity + 1]));
            intensity++;
        }
    }

    private float timer;
    private IEnumerator Transistion(PostProcessConfig configStart, PostProcessConfig configEnd)
    {
        timer = 0;
        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp(timer / transitionTime, 0, 1);
            
            vignette.color.SetValue(new NoInterpColorParameter(Color.Lerp(configStart.vignetteColor, configEnd.vignetteColor, t) , true));
            vignette.intensity.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.vignetteIntensity, configEnd.vignetteIntensity, t) , true));
            vignette.smoothness.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.vignetteSmoothness, configEnd.vignetteSmoothness, t), true));
            depthOfField.gaussianEnd.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.gausianEnd, configEnd.gausianEnd, t), true));
            filmGrain.intensity.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.filmGrainIntensity, configEnd.filmGrainIntensity, t), true));

            yield return null;
        }
    }

    private void OnApplicationQuit()
    {
        Start();
    }
}
