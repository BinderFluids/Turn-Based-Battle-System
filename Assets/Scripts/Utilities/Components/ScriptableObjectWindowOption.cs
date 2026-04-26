using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScriptableObjectWindowOption<T> : MonoBehaviour where T : ScriptableObject
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text text; 
    public event Action<T> onOptionSelected;
    [SerializeField] private T scriptableObject;
    

    private void Start()
    {
        button.onClick.AddListener(OptionSelected);
    }

    public void Initialize(T scriptableObject)
    {
        this.scriptableObject = scriptableObject;
        text.text = scriptableObject.name;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    void OptionSelected()
    {
        onOptionSelected?.Invoke(scriptableObject); 
    }
}