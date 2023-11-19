using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GridNode node;
    public float offset; //Variable para separar
    public int width, height; //Ancho, Alto
    GridNode[/*X*/, /*Y*/] _grid;

    private void Start()
    {
        //La grilla va a tener de tamaño los ya hechos
        _grid = new GridNode[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Instancio el nodo y lo guardo en una variable temporal
                GridNode node = Instantiate(this.node);
                //Le asigno su posicion en la escena y lo multiplico por su escala, asi queda uniforme. Tambien Lo multiplico por offset para que haya una separacion.
                node.transform.position = new Vector3(x * node.transform.localScale.x , y * node.transform.localScale.y , 0) * offset;


                //node.GetComponent<Renderer>().material.color = Random.ColorHSV(); //Le doy un color aleatorio
                //Llamo su metodo y le paso su posicion y la referencia de grilla
                node.Intializaze(this, x, y);
                //Me lo guardo en la grilla
                _grid[x, y] = node;


            }
        }
    }

    /// <summary>
    /// Metodo que sirve para que el nodo llame y pueda conocer a sus vecinos
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public GridNode GetNode(int x, int y)
    {
        if( x < 0 || y < 0 || x >= width || y >= height) //Por si no existe ese nodo
            return null;

        return _grid[x, y];
    }
}
