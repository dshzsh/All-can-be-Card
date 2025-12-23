using UnityEditor;
using UnityEngine;

public class SimpleAutoRefresh
{
    [InitializeOnLoadMethod]
    public static void Init()
    {
        // 每次进入 Play 模式时自动刷新
        EditorApplication.playModeStateChanged += (state) =>
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                AssetDatabase.Refresh();
                Debug.Log("自动更新资源中……");
            }
        };
    }
}