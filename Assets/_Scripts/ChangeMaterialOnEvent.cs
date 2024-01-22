using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeMaterialOnEvent : MonoBehaviour
{
    private const float APPROACHED_DISTANCE = 10.33f;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material approachedMaterial;
    [SerializeField] private GameObject canvasUI;
    [SerializeField] private TMP_Text text;

    private MeshRenderer mesh;

    private void Awake() => mesh = GetComponent<MeshRenderer>();

    private void Start()
    {
        GameController.Instance.OnSceneLoaded += Instance_OnSceneLoaded;
        GameController.Instance.DebugVisionActivated += ShowApproachedMaterial;
    }

    private void Instance_OnSceneLoaded()
    {
        var data = GetComponentInParent<OVRSemanticClassification>();
        if (data != null && data.Labels != null && data.Labels.Count > 0)
        {
            text.text = data.Labels[0];
        }
    }

    private void ShowApproachedMaterial(bool show)
    {
        if (show && mesh.material != approachedMaterial)
            mesh.material = approachedMaterial;
        else if (!show && mesh.material != defaultMaterial)
            mesh.material = defaultMaterial;
        canvasUI.SetActive(show);
    }

    private void OnDestroy()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.OnSceneLoaded -= Instance_OnSceneLoaded;
            GameController.Instance.DebugVisionActivated += ShowApproachedMaterial;
        }
    }
}
