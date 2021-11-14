using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RobotArm : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;

    [SerializeField]private GameObject[] magentaObjects;
    [SerializeField] private Player player;
    [SerializeField] private GameObject eye;

    private bool active;
    private bool insertedMagenta;

    [SerializeField] private AudioClip bootClip;
    [SerializeField] private AudioClip insertClip;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetTransparency(0);
    }

    public void Activate()
    {
        if (!active)
        {
            animator.SetTrigger("Activate");
            audioSource.clip = bootClip;
            audioSource.Play();
            active = true;
        }
    }

    public void Insert()
    {
        if (active && !insertedMagenta)
        {
            player.OnInsertionStart();
            animator.SetTrigger("Insert");
            audioSource.clip = insertClip;
            audioSource.Play();
        }
    }

    public void OnInsertFinish()
    {
        insertedMagenta = true;
        player.OnInsertionEnd();
        StartCoroutine(RevealMagenta());
    }

    private float timer;
    private IEnumerator RevealMagenta()
    {
        timer = 0;
        while (timer < 10)
        {
            timer += Time.deltaTime;

            SetTransparency(Mathf.Pow(timer / 10, 4f));

            yield return null;
        }
    }

    private void SetTransparency(float transparency)
    {
        foreach(GameObject o in magentaObjects)
        {
            Material m = o.GetComponent<Renderer>().material;
            m.SetFloat("_Transparency", transparency);
        }
    }

    private void OnApplicationQuit()
    {
        SetTransparency(1);
    }

    public void DestroyEye()
    {
        Destroy(eye.gameObject);
    }
}
