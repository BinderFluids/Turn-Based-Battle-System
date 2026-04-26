using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using Battle.Events;
using Registry;
using RequestHub;
using UnityEngine;

namespace Battle
{
    public partial class BattleEntity : MonoBehaviour, IRequestProvider, IDamageSource
    {
        private static List<BattleEntity> _allEntities = new();
        public static IReadOnlyList<BattleEntity> AllEntities => _allEntities;
        
        public PhysicalBattleEntityModifier physicalBattleEntityModifier;

        [SerializeField] private Transform _transform; 
        public Transform Transform => _transform;

        private Pose startPose; 
        public Pose StartPose => startPose;
    
    
        void Awake()
        {
            _allEntities.Add(this); 
            components = GetComponents<BattleEntityComponent>()
                .ToDictionary(c => c.GetType());
        }

        private void Start()
        {
            if (_transform == null)
                _transform = transform; 
            startPose = new Pose(_transform.position, _transform.rotation);
        }

        private void OnDestroy()
        {
            Registry<BattleEntity>.Remove(this); 
        }
    
        private Dictionary<Type, BattleEntityComponent> components;
        public new bool TryGetComponent<T>(out T component)
        {
            //Attempt to find BattleComponent from dictionary
            if (typeof(T).IsAssignableFrom(typeof(BattleEntityComponent)) &&
                components.TryGetValue(typeof(T), out var foundBattleComponent) && 
                foundBattleComponent is T typed)
            {
                component = typed;
                return true;
            }
        
            //Pass through to base TryGetComponent method
            if (gameObject.TryGetComponent(out T foundComponent))
            {
                component = foundComponent;
                return true; 
            }

            //Return false
            component = default; 
            return false;
        }
    }
}