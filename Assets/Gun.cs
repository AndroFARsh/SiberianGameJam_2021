using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Gun : MonoBehaviour
{
    [SerializeField] private CityPlace place;

    private void OnTriggerEnter(Collider other)
    {
        if (!place.IsEmpty)
        {
            var cat = other.GetComponent<EnemyCat>();
            if (cat)
            {
                Destroy(cat, 0.5f);
            }
        }
    }
}
