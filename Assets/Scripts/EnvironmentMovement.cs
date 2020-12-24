using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnvironmentMovement : MonoBehaviour
    {
        public float Angle = 7.0f;


        // Update is called once per frame
        private void Update()
        {
            transform.RotateAround(transform.position, transform.up, -1.0f * Angle * Time.deltaTime);
        }
    }
}
