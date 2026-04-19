using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScriptableObjectWindow<T> : MonoBehaviour where T : ScriptableObject
{
    private List<T> scriptableObjects;
    private ScriptableObjectAssetRetriever<T> retriever;

    
    [SerializeField] private ScriptableObjectWindowOption<T> optionPrefab;
    [SerializeField] private RectTransform optionContainer; 
    private List<ScriptableObjectWindowOption<T>> options = new(); 
    
    [SerializeField] private UnityEvent<T> onOptionSelected;
    
    private void Start()
    {
        retriever = new ScriptableObjectAssetRetriever<T>();
        scriptableObjects = retriever.GetAllInstances();
        GenerateOptions();
    }

    public void GenerateOptions()
    {
        options.Clear();
        //loop through options list backwards
        for (int i = optionContainer.childCount - 1; i >= 0; i--)
        {
            if (!optionContainer.GetChild(i).gameObject.activeSelf) continue; 
            
            ScriptableObjectWindowOption<T> option = optionContainer.GetChild(i).GetComponent<ScriptableObjectWindowOption<T>>();
            option.onOptionSelected -= TriggerOptionSelected;
            Destroy(option.gameObject);
        }
        
        foreach (T scriptableObject in scriptableObjects)
        {
            GameObject newOptionGameObject = Instantiate(optionPrefab.gameObject, optionContainer);
            newOptionGameObject.SetActive(true); 

            ScriptableObjectWindowOption<T> newOption = newOptionGameObject.GetComponent<ScriptableObjectWindowOption<T>>();
            newOption.Initialize(scriptableObject);
            options.Add(newOption); 
            
            newOption.onOptionSelected += TriggerOptionSelected;
        }
    }

    void TriggerOptionSelected(T option)
    {
        onOptionSelected?.Invoke(option);
    }
}