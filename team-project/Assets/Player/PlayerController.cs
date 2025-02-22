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
    private Vector3 dashDirection;

    void Update()
    {
        checkForBubble();
        checkForDash();
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
        if (!isDashing)
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
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
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isDashing)
        {
            Instantiate(bubblePrefab, transform.position + inputDirection, Quaternion.identity);
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
