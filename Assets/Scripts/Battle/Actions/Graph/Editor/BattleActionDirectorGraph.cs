using System;
using Unity.GraphToolkit.Editor;
using UnityEditor;


[Serializable]
[Graph(AssetExtension)]
internal class BattleActionDirectorGraph : Graph
{
    internal const string AssetExtension = "battlegraph";

    [MenuItem("Assets/Create/Battle/Action/Graph")]
    static void CreateAssetFile()
    {
        GraphDatabase.PromptInProjectBrowserToCreateNewAsset<BattleActionDirectorGraph>();
    }

    public override void OnGraphChanged(GraphLogger graphLogger)
    {
        base.OnGraphChanged(graphLogger);
        //TODO Add error checking / validation
    }
}