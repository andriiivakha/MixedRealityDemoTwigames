using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private GameObject miniBrickPrefab;
    [SerializeField] private GameObject tapes;

    private MeshRenderer mesh;
    private bool isAlive = true;

    public bool IsAlive => isAlive;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        tapes.SetActive(false);
    }

    public void Destroy()
    {
        if (!isAlive)
            return;
        for (int i = 0; i < 5; i++)
        {
            GameObject miniBrick = Instantiate(miniBrickPrefab, transform.position, Quaternion.identity);
            var rb = miniBrick.GetComponent<Rigidbody>();
            Vector3 randomForce = Random.insideUnitSphere * 4;
            rb.AddForce(randomForce, ForceMode.Impulse);
            Vector3 randomTorque = Random.insideUnitSphere * 4;
            rb.AddTorque(randomTorque, ForceMode.Impulse);
        }
        mesh.enabled = false;
        tapes.SetActive(false);
        isAlive = false;
    }

    public void Restore()
    {
        if (isAlive)
            return;

        mesh.enabled = true;
        tapes.SetActive(true);
        isAlive = true;
    }
}
