using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class StandardToLitConverter : EditorWindow
{
    [MenuItem("Tools/材质转换/Standard to Lit转换器")]
    public static void ShowWindow()
    {
        GetWindow<StandardToLitConverter>("Standard to Lit转换器");
    }

    private Shader litShader;
    private bool backupOriginal = true;
    private string suffix = "_Lit";

    void OnGUI()
    {
        GUILayout.Label("Standard材质转Lit材质", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        litShader = (Shader)EditorGUILayout.ObjectField("目标Lit着色器", litShader, typeof(Shader), false);

        if (litShader == null)
        {
            litShader = Shader.Find("Universal Render Pipeline/Lit");
            if (litShader == null)
                litShader = Shader.Find("Lightweight Render Pipeline/Lit");
        }

        backupOriginal = EditorGUILayout.Toggle("备份原材质", backupOriginal);
        suffix = EditorGUILayout.TextField("新材质后缀", suffix);

        EditorGUILayout.Space();

        if (GUILayout.Button("转换选中材质"))
        {
            ConvertSelectedMaterials();
        }

        if (GUILayout.Button("转换项目中的所有Standard材质"))
        {
            ConvertAllMaterials();
        }
    }

    private void ConvertSelectedMaterials()
    {
        int convertedCount = 0;

        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.sharedMaterials;
                bool changed = false;

                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] != null && IsStandardShader(materials[i].shader))
                    {
                        Material newMat = ConvertMaterial(materials[i]);
                        if (newMat != null)
                        {
                            materials[i] = newMat;
                            changed = true;
                            convertedCount++;
                        }
                    }
                }

                if (changed)
                {
                    renderer.sharedMaterials = materials;
                }
            }
        }

        EditorUtility.DisplayDialog("转换完成", $"成功转换 {convertedCount} 个材质", "确定");
    }

    private void ConvertAllMaterials()
    {
        string[] materialGuids = AssetDatabase.FindAssets("t:Material");
        int convertedCount = 0;

        foreach (string guid in materialGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && IsStandardShader(mat.shader))
            {
                ConvertMaterial(mat);
                convertedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        EditorUtility.DisplayDialog("转换完成", $"成功转换 {convertedCount} 个材质", "确定");
    }

    private bool IsStandardShader(Shader shader)
    {
        return shader != null &&
               (shader.name.Contains("Standard") ||
                shader.name.Contains("Legacy Shaders/"));
    }

    private Material ConvertMaterial(Material originalMat)
    {
        if (litShader == null)
        {
            Debug.LogError("请先指定目标Lit着色器");
            return null;
        }

        Material newMat;

        if (backupOriginal)
        {
            // 创建新材质
            string newPath = AssetDatabase.GetAssetPath(originalMat).Replace(".mat", $"{suffix}.mat");
            newMat = new Material(litShader);
            AssetDatabase.CreateAsset(newMat, newPath);
        }
        else
        {
            // 直接修改原材质
            newMat = originalMat;
            newMat.shader = litShader;
        }

        // 映射属性
        MapStandardToLitProperties(originalMat, newMat);

        EditorUtility.SetDirty(newMat);
        return newMat;
    }

    private void MapStandardToLitProperties(Material source, Material target)
    {
        // 基础颜色/主纹理
        if (source.HasProperty("_Color"))
            target.SetColor("_BaseColor", source.GetColor("_Color"));
        else if (source.HasProperty("_TintColor"))
            target.SetColor("_BaseColor", source.GetColor("_TintColor"));

        if (source.HasProperty("_MainTex"))
            target.SetTexture("_BaseMap", source.GetTexture("_MainTex"));

        // 法线贴图
        if (source.HasProperty("_BumpMap"))
            target.SetTexture("_BumpMap", source.GetTexture("_BumpMap"));

        if (source.HasProperty("_BumpScale"))
            target.SetFloat("_BumpScale", source.GetFloat("_BumpScale"));

        // 金属度和光滑度
        if (source.HasProperty("_MetallicGlossMap"))
            target.SetTexture("_MetallicGlossMap", source.GetTexture("_MetallicGlossMap"));

        if (source.HasProperty("_Metallic"))
            target.SetFloat("_Metallic", source.GetFloat("_Metallic"));

        if (source.HasProperty("_Glossiness"))
            target.SetFloat("_Smoothness", source.GetFloat("_Glossiness"));
        else if (source.HasProperty("_GlossMapScale"))
            target.SetFloat("_Smoothness", source.GetFloat("_GlossMapScale"));

        // 自发光
        if (source.HasProperty("_EmissionMap"))
            target.SetTexture("_EmissionMap", source.GetTexture("_EmissionMap"));

        if (source.HasProperty("_EmissionColor"))
            target.SetColor("_EmissionColor", source.GetColor("_EmissionColor"));

        // 高度图（视差）
        if (source.HasProperty("_ParallaxMap"))
            target.SetTexture("_ParallaxMap", source.GetTexture("_ParallaxMap"));

        if (source.HasProperty("_Parallax"))
            target.SetFloat("_Parallax", source.GetFloat("_Parallax"));

        // 遮挡贴图
        if (source.HasProperty("_OcclusionMap"))
            target.SetTexture("_OcclusionMap", source.GetTexture("_OcclusionMap"));

        if (source.HasProperty("_OcclusionStrength"))
            target.SetFloat("_OcclusionStrength", source.GetFloat("_OcclusionStrength"));

        // 细节纹理
        if (source.HasProperty("_DetailMask"))
            target.SetTexture("_DetailMask", source.GetTexture("_DetailMask"));

        if (source.HasProperty("_DetailAlbedoMap"))
            target.SetTexture("_DetailMap", source.GetTexture("_DetailAlbedoMap"));

        // 渲染模式设置
        SetupRenderMode(source, target);

        // 双面渲染
        if (source.HasProperty("_Cull"))
            target.SetInt("_Cull", source.GetInt("_Cull"));

        Debug.Log($"已转换材质: {source.name} -> {target.name}");
    }

    private void SetupRenderMode(Material source, Material target)
    {
        // 处理渲染模式
        if (source.HasProperty("_Mode"))
        {
            int mode = source.GetInt("_Mode");

            switch (mode)
            {
                case 0: // Opaque
                    target.SetFloat("_Surface", 0);
                    target.SetFloat("_AlphaClip", 0);
                    break;

                case 1: // Cutout
                    target.SetFloat("_Surface", 0);
                    target.SetFloat("_AlphaClip", 1);
                    if (source.HasProperty("_Cutoff"))
                        target.SetFloat("_Cutoff", source.GetFloat("_Cutoff"));
                    break;

                case 2: // Fade
                case 3: // Transparent
                    target.SetFloat("_Surface", 1);
                    target.SetFloat("_AlphaClip", 0);
                    target.SetFloat("_Blend", 0); // Alpha blending
                    break;
            }

            // 设置Alpha模式
            if (mode == 1 && source.HasProperty("_Cutoff"))
            {
                target.SetFloat("_AlphaClip", 1);
                target.SetFloat("_Cutoff", source.GetFloat("_Cutoff"));
            }
        }
    }
}