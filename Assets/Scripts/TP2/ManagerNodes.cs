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


    /// <summary>
    /// El nodo mas cercano al objetivo
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Node GetNodeProx(Vector3 pos)
    {
        var disProx = Mathf.Infinity;
        Node nodeMasCercano = default;

        for (int i = 0; i < nodes.Length; i++)
        {
            if (GameManager.Instance.InLineOfSight(nodes[i].transform.position, pos)) //Pregunto si hay algo que interfiera entre el nodo y la pos del objetivo
            {
                var dis = pos - nodes[i].transform.position;

                if (dis.magnitude < disProx)
                {
                    Debug.Log("A");
                    disProx = dis.magnitude;
                    nodeMasCercano = nodes[i];
                }
            }
        }

        return nodeMasCercano;
    }
}
