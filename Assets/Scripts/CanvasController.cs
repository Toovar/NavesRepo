using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    //Variables del player

    private Controller player;

    //Objetos del Canvas

    public GameObject Win;

    public GameObject BossHP;

    public GameObject GameOverPanel;

    public Slider slider;

    void Start()
    {
        //Accedemos a las variables del Player

        player = GameObject.Find("Player").GetComponent<Controller>();
    }

    void Update()
    {
        //Si la vida del boss llega a 0, desactivamos la barra de vida, activamos el mensaje de victoria, 
        //hacemos al player invencible por si le diera una bala que haya quedado suelta y empezamos a cargar el menu principal

        if (slider.value <= 0)
        {
            Win.SetActive(true);

            player.invincible = true;

            BossHP.SetActive(false);

            StartCoroutine("LoadMenu");
        }


        //Si la vida del player llega a 0, paramos el tiempo y empezamos a cargar el panel de game over

        if (player.HP <= 0 && GameOverPanel.activeInHierarchy == false)
        {
            Time.timeScale = 0f;

            StartCoroutine("GameOver");
        }
    }

    //Recargar escena actual

    public void Yes()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("Naves");
    }

    //Volver al menu principal y desactivamos el checkpoint del jefe

    public void No()
    {
        Time.timeScale = 1f;

        EnemySpawner.BossCP = false;

        SceneManager.LoadScene("MainMenu");
    }

    //Delay para que aparezca la pantalla de game over

    IEnumerator GameOver()
    {
        yield return new WaitForSecondsRealtime(3f);

        GameOverPanel.SetActive(true);
    }

    //Delay para volver al menu tras ganar y desactivamos el checkpoint del jefe

    IEnumerator LoadMenu()
    {
        yield return new WaitForSecondsRealtime(6f);

        Time.timeScale = 1f;

        EnemySpawner.BossCP = false;

        SceneManager.LoadScene("MainMenu");
    }
}
