using UnityEngine;

public class MeteorRing : MonoBehaviour
{
    public Camera mainCamera;
    public float radius = 5f;
    public GameObject panda;



    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        Vector3 centerPoint = panda.transform.position;

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            Vector3 targetPosition = raycastHit.point;

            Vector3 direction = targetPosition - centerPoint;

            if (direction.magnitude > radius)
            {
                direction.Normalize();
                targetPosition = centerPoint + direction * radius;
            }

            transform.position = targetPosition;
        }
    }
}
