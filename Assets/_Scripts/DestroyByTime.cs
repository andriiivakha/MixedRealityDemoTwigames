using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float lifeTimeSeconds = 10f;

    private float destroyTime;

    private void Start() => destroyTime = Time.time + lifeTimeSeconds;

    private void Update()
    {
        if (Time.time > destroyTime)
            Destroy(gameObject);
    }
}
