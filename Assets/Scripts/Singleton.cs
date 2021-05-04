using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    //Para reproducir sonidos del juego
    [SerializeField] private AudioSource Musica;
    [SerializeField] private AudioSource SFX;
    [SerializeField] private AudioClip[] songs      = default; //Contendrá todos los sonidos
    [SerializeField] private int nextSongNumber     = default; //Numero de cancion a reproducir

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }

    private void Start()
    {

    }

    private void Update()
    {
        if (!Musica.isPlaying)
        {
            Musica.PlayOneShot(songs[nextSongNumber]);
            nextSongNumber += 1;
            if (nextSongNumber >= 3) { nextSongNumber = 0; }
        }
    }
    private void OnDestroy()
    {
        
    }
}