using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    public bool thrustActivated = false;
    [SerializeField] float mainThrust = 20000f;

    Vector2 input;
    [SerializeField] float rotationThrust = 100f;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ThrustInput();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        
    }

    private void ThrustInput()
    {
        if(thrustActivated) 
        {
            ProcessThrust();
            thrustActivated = false;
        }
        else if(audioSource.isPlaying && !thrustActivated)
        {
            Invoke(nameof(StopAudio), 0.2f);
        }

    }

    public void StopAudio()
    {
        audioSource.Stop();
    }

    void ProcessRotation()
    {
        float xValue = input.x;
        if (xValue < 0)
        {
            ApplyRotation(rotationThrust);
        }

        else if (xValue > 0) 
        {
            ApplyRotation(-rotationThrust);
        }
    }

    public void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // frezzing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }

    public void Move(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void Thrust(InputAction.CallbackContext context)
    { 
        if(context.started)
        {
            thrustActivated = true;
        }
    }
}
