using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    //Velocidad de rotaci�n

    private Vector2 velocity = new Vector2(0, 5f);

    //Rotamos el objeto cuando se active

    void Update()
    {
        transform.Rotate(velocity);
    }
}
