using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    void Start()
    {
        GameController.Instance.OnSceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded()
    {
        Mesh mesh = GetComponentInParent<MeshFilter>().mesh;
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        Vector3 newSize = mesh.bounds.size;
        newSize.z = .05f;
        boxCollider.size = newSize;
        boxCollider.center = mesh.bounds.center;
    }

    private void OnDestroy()
    {
        if (GameController.Instance != null)
            GameController.Instance.OnSceneLoaded -= OnSceneLoaded;
    }
}
