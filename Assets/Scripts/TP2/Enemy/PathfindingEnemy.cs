using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PathfindingEnemy : IState
{
    FSM _fsm;
    Transform[] _wayPoints;
    Transform _target;
    Enemy _enemy;

    public PathfindingEnemy(FSM fsm, Transform[] wayPoints, Transform target, Enemy enemy)
    {
        _fsm = fsm;
        _wayPoints = wayPoints;
        _target = target;
        _enemy = enemy;
    }



    public void OnEnter()
    {

       _enemy.path = CalculateThetaStar(_enemy.initialNode, _enemy.goalNode);

    }

    public void OnUpdate()
    {

        if (_enemy.path.Count > 0 )
        {

            AddForce(Seek(_enemy.path[0].transform.position));

            if (Vector3.Distance(_enemy.gameObject.transform.position, _enemy.path[0].transform.position) <= 0.3f) _enemy.path.RemoveAt(0);

            _enemy.transform.position += _enemy.velocity * Time.deltaTime;
            _enemy.transform.forward = _enemy.velocity;

        }

        else if (_enemy.path.Count <= 0 ) _fsm.ChangeState("Patrullar");
    }

    public void OnExit()
    {
        
    }

    #region AStar
    public List<Node> CalculateAStar(Node startingNode, Node goalNode)
    {
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.neighbors)
            {

                int newCost = costSoFar[current] + item.cost; //Calculo el costo como en Dijkstra
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position); //Calculo la distancia del nodo actual hasta la meta

                if (!costSoFar.ContainsKey(item))
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom.Add(item, current);
                    costSoFar.Add(item, newCost);
                }
                else if (costSoFar[item] > newCost)
                {
                    if (!frontier.ContainsKey(item))
                        frontier.Enqueue(item, priority);
                    cameFrom[item] = current;
                    costSoFar[item] = newCost;
                }
            }
        }
        return new List<Node>();
    }
    #endregion

    #region Theta AStar
    public List<Node> CalculateThetaStar(Node startingNode, Node goalNode) //Me borra los nodos q estan de más en el recorrido
    {
        var listNode = CalculateAStar(startingNode, goalNode); //Llamo a AStar

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            if (GameManager.Instance.InLineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position)) //Si puedo llegar a un nodo siguiente
            {
                listNode.RemoveAt(current + 1); //Borro el anterior nodo
            }
            else
                current++; //Sino me lo sumo
        }

        return listNode;
    }

    #endregion

    Vector3 Seek(Vector3 target)
    {
        var desired = target - _enemy.gameObject.transform.position;
        desired.Normalize();
        desired *= _enemy.maxVelocity;

        var steering = desired - _enemy.velocity;
        steering = Vector3.ClampMagnitude(steering, _enemy.maxForce);
        return steering;
    }

    public void AddForce(Vector3 dir)
    {
        _enemy.velocity += dir;

        _enemy.velocity = Vector3.ClampMagnitude(_enemy.velocity, _enemy.maxVelocity);
    }
}
