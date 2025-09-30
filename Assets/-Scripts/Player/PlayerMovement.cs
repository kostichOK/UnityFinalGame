using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 0.5f;
    public float runSpeed = 12f;
    public float gravity = -500f;
    public float lookSpeed = 0f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private AudioSource audioSource;

    private bool canMove = true;
    public float walkSpeedRef;
    private float runSpeedRef;

    // Footstep system
    [System.Serializable]
    public class SurfaceFootstep
    {
        public string tag; // тег поверхности, например "Wood", "Grass"
        public AudioClip[] footstepSounds;
    }

    public SurfaceFootstep[] surfaces;
    public float stepInterval = 0.5f;
    private float stepTimer;

    // Триггерная поверхность
    private SurfaceFootstep currentTriggerSurface = null;

    void Start()
    {
        walkSpeedRef = walkSpeed;
        runSpeedRef = runSpeed;

        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;
        else
            moveDirection.y = -300f;

        if (Input.GetKey(KeyCode.LeftControl) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;
        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = walkSpeedRef;
            runSpeed = runSpeedRef;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        Vector3 horizontalVelocity = new Vector3(moveDirection.x, 0, moveDirection.z);

        if (characterController.isGrounded && horizontalVelocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
    }

    private void PlayFootstep()
    {
        // Если есть триггерная поверхность
        if (currentTriggerSurface != null && currentTriggerSurface.footstepSounds.Length > 0)
        {
            int index = Random.Range(0, currentTriggerSurface.footstepSounds.Length);
            audioSource.PlayOneShot(currentTriggerSurface.footstepSounds[index]);
            return;
        }

        // Иначе проверяем обычный Raycast вниз
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            foreach (var surface in surfaces)
            {
                if (hit.collider.CompareTag(surface.tag))
                {
                    if (surface.footstepSounds.Length > 0)
                    {
                        int index = Random.Range(0, surface.footstepSounds.Length);
                        audioSource.PlayOneShot(surface.footstepSounds[index]);
                    }
                    return;
                }
            }
        }
    }

    // Триггеры для специальных поверхностей
    private void OnTriggerEnter(Collider other)
    {
        foreach (var surface in surfaces)
        {
            if (other.CompareTag(surface.tag))
            {
                currentTriggerSurface = surface;
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentTriggerSurface != null && other.CompareTag(currentTriggerSurface.tag))
        {
            currentTriggerSurface = null;
        }
    }
}