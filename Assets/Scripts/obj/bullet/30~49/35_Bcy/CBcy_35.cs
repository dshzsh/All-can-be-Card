using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CBcy_35 : Cbullet_10
{

}
public class DBcy_35 : DataBase
{
    public string materialName;

    [JsonIgnore]
    public Material material;
    public override void Init(int id)
    {
        material = DataManager.LoadResource<Material>(id, materialName);
    }
}
public class SBcy_35 : Sbullet_10
{
    public static void GiveCyBullet(CObj_2 obj,MsgBullet bmsg)
    {
        CBcy_35 cy = CreateCard<CBcy_35>(); cy.needGiveObj = false;
        DBcy_35 config = DataManager.GetConfig<DBcy_35>(cy.id);
        CreateCardObj(obj.id, cy, obj.obj.transform.position);
        cy.height = obj.height;
        cy.centerOffset = obj.centerOffset;
        AddToWorld(cy);
        // cy.obj.AddGhost(1);
        if (cy.obj.TryGetComponent<Collider>(out var collider))
        {
            collider.isTrigger = true;
        }
        if (cy.obj.TryGetComponent<Rigidbody>(out var rigidbody))
        {
            rigidbody.useGravity = false;
        }
        cy.obj.ToMaterial(config.material);

        Sbullet_10.GiveBullet(cy, bmsg);
    }
    public override void Init()
    {
        base.Init();
    }
    // 原则上子弹自身不要带有任何绑定的效果，要有也得以buff的形式附加
}