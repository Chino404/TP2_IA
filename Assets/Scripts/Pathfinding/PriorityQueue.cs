using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    Dictionary<T, float> _allElements = new();

    public int Count { get { return _allElements.Count; } }

    //Creo los metodos de Enqueue y Dequeue
    
    /// <summary>
    /// Me lo agrega al diccionario
    /// </summary>
    /// <param name="elem"></param>
    /// <param name="cost"></param>
    public void Enqueue(T elem, float cost)
    {
        _allElements.Add(elem, cost);
    }

    /// <summary>
    /// Me devuelve el siguiente valor y lo borra
    /// </summary>
    /// <returns></returns>
    public T Dequeue()
    {
        float lowestValue = Mathf.Infinity;
        T elem = default;

        foreach (var item in _allElements)
        {
            if(item.Value < lowestValue) //Si el valor de este nodo es menor
            {
                elem = item.Key; //Me guardo el nodo
                lowestValue = item.Value; //Me guardo su valor
            }
        }
        
        _allElements.Remove(elem);

        return elem;
    }

    /// <summary>
    /// Pregunta si tengo guardado ese nodo
    /// </summary>
    /// <param name="elem"></param>
    /// <returns></returns>
    public bool ContainsKey(T elem)
    {
        return _allElements.ContainsKey(elem);
    }
}
