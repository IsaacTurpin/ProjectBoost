using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuitApplication : MonoBehaviour
{
    bool quit = false;
    void Update()
    {
        if(quit)
        {
            Debug.Log("QUIT");
            Application.Quit();
            quit = false;
        }
    }

    public void Quit(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            quit = true;
        }
    }
}
