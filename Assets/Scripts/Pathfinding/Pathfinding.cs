using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding instance;

    private void Awake()
    {
        instance = this;
    }

    #region BFS
    public List<GridNode> CalculateBFS(GridNode startingNode, GridNode goalNode)
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
        Queue<GridNode> frontier = new Queue<GridNode>();
        frontier.Enqueue(startingNode); //De entrada nos guardamos el nodo donde empezamos

        Dictionary<GridNode, GridNode> cameFrom = new Dictionary<GridNode, GridNode>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

        while (frontier.Count > 0) //Mientras no este vacio
        {
            GridNode current = frontier.Dequeue();//Para que me de el siguiente nodo

            if(current == goalNode) 
            {
                List<GridNode> path = new List<GridNode>();

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

        return new List<GridNode>(); //Si no encuentra camino o no hay meta, devuelve una lista vacia
    }

    //Para verlo VISUALMENTE
    public IEnumerator CoroutineCalculateBFS(GridNode startingNode, GridNode goalNode)
    {
        
        Queue<GridNode> frontier = new Queue<GridNode>();
        frontier.Enqueue(startingNode); //De entrada nos guardamos el nodo donde empezamos

        Dictionary<GridNode, GridNode> cameFrom = new Dictionary<GridNode, GridNode>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

        while (frontier.Count > 0) //Mientras no este vacio
        {
            GridNode current = frontier.Dequeue();//Para que me de el siguiente nodo
            current.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            yield return new WaitForSeconds(0.05f);

            if (current == goalNode)
            {
                List<GridNode> path = new List<GridNode>();

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

    #region Dijkstra
    public List<GridNode> CalculateDijkstra(GridNode startingNode, GridNode goalNode)
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
        PriorityQueue<GridNode> frontier = new PriorityQueue<GridNode>();
        frontier.Enqueue(startingNode, 0); //De entrada nos guardamos el nodo donde empezamos y su valor

        Dictionary<GridNode, GridNode> cameFrom = new Dictionary<GridNode, GridNode>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

                             //costSoFar
        Dictionary<GridNode,int> costoHastaAhora = new Dictionary<GridNode, int>();
        costoHastaAhora.Add(startingNode, 0);

        while (frontier.Count > 0) //Mientras no este vacio
        {
            GridNode nodoActual = frontier.Dequeue();//Para que me de el siguiente nodo

            if (nodoActual == goalNode)
            {
                List<GridNode> path = new List<GridNode>();

                while (nodoActual != startingNode)
                {
                    path.Add(nodoActual);
                    nodoActual = cameFrom[nodoActual]; //current va a pasar a ser el nodo donde proviene. EJ (el nodo 5, proviene de 1) current = 1;
                }

                path.Reverse(); //La lista la doy vuelta

                return path;
            }

            foreach (var node in nodoActual.GetNeighbors())
            {
                if (node.blocked)
                    continue;

               int newCost = costoHastaAhora[nodoActual] + node.cost; //Nodo con su respectivo costo + costo del nodo vecino

                if(!costoHastaAhora.ContainsKey(node))//Si no tengo ese nodo en mi diccionario lo agrego
                {
                    if (!frontier.ContainsKey(node))
                        frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom.Add(node, nodoActual);    //Me agrego ese nodo y de donde vengo
                    costoHastaAhora.Add(node, newCost);   // "     "   "    "  y ese costo
                }
                else if (costoHastaAhora[node] > newCost) //Si el costo del nodo es mayor al nuevo costo
                {
                    if (!frontier.ContainsKey(node))
                        frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom[node] = nodoActual;    
                    costoHastaAhora[node] = newCost;
                }
            }
        }

        return new List<GridNode>(); //Si no encuentra camino o no hay meta, devuelve una lista vacia
    }

    public IEnumerator CoroutineCalculateDijkstra(GridNode startingNode, GridNode goalNode)
    {
        //RECORRIDO EN  C#
        PriorityQueue<GridNode> frontier = new PriorityQueue<GridNode>();
        frontier.Enqueue(startingNode, 0); //De entrada nos guardamos el nodo donde empezamos y su valor

        Dictionary<GridNode, GridNode> cameFrom = new Dictionary<GridNode, GridNode>();
        cameFrom.Add(startingNode, null); //(Punto de partida, no proviene de ningun lado)

        //Costo hasta ahora
        Dictionary<GridNode, int> costSoFar = new Dictionary<GridNode, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0) //Mientras no este vacio
        {
            GridNode nodoActual = frontier.Dequeue();//Para que me de el siguiente nodo

            nodoActual.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
            yield return new WaitForSeconds(.05f);

            if (nodoActual == goalNode)
            {
                List<GridNode> path = new List<GridNode>();

                while (nodoActual != startingNode)
                {
                    path.Add(nodoActual);
                    nodoActual = cameFrom[nodoActual]; //current va a pasar a ser el nodo donde proviene. EJ (el nodo 5, proviene de 1) current = 1;

                    nodoActual.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                    yield return new WaitForSeconds(.05f);

                }

                path.Reverse(); //La lista la doy vuelta

                break;
            }

            foreach (var node in nodoActual.GetNeighbors())
            {
                if (node.blocked)
                    continue;

                int newCost = costSoFar[nodoActual] + node.cost; //Nodo con su respectivo costo + costo del nodo vecino

                if (!costSoFar.ContainsKey(node))//Si no tengo ese nodo en mi diccionario lo agrego
                {
                    frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom.Add(node, nodoActual);    //Me agrego ese nodo y de donde vengo
                    costSoFar.Add(node, newCost);   // "     "   "    "  y ese costo
                }
                else if (costSoFar[node] > newCost)
                {
                    frontier.Enqueue(node, newCost); //Me guardo en el diccionario el nodo con su nuevo costo
                    cameFrom[node] = nodoActual;
                    costSoFar[node] = newCost;
                }

                nodoActual.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan;
                yield return new WaitForSeconds(.05f);

            }
        }
    }
    #endregion

    #region AStar
    public List<GridNode> CalculateAStar(GridNode startingNode, GridNode goalNode)
    {
        PriorityQueue<GridNode> frontier = new PriorityQueue<GridNode>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<GridNode, GridNode> cameFrom = new Dictionary<GridNode, GridNode>();
        cameFrom.Add(startingNode, null);

        Dictionary<GridNode, int> costSoFar = new Dictionary<GridNode, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            GridNode current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<GridNode> path = new List<GridNode>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];
                }

                path.Reverse();
                return path;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (item.blocked)
                    continue;

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
        return new List<GridNode>();
    }

    public IEnumerator CoroutineCalculateAStar(GridNode startingNode, GridNode goalNode)
    {

        PriorityQueue<GridNode> frontier = new PriorityQueue<GridNode>();
        frontier.Enqueue(startingNode, 0);

        Dictionary<GridNode, GridNode> cameFrom = new Dictionary<GridNode, GridNode>();
        cameFrom.Add(startingNode, null);

        Dictionary<GridNode, int> costSoFar = new Dictionary<GridNode, int>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            GridNode current = frontier.Dequeue();

            if (current == goalNode)
            {
                List<GridNode> path = new List<GridNode>();

                while (current != startingNode)
                {
                    path.Add(current);
                    current = cameFrom[current];

                    yield return new WaitForSeconds(0.1f);
                    current.gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
                }

                break;
            }

            foreach (var item in current.GetNeighbors())
            {
                if (item.blocked)
                    continue;

                int newCost = costSoFar[current] + item.cost;
                float priority = newCost + Vector3.Distance(item.transform.position, goalNode.transform.position);

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

                yield return new WaitForSeconds(0.02f);
                current.gameObject.GetComponent<MeshRenderer>().material.color = Color.cyan; 
            }
        }
    }
    #endregion

    #region Theta AStar
    public List<GridNode> CalculateThetaStar(GridNode startingNode, GridNode goalNode) //Me borra los nodos q estan de más en el recorrido
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

    public IEnumerator CoroutineCalculateThetaStar(GridNode startingNode, GridNode goalNode)
    {
        var listNode = CalculateAStar(startingNode, goalNode);

        foreach (var item in listNode)
        {
            GameManager.Instance.ChangeColor(item, Color.blue);
        }

        int current = 0;

        while (current + 2 < listNode.Count)
        {
            GameManager.Instance.ChangeColor(listNode[current], Color.red);
            GameManager.Instance.ChangeColor(listNode[current + 2], Color.green);

            yield return new WaitForSeconds(0.7f);

            if (GameManager.Instance.InLineOfSight(listNode[current].transform.position, listNode[current + 2].transform.position))
            {
                GameManager.Instance.ChangeColor(listNode[current + 1], Color.white);
                listNode.RemoveAt(current + 1);
            }
            else
                current++;
        }
    }

    #endregion
}
