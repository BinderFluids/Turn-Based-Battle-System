using System;
using UnityEngine;

namespace Util.Components
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private Transform _transform; 
        [SerializeField] private Transform target;

        private void Start()
        {
            if (target == null)
                target = Camera.main.transform; 
            if (_transform == null)
                _transform = transform;
        }

        private void Update()
        {
            _transform.LookAt(target);
        }
    }
}