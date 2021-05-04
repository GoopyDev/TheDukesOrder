using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float segundosSpawn;
    [SerializeField] private float timerSpawn;
    [SerializeField] private float posX;
    [SerializeField] private float posY;

    public GameObject hormiga;

    // Start is called before the first frame update
    void Start()
    {
        posX = Random.Range(50, 90);
        posY = Random.Range(20, 30);
    }

    // Update is called once per frame
    void Update()
    {
        timerSpawn += Time.deltaTime;
        if (timerSpawn > segundosSpawn)
        {
            Instantiate(hormiga);
            hormiga.transform.position = new Vector3(posX, posY, -5);
            timerSpawn = 0;
            //segundosSpawn = Random.Range(3, 7);
            posX = Random.Range(50, 90);
            posY = Random.Range(20, 30);
        }
    }
}
