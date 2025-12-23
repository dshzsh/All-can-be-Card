using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CFCmat_24 : Cbuffbase_36
{
    public Material mat;
    public List<GameObject> onTxs;
}
public class DFCmat_24 : DataBase
{

}
public class SFCmat_24 : Sbuffbase_36
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFCmat_24 card = _card as CFCmat_24;
        MsgOnItem msg = _msg as MsgOnItem;

        if (!TryGetCobj(card, out var cobj)) return;


        if (msg.op == 1)
        {
            card.onTxs = cobj.obj.AddMaterial(card.mat);
        }
        else
        {
            foreach (GameObject obj in card.onTxs)
            {
                GameObject.Destroy(obj);
            }
        }
    }
}