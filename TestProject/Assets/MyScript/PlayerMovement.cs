using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 15f;
    public float runSpeed = 25f;
    public float jumpPower;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;
    public AudioSource started;
    public AudioSource normal;
    public AudioSource run;
    public AudioSource speedRun;
    public float fadeSpeed ; // Tốc độ fade âm thanh

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;

    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Bắt đầu với âm thanh đứng yên
        started.volume = 1f;
        normal.volume = 1f;
        run.volume = 0f;
        speedRun.volume = 0f;
    }

    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 15f;
            runSpeed = 25f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        // Kiểm tra trạng thái di chuyển
        if (characterController.velocity.magnitude > 1f) // Nhân vật đang di chuyển
        {
            if (isRunning)
            {
                FadeAudio(speedRun, 1f);
                FadeAudio(run, 0f);
                FadeAudio(normal, 0f);
            }
            else
            {
                FadeAudio(speedRun, 0f);
                FadeAudio(run, 1f);
                FadeAudio(normal, 0f);
            }
        }
        else // Nhân vật đứng yên
        {
            FadeAudio(speedRun, 0f);
            FadeAudio(run, 0f);
            FadeAudio(normal, 1f);
        }

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    // Hàm điều chỉnh âm lượng dần dần
    void FadeAudio(AudioSource audioSource, float targetVolume)
    {
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, targetVolume, fadeSpeed * Time.deltaTime);

        // Bật hoặc tắt phát âm thanh khi cần thiết
        if (targetVolume > 0f && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (targetVolume == 0f && audioSource.isPlaying && audioSource.volume == 0f)
        {
            audioSource.Stop();
        }
    }
}