using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] bool thrustActivated = false;
    [SerializeField] float mainThrust = 20000f;

    Vector2 input;
    [SerializeField] float rotationThrust = 100f;

    AudioSource audioSource;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

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
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        
        if(!mainEngineParticles.isPlaying) 
        {
            mainEngineParticles.Play();
        }
        Invoke(nameof(StopActiveThrust), 0.1f);
    }
    void StopActiveThrust()
    {
        thrustActivated = false;
    }
    private void ThrustInput()
    {
        if(thrustActivated) 
        {
            ProcessThrust();
            //thrustActivated = false;
        }
        else if(audioSource.isPlaying && !thrustActivated)
        {
            Invoke(nameof(StopAudio), 0.2f);
        }

    }

    public void StopAudio()
    {
        audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void ProcessRotation()
    {
        float xValue = input.x;
        if (xValue < 0)
        {
            RotateLeft();
        }

        else if (xValue > 0) 
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void RotateLeft()
    {
        ApplyRotation(rotationThrust);
        if (!rightThrusterParticles.isPlaying)
        {
            rightThrusterParticles.Play();
        }
    }
    void RotateRight()
    {
        ApplyRotation(-rotationThrust);
        if (!leftThrusterParticles.isPlaying)
        {
            leftThrusterParticles.Play();
        }
    }
    void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
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
