using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHealth : MonoBehaviour
{
    [SerializeField] private int energia;

    // Update is called once per frame
    void Update()
    {
        
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
