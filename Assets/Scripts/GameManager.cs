using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #region TP1 
    [Header ("TP1")]
    public Hunter hunter;
    public List<Boid> boids = new List<Boid>();
    public int width, height; //anchura y altura

    [Range(0,4f)]
    public float weightSeparation, weightAlignment, weightCohesion; //El peso que va a tener cada metodo. Cual quiero que sea mas prioritario

    //Aplicar Limites
    public Vector3 ApplyBounds(Vector3 pos) 
    {
        //Me teletransporta al otro lado, el opuesto
        if (pos.x > width) pos.x = -width;
        if (pos.x <  -width) pos.x = width;
        if (pos.z > height) pos.z = -height;
        if (pos.z < -height) pos.z = height;

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
    #endregion

    #region TP2
    [Header("PATHFINDING")]
    public LayerMask maskWall;
    public PlayerGrid player;
    GridNode _startingNode;
    GridNode _goalNode;

    public event Action<Vector3> eventCall; //Evento para que se suscriban los enemigos y cuando uno lo ve, alerte al resto de su posicion
    public Transform target;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _startingNode != null && _goalNode != null)
        {

            //BFS
            //player.path = Pathfinding.instance.CalculateBFS(_startingNode, _goalNode);
            //StartCoroutine(Pathfinding.instance.CoroutineCalculateBFS(_startingNode, _goalNode));

            //Dijkstra
            //player.path = Pathfinding.instance.CalculateDijkstra(_startingNode, _goalNode);
            //StartCoroutine(Pathfinding.instance.CoroutineCalculateDijkstra(_startingNode, _goalNode));

            //AStar
            //player.path = Pathfinding.instance.CalculateAStar(_startingNode, _goalNode);
            //StartCoroutine(Pathfinding.instance.CoroutineCalculateAStar(_startingNode, _goalNode));

            //Theta AStar
            //player.SetPath(Pathfinding.instance.CalculateThetaStar(_startingNode, _goalNode));
            //StartCoroutine(Pathfinding.instance.CoroutineCalculateThetaStar(_startingNode, _goalNode));

        }
    }

    public void LookPlayer(Vector3 pos)
    {
        eventCall?.Invoke(pos);
    }


    /// <summary>
    /// Si hay algo entre medio del Raycast
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public bool InLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = end - start;

        return !Physics.Raycast(start, dir, dir.magnitude, maskWall); //Si no hay ningun objeto de con la layer "maskWall" entonces quiere decir que estoy viendo a mi objetico (por eso lo invierto para que me de True)
    }

    /// <summary>
    /// Setear el nodo donde se empieza
    /// </summary>
    /// <param name="node"></param>
    public void SetStartingNode(GridNode node)
    {
        if (_startingNode != null) //Si ya tenia un nodo guardado
            _startingNode.GetComponent<Renderer>().material.color = Color.white; //Lo cambio a blanco

        _startingNode = node;
        node.GetComponent<Renderer>().material.color = Color.red; //Inicio
        player.transform.position = _startingNode.transform.position;
    }

    /// <summary>
    /// Seteo el nodo de la meta
    /// </summary>
    /// <param name="node"></param>
    public void SetGoalNode(GridNode node)
    {
        if (_goalNode != null) //Si ya tenia un nodo guardado
            _goalNode.GetComponent<Renderer>().material.color = Color.white; //Lo cambio a blanco

        _goalNode = node;
        node.GetComponent<Renderer>().material.color = Color.green; //Inicio
    }

    public void ChangeColor(GridNode node, Color color)
    {
        node.GetComponent<Renderer>().material.color = color;
    }

    #endregion
}
