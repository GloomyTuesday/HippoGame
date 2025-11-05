using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.BaseSystems.Core
{
    [CreateAssetMenu(fileName = "RaycasterTools", menuName = "Scriptable Obj/Base systems/Core/Raycaster/Raycaster tools")]
    public class RaycasterToolsSrc : ScriptableObject, IRaycasterTools, IActiveStateAccessible
    {
        [SerializeField, Header("This field is used only to visualize active state")]
        private bool _active;

        private List<GameObject> LineRendererGameObject { get; set; } = new List<GameObject>(); 
        private List<LineRenderer> LineRendererList { get; set; } = new List<LineRenderer>();

        private GameObject _holder;

        [NonSerialized]
        private bool _ready = true; 
        public bool Ready { 
            get => _ready;
            set
            {
                _ready = value;
                _active = _ready;
            }
        }

        private void OnValidate()
        {
            if (!Application.isPlaying) return;
            _active = true;
            _ready = true; 
        }

        //  Area of effect
        //  Single ray cast
        public Collider CastRayHit(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask)
        {
            if (!Ready) return null; 

            RaycastHit hit; 
            var hitData = Physics.Raycast(origin , direction ,out hit, rayLength , layerMask); 
            return hit.collider; 
        }

        public Collider[] CastRayHitAll(Vector3 origin, Vector3 direction, float rayLength, LayerMask layerMask)
        {
            if (!Ready) return new Collider[0];

            RaycastHit[] hitResults = Physics.RaycastAll(origin, direction, rayLength, layerMask);
            var collidersHit = new Collider[hitResults.Length];

            for (int i = 0; i < hitResults.Length; i++)
                collidersHit[i] = hitResults[i].collider;

            return collidersHit;
        }

        public Collider CastCameraRayHit(Camera cam, LayerMask layerMask, Vector2 position)
        {
            if (!Ready) return null;

            Ray cameraRay = cam.ScreenPointToRay(position);
            var hitResult = Physics.Raycast(cameraRay, out RaycastHit raycastHit, float.MaxValue, layerMask );
            return raycastHit.collider; 
        }

        public Collider[] CastCameraRayHitAll(Camera cam, LayerMask layerMask, Vector2 position)
        {
            if (!Ready) return new Collider[0];

            Ray cameraRay = cam.ScreenPointToRay(position);
            RaycastHit[] hitResults = Physics.RaycastAll(cameraRay, float.MaxValue, layerMask);
            var collidersHit = new Collider[hitResults.Length];

            for (int i = 0; i < hitResults.Length; i++)
                collidersHit[i] = hitResults[i].collider;

            return collidersHit;
        }
    }
}

