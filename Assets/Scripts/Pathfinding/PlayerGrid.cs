using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrid : MonoBehaviour
{
    public float speed;
    public List<GridNode> path = new List<GridNode>();

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

    public void SetPath(List<GridNode> newPath)
    {
        path.Clear();

        foreach (var item in newPath)
             path.Add(item);
    }
}
