using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private Vector3 inputDirection;

    [SerializeField] private Camera mainCamera;

    public float moveSpeed = 10f;
    public float rotSpeed = 10f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        updateMovementInput();
        rotatePlayer();
    }

    void FixedUpdate()
    {
        controller.Move(inputDirection * moveSpeed * Time.deltaTime);
    }

    private void updateMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDirection = new Vector3(h, 0f, v).normalized;

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

        // if (inputDirection != Vector3.zero)
        // {
        //     Quaternion targetRotation = Quaternion.LookRotation(inputDirection);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        // }
    }
}
