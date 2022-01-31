using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    //Animator para la animación de cuando golpeamos al boss

    public Animator anim;

    //Velocidad a la que salen las balas

    float firerate = 0.1f;

    float focusrate = 0.5f;

    //Vida del boss

    int HP = 8000;

    //Array de puntos desde los que disparamos las balas

    public Transform[] firePoint;

    //Anillos que dispara el boss

    public GameObject rings;

    //Contador de disparos

    int shotcount = 0;

    int focuscount = 0;

    //Particulas para el laser y para cuando muera el boss

    public GameObject laserParticle;

    public GameObject deadParticle;

    //Bool para activar la 2ª fase del boss

    public bool fase2;

    //Laser que dispara el boss

    public GameObject Laser;

    //Bool para evitar que el boss reciba daño hasta que no empiece a disparar

    bool invincible = true;

    //Script al que le pasamos la vida del boss y se la aplicamos al valor del slider en el canvas

    CanvasController canvas;

    //Variables que uso para alternar el punto de disparo

    int fireswitch = 0;

    int focusswitch = 2;

    private void Start()
    {
        //Accedemos a las variables del canvas

        canvas = GameObject.Find("Canvas").GetComponent<CanvasController>();
    }

    void Update()
    {
        //Le pasamos al slider la salud actual del boss

        canvas.slider.value = HP;

        //Si la vida del boss llega a 0 desenparentamos las particulas, las activamos y desactivamos al boss

        if (HP <= 0)
        {
            deadParticle.transform.parent = null;

            deadParticle.SetActive(true);

            gameObject.SetActive(false);
        }

        //Si la vida del boss llega a la mitad activamos su segunda fase y cambiamos los ataques que realiza

        if (HP <= 4000)
            fase2 = true;
    }

    //Corrutina para el disparo normal
    //Le pasamos el ratio de disparo como el tiempo de espera antes ejecutarse

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(firerate);

        //Accedemos a las balas almacenadas en la pool de objetos

        GameObject Instance = Pool.singleton.Get("EnemyBullet");

        //Contadores que uso para cambiar la posicion desde la que se disparan las balas
        //Le pasamos el fireswitch al array de transforms firepoint para que las balas se instancien desde ese punto

        if (shotcount >= 20)
        {
            shotcount = 0;
            fireswitch = 0;
        }

        if (shotcount >= 10)
        {
            fireswitch = 1;
        }

        //Si Instance devuelve las balas de la pool, le asignamos una posición y una rotación y la activamos

        if (Instance != null)
        {
            Instance.transform.position = firePoint[fireswitch].position;
            Instance.transform.rotation = firePoint[fireswitch].rotation;
            Instance.SetActive(true);

            //Sumamos 1 al contador de disparos por cada bala que activamos

            shotcount = shotcount + 1;
        }

        //Paramos la corrutina para interrumpir su ejecución y asegurarme de que esperamos al ratio de disparo antes de volver a ejecutarla

        StopCoroutine("Fire");
    }

    //Corrutina para el disparo que apunta hacia el player
    //El funcionamiento es idéntico al de el disparo normal a excepción de que tiene un ratio de disparo diferente

    IEnumerator FireFocus()
    {
        yield return new WaitForSeconds(focusrate);

        GameObject Instance = Pool.singleton.Get("EnemyFocusBullet");

        if (focuscount >= 4)
        {
            focuscount = 0;
            focusswitch = 2;
        }

        if (focuscount >= 2)
        {
            focusswitch = 3;
        }

        if (Instance != null)
        {
            Instance.transform.position = firePoint[focusswitch].position;
            Instance.SetActive(true);

            focuscount = focuscount + 1;
        }

        StopCoroutine("FireFocus");
    }

    //Corrutina para activar el laser
    //Primero activamos unas particulas para avisar al jugador y cuando terminan activamos el laser

    IEnumerator ActiveLaser()
    {
        laserParticle.SetActive(true);

        yield return new WaitForSeconds(2f);

        Laser.SetActive(true);
    }

    //Corrutina para desactivar el laser
    //Desactivamos el laser y las particulas para poder reproducirlas de nuevo mas tarde

    IEnumerator QuitLaser()
    {
        yield return new WaitForSeconds(0.1f);

        Laser.SetActive(false);

        laserParticle.SetActive(false);
    }

    //Activamos los anillos que invierten los inputs del player

    public void ActiveRing()
    {
        rings.gameObject.SetActive(true);
    }

    //Desactivamos los anillos y llamamos a una función que resetea su escala

    public void DisableRings()
    {
        rings.gameObject.SetActive(false);

        rings.GetComponent<RingScale>().ResetScale();
    }

    //Desactivamos la invencibilidad del boss

    public void DisableInvincible()
    {
        invincible = false;
    }

    //Detectamos las colisiones con las balas de player y reproducimos la animación para indicar que el boss ha recibido daño

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet" && invincible != true)
        {
            anim.SetTrigger("Hitted");
            HP = HP - 20;
        }
    }
}