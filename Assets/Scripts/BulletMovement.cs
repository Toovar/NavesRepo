using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    //Velocidad a la que se va a mover la bala

    private Vector3 velocity = new Vector3(0.3f, 0, 0);

    //Movemos la bala hacia delante

    void Update()
    {
        transform.Translate(velocity);
    }

    //Desactivamos la bala cuando se vuelva invisible

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    //Detectamos colisiones y desactivamos la bala cuando choque

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss" || other.tag == "Enemy" || other.tag == "Player" || other.tag == "Collider")
        {
            gameObject.SetActive(false);
        }
    }
}
