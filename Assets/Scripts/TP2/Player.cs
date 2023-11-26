using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += speed * new Vector3(0, 0, 1) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += speed * new Vector3(0, 0, -1) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += speed * new Vector3(-1, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += speed * new Vector3(1, 0, 0) * Time.deltaTime;
        }
    }
}
