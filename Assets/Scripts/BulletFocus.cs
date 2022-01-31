using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFocus : MonoBehaviour
{
    //Velocidad a la que se va a mover la bala

    private Vector3 velocity = new Vector3(0, 0, 0.1f);

    //Punto al que se va a dirigir

    private Transform target;

    //Le decimos a la bala que target es el transform del player

    private void Awake()
    {
        target = GameObject.Find("Player").GetComponent<Transform>();
    }

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

    //Le decimos que rote en dirección al transform del player cuando se activa

    private void OnEnable()
    {
        transform.LookAt(target);
    }

    //Detectamos colisiones y desactivamos la bala cuando choca

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Collider")
        {
            gameObject.SetActive(false);
        }
    }
}
