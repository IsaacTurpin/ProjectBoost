using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        switch(other.gameObject.tag) 
        {
            case "Friendly":
                Debug.Log("Friendly");
                break;

            case "Finish":
                Debug.Log("Finish");
                break;

            case "Fuel":
                Debug.Log("Fuel");
                break;

            default:
                ReloadLevel();
                Debug.Log("Explode");
                break;
        }
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
