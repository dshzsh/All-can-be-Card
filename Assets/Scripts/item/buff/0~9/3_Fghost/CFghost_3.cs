using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;
using Newtonsoft.Json;

public class CFghost_3 : Cbuffbase_36
{
    public List<GameObject> onPars;
    public List<GameObject> onTxs;
}
public class DFghost_3 : DataBase
{
    public string materialName;

    [JsonIgnore]
    public Material material;
    public override void Init(int id)
    {
        material = DataManager.LoadResource<Material>(id, materialName);
    }
}
public class SFghost_3 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFghost_3 card = _card as CFghost_3;
        MsgOnItem msg = _msg as MsgOnItem;
        DFghost_3 config = basicConfig as DFghost_3;

        if (!TryGetCobj(card, out var cobj)) return;

        
        if(msg.op==1)
        {
            card.onTxs = cobj.obj.AddMaterial(config.material);
        }
        else
        {            
            foreach(GameObject obj in card.onTxs)
            {
                GameObject.Destroy(obj);
            }
        }
    }
}