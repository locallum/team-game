using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Camera mainCamera;
    public Animator anim;

    [SerializeField] private float defaultMoveSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashMoveSpeed;
    [SerializeField] private float basicAtkDuration;

    private Vector3 inputDirection;
    private float currentMoveSpeed;

    private bool canCast = true;
    private bool canMove = true;

    // Bubble
    public GameObject bubblePrefab;
    public GameObject toggleRing;
    public GameObject toggleSmallRing;

    void Start()
    {
        toggleRing.SetActive(false);
        toggleSmallRing.SetActive(false);
        currentMoveSpeed = defaultMoveSpeed;
    }

    void Update()
    {
        //attack and ability checks
        checkForBubble();
        checkForDash();
        checkForRing();
        checkForAtk();

        //movement checks
        updateMovementInput();
        rotatePlayer();
    }

    void FixedUpdate()
    {
        Vector3 movement = inputDirection * currentMoveSpeed;
        controller.Move(movement * Time.deltaTime);
    }

    private void updateMovementInput()
    {
        if (canMove)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            inputDirection = (mainCamera.transform.right * h + mainCamera.transform.forward * v).normalized;
            inputDirection.y = 0f;

            anim.SetBool("isRunning", inputDirection != Vector3.zero);
        }
    }

    private void checkForAtk()
    {
        if (Input.GetMouseButtonDown(0) && canCast)
        {
            StartCoroutine(BasicAtkCoroutine());
        }
    }

    private IEnumerator BasicAtkCoroutine()
    {
        anim.SetTrigger("basicAtk");
        canCast = false;

        yield return new WaitForSeconds(basicAtkDuration);

        canCast = true;
    }

    private void checkForDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canCast)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canCast = false;
        currentMoveSpeed = dashMoveSpeed;
        anim.SetTrigger("roll");

        yield return new WaitForSeconds(dashDuration);

        currentMoveSpeed = defaultMoveSpeed;
        canCast = true;
    }

    private void checkForBubble()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && canCast)
        {
            Instantiate(bubblePrefab, transform.position + inputDirection, Quaternion.identity);
        }
    }

    private void checkForRing()
    {
        if (Input.GetKey(KeyCode.Q) && canCast)
        {
            canMove = false;
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
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.Euler(0, targetRotation.eulerAngles.y, 0),
                    rotSpeed * Time.deltaTime
                );
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            canMove = true;
            toggleRing.SetActive(false);
            toggleSmallRing.SetActive(false);
        }
    }

    private void rotatePlayer()
    {
        if (inputDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.Euler(0, targetRotation.eulerAngles.y, 0),
                rotSpeed * Time.deltaTime
            );
        }
    }
}
