using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; //Dijkstra

public class GridNode : MonoBehaviour
{
    List<GridNode> _neighbors = new List<GridNode>();
    public bool blocked = default;
    #region Dijkstra
    public TextMeshProUGUI textCost; //Para ver los costos
    public int cost;
    #endregion

    Grid _grid; //Para poder llamar a sus metodo "GetNode"
    int _x, _y; //Para que cada nodo sepa su posicion para dsp sumarle o restarle y conocer sus vecinos

    //Metodo para que el nodo pueda conocer su posicion en escena
    public void Intializaze( Grid grid,int x, int y) 
    {
        _grid = grid;
        _x = x;
        _y = y;

        SetCost(1);//Dijkstra
    }

    /// <summary>
    /// ME DA LA LISTA DE SUS VECINOS
    /// </summary>
    /// <returns></returns>
    public List<GridNode> GetNeighbors()
    {
        if(_neighbors.Count > 0) //Si ya tengo un nodo(vecino) en mi lista, la devuelvo, porque quiere decir que ya hice este paso
            return _neighbors;

        //Sino empiezo a pedir los nodos vecinos

            //actual
        GridNode current = _grid.GetNode(_x -1 , _y); //Left

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

            if (blocked)
                gameObject.layer = 6;
            else
                gameObject.layer = 0;
                                                    //Si esta bloqueado    sino
            GetComponent<Renderer>().material.color = blocked ? Color.gray : Color.white; //Bloqueo
        }

        #region Dijkstra
        var c = (int)Input.GetAxisRaw("Vertical"); 

        SetCost(c);
        #endregion
    }

    #region Dijkstra
    public void SetCost(int vnewCost)
    {
        cost += vnewCost;
        cost = Mathf.Clamp(cost, 0, 99);
        textCost.text = "" + cost;
    }
    #endregion
}
