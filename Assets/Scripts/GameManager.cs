using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Grimace grimace;
    [SerializeField] private PostProcessingBehaviour postProcessing;

    public void Trigger(string triggerName)
    {
        switch (triggerName)
        {
            case "GaspTrigger":
                grimace.Gasp();
                break;
            case "AttackTrigger":
                grimace.Attack();
                ResetGame();
                break;
            case "":

                break;
            default:
                Debug.Log("Unrecognized trigger: " + triggerName);
                break;
        }
    }

    private void ResetGame()
    {
        StartCoroutine(TransitionGameEnd());
    }

    private float timer;
    private IEnumerator TransitionGameEnd()
    {
        timer = 0;
        while(timer < 5)
        {
            timer += Time.deltaTime;
            postProcessing.BlackenScreen(timer);
            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        postProcessing.BlackenScreen(0);
    }
}
