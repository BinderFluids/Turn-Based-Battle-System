using System;
using UnityEngine;

namespace Battle
{
    public class BattleEnvironmentGizmos : MonoBehaviour
    {
        [Serializable]
        public struct TransformColorTuple
        {
            [SerializeField] private Transform transform;
            public Transform Transform => transform;
            
            [SerializeField] private Color color;
            public Color Color => color;
        }

        [SerializeField, Range(0.1f, 1f)] private float gizmoSphereSize; 
        [SerializeField] private TransformColorTuple player; 
        [SerializeField] private TransformColorTuple partner;
        
        
        private void OnDrawGizmos()
        {
            DrawSphere(player.Color, player.Transform, gizmoSphereSize);
            DrawSphere(partner.Color, partner.Transform, gizmoSphereSize);
        }

        private void DrawSphere(Color color, Transform transform, float size)
        {
            if (transform == null) return;
            
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, size);
        }
    }
}
