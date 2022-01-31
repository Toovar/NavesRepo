using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Cargamos el nivel

    public void LoadGame()
    {
        SceneManager.LoadScene("Naves");
    }

    //Salimos del juego

    public void ExitGame()
    {
        Application.Quit();
    }
}
