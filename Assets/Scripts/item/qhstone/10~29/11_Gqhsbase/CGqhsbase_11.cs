using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGqhsbase_11 : Citem_33
{

}
public class DGqhsbase_11 : DataBase
{
    public BasicAtt cdRevise = new();
    public BasicAtt costRevise = new();
    public BasicAtt powRevise = new();
    public BasicAtt usePowRevise = new();
}
public class SGqhsbase_11 : Sitem_33
{
    public int priorityQhstoneMagicUse = HandlerPriority.Before;

    public DGqhsbase_11 qhstoneOnRevise;
    public override Color GetColor(CardBase _card)
    {
        return GoodUIColor.QhStone;
    }
    public override void Init()
    {
        base.Init();
        if (DataManager.GetConfig<DGqhsbase_11>(id) != null)
        {
            qhstoneOnRevise = DataManager.GetConfig<DGqhsbase_11>(id);
            AddHandle(MsgType.OnItem, OnRevise, HandlerPriority.High);
            if(!qhstoneOnRevise.usePowRevise.IsNoRevise())
            {
                AddHandle(MsgType.MyUseMagic, UsePowAdd, HandlerPriority.Before);// before处进行强化石的序列判定
            }
        }
    }
    public override string ParseDescribe(CardBase _card, string text)
    {
        if (qhstoneOnRevise == null) return base.ParseDescribe(_card, text);
        string ans = base.ParseDescribe(_card, text);
        string reviseDes = "";
        if (!qhstoneOnRevise.cdRevise.IsNoRevise())
            reviseDes += DataManager.GetBasicAttText(qhstoneOnRevise.cdRevise, BasicAttID.cd) + "\n";
        if (!qhstoneOnRevise.costRevise.IsNoRevise())
            reviseDes += DataManager.GetBasicAttText(qhstoneOnRevise.costRevise, BasicAttID.cost) + "\n";
        if (!qhstoneOnRevise.powRevise.IsNoRevise())
            reviseDes += DataManager.GetBasicAttText(qhstoneOnRevise.powRevise, BasicAttID.pow) + "\n";
        if (!qhstoneOnRevise.usePowRevise.IsNoRevise())
            reviseDes += DataManager.GetBasicAttText(qhstoneOnRevise.usePowRevise, BasicAttID.usePow) + "\n";
        ans = InsertFront(ans, reviseDes);
        return ans;
    }
    void OnRevise(CardBase _card, MsgBase _msg)
    {
        CGqhsbase_11 card = _card as CGqhsbase_11;
        MsgOnItem msg = _msg as MsgOnItem;

        CardBase cmagic = Sqhc_38.GetQhMagic(card);
        if (cmagic != null && cmagic is Cmagicbase_17 magic)
        {
            magic.mdata.cd = qhstoneOnRevise.cdRevise.UseAttTo(magic.mdata.cd, msg.op);
            magic.mdata.cost = qhstoneOnRevise.costRevise.UseAttTo(magic.mdata.cost, msg.op);
            magic.pow = qhstoneOnRevise.powRevise.UseAttTo(magic.pow, msg.op);
        }
    }
    void UsePowAdd(CardBase _card, MsgBase _msg)
    {
        CGqhsbase_11 card = _card as CGqhsbase_11;
        MsgMagicUse msg = _msg as MsgMagicUse;

        msg.pow = qhstoneOnRevise.usePowRevise.UseAttTo(msg.pow, 1);
    }
}