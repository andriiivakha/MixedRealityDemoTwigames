using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WallToBricks : MonoBehaviour
{
    public GameObject brickPrefab;  // Reference to the brick prefab
    public int rows = 4;            // Number of rows of bricks
    public int columns = 5;         // Number of columns of bricks
    public float brickSpacing = 0.1f;  // Spacing between bricks

    void Start()
    {
        GameController.Instance.OnSceneLoaded += ReplaceMeshWithBricks;
    }

    void ReplaceMeshWithBricks()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] verticesArray = mesh.vertices;
        List<Vector3> vertices = GetFixedVertices(verticesArray);
        float step = .4f;
        for (float row = 0; row < Mathf.Abs(vertices[0].x * 2); row += step)
        {
            for (float col = 0; col < Mathf.Abs(vertices[0].y * 2); col += step)
            {
                Vector3 spawnPoint = vertices[0] + new Vector3(row, -col, 0);
                GameObject brick = Instantiate(brickPrefab, transform.TransformPoint(spawnPoint), Quaternion.identity, transform);
                brick.transform.rotation = transform.rotation;
            }
        }
        GetComponent<MeshRenderer>().enabled = false;
    }

    private List<Vector3> GetFixedVertices(Vector3[] inputList)
    {
        List<Vector3> result = new();
        result.Add(inputList.FirstOrDefault(v => v.x < 0 && v.y > 0));
        result.Add(inputList.FirstOrDefault(v => v.x > 0 && v.y > 0));
        result.Add(inputList.FirstOrDefault(v => v.x < 0 && v.y < 0));
        result.Add(inputList.FirstOrDefault(v => v.x > 0 && v.y < 0));
        return result;
    }

    private void OnDestroy()
    {
        if (GameController.Instance != null)
        {
            GameController.Instance.OnSceneLoaded -= ReplaceMeshWithBricks;
        }
    }
}
