using UnityEngine;
using Woodman;

public class BoundsComponent : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 size = Vector2.one;

    private void OnDrawGizmosSelected()
    {
        DebugDraw.DrawBox(offset, size, Color.green, transform);
    }
}
