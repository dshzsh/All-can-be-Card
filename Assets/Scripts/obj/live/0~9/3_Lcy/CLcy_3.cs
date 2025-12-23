using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static CardManager;
using static SystemManager;

public class CLcy_3 : Clive_19
{

}
public class DLcy_3 :DataBase
{
    public string materialName;

    [JsonIgnore]
    public Material material;
    public override void Init(int id)
    {
        material = DataManager.LoadResource<Material>(id, materialName);
    }
}
public class SLcy_3 : Slive_19
{
    public static CLcy_3 GiveCy(CObj_2 obj, float time = -1, float fadeTime = 0.5f)
    {
        CLcy_3 cy = CreateCard<CLcy_3>();cy.needGiveObj = false;
        DLcy_3 config = DataManager.GetConfig<DLcy_3>(cy.id);
        CreateCardObj(obj.id, cy, obj.obj.transform.position);
        cy.height = obj.height;
        cy.centerOffset = obj.centerOffset;
        AddToWorld(cy);

        cy.obj.AddGhost(1);

        cy.obj.transform.rotation = obj.obj.transform.rotation;
        cy.obj.ToMaterial(config.material);
        //复制属性
        if(obj.myHealth != null && cy.myHealth!=null)
        {
            TriClone(obj.myHealth, cy.myHealth);
        }
        if(time>0)
        {
            GameObject.Destroy(cy.obj.gameObject, time);
            cy.obj.gameObject.AddComponent<FadeMaterial>().Set(time, fadeTime);
        }

        return cy;
    }

    public override void Init()
    {
        base.Init();
        
    }
}