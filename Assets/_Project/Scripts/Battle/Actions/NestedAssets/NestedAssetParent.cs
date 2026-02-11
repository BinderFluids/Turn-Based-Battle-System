using System;
using System.Collections.Generic;
using UnityEngine;


    public abstract class NestedAssetParent : ScriptableObject 
    {
        
        [SerializeField, HideInInspector] private List<NestedAsset> _childs = new();
        public abstract Type nestedAssetChildType { get; }


#if UNITY_EDITOR
        public static string childs_prop_name => nameof(_childs);
#endif
    }
