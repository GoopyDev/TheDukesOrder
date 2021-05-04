using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon : MonoBehaviour
{
    [SerializeField] public int    energia;
    [SerializeField] private float direccion;
    [SerializeField] private float segundosCambioDireccion;
    [SerializeField] private float timerCambioDireccion;
    [SerializeField] private float segundosSaltar;
    [SerializeField] private float timerSaltar;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb = default;


    //private enum State { idle, caminando, corriendo, cayendo, saltando, agachado, cubriendose, atacando1, atacando2, atacando3 };
    //private State state = State.idle;
    //private State lastState = State.idle;






    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        segundosSaltar = Random.Range(3, 6);
    }

    private void Update()
    {
        timerCambioDireccion += Time.deltaTime;
        timerSaltar += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal Dragon
        rb.AddForce(new Vector2(direccion, 0));

        // Mover de derecha a izquierda
        if (timerCambioDireccion > segundosCambioDireccion)
        {
            timerCambioDireccion = 0;
            direccion = -direccion;
        }

        // Saltos del Dragón
        if (timerSaltar > segundosSaltar)
        {
            rb.AddForce(new Vector2(0, 200));
            timerSaltar = 0;
            segundosSaltar = Random.Range(3, 6);
        }
            
    }

    public void TakeDamage(int damage)
    {
        energia -= damage;
        //rb.AddRelativeForce(800 * Vector2.left + 100 * Vector2.up);
        if (energia <= 0)
        {
            Destroy(gameObject);
        }
    }
}
