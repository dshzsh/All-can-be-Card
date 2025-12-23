using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CFfyjs_23 : Cbuffbase_36
{
    public AttAndRevise att;

    public CFCmat_24 cmat;
}
public class DFfyjs_23 : DataBase
{
    public string matName;

    public Material mat;
    public override void Init(int id)
    {
        base.Init(id);
        mat = DataManager.LoadResource<Material>(id, matName);
    }
}
public class SFfyjs_23 : Sbuffbase_36
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CFfyjs_23 card = _card as CFfyjs_23;
        DFfyjs_23 config = basicConfig as DFfyjs_23;
        card.cmat = CreateCard<CFCmat_24>();
        card.cmat.mat = config.mat;
        ActiveComponent(card, card.cmat);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItem);
        AddHandle(MsgType.MagicBegin, MagicBegin, HandlerPriority.After);
    }
    void OnItem(CardBase _card, MsgBase _msg)
    {
        CFfyjs_23 card = _card as CFfyjs_23;
        MsgOnItem msg = _msg as MsgOnItem;

        card.att.UseOnLive(card, msg.op);
    }
    void MagicBegin(CardBase _card, MsgBase _msg)
    {
        CFfyjs_23 card = _card as CFfyjs_23;
        MsgMagicUse msg = _msg as MsgMagicUse;

        DestroyCard(card);
    }
}