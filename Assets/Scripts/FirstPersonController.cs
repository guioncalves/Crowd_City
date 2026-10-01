using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpSpeed = 5f;
    public float gravity = 20f;

    [Header("Mouse")]
    public float mouseSensitivity = 2f;
    public Camera playerCamera;

    private CharacterController controller;
    private Vector3 moveDirection;

    private float cameraRotationX = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // =========================
        // OLHAR COM O RATO
        // =========================

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);

        cameraRotationX -= mouseY;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90f, 90f);

        playerCamera.transform.localRotation =
            Quaternion.Euler(cameraRotationX, 0f, 0f);


        // =========================
        // MOVIMENTO
        // =========================

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float speed = Input.GetKey(KeyCode.LeftShift)
            ? runSpeed
            : walkSpeed;

        Vector3 movement =
            transform.right * horizontal +
            transform.forward * vertical;

        movement *= speed;


        // =========================
        // SALTO E GRAVIDADE
        // =========================

        if (controller.isGrounded)
        {
            moveDirection.y = -1f;

            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        moveDirection.x = movement.x;
        moveDirection.z = movement.z;

        controller.Move(moveDirection * Time.deltaTime);
    }
}