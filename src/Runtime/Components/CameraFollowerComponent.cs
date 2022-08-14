using Mirror;
using UnityEngine;

namespace MyRpg.Components
{
    public class CameraFollowerComponent : NetworkBehaviour
    {
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
        }

        private void Update()
        {
            if (!isLocalPlayer) return;
            _cameraTransform.position = new Vector3
            {
                x = transform.position.x,
                y = transform.position.y,
                z = _cameraTransform.position.z
            };
        }
    }
}