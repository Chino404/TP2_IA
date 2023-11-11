using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PLANO EN DONDE SE PUEDEN MOVER
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
    [Header("TP 2")]
    public Player player;
    public bool BFS, Dijkstra, visualization;

    Node _startingNode;
    Node _goalNode;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _startingNode != null && _goalNode != null)
        {
            if(BFS && !Dijkstra)
            {
                if(!visualization)
                    player.path = Pathfinding.instance.CalculateBFS(_startingNode, _goalNode);
                else
                    StartCoroutine(Pathfinding.instance.CoroutineCalculateBFS(_startingNode, _goalNode));
            }

            if(Dijkstra && !BFS)
            {
                if (!visualization)
                    player.path = Pathfinding.instance.CalculateDijkstra(_startingNode, _goalNode);
                else
                    StartCoroutine(Pathfinding.instance.CoroutineCalculateDijkstra(_startingNode, _goalNode));

            }
        }
    }

    /// <summary>
    /// Setear el nodo donde se empieza
    /// </summary>
    /// <param name="node"></param>
    public void SetStartingNode(Node node)
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
    public void SetGoalNode(Node node)
    {
        if (_goalNode != null) //Si ya tenia un nodo guardado
            _goalNode.GetComponent<Renderer>().material.color = Color.white; //Lo cambio a blanco

        _goalNode = node;
        node.GetComponent<Renderer>().material.color = Color.green; //Inicio
    }

    #endregion
}
