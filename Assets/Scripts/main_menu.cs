using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    public float puntos_finales = 0.0;
    public float tiempo_final = 0.0;

    public start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            // puntos_finales = GameManager.Instance. PUNTOS GANADOS
            // tiempo_final = GameManager.Instance. DURACION JUEGO
        }

    }
    public void change_scene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quit_game()
    {
        Application.Quit();
    }

    public void back_main_menu()
    {
        SceneManager.LoadScene(0);
    }
}
