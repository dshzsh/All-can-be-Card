using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQsmqy_44 : CGqhsbase_11
{

}
public class DGQsmqy_44 : DataBase
{
    public float costReducePer;
    public float healthPerManaCost;
}
public class SGQsmqy_44 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyMagicBegin, MyMagicBegin, HandlerPriority.JustBefore);// 在判定技能消耗前降低消耗
        AddHandle(MsgType.UseMagicBefore, UseMagicBefore, HandlerPriority.JustBefore);// 在实际进行消耗前进行“生命”消耗
    }
    void MyMagicBegin(CardBase _card, MsgBase _msg)
    {
        CGQsmqy_44 card = _card as CGQsmqy_44;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQsmqy_44 config = basicConfig as DGQsmqy_44;

        float costReduce = msg.mdata.cost * config.costReducePer * card.pow;
        if (costReduce > msg.mdata.cost) costReduce = msg.mdata.cost;
        msg.mdata.cost -= costReduce;
        
        CGQCsmqy_45 tag = CreateCard<CGQCsmqy_45>();tag.costReduce = costReduce;
        msg.AddTag(tag);
    }
    void UseMagicBefore(CardBase _card, MsgBase _msg)
    {
        CGQsmqy_44 card = _card as CGQsmqy_44;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQsmqy_44 config = basicConfig as DGQsmqy_44;

        if (!msg.TryGetTag<CGQCsmqy_45>(out var tag))
            return;

        Shealth_4.GiveHeal(msg.live, msg.live, new MsgBeHeal(-tag.costReduce * config.healthPerManaCost));
    }
}