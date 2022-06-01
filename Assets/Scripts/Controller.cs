using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    //Controller del personaje

    public CharacterController controller;

    //Animator para cuando recivamos golpes

    public Animator anim;

    //Bool para impedir el movimiento al jugador durante la esquiva

    public bool playerCanMove = true;

    //Velocidad de movimiento

    float MoveSpeed = 8;

    //Punto desde el que disparamos las balas

    public Transform firePoint;

    //Velocidad de disparo

    float firerate = 0.15f;

    //Tiempo que dura el dash y el cooldown

    float dashLenght = 0.18f;
    float dashCooldown = 5;

    //float NumberCooldown = 0.5f;

    //Particulas para cuando morimos y cuando estamos con los inputs invertidos

    public GameObject particles;

    public GameObject confusionEffect;

    //int dashNumber;

    //Floats a los que vamos a asignar los valores de los inputs

    float moveX;
    float moveY;

    //Bool para cambiar al estado con los inputs invertidos

    bool invertInput;

    //Vida del player

    public int HP = 100;

    public int DeathCount = 0;

    //Bool para declarar al player como invencible

    public bool invincible = false;

    //Puntos del Player

    public int Puntos;

    void Update()
    {
        //MOVIMIENTO

        //Si el jugador se puede mover, es decir no esta esquivando, asignamos los valores de los inputs a moveX y moveY

        if (playerCanMove != false)
        {
            //Si invertInput es true, invertimos los valores que reciben moveX y moveY

            if(invertInput == true)
            {
                moveX = -Input.GetAxisRaw("Horizontal");
                moveY = -Input.GetAxisRaw("Vertical");
            }
            else
            {
                moveX = Input.GetAxisRaw("Horizontal");
                moveY = Input.GetAxisRaw("Vertical");
            }
        }

        //Guardamos los valores de moveX y moveY en un Vector3 y se los pasamos al transform del player

        Vector3 move = transform.right * moveX + transform.up * moveY;

        //Pasamos los valores guardados en move al controlador y los normalizamos para evitar que 
        //se mueva en diagonal más rapido que en horizontal y vertical

        controller.Move(move.normalized * MoveSpeed * Time.deltaTime);

        //Para evitar que al colisionar con el jefe la posicion Z del player cambie

        if (transform.position.z > 0 || transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        //DISPARO

        //Si pulsamos o mantenemos pulsado espacio activamos la corrutina encargada de instanciar las balas

        if (Input.GetKey(KeyCode.Space))
        {
            StartCoroutine("Fire");
        }

        //VIDA

        //Si la vida del player llega 0 desenparentamos las particulas, las activamos y desactivamos el player

        if (HP <= 0)
        {
            particles.transform.parent = null;

            particles.SetActive(true);

            gameObject.SetActive(false);

            DeathCount = DeathCount + 1;
        }

        //DASH

        //Si el cooldown esta listo y el jugador no esta esquivando, con mayus izq activamos la corrutina que se encarga de el dash del player

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (dashCooldown >= 0.5f && playerCanMove != false)
            {
                StartCoroutine("Dash");
            }

            //dashNumber = dashNumber + 1;
        }

        //if (dashNumber > 0)
        //{
        //    NumberCooldown -= Time.deltaTime;
        //    if (NumberCooldown <= 0)
        //    {
        //        dashNumber = 0;
        //        NumberCooldown = 0.5f;
        //    }
        //}

        //Reponemos el cooldown tras esquivar

        if (dashCooldown <= 0.5f)
            dashCooldown += Time.deltaTime;
    }

    //Corrutina para el disparo
    //Esperamos al firerate para empezar a ejecutarla

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(firerate);

        //Accedemos a las balas de la pool de objetos

        GameObject Instance = Pool.singleton.Get("PlayerBullet");

        //Si Instance devuelve las balas de la pool, le asignamos una posición y una rotación y la activamos

        if (Instance != null)
        {
            Instance.transform.position = firePoint.transform.position;
            Instance.SetActive(true);
        }

        //Paramos la corrutina para interrumpir su ejecución y asegurarme de que esperamos al ratio de disparo antes de volver a ejecutarla

        StopCoroutine("Fire");
    }

    //Corrutina para la esquiva
    //Cambiamos la velocidad del player, le impedimos moverse y ponemos el cooldown a 0

    IEnumerator Dash()
    {
        MoveSpeed = 30;
        playerCanMove = false;
        dashCooldown = 0;

        //Esperamos a la duración de la esquiva para revertir los valores de arriba

        yield return new WaitForSeconds(dashLenght);

        MoveSpeed = 8;
        playerCanMove = true;

        StopCoroutine("Dash");
    }


    //Sin usar, lo dejo aquí por si vuelvo con esto mas adelante
    //La idea era implementar un cooldown si hacemos 3 dashes seguidos en X fracción de tiempo

    //IEnumerator DashNumberCooldown()
    //{
    //    if (dashNumber == 3)
    //        dashCooldown = 1f;

    //    yield return new WaitForSeconds(1f);

    //    dashNumber = 0;

    //    dashCooldown = 0.1f;

    //    StopCoroutine("DashNumberCooldown");
    //}


    //Tiempo que van a estar los inputs invertidos y el efecto de confusión activo

    IEnumerator InvertCooldown()
    {
        yield return new WaitForSeconds(15f);

        invertInput = false;

        confusionEffect.SetActive(false);
    }

    //COLISIONES

    //Detectamos colisiones con las balas si no estamos esquivando y reproducimos la animación de cuando dañan al jugador

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyBullet" && playerCanMove != false || other.gameObject.tag == "EnemyFocusBullet" && playerCanMove != false || other.gameObject.tag == "Enemy" && playerCanMove != false)
        {
            if (invincible != true)
            {
                HP = HP - 35;

                anim.SetTrigger("Hit");
            }
        }

        //Si chocamos con los anillos del boss y no estamos esquivando se invierten los inputs del jugador

        if (other.gameObject.tag == "Invert" && playerCanMove == true)
        {
            invertInput = true;

            confusionEffect.SetActive(true);

            StartCoroutine("InvertCooldown");
        }
    }
}
