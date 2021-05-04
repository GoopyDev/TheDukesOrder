using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ant : MonoBehaviour
{

    [SerializeField] private float velocidad;
    [SerializeField] private bool atacar;
    [SerializeField] private GameObject miColliderSuperior;
    public int vidas;

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    
    private Vector2 direccion;
    private Vector2 posicionJugador;

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("Nombre Collider: " + collision.tag + " Nombre de objeto detectado: " + collision.name);
        if (collision.CompareTag("Player"))
        {
            posicionJugador = collision.gameObject.transform.position;
            atacar = true;
        }
    }

    private void OnDestroy()
    {
        Destroy(miColliderSuperior);
    }
    //public void TakeDamage(int damage)
    //{
    //    vidas -= damage;
    //    //rb.AddRelativeForce(800 * Vector2.left + 100 * Vector2.up);
    //    if (vidas <= 0)
    //    {
    //        Destroy(miColliderSuperior);
    //        Destroy(gameObject);
    //    }
    //}

    private void Caminar()
    {
        rb.velocity = new Vector2(velocidad * (- direccion.x) * Time.deltaTime, rb.velocity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        direccion = new Vector2(transform.position.x, transform.position.y) - new Vector2(posicionJugador.x, posicionJugador.y);
    }
    private void FixedUpdate()
    {
        if (atacar)
        {
            //Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.5f);
            //colliders[0].gameObject.TakeDamage();
            Caminar();
        }

        if (rb.velocity.x > -0.01 && rb.velocity.x < 0.01)
        {
            anim.SetBool("caminando", false);
            anim.SetBool("idle", true);
        }
        else
        {
            anim.SetBool("caminando", true);
            anim.SetBool("idle", false);
        }

        if (direccion.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (direccion.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
