using System;
using UnityEngine;

namespace MyRpg.Components
{
    public class RainbowSkyboxComponent : MonoBehaviour
    {
        
        private Camera _camera;
        private Color _originalColor;
        private float _speed = 0.5f;
        private float _t;
        private bool _play;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (_camera == null)
            {
                enabled = false;
                return;
            }

            _originalColor = _camera.backgroundColor;
            Color.RGBToHSV(_originalColor, out var startingHue, out _, out _);
            _t = startingHue;
        }

        private void Update()
        {
            if (_play)
            {
                _t += Time.deltaTime * _speed;
                var hue = _t % 1;
                Color.RGBToHSV(_originalColor, out _, out var s, out var v);
                _camera.backgroundColor = Color.HSVToRGB(hue, s, v);
            }

            if (Input.GetKeyDown(KeyCode.P)) _play = !_play;
            if (Input.GetKeyDown(KeyCode.O)) _speed *= 1.5f;
            if (Input.GetKeyDown(KeyCode.L)) _speed /= 1.5f;
        }
    }
}