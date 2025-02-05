using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideNSlideHandler : MonoBehaviour
{
    public int maxBounces = 5;
    public float skinWidth = 0.015f;
    Bounds bounds;
    [SerializeField] Collider collider;
    [SerializeField] LayerMask layerMask;

    private void FixedUpdate()
    {
        bounds = collider.bounds;
        bounds.Expand(-2 * skinWidth);
        Debug.Log(bounds);
    }
    private void OnDrawGizmos()
    {
        if (bounds == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bounds.extents.x + skinWidth);
    }
    private Vector3 CollideAndSlide(Vector3 vel, Vector3 pos, int depth)
    {
        if (depth >= maxBounces) //if reach max bounce then zero no recursion no calculation
        {
            return Vector3.zero;
        }
        float dist = vel.magnitude + skinWidth;
        RaycastHit hit;
        if (Physics.SphereCast(pos, bounds.extents.x, vel.normalized, out hit, dist, layerMask))
        {
            Vector3 snapToSurface = vel.normalized * (hit.distance - skinWidth);
            Vector3 leftover = vel - snapToSurface;
            if(snapToSurface.magnitude <= skinWidth)
            {
                snapToSurface = Vector3.zero;
            }
            float mag = leftover.magnitude;
            leftover = Vector3.ProjectOnPlane(leftover, hit.normal).normalized;
            leftover *= mag;

            return snapToSurface + CollideAndSlide(leftover, pos + snapToSurface, depth + 1);
        }
        return vel;
    }

}
