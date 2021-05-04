using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    // Para el movimiento horizontal
    public float movimientoEnX;     // Contiene la velocidad total con la cual moverse (movHorizontal * (velocidad + velocidadAlCorrer))
    private float movHorizontal;    // Contiene la información del Axis Horizontal (entre -1f y 1f)
    public float velocidad;         // Velocidad normal preestablecida desde el editor
    public float velocidadAlCorrer; // Velocidad que puede tomar dos valores (0 o plusVelocidad). Al presionar ShiftL toma el valor de plusVelocidad
    public float plusVelocidad;     // Velocidad adicional al presionar shift, preestablecida desde el editor

    // Para el salto
    public float fuerzaSalto;       // Fuerza con la cual nos impulsamos hacia arriba
    public float tiempoDeSalto;     // Para contar el tiempo desde que saltamos (que apretamos W)
    public float tiempoDeCarga;     // Para definir en el editor cuánto tiempo permitimos que se cargue el salto
    public bool grounded;           // Para detectar si estamos en el suelo
    public bool soltoSalto;         // Para saber si hubo un KeyUp de la tecla de Salto
    public Collider2D checkGroundCollider;

    // Otras caracteristicas
    public int energia;

    public Rigidbody2D rb;
    public Animator anim;
    //public SpriteRenderer spriteR;

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.name == "GroundCheck" && collision.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("hay colision");
        if (collision.otherCollider.gameObject.name == "GroundCheck" && collision.gameObject.tag == "Ground")
        {
            grounded = true;
            anim.SetBool("cayendo", false);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.otherCollider.gameObject.name == "GroundCheck" && collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void Saltar()
    {
        rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Force);
    }

    private void ActivarEstado(string estadoDeseado)
    {
        //Primero apagamos todos los estados
        anim.SetBool("caminando", false);
        anim.SetBool("saltando", false);
        anim.SetBool("idle", false);
        anim.SetBool("cayendo", false);
        anim.SetBool("corriendo", false);

        //Luego, activamos el estado deseado
        anim.SetBool(estadoDeseado, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        energia = 100;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //spriteR = GetComponent<SpriteRenderer>();
        grounded = false;
        soltoSalto = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            movHorizontal = Input.GetAxisRaw("Horizontal");     //Obtenemos el valor del eje X (Horizontal)
            movimientoEnX = movHorizontal * (velocidad + velocidadAlCorrer);                   //Actualizamos posicion en X
            //rb.AddForce(new Vector2(posX, 0) * Time.deltaTime);
            Debug.Log("Velocidad: " + rb.velocity.x);
            if (rb.velocity.x > -10 && rb.velocity.x < 10)
            {
                rb.velocity = new Vector2(movimientoEnX * Time.deltaTime, rb.velocity.y);
            }
            //else
            //{
            //    rb.velocity = new Vector2(-(rb.velocity.x / -rb.velocity.x) * 10, rb.velocity.y) * Time.deltaTime;
            //}

            //anim.SetBool("caminando", true);
            //anim.SetBool("idle", false);
            if (grounded)
            {
                anim.SetBool("caminando", true);
                anim.SetBool("idle", false);
            }
            else { anim.SetBool("caminando", false); }

            //if (Input.GetAxis("Horizontal") > 0)
            //{
            //    spriteR.flipX = false;
            //}
            //else if (Input.GetAxis("Horizontal") < 0)
            //{
            //    spriteR.flipX = true;
            //}
            if (Input.GetAxis("Horizontal") > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector2 (0, 180));
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                transform.rotation = Quaternion.Euler(new Vector2(0, 0));
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (grounded)
            {
                Saltar();
                ActivarEstado("saltando");
                tiempoDeSalto = 0;
                soltoSalto = false;
            }
        }

        if (Input.GetKey(KeyCode.W) && !soltoSalto)
        {
            if (tiempoDeSalto < tiempoDeCarga)
            {
                rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Force);
            }
            if (tiempoDeSalto > 0.5f)
            {
                ActivarEstado("cayendo");
                soltoSalto = true;
            }
            tiempoDeSalto += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            soltoSalto = true;
            if (!grounded)
            {
                ActivarEstado("cayendo");
            }
        }

        //Para hacer correr al personaje
        if (Input.GetKey(KeyCode.LeftShift))
        {
            //<--Aumentar la fuerza aqui-->
            velocidadAlCorrer = plusVelocidad;
            if (!anim.GetBool("saltando") && !anim.GetBool("cayendo"))
            {
                ActivarEstado("corriendo");
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            velocidadAlCorrer = 0;
            if (!anim.GetBool("saltando") && !anim.GetBool("cayendo"))
            {
                anim.SetBool("corriendo", false);
            }
        }

        // Si la velocidad es muy cercana a 0, y no estamos saltando o cayendo, activamos el estado IDLE
        if (rb.velocity.x > -0.01 && rb.velocity.x < 0.01 && anim.GetBool("saltando") == false && anim.GetBool("cayendo") == false)
        {
            ActivarEstado("idle");
        }
    }
}
