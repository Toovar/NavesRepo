using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject Ranking;

    public GameObject Menu;

    public GameObject Stats;

    public GameObject Input;

    public RankingManager ranking;

    public bool GamePlayed;

    //Cargamos el nivel

    public void LoadGame()
    {
        SceneManager.LoadScene("Naves");

        GamePlayed = true;
    }

    //Mostramos el imput field

    public void ShowInput()
    {
        Menu.SetActive(false);

        Input.SetActive(true);
    }

    //Mostramos el ranking

    public void ShowRanking()
    {
        Menu.SetActive(false);

        if (ranking.RankingOpened != true)
        ranking.MostrarRanking();

        ranking.BorrarPuntosExtra();

        Ranking.SetActive(true);
    }

    //Quitamos el ranking

    public void QuitRanking()
    {
        Ranking.SetActive(false);

        Menu.SetActive(true);
    }

    //Mostramos las estadísticas

    public void ShowStats()
    {
        Menu.SetActive(false);

        Stats.SetActive(true);
    }

    //Quitamos las estadísticas

    public void QuitStats()
    {
        Stats.SetActive(false);

        Menu.SetActive(true);
    }

    //Salimos del juego

    public void ExitGame()
    {
        Application.Quit();
    }
}
