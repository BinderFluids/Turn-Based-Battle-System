using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NestedAssetList<T> where T : ScriptableObject
{
    [SerializeField, HideInInspector] private ScriptableObject parentAsset;
    [SerializeField] private T[] nestedAssets;
    
    public IReadOnlyList<T> NestedAssets => nestedAssets;
}
