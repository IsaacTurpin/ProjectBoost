using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    public GameObject deathEffect;
    public GameObject rocket;
    [SerializeField] float levelLoadDelay = 1.0f;

    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;
    AudioSource audioSourceCrash;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //audioSourceCrash = deathEffect.GetComponent<AudioSource>();
        audioSourceCrash = crashParticles.GetComponent<AudioSource>();
    }

    private void Update()
    {
        RespondToDebugKeys();

    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled) return;

        switch(other.gameObject.tag) 
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;

            case "Finish":
                StartSuccessSequence();
                break;

            case "Fuel":
                Debug.Log("Fuel");
                break;

            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke(nameof(LoadNextLevel), levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
        //deathEffect.transform.SetParent(null);
        //rocket.SetActive(false);
        //deathEffect.SetActive(true);
        audioSourceCrash.PlayOneShot(crash);
        crashParticles.Play();
        Invoke(nameof(ReloadLevel), levelLoadDelay);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
}
