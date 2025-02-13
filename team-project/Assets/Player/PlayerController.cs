using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 inputDirection;

    [SerializeField] private Camera mainCamera;

    // Move variables
    public float moveSpeed = 10f;
    public float rotSpeed = 10f;

    // Dash variables
    public float dashSpeed = 100f;
    public float dashDuration = 0.5f;

    private float dashTime;
    private bool isDashing;
    private Vector3 dashDirection;

    // Bubble
     public GameObject bubblePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        checkForBubble();
        checkForDash();
        updateMovementInput();
        rotatePlayer();
    }

    void FixedUpdate()
    {
        if (isDashing) {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);
        } else {
            controller.Move(inputDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void updateMovementInput()
    {
        if (!isDashing)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            inputDirection = new Vector3(h, 0f, v).normalized;
        }
    }

    private void checkForDash() 
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashDirection = transform.forward;
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f)
            {
                isDashing = false;
            }
        }
    }

    private void checkForBubble() {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing) {
            Instantiate(bubblePrefab, transform.position + transform.forward, Quaternion.identity);
        }
    }

    private void rotatePlayer() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        float rayDistance;
        if (groundPlane.Raycast(ray, out rayDistance)) {
            Vector3 targetPoint = ray.GetPoint(rayDistance);
            
            Vector3 inputDirection = targetPoint - transform.position;
            inputDirection.y = 0;

            if (inputDirection != Vector3.zero) {
                Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            }

        }
    }
}
