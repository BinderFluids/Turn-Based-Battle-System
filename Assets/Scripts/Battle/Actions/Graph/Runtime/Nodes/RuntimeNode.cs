using System;
using System.Collections.Generic;

[Serializable]
public abstract class RuntimeNode
{
    public List<int> NextNodeIndices = new();
}