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
            GetComponent<AudioSource>()?.Play();
        }
    }

    private IEnumerator Foam(GameObject obj)
    {
        foamParticleSystem.Play();
        yield return new WaitForSeconds(foamDuration);

        foamParticleSystem.Stop();
        _foamCoroutine = null;
        
        obj.SetActive(false);
    }
}