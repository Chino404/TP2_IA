using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLANO EN DONDE SE PUEDEN MOVER
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<Boid> boids = new List<Boid>();
    public int width, height; //anchura y altura

    private void Awake()
    {
        Instance = this;
    }

    //Aplicar Limites
    public Vector3 ApplyBounds(Vector3 pos) 
    {
        //Me teletransporta al otro lado, el opuesto
        if (pos.x > width) pos.x = -width;
        if(pos.x <  -width) pos.x = width;
        if(pos.z > height) pos.z = -height;
        if(pos.z < -height) pos.z = height;

        return pos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        //Los dos puntos de arriba del plano
        Vector3 topLeft = new Vector3(-width, 0, height);  
        Vector3 topRight = new Vector3(width, 0, height);

        //Los dos puntos de abajo del plano
        Vector3 downRight = new Vector3(width, 0, -height);
        Vector3 downLeft = new Vector3(-width, 0, -height); 
        
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, downRight);
        Gizmos.DrawLine(downRight, downLeft);
        Gizmos.DrawLine(downLeft, topLeft);
    }
}
