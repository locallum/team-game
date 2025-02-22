using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;
    public Animator anim;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotSpeed = 10f;
    [SerializeField] private float dashSpeed = 100f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private GameObject bubblePrefab;

    private Vector3 inputDirection;
    private float dashTime;
    private bool isDashing;
    private bool isCasting;
    private Vector3 dashDirection;

    // Bubble
    public GameObject bubblePrefab;

    public GameObject toggleRing;
    public GameObject toggleSmallRing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleRing.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkForBubble();
        checkForDash();
        checkForRing();
        updateMovementInput();
        rotatePlayer();
    }

    void FixedUpdate()
    {
        Vector3 movement = isDashing ? dashDirection * dashSpeed : inputDirection * moveSpeed;
        controller.Move(movement * Time.deltaTime);
    }

    private void updateMovementInput()
    {
        if (!isDashing && !isCasting)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            // Adjust for 45-degree ortho camera perspective
            inputDirection = (mainCamera.transform.right * h + mainCamera.transform.forward * v).normalized;
            inputDirection.y = 0f; // Ensure movement stays on the XZ plane

            anim.SetBool("isRunning", inputDirection != Vector3.zero);
        }
    }

    private void checkForDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && !isCasting)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashDirection = inputDirection; // Dash in the direction of movement
        }

        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0f) isDashing = false;
        }
    }

    private void checkForBubble()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing && !isCasting)
        {
            Instantiate(bubblePrefab, transform.position + inputDirection, Quaternion.identity);
        }
    }

    private void checkForRing()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            isCasting = true;
            inputDirection = Vector3.zero;
            anim.SetBool("isRunning", false);
            toggleRing.SetActive(true);
            toggleSmallRing.SetActive(true);

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 centerPoint = transform.position;

            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Vector3 targetPosition = raycastHit.point;
                Vector3 direction = targetPosition - centerPoint;

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            }

        }
        else
        {
            isCasting = false;
            toggleRing.SetActive(false);
            toggleSmallRing.SetActive(false);
        }
    }

    private void rotatePlayer()
    {
        if (inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }
}
