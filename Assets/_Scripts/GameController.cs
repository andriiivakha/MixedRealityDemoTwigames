using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public event Action OnSceneLoaded;
    public event Action<bool> DebugVisionActivated;

    [SerializeField] private MyCustomSceneModelLoader sceneModelLoader;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject environmentGameObject;
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private Transform[] enemySpawnPoints;

    private bool isDebugVisionActive = false;

    public Transform PlayerTransform => playerTransform;

    protected override void Awake()
    {
        base.Awake();
        environmentGameObject.SetActive(false);
    }

    private void Start()
    {
        sceneModelLoader.OnSceneLoaded += OnSceneLoadedEvent;
    }

    private void OnSceneLoadedEvent()
    {
        Invoke(nameof(StartGame), .25f);
    }

    private void StartGame()
    {
        OnSceneLoaded?.Invoke();
        environmentGameObject.SetActive(true);
        Instantiate(enemyPrefab, enemySpawnPoints[0].position, Quaternion.identity);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            isDebugVisionActive = !isDebugVisionActive;
            DebugVisionActivated?.Invoke(isDebugVisionActive);
        }
    }

    protected override void OnDestroy()
    {
        sceneModelLoader.OnSceneLoaded -= OnSceneLoaded;
        base.OnDestroy();
    }
}
