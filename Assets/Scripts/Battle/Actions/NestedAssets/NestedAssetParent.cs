using System;
using System.Collections.Generic;
using UnityEngine;


    public abstract class NestedAssetParent : ScriptableObject 
    {
        
        [SerializeField, HideInInspector] private List<NestedAsset> nestedChildren = new();
        public List<NestedAsset> NestedChildren => nestedChildren;
        public abstract Type nestedAssetChildType { get; }
    }
