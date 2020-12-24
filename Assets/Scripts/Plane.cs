using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class Plane : MonoBehaviour
    {
        private const float PitchSpeed = 45f;
        private const float MaxPitch = 45f;
        private const float Precision = 0.01f;

        private float _initialAltitude;
        private float _minAltitude;
        private float _maxAltitude;
        private float _threshold;

        private void Start()
        {
            _initialAltitude = transform.position.y;
            var planeHeight = GetComponent<MeshRenderer>().bounds.size.y;
            var distanceToCamera = Math.Abs(transform.position.x - Camera.main.transform.position.x);
            _minAltitude = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, distanceToCamera)).y ;
            _maxAltitude = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, distanceToCamera)).y - planeHeight;
            _threshold = (_maxAltitude - _minAltitude) * 0.2f;
        }

        private void Update()
        {
            var altitude = transform.position.y;
            var currentPitch = CurrentPitchAngle(); // Down: positive; Up: negative

            // Fast in the middle of the screen, slows to the up and down edges
            var acceleration = NormalizeAltitude(altitude);

            var pitchInput = Input.GetAxis("Vertical"); // Down: 1; Up: -1

            // Keep the plane within the allowed heights and pitches ranges
            if (pitchInput > 0f && altitude <= _minAltitude + _threshold ||
                pitchInput < 0f && altitude >= _maxAltitude - _threshold)
            {
                pitchInput = 0f;
            }

            // Align the plane horizontally if no user input
            if (pitchInput == 0f)
            {
                if (Math.Abs(currentPitch) < Precision)
                {
                    return;
                }

                pitchInput = -transform.rotation.x / (acceleration == 0f ? 1f : acceleration);
            }

            var deltaPitch = Time.deltaTime * PitchSpeed * pitchInput;
            var expectedPitch = currentPitch + deltaPitch;

            if (Math.Abs(expectedPitch) <= MaxPitch)
            {
                transform.Rotate(Vector3.right, deltaPitch);
                currentPitch = expectedPitch;
            }

            if (currentPitch < 0f && altitude < _initialAltitude ||
                currentPitch > 0f && altitude > _initialAltitude)
            {
                acceleration = 1f;
            }

            var deltaAlt = currentPitch * Time.deltaTime * -1f * acceleration;
            var expectedAlt = altitude + deltaAlt;

            if (expectedAlt >= _minAltitude && expectedAlt <= _maxAltitude)
            {
                transform.Translate(0f, deltaAlt, 0f, Space.World);
            }
        }

        private float CurrentPitchAngle()
        {
            var pitchAngle = transform.eulerAngles.x;
            return pitchAngle > 180f ? pitchAngle - 360f : pitchAngle;
        }

        private float NormalizeAltitude(float value)
        {
            const float newMax = (float)Math.PI / 2f;
            const float newMin = -newMax;
            var min = _minAltitude;
            var max = _maxAltitude;
            var normalized = (newMax - newMin) / (max - min) * (value - min) + newMin;

            return (float)Math.Cos(normalized);
        }
    }
}