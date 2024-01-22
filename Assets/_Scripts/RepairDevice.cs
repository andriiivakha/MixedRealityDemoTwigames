using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class RepairDevice : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private List<Brick> bricks = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14) // destructable
        {
            var brick = other.gameObject.GetComponent<Brick>();
            if (!bricks.Contains(brick))
            {
                bricks.Add(brick);
            }  
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 14) // destructable
        {
            bricks.Remove(other.gameObject.GetComponent<Brick>());
        }
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger) && bricks.Count > 0 && bricks.Any(x => !x.IsAlive))
        {
            var brick = bricks.FirstOrDefault(x => !x.IsAlive);
            brick.Restore();
        }
    }
}
