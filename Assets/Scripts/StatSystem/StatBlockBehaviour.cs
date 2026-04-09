
using System;
using UnityEngine;

public class StatBlockBehaviour : MonoBehaviour
{
    [SerializeField] private StatBlockTemplate statBlockTemplate;
    public StatBlock statBlock;

    private void Start()
    {
        statBlock = statBlockTemplate.StatBlock;
    }
}