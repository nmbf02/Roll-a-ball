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
    }

    //Actualizo el texto del contador (O muestro el de ganar si las ha cogido todas)
    void setTextoContador()
    {
        textoContador.text = "Contador: " + contador.ToString();

        if (contador >= 12)
        {
            textoGanar.text = "¡GANASTE!";
            StartCoroutine(VolverAlMenu());
        }
    }

    // Se ejecuta después de ganar para volver al menú
    IEnumerator VolverAlMenu()
    {
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
