using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ManagerNodes : MonoBehaviour
{
    public static ManagerNodes Instance;
    public Node[] nodes;

    private void Awake()
    {
        Instance = this;
    }

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

    /// <summary>
    /// El nodo mas cercano al Player
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Node GetNodeProxPlayer(Vector3 pos)
    {
        var disProx = Mathf.Infinity;
        Node nodeMasCercano = default;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (GameManager.Instance.InLineOfSight(nodes[i].transform.position, pos)) //Pregunto si hay algo que interfiera entre el nodo y la pos del player
            {
                var dis = pos - nodes[i].transform.position;

                if (dis.magnitude > disProx)
                {
                    disProx = dis.magnitude;
                    nodeMasCercano = nodes[i];
                }
            }

        }

        return nodeMasCercano;
    }
}
