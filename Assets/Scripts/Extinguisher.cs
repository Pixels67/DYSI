using System.Collections;
using UnityEngine;

public class Extinguisher : Item
{
    [SerializeField] private ParticleSystem foamParticleSystem;
    [SerializeField] private float foamDuration;

    private static Coroutine _foamCoroutine;

    private void Awake()
    {
        foamParticleSystem.Stop();
    }

    public override void Use(GameObject obj)
    {
        if (_foamCoroutine != null)
        {
            return;
        }

        if (obj.CompareTag("Fire"))
        {
            obj.GetComponent<ParticleSystem>()?.Stop();
            _foamCoroutine = StartCoroutine(Foam(obj));
        }
    }

    private IEnumerator Foam(GameObject obj)
    {
        var particle = Instantiate(foamParticleSystem, foamParticleSystem.transform.position, foamParticleSystem.transform.rotation);

        particle.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(foamDuration);

        particle.GetComponent<ParticleSystem>().Stop();
        _foamCoroutine = null;
        
        Destroy(particle);
        obj.SetActive(false);
    }
}