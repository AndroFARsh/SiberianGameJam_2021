using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(SphereCollider))]
public class Gun : MonoBehaviour
{
    [SerializeField] private VisualEffect effect;

    private void OnTriggerEnter(Collider other)
    {
        var cat = other.GetComponent<EnemyCat>();
        if (cat)
        {
            effect.Play();
            Destroy(cat.gameObject, 0.5f);
        }        
    }
}
