using UnityEngine;

public class MeteoriteRing : MonoBehaviour
{
    public ParticleSystem particleSystem;

    void Start()
    {
        particleSystem.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            bool isActive = particleSystem.gameObject.activeInHierarchy;

            if (isActive) {
                particleSystem.Stop();
                particleSystem.gameObject.SetActive(false);
            } else {
                particleSystem.gameObject.SetActive(true);
                particleSystem.Play();
            }
        }
    }
}