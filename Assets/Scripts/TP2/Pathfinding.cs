using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Node> CalculateBFS(Node startingNode, Node goalNode)
    {
        /* Queue y HashSet
        Queue<int> queue = new Queue<int>(); //Queue sirve para que me devuelva los valores en el orden en que fueron agregados
        queue.Enqueue(10); //Agregar
        queue.Peek(); //Te devuevlo el que sigue
        queue.Dequeue(); //Te devuelvo el que sigue y lo borro

        HashSet<Node> reache = new HashSet<Node>(); //Sirve solo para guardar valores, no repetidos, y para solo ver si lo contiene
        reached.Contains(); //Si lo contiene
        */

        //Lenguaje PYTHON de esta pagina (https://www.redblobgames.com/pathfinding/a-star/introduction.html)

        /*RECORRIDO EN PYTHON
        frontier = Queue()
        frontier.put(start )
        came_from = dict() # path A->B is stored as came_from[B] == A
        came_from[start] = None

        while not frontier.empty():
           current = frontier.get()
           for next in graph.neighbors(current):
              if next not in came_from:
                 frontier.put(next)
                 came_from[next] = current*/

        //RECORRIDO EN  C#
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode); //De entrada nos guardamos el nodo donde empezamos

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

        while (frontier.Count > 0) //Mientras no este vacio
        {
            Node current = frontier.Dequeue();//Para que me de el siguiente nodo

            if(current == goalNode) 
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current]; //current va a pasar a ser el nodo donde proviene. EJ (el nodo 5, proviene de 1) current = 1;
                }

                path.Reverse(); //La lista la doy vuelta

                return path;
            }

            foreach (var node in current.GetNeighbors())
            {
                if ( !node.blocked && !cameFrom.ContainsKey(node)) //Si no tiene esa key
                {
                    frontier.Enqueue(node); //Que lo agregue
                    cameFrom.Add(node, current); //Me guardo (el nodo, de donde proviene)
                }
            }
        }

        return new List<Node>(); //Si no encuentra camino o no hay meta, devuelve una lista vacia
    }
}
