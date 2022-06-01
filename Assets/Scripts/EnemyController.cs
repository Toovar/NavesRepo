using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //Ratio de disparo

    float firerate = 1;

    //Vida de los enemigos

    int HP = 100;

    //Punto desde el que se disparan las balas

    public Transform firePoint;

    //Animator para cuando reciben daño los enemigos

    public Animator anim;

    //Animator para el movimiento

    public Animator Parentanim;

    //Variables del spawner

    private EnemySpawner spawner;

    //Variables del Player

    private Controller player;

    private void Awake()
    {
        //Accedemos al spawner de enemigos

        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();

        player = GameObject.Find("Player").GetComponent<Controller>();
    }

    void Update()
    {
        //Activamos la corrutina encargada de instanciar las balas

        StartCoroutine("Fire");

        //Si la vida del enemigo llega a 0, desactivamos el objeto, sumamos 1 al contador de enemigos muertos y restamos 1 al número de enemigos activos

        if (HP <= 0)
        {
            transform.parent.gameObject.SetActive(false);
            spawner.DeadEnemy = spawner.DeadEnemy + 1;
            spawner.Enemynumber = spawner.Enemynumber - 1;
            player.Puntos = player.Puntos + 100;
        }
    }

    //Corrutina para el disparo
    //Funcionamiento idéntico al del player

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(firerate);

        GameObject Instance = Pool.singleton.Get("EnemyBullet");

        if (Instance != null)
        {
            Instance.transform.position = firePoint.position;
            Instance.transform.rotation = firePoint.rotation;
            Instance.SetActive(true);
        }

        StopCoroutine("Fire");
    }

    //Detectamos las colisiones con las balas del player y reproducimos la animación para cuando el enemigo recibe daño

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerBullet")
        {
            anim.SetTrigger("Hitted");
            HP = HP - 50;
        }

        //Si el player no esta esquivando y impactamos con el, el enemigo muere

        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Controller>().playerCanMove != false)
        {
            HP = 0;
        }
    }

    //Cuando se salen de la pantalla restamos 1 al número de enemigos activos cuando y desactivamos el objeto
    //Comprobamos que su vida no sea menor que 0 para que no tenga en cuenta a los enemigos que matamos

    private void OnBecameInvisible()
    {
        if (HP > 0)
        {
            spawner.Enemynumber = spawner.Enemynumber - 1;
            transform.parent.gameObject.SetActive(false);
        }
    }

    //Cuando se activan comprueban que no haya iniciado la segunda oleada
    //Si es el caso, comienzan a moverse hacia arriba y hacia abajo

    private void OnEnable()
    {
        if (spawner.DeadEnemy >= 10)
            Parentanim.SetBool("Stage2", true);

        if (Parentanim.GetBool("Stage2") == true)
        {
            if (spawner.index == 0)
                Parentanim.SetTrigger("Up");

            if (spawner.index == 2)
                Parentanim.SetTrigger("Down");
        }
    }

    //Reseteamos la vida de los enemigos al desactivarlos

    private void OnDisable()
    {
        HP = 100;
    }
}
