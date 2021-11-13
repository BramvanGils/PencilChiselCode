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

    [NonSerialized] public bool facingLeft;
    private bool inSlide = false;

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

    private void Update()
    {
        PositionVignette();
    }

    public void IncreaseIntensity()
    {
        if (intensity+1 < configs.Length)
        {
            StartCoroutine(Transistion(configs[intensity], configs[intensity + 1]));
            intensity++;
        }
    }

    public void SlideVignetteFocus(bool facignLeft)
    {
        this.facingLeft = facignLeft;
        if (!inSlide)
        {
            StartCoroutine(SlideFocus());
        }
        else slideProgress = 1 - slideProgress;
    }

    private void PositionVignette()
    {
        float x;
        float xl = (1.15f * vignette.intensity.GetValue<float>()) - .25f;
        float xr = 1 - xl;
        x = Mathf.Lerp(xl, xr, lrPos);
        vignette.center.SetValue(new NoInterpVector2Parameter(new Vector2(x, 0.5f), true));
    }

    private void OnApplicationQuit()
    {
        vignette.color.SetValue(new NoInterpColorParameter(Color.black, true));
        vignette.intensity.SetValue(new NoInterpFloatParameter(0, true));
        vignette.smoothness.SetValue(new NoInterpFloatParameter(0, true));
        depthOfField.gaussianEnd.SetValue(new NoInterpFloatParameter(1000, true));
        filmGrain.intensity.SetValue(new NoInterpFloatParameter(0, true));

    }

    private float timerA;
    private IEnumerator Transistion(PostProcessConfig configStart, PostProcessConfig configEnd)
    {
        timerA = 0;
        while (timerA < transitionTime)
        {
            timerA += Time.deltaTime;
            float t = Mathf.Clamp(timerA / transitionTime, 0, 1);

            vignette.color.SetValue(new NoInterpColorParameter(Color.Lerp(configStart.vignetteColor, configEnd.vignetteColor, t), true));
            vignette.intensity.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.vignetteIntensity, configEnd.vignetteIntensity, t), true));
            vignette.smoothness.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.vignetteSmoothness, configEnd.vignetteSmoothness, t), true));
            depthOfField.gaussianEnd.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.gausianEnd, configEnd.gausianEnd, t), true));
            filmGrain.intensity.SetValue(new NoInterpFloatParameter(Mathf.Lerp(configStart.filmGrainIntensity, configEnd.filmGrainIntensity, t), true));

            yield return null;
        }
    }

    private float slideProgress;
    private float lrPos;
    private IEnumerator SlideFocus()
    {
        inSlide = true;

        slideProgress = 0;
        while (slideProgress < 1)
        {
            slideProgress += Time.deltaTime;
            float augmentedProgress = Mathf.Pow((Mathf.Cos((slideProgress + 1) * Mathf.PI) + 1) / 2, 0.5f);
            lrPos = facingLeft ? 1 - augmentedProgress : augmentedProgress;

            yield return null;
        }

        inSlide = false;
    }

    
}
