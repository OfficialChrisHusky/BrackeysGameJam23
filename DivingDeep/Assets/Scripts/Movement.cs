using UnityEngine;

public class Movement : MonoBehaviour {
    
    [Header("Movement")]
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float jumpForce = 10.0f;
    [SerializeField] private float airMultiplier = 0.5f;
    [SerializeField] private float groundDrag = 3.0f;
    [SerializeField] private float airDrag = 5.0f;

    [Header("Sensitivity")]
    [SerializeField] private float sensitivity = 1.0f;

    [Header("Ground Detection")]
    [SerializeField] private bool grounded = true;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;

    [Header("Body Parts")]
    [SerializeField] private Transform eyes;

    Vector3 direction;
    Vector2 rot;

    float multiplier = 1.0f;

    Rigidbody rb;

    void Start() {
        
        rot.x = transform.rotation.eulerAngles.x;
        rot.y = transform.rotation.eulerAngles.y;

        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update() {

        if (grounded) {

            rb.drag = groundDrag;
            multiplier = 1.0f;

        } else {

            rb.drag = airDrag;
            multiplier = airMultiplier;
            
        }

        if (grounded && rb.drag == airDrag) rb.drag = groundDrag;
        else if (!grounded && rb.drag == groundDrag) rb.drag = airDrag;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");
        direction = transform.forward * vertical + transform.right * horizontal;
        
        rot.y += Input.GetAxis("Mouse X") * sensitivity;
        rot.x -= Input.GetAxis("Mouse Y") * sensitivity;
        rot.x  = Mathf.Clamp(rot.x, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(0.0f, rot.y, 0.0f);
        eyes.rotation = Quaternion.Euler(rot.x, rot.y, 0.0f);

        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if(grounded && Input.GetKeyDown(KeyCode.Space)) {

            rb.velocity = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        }

    }

    void FixedUpdate() {
        
        rb.AddForce(direction.normalized * speed * multiplier * 1000.0f * Time.deltaTime);
        
    }

}