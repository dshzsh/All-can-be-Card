using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestWorld : MonoBehaviour
{
    public GameObject obj;
    public Material shaderS1;

    public string text = "Hello 3D";
    public float thickness = 0.2f;
    public Color color = Color.white;
    public int fontSize = 24;

    void Start()
    {
        
    }
    private void Update()
    {
        
    }
    void TestHash()
    {
        string text = "技能";
        Debug.Log(text.GetHashCode());
        text = "道具";
        Debug.Log(text.GetHashCode());
        text = "普通池";
        Debug.Log(text.GetHashCode());
        text = "Boss池";
        Debug.Log(text.GetHashCode());
        text = "位移";
        Debug.Log(text.GetHashCode());
        text = "治疗";
        Debug.Log(text.GetHashCode());
        text = "回复法力";
        Debug.Log(text.GetHashCode());
    }

    void GenerateMesh(int[,] heightMap)
    {
        GameObject obj = new GameObject();
        int width = heightMap.GetLength(1);
        int depth = heightMap.GetLength(0);

        MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));

        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int z = 0; z < depth; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float y = heightMap[z, x];
                AddCube(vertices, triangles, new Vector3(x, 0, z), y);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    void AddCube(List<Vector3> vertices, List<int> triangles, Vector3 position, float height)
    {
        Vector3 basePos = position;
        Vector3 topPos = position + Vector3.up * height;

        // 顶面
        AddQuad(vertices, triangles, topPos + new Vector3(0, 0, 1), topPos + new Vector3(1, 0, 1), topPos + new Vector3(1, 0, 0), topPos + new Vector3(0, 0, 0));
        // 底面
        AddQuad(vertices, triangles, basePos + new Vector3(0, 0, 0), basePos + new Vector3(1, 0, 0), basePos + new Vector3(1, 0, 1), basePos + new Vector3(0, 0, 1));

        // 前侧
        AddQuad(vertices, triangles, basePos + new Vector3(0, 0, 1), basePos + new Vector3(1, 0, 1), topPos + new Vector3(1, 0, 1), topPos + new Vector3(0, 0, 1));
        // 后侧
        AddQuad(vertices, triangles, basePos + new Vector3(1, 0, 0), basePos + new Vector3(0, 0, 0), topPos + new Vector3(0, 0, 0), topPos + new Vector3(1, 0, 0));
        // 左侧
        AddQuad(vertices, triangles, basePos + new Vector3(0, 0, 0), basePos + new Vector3(0, 0, 1), topPos + new Vector3(0, 0, 1), topPos + new Vector3(0, 0, 0));
        // 右侧
        AddQuad(vertices, triangles, basePos + new Vector3(1, 0, 1), basePos + new Vector3(1, 0, 0), topPos + new Vector3(1, 0, 0), topPos + new Vector3(1, 0, 1));
    }

    void AddQuad(List<Vector3> vertices, List<int> triangles, Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        int startIndex = vertices.Count;
        vertices.Add(v0);
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);

        triangles.Add(startIndex);
        triangles.Add(startIndex + 1);
        triangles.Add(startIndex + 2);

        triangles.Add(startIndex);
        triangles.Add(startIndex + 2);
        triangles.Add(startIndex + 3);
    }

    private void TestTexture()
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            foreach (Material mat in materials)
            {
                Texture texture = mat.mainTexture;
                // 创建Render Texture
                RenderTexture renderTexture = new RenderTexture(1024, 1024, 24);
                renderTexture.Create();

                // 使用S1渲染到Render Texture
                Material material = new Material(shaderS1);
                //material.mainTexture = texture;
                Graphics.Blit(null, renderTexture, material);
                mat.mainTexture = renderTexture;
            }

        }
    }
}
