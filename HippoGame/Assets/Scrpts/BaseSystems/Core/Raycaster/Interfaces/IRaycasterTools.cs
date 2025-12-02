using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    public interface IRaycasterTools 
    {
        public bool Ready { get; set; }

        public Collider CastRayHit(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask);
        public Collider[] CastRayHitAll(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask);

        public Collider CastCameraRayHit(Camera cam, LayerMask layerMask, Vector2 position);
        public Collider[] CastCameraRayHitAll(Camera cam, LayerMask layerMask, Vector2 position);
        
    }
}
