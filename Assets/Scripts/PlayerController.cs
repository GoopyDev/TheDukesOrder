using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [Header("Config")]
    [SerializeField] private float      checkRadius    = default;
    [SerializeField] private LayerMask  groundLayer    = default;
    [SerializeField] private float      jumpForce      = default;
    [SerializeField] private float      moveSpeed      = default;
    [SerializeField] private float      runSpeed       = default;
    [SerializeField] private Vector2    deadzone       = default;
    [SerializeField] private float      energia        = default;
    [SerializeField] private float      rangeAttack    = default;
    [SerializeField] private bool       atacando;
    [SerializeField] private float      timerAtacando  = default;
    [SerializeField] private int        damage         = default;
    [SerializeField] private LayerMask  enemyLayer     = default;
    [SerializeField] private float      tiempoDeSalto  = default;
    [SerializeField] private float      tiempoDeCarga  = default;
    [SerializeField] private bool       grounded       = default;
    [SerializeField] private bool       saltando       = default;
    [SerializeField] private float      duracionAtaque = default;

    [Header("References")]
    [SerializeField] private AudioSource music;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb            = default;
    [SerializeField] private Transform groundCheck     = default;
    [SerializeField] private Animator animator         = default;
    [SerializeField] private Transform[] attackEmissionPoint = default;

    [Header("Audio")]
    [SerializeField] private AudioSource SFX           = default;
    [SerializeField] private AudioClip[] espada        = default;
    [SerializeField] private AudioClip[] pasos         = default;
    [SerializeField] private AudioClip   salto         = default;
    [SerializeField] private AudioClip   caida         = default;

    // Variables privadas //
    private enum State { idle, caminando, corriendo, cayendo, saltando, agachado, cubriendose, atacando1, atacando2, atacando3 };
    private State state = State.idle;
    private State lastState = State.idle;

    // Ejes //
    private const string HORIZONTAL = "Horizontal";
    //private const string FIRE       = "Fire1";
    #endregion

    private void Update()
    {
        lastState = state;

        #region Movimiento horizontal del personaje
        float xAxisInput = Input.GetAxisRaw(HORIZONTAL);
        float move = moveSpeed;

        // Añadimos más fuerza al movimiento al presionar LShift
        if (Input.GetKey(KeyCode.LeftShift)) { move += runSpeed; }

        move *= xAxisInput;
        rb.velocity = new Vector2(move, rb.velocity.y);

        // Giramos el personaje hacia la derecha o hacia la izquierda basados en xAxisInput
        ActualizarDirecion(xAxisInput);
        #endregion

        #region Salto del personaje
        if (grounded && Input.GetKeyDown(KeyCode.W))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            tiempoDeSalto = 0;
            saltando = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            saltando = false;
        }

        if (saltando)
        {
            MantenerSalto();
        }
        #endregion

        #region Estados del personaje
        if (rb.velocity.y > deadzone.y && !grounded)
        {
            state = State.saltando;
        }
        else if (rb.velocity.y < -deadzone.y && !grounded)
        {
            state = State.cayendo;
        }
        else if (Mathf.Abs(rb.velocity.x) == moveSpeed && grounded)
        {
            state = State.caminando;
        }
        else if (Mathf.Abs(rb.velocity.x) == moveSpeed + runSpeed && grounded && !atacando)
        {
            state = State.corriendo;
        }
        else if ((rb.velocity.x < deadzone.x || rb.velocity.x > -deadzone.x) && (rb.velocity.y < deadzone.y || rb.velocity.y > -deadzone.y) && grounded && !atacando)
        {
            state = State.idle;
        }

        //if (Input.GetKeyDown(KeyCode.J) && state != State.saltando && state != State.cayendo && state != State.atacando3)
        if (Input.GetKeyDown(KeyCode.J) && state != State.atacando3)
        {
            if (state != State.atacando1 && state != State.atacando2 && state != State.atacando3)
            {
                state = State.atacando1;
            }
            else if (state == State.atacando1)
            {
                state = State.atacando2;
            }
            else if (state == State.atacando2)
            {
                state = State.atacando3;
            }

            SonidoDeEspada(); // Reproducimos sonido de espada
            rb.velocity = new Vector2(0, rb.velocity.y);
            atacando = true;
            timerAtacando = 0;
            List<Collider2D> collidersDetectados = new List<Collider2D>();
            for (int i = 0; i < attackEmissionPoint.Length; i++)
            {
                RaycastHit2D hit = Physics2D.Raycast(attackEmissionPoint[i].position, -transform.right, rangeAttack, enemyLayer);
                if (hit.collider != null && !collidersDetectados.Contains(hit.collider))
                {
                    Debug.Log("Nombre " + hit.collider.gameObject.name + " Layer: " + hit.collider.gameObject.layer + " Enemy Layer: " + enemyLayer.value);
                    Debug.Log("ouch");
                    hit.collider.GetComponent<ObjectHealth>().TakeDamage(damage);
                }
            }
        }
        timerAtacando += Time.deltaTime;
        if (atacando && timerAtacando > duracionAtaque)
        {
            atacando = false;
        }

        if (lastState != state)
        {
            animator.SetTrigger(state.ToString());
        }
        #endregion
    }
    private void FixedUpdate()
    {
        Collider2D groundCollider = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        if (groundCollider != null) { grounded = true; }
        else                        { grounded = false; }
    }
    
    private void ActualizarDirecion(float ejeHorizontal)
    {
        if (ejeHorizontal > 0) { transform.rotation = Quaternion.Euler(new Vector2(0, 180)); }
        else if (ejeHorizontal < 0) { transform.rotation = Quaternion.Euler(new Vector2(0, 0)); }
    }
    private void MantenerSalto()
    {
        tiempoDeSalto += Time.deltaTime;
        if (tiempoDeSalto < tiempoDeCarga)
        {
            rb.AddForce(Vector2.up * jumpForce * Time.deltaTime, ForceMode2D.Force);
        }
    }
    private void SonidoDeEspada()
    {
        if (state != State.atacando3)
        {
            SFX.PlayOneShot(espada[Random.Range(1, espada.Length)]);
        }
        else { SFX.PlayOneShot(espada[0]); }
    }

    private void SonidoDePasos()
    {
        SFX.PlayOneShot(pasos[Random.Range(0, pasos.Length)]);
    }

    private void SonidoDeSalto()
    {
        SFX.PlayOneShot(salto);
    }

    private void SonidoDeAterrizaje()
    {
        SFX.PlayOneShot(caida);
    }

}