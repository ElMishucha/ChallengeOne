using UnityEngine;

namespace Assets.Scripts
{
    public class CameraMovement : MonoBehaviour
    {
        public GameObject Plane;

        private Vector3 _offset;

        // Start is called before the first frame update
        private void Start()
        {
            _offset = transform.position - Plane.transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            //transform.position = Plane.transform.position + _offset;
        }
    }
}
