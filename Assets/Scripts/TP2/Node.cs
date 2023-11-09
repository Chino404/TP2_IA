using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    List<Node> _neighbors = new List<Node>();
    public bool blocked = default;

    Grid _grid; //Para poder llamar a sus metodo "GetNode"
    int _x, _y; //Para que cada nodo sepa su posicion para dsp sumarle o restarle y conocer sus vecinos

    //Metodo para que el nodo pueda conocer su posicion en escena
    public void Intializaze( Grid grid,int x, int y) 
    {
        _grid = grid;
        _x = x;
        _y = y;
    }

    /// <summary>
    /// ME DA LA LISTA DE SUS VECINOS
    /// </summary>
    /// <returns></returns>
    public List<Node> GetNeighbors()
    {
        if(_neighbors.Count > 0) //Si ya tengo un nodo(vecino) en mi lista, la devuelvo, porque quiere decir que ya hice este paso
            return _neighbors;

        //Sino empiezo a pedir los nodos vecinos

            //actual
        Node current = _grid.GetNode(_x -1 , _y); //Left

        if(current != null)
            _neighbors.Add(current);

        current = _grid.GetNode(_x + 1, _y); //Right

        if (current != null)
            _neighbors.Add(current);

        current = _grid.GetNode(_x, _y + 1); //Up

        if (current != null)
            _neighbors.Add(current);

        current = _grid.GetNode(_x, _y - 1); //Down 

        if (current != null)
            _neighbors.Add(current);

        return _neighbors; //Devuelvo la lista
    }

    private void OnMouseOver() //Se ejecuta este metodo cuando detecte el mouse, cuando lo pase por arriba
    {
        if(Input.GetMouseButtonDown(0)) //Click Izquierdo
        {
            GameManager.Instance.SetStartingNode(this);
        }

        if(Input.GetMouseButtonDown(1)) //Click Derecho
        {
            GameManager.Instance.SetGoalNode(this);
        }

        if (Input.GetMouseButtonDown(2)) //Click Rueda
        {
            blocked = !blocked;
                                                    //Si esta bloqueado    sino
            GetComponent<Renderer>().material.color = blocked ? Color.gray : Color.white; //Bloqueo
        }
    }
}
