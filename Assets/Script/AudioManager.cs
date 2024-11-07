using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("------------Audio------------")]
    public AudioSource audioSource;
    public AudioClip endGame;
    public AudioClip winLv;

    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayAudioEndGame()
    {
        audioSource.clip = endGame;
        audioSource.Play();
    }
    public void PlayAudioNextLv()
    {
        audioSource.clip = winLv;
        audioSource.Play();
    }

    
}
