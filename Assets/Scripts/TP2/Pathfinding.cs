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

    #region BFS
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

    //Para verlo VISUALMENTE
    public IEnumerator CoroutineCalculateBFS(Node startingNode, Node goalNode)
    {
        
        Queue<Node> frontier = new Queue<Node>();
        frontier.Enqueue(startingNode); //De entrada nos guardamos el nodo donde empezamos

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

        while (frontier.Count > 0) //Mientras no este vacio
        {
            Node current = frontier.Dequeue();//Para que me de el siguiente nodo
            current.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            yield return new WaitForSeconds(0.05f);

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    current.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    yield return new WaitForSeconds(0.05f);

                    path.Add(current);
                    current = cameFrom[current]; //current va a pasar a ser el nodo donde proviene. EJ (el nodo 5, proviene de 1) current = 1;
                }

                path.Reverse(); //La lista la doy vuelta

                break;
            }

            foreach (var node in current.GetNeighbors())
            {
                if (!node.blocked && !cameFrom.ContainsKey(node)) //Si no tiene esa key
                {
                    frontier.Enqueue(node); //Que lo agregue
                    cameFrom.Add(node, current); //Me guardo (el nodo, de donde proviene)

                    current.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
    #endregion

    public List<Node> CalculateDijkstra(Node startingNode, Node goalNode)
    {
        //Lenguaje PYTHON de esta pagina (https://www.redblobgames.com/pathfinding/a-star/introduction.html)

        /*RECORRIDO EN PYTHON
        frontier = PriorityQueue()
        frontier.put(start, 0)
        came_from = dict()
        cost_so_far = dict()
        came_from[start] = None
        cost_so_far[start] = 0
        
        while not frontier.empty():
           current = frontier.get()
        
           if current == goal:
              break
           
           for next in graph.neighbors(current):
              new_cost = cost_so_far[current] + graph.cost(current, next)
              if next not in cost_so_far or new_cost < cost_so_far[next]:
                 cost_so_far[next] = new_cost
                 priority = new_cost
                 frontier.put(next, priority)
                 came_from[next] = current*/
        
        //RECORRIDO EN  C#
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0); //De entrada nos guardamos el nodo donde empezamos y su valor

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

                         //Costo hasta ahora
        Dictionary<Node,int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0) //Mientras no este vacio
        {
            Node current = frontier.Dequeue();//Para que me de el siguiente nodo

            if (current == goalNode)
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
                if (node.blocked)
                    continue;

               int newCost = costSoFar[current] + node.cost; //Nodo con su respectivo costo + costo del nuevo nodo

                if(!costSoFar.ContainsKey(node))//Si no tengo ese nodo en mi diccionario lo agrego
                {
                    if (!frontier.ContainsKey(node))
                        frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom.Add(node, current);    //Me agrego ese nodo y de donde vengo
                    costSoFar.Add(node, newCost);   // "     "   "    "  y ese costo
                }
                else //Sino lo piso
                {
                    if (!frontier.ContainsKey(node))
                        frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom[node] = current;    
                    costSoFar[node] = newCost;
                }
            }
        }

        return new List<Node>(); //Si no encuentra camino o no hay meta, devuelve una lista vacia
    }

    public IEnumerator CoroutineCalculateDijkstra(Node startingNode, Node goalNode)
    {
        //RECORRIDO EN  C#
        PriorityQueue<Node> frontier = new PriorityQueue<Node>();
        frontier.Enqueue(startingNode, 0); //De entrada nos guardamos el nodo donde empezamos y su valor

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

        //Costo hasta ahora
        Dictionary<Node, int> costSoFar = new Dictionary<Node, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0) //Mientras no este vacio
        {
            Node current = frontier.Dequeue();//Para que me de el siguiente nodo

            current.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            yield return new WaitForSeconds(.05f);

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current]; //current va a pasar a ser el nodo donde proviene. EJ (el nodo 5, proviene de 1) current = 1;

                    current.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    yield return new WaitForSeconds(.05f);

                }

                path.Reverse(); //La lista la doy vuelta

                break;
            }

            foreach (var node in current.GetNeighbors())
            {
                if (node.blocked)
                    continue;

                int newCost = costSoFar[current] + node.cost; //Nodo con su respectivo costo + costo del nuevo nodo

                if (!costSoFar.ContainsKey(node))//Si no tengo ese nodo en mi diccionario lo agrego
                {
                    if (!frontier.ContainsKey(node))
                        frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom.Add(node, current);    //Me agrego ese nodo y de donde vengo
                    costSoFar.Add(node, newCost);   // "     "   "    "  y ese costo
                }
                else //Sino lo piso
                {
                    if(!frontier.ContainsKey(node))
                        frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom[node] = current;
                    costSoFar[node] = newCost;
                }

                current.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                yield return new WaitForSeconds(.05f);

            }
        }
    }
}
