using UnityEngine;

namespace Assets.Scripts
{
    public class Propeller : MonoBehaviour
    {
        public float RotationSpeed;

        private void Update()
        {
            transform.Rotate(Vector3.forward, Time.deltaTime * RotationSpeed);
        }
    }
}
