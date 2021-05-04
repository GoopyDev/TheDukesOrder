using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour
{
    public Transform jugador;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Seguir al jugador en posición X e Y, con distancia de -10
        transform.position = new Vector3(jugador.position.x, jugador.position.y + 1f, -10);
    }
}
