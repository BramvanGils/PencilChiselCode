using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameManager manager;

    private void OnTriggerEnter(Collider other)
    {
        manager.Trigger(gameObject.name);
    }
}
