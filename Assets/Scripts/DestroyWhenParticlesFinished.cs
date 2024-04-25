using UnityEngine;

public class DestroyWhenParticlesFinished : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;

    private void Update()
    {
        if (!this.GetComponent<ParticleSystem>().IsAlive())
        {
            Destroy(this.particleSystem.gameObject);
        }
    }
}