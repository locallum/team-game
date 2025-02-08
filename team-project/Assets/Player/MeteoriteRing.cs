using UnityEngine;

public class MeteoriteRing : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ParticleSystem particleSystem;
    void Start()
    {
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (!particleSystem.isPlaying) {
                particleSystem.Play();
            }
        } else {
            particleSystem.Stop();
        }
    }
}
