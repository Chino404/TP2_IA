using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ManagerNodes : MonoBehaviour
{
    public Node[] nodes;

    private void Start()
    {

        //for (int i = 0; i < nodes.Length; i++)
        //{

        //    if (GameManager.Instance.InLineOfSight(nodes[i].transform.position, nodes[i + 1].transform.position))
        //    {
        //        nodes[i].neighbors.Add(nodes[i+1]);
        //    }

        //}
    }

    public Node GetNode(Vector3 pos)
    {
        return null;
    }
}
