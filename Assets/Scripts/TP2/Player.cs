using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public List<Node> path = new List<Node>();

    void Update()
    {
        if (path.Count > 0)
        {
            var dir = path[0].transform.position - transform.position;

            transform.position += dir.normalized * speed * Time.deltaTime;

            //longitud del vector <= 0.3f
            if (dir.magnitude <= 0.3f)
                path.RemoveAt(0);
        }
    }
}
