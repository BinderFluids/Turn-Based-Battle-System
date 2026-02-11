using UnityEngine;


    public class NestedAsset : ScriptableObject
    {
        [SerializeField, Delayed, HideInInspector] private string _editorName = nameof(NestedAsset);

#if UNITY_EDITOR
        public static string editor_name_prop_name = nameof(_editorName);
#endif
    }