using SimplestarGame;
using System;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(VoronoiFragmenter))]
public class BreakableObject : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] float explosionForce = 300f;
    [SerializeField] float explosionRadius = 1f;

    [Header("Effect")]
    [SerializeField] GameObject breakEffectPrefab;
    [SerializeField] AudioClip breakSE;
    [SerializeField] AudioSource audioSource;

    bool isBroken = false;

    public event Action<BreakableObject> OnBreakTriggered;
    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
            audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
    }

    public void Break(Vector3 hitPoint, Vector3 hitNormal)
    {
        if (isBroken) return;
        isBroken = true;

        if (TryGetComponent(out VoronoiFragmenter fragmenter))
        {
            fragmenter.FragmentAtPoint(hitPoint);
        }

        Collider[] colliders = Physics.OverlapSphere(hitPoint, explosionRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out Rigidbody rb))
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(
                    explosionForce,
                    hitPoint + hitNormal * 0.1f,
                    explosionRadius
                );
            }
        }

        // 🔥 3. エフェクト生成
        if (breakEffectPrefab != null)
        {
            Instantiate(breakEffectPrefab, hitPoint, Quaternion.identity);
        }

        // 🔥 4. SE再生
        if (audioSource != null && breakSE != null)
        {
            audioSource.transform.position = hitPoint;
            audioSource.PlayOneShot(breakSE);
        }

        // 🔥 5. イベント発火
        OnBreakTriggered?.Invoke(this);
    }
}
