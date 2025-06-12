using System.Collections;
using System.Collections.Generic;
// Agregamos
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorController : MonoBehaviour
{
    //Declaro la variable de tipo RigidBody que luego asociaremos a nuestro Jugador
    private Rigidbody rb;

    //Inicializo el contador de coleccionables recogidos
    private int contador;

    //Inicializo variables para los textos
    public TextMeshProUGUI textoContador,
        textoGanar;

    //Declaro la variable pública velocidad para poder modificarla desde la Inspector window
    public float velocidad;

    public TextMeshProUGUI textoTimer;

    // Inicializo el tiempo restante y la variable de victoria
    private float tiempoRestante = 120f;
    private bool haGanado = false;

    // Use this for initialization
    void Start()
    {
        //Capturo esa variable al iniciar el juego
        rb = GetComponent<Rigidbody>();

        //Inicio el contador a 0
        contador = 0;

        //Actualizo el texto del contador por primera vez
        setTextoContador();

        //Inicio el texto de ganar a vacío
        textoGanar.text = "";
    }

    // Para que se sincronice con los frames de física del motor
    void FixedUpdate()
    {
        //Estas variables nos capturan el movimiento en horizontal y vertical de nuestro teclado
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");

        //Un vector 3 es un trío de posiciones en el espacio XYZ, en este caso el que corresponde al movimiento
        Vector3 movimiento = new Vector3(movimientoH, 0.0f, movimientoV);

        //Asigno ese movimiento o desplazamiento a mi RigidBody, multiplicado por la velocidad que quiera darle
        rb.AddForce(movimiento * velocidad);

        // Reiniciar nivel con tecla R
        if (Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(
                UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
            );
        }

        if (!haGanado)
        {
            tiempoRestante -= Time.deltaTime;

            int minutos = Mathf.FloorToInt(tiempoRestante / 60);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60);
            textoTimer.text = "Tiempo: " + minutos.ToString("00") + ":" + segundos.ToString("00");

            if (tiempoRestante <= 0)
            {
                textoTimer.text = "¡Tiempo agotado!";
                StartCoroutine(PerderPorTiempo());
            }
        }
    }

    //Se ejecuta al entrar a un objeto con la opción isTrigger seleccionada
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coleccionable"))
        {
            //Desactivo el objeto
            other.gameObject.SetActive(false);

            //Incremento el contador en uno
            contador = contador + 1;

            //Actualizo el texto del contador
            setTextoContador();
        }
        else if (other.gameObject.CompareTag("Obstaculo"))
        {
            textoGanar.text = "¡Perdiste!";
            haGanado = true;
            StartCoroutine(VolverAlMenu()); // o puedes crear otra coroutine llamada Perder()
        }
    }

    //Actualizo el texto del contador (O muestro el de ganar si las ha cogido todas)
    void setTextoContador()
    {
        textoContador.text = "Contador: " + contador.ToString();

        if (contador >= 12)
        {
            textoGanar.text = "¡GANASTE!";
            haGanado = true;

            if (SceneManager.GetActiveScene().name == "Nivel10")
            {
                // Si estamos en el último nivel, volver al menú
                StartCoroutine(VolverAlMenu());
            }
            else
            {
                // Si no, pasar al siguiente nivel
                StartCoroutine(PasarAlSiguienteNivel());
            }
        }
    }

    // Se ejecuta después de perder por tiempo para volver al menú
    IEnumerator PerderPorTiempo()
    {
        yield return new WaitForSeconds(3);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }

    // Se ejecuta después de ganar para volver al menú o pasar al siguiente nivel
    IEnumerator VolverAlMenu()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Menu");
    }

    // Se ejecuta después de ganar para pasar al siguiente nivel
    IEnumerator PasarAlSiguienteNivel()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
