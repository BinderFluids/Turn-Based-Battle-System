using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Enums;
using Battle.Interfaces;
using Codice.CM.Client.Differences.Graphic;
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
            battleEntityComponents = GetComponents<BattleEntityComponent>()
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
    
        private Dictionary<Type, BattleEntityComponent> battleEntityComponents;
        public new bool TryGetComponent<T>(out T component)
        {
            //Attempt to find BattleComponent from dictionary
            if (typeof(T).IsAssignableFrom(typeof(BattleEntityComponent)) &&
                battleEntityComponents.TryGetValue(typeof(T), out var cachedBattleComponent) && 
                cachedBattleComponent is T typed)
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
        public bool TryGetComponent(ComponentType type, out BattleEntityComponent component)
        {
            foreach (var kvp in battleEntityComponents)
            {
                BattleEntityComponent battleEntityComponent = kvp.Value;
                if (battleEntityComponent.ComponentType == type)
                {
                    component = battleEntityComponent;
                    return true;
                } 
            }
            
            component = null;  
            return false; 
        }
    }
}