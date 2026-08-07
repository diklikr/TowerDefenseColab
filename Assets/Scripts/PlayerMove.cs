using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpHeight = 1.5f;
    public float gravity = -18f;
    public float mouseSensitivity = 2.0f;
    public Transform cameraTransform;
    public Transform groundCheck;
    public float groundDistance = 0.35f;
    public LayerMask groundMask;

    CharacterController controller;
    Vector3 velocity;
    float pitch;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f) velocity.y = -2f;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(x, 0f))
        {
            if (Input.GetKey(KeyCode.A)) x = -1f;
            if (Input.GetKey(KeyCode.D)) x = 1f;
        }

        Vector3 input = Vector3.ClampMagnitude(new Vector3(x, 0f, z), 1f);
        Vector3 move = transform.right * input.x + transform.forward * input.z;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float mouseX = Input.mousePositionDelta.x * mouseSensitivity;
        float mouseY = Input.mousePositionDelta.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
        if (cameraTransform != null) cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}