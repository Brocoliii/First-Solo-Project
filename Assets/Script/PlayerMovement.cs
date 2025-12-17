using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Camera")]
    public Camera playerCamera;
    public float mouseSensitivity = 100f;

    // ตัวแปรภายในสำหรับคำนวณ
    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        // ล็อกเมาส์ให้อยู่กลางจอและซ่อนเมาส์
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // 1. เช็คว่ายืนอยู่บนพื้นไหม (Ground Check)
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // กดตัวให้ติดพื้นเล็กน้อยเพื่อให้เดินลงเนินสมูท
        }

        // 2. โค้ดส่วนหันหน้า (Mouse Look)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // จำกัดมุมก้มเงยไม่ให้คอหัก

        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // หมุนกล้องขึ้นลง
        transform.Rotate(Vector3.up * mouseX); // หมุนตัวซ้ายขวา

        // 3. โค้ดส่วนการเดิน (Movement)
        float x = Input.GetAxis("Horizontal"); // ปุ่ม A, D
        float z = Input.GetAxis("Vertical");   // ปุ่ม W, S

        // เช็คว่ากด Shift เพื่อวิ่งหรือไม่
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // คำนวณทิศทางเดิน (อิงตามทิศที่ตัวละครหันหน้าอยู่)
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed * Time.deltaTime);

        // 4. โค้ดส่วนกระโดด (Jump)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // สูตรฟิสิกส์: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // 5. ใส่แรงโน้มถ่วง (Gravity)
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
