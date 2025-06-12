using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPausa : MonoBehaviour
{
    public GameObject panelPausa;
    private bool estaPausado = false;

    void Start()
    {
        panelPausa.SetActive(false);
        Time.timeScale = 1f; // Asegura que el tiempo corre
    }

    public void AlternarPausa()
    {
        estaPausado = !estaPausado;

        if (estaPausado)
        {
            panelPausa.SetActive(true);
            Time.timeScale = 0f; // Pausa el juego
        }
        else
        {
            panelPausa.SetActive(false);
            Time.timeScale = 1f; // Reanuda
        }
    }

    public void SalirAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
