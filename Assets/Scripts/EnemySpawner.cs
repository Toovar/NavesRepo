using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //Int con el que vamos a contar el numero de enemigos activos

    public int Enemynumber;

    //Int con el que vamos a contar el numero de enemigos muertos

    public int DeadEnemy;

    //Int al que le vamos a asignar un valor aleatorio para determinar en que punto va a spawnear un enemigo

    public int index;

    //El jefe final y la barra de vida

    public GameObject Boss;

    public GameObject BossUI;

    //Checks para comprobar en que punto ha spawneado un enemigo

    private bool enemyT1;
    private bool enemyT2;
    private bool enemyT3;

    //Array de transforms que vamos a utilizar como puntos de spawn para los enemigos

    public Transform[] Spawnpositions;

    //Check para comprobar si hemos llegado hasta el boss y poder recargar la escena desde ese punto en caso de morir

    public static bool BossCP;

    void Awake()
    {
        //Si hemos llegado hasta el boss, al recargar la escena se activara el boss directamente y este objeto se desactivara

        if (BossCP == true)
        {
            Boss.SetActive(true);

            BossUI.SetActive(true);

            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        //Comprobamos cuantos enemigos se han muerto para activar el boss y desactivar el spawner

        if (DeadEnemy >= 15)
        {
            ActiveBoss();
        }

        //Comprobamos cuantos enemigos hay activos en la escena y si el valor es menor a 2, instanciamos uno nuevo

        if (Enemynumber < 2)
        {
            //Obtenemos una instancia de los enemigos almacenados en la pool

            GameObject Instance = Pool.singleton.Get("Enemy");

            //Generamos un valor aleatorio entre 0 y el numero transforms que hay en el array

            index = Random.Range(0, Spawnpositions.Length);

            if (Instance != null)
            {
                //Comprobamos que spawn esta registrado como activo y cambiamos el valor del index en consecuencia 
                //para evitar que dos enemigos puedan spawnear en el mismo punto

                if (enemyT1 == true)
                    index = 1;

                if (enemyT2 == true)
                    index = 2;

                if (enemyT3 == true)
                    index = 0;

                //Instanciamos a los enemigos en el spawn que haya cogido el index y lo activamos

                Instance.transform.position = Spawnpositions[index].position;
                Instance.transform.rotation = Spawnpositions[index].rotation;
                Instance.SetActive(true);

                //Sumamos 1 a la cantidad de enemigos activos

                Enemynumber = Enemynumber + 1;
            }

            //Comprobamos que spawn se ha activado en base al valor del index y lo registramos.
            //Cuando el valor del index sea diferente en la siguiente pasada, desregistramos el spawns anterior.

            if (index == 0)
                enemyT1 = true;
            else
                enemyT1 = false;

            if (index == 1)
                enemyT2 = true;
            else
                enemyT2 = false;

            if (index == 2)
                enemyT3 = true;
            else
                enemyT3 = false;
        }
    }

    //Se activa el boss y la barra de vida, ponemos el checkpoint como true y desactivamos el spawner

    void ActiveBoss()
    {
        Boss.SetActive(true);

        BossUI.SetActive(true);

        BossCP = true;

        gameObject.SetActive(false);
    }
}
