using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CFcyxx_5 : Cbuffbase_36
{
    //public Color color;
    public float time;
}
public class DFcyxx_5 : DataBase
{
    public string matName;
    public float interval;
    public float cyTime;

    [JsonIgnore]
    public static Material mat;
    public override void Init(int id)
    {
        mat = DataManager.LoadResource<Material>(id, matName);
    }
}
public class SFcyxx_5 : Sbuffbase_36
{
    public static GameObject GiveCy(GameObject obj, float time)
    {
        GameObject main = new GameObject("cy");
        Renderer[] meshRenderers = obj.gameObject.GetComponentsInChildren<Renderer>();

        foreach (Renderer meshRenderer in meshRenderers)
        {
            GameObject mobj = new GameObject();
            mobj.transform.SetPositionAndRotation(meshRenderer.gameObject.transform.position, meshRenderer.gameObject.transform.rotation);
            mobj.transform.localScale = meshRenderer.gameObject.transform.lossyScale;

            MeshRenderer mr = mobj.AddComponent<MeshRenderer>();
            MeshFilter mf = mobj.AddComponent<MeshFilter>();
            Mesh mesh = new Mesh();
            if(meshRenderer is SkinnedMeshRenderer skinnedMeshRenderer)
            {
                skinnedMeshRenderer.BakeMesh(mesh);
            }
            else
            {
                if (meshRenderer.TryGetComponent<MeshFilter>(out var meshFilter))
                    mesh = meshFilter.mesh;
            }
            mf.mesh= mesh;
            Material[] materials = new Material[mesh.subMeshCount];
            Material nm = new Material(DFcyxx_5.mat);
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = nm;
            }
            mr.materials = materials;
            mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mobj.transform.SetParent(main.transform, false);
            
            mobj.AddComponent<FadeMaterial>().Set(mr, time, time);
        }
        
        GameObject.Destroy(main, time);
        return main;
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CFcyxx_5 card = _card as CFcyxx_5;
        MsgUpdate msg = _msg as MsgUpdate;
        DFcyxx_5 config = basicConfig as DFcyxx_5;

        if (!MyTool.IntervalTime(config.interval, ref card.time, msg.time)) return;
        if (!TryGetCobj(card, out CObj_2 cobj)) return;

        GiveCy(cobj.obj.gameObject, config.cyTime);
    }
    
}