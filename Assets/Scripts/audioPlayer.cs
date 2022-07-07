using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioPlayer : MonoBehaviour
{

    public AudioSource soundPlayer;
    public AudioClip audioClip1;
    public AudioClip audioClip2;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(PlayMySounds());
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            soundPlayer.PlayOneShot(audioClip2 , 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            soundPlayer.PlayOneShot(audioClip1 , 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayClipAtPoint(audioClip1 , Camera.main.transform.position);
        }
    }

    IEnumerator PlayMySounds()
    {
        yield return new WaitForSeconds(2);
        soundPlayer.clip = audioClip1;
        soundPlayer.Play();
        yield return new WaitForSeconds(audioClip1.length);

        soundPlayer.clip = audioClip2;
        soundPlayer.Play();
    }
    
}
