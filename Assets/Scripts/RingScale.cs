using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingScale : MonoBehaviour
{
    //Velocidad a la que se va a escalar

    public float scaleVelocity;

    //Variable que a va a sumar o restar a la escala de los anillos y su objeto padre

    private Vector3 scaleChange;

    //Array con los parents de los anillos

    public Transform[] parents;

    //Array con los anillos

    public Transform[] childs;

    void Start()
    {
        //Le damos un valor a scaleChange en los ejes X e Y

        scaleChange = new Vector2(scaleVelocity, scaleVelocity);
    }

    void Update()
    {
        //Aumentamos el valor de la escala de los parents y reducimos el de los anillos
        //De esta manera los anillos se escalan más rápido y la cantidad que se escalan tanto los padres 
        //como los anillos es menor (Esto evita problemas con las colisiones)

        parents[0].localScale += scaleChange * Time.deltaTime;

        parents[1].localScale += scaleChange * Time.deltaTime;

        childs[0].localScale -= scaleChange * Time.deltaTime;

        childs[1].localScale -= scaleChange * Time.deltaTime;
    }

    //Reseteamos la escala

    public void ResetScale()
    {
        parents[0].localScale = Vector3.one;
        parents[1].localScale = Vector3.one;
        childs[0].localScale = Vector3.one;
        childs[1].localScale = Vector3.one;
    }
}
