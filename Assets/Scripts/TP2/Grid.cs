using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Node node;
    public float offset; //Variable para separar
    public int width, height; //Ancho, Alto
    Node[/*X*/, /*Y*/] _grid;

    private void Start()
    {
        //La grilla va a tener de tamaño los ya hechos
        _grid = new Node[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Instancio el nodo y lo guardo en una variable temporal
                Node go = Instantiate(node);
                //Le asigno su posicion en la escena y lo multiplico por su escala, asi queda uniforme. Tambien Lo multiplico por offset para que haya una separacion.
                go.transform.position = new Vector3(x * go.transform.localScale.x , y * go.transform.localScale.y , 0) * offset;

                //Le doy un color aleatorio
                go.GetComponent<Renderer>().material.color = Random.ColorHSV();
                //Me lo guardo en la grilla
                _grid[x, y] = go;

            }
        }
    }
}
