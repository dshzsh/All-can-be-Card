using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGattItemBase_40 : Citem_33
{
    public List<AttAndRevise> atts = new();
}
public class DGattItemBase_40 : DataBase
{

}
public class SGattItemBase_40 : Sitem_33
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.OnItem, OnItemAtt);
        AddHandle(MsgType.ParseDescribe, ParseDescribe);
    }
    public static void ChangeAtt(CGattItemBase_40 card, params AttAndRevise[] atts)
    {
        foreach (AttAndRevise att in card.atts)
            att.UseOnLive(card, -1);

        card.atts = new List<AttAndRevise>(atts);

        foreach (AttAndRevise att in card.atts)
            att.UseOnLive(card, 1);
    }
    public static void AddAtt(CGattItemBase_40 card, params AttAndRevise[] atts)
    {
        foreach(var addAtt in atts)
        {
            addAtt.UseOnLive(card, 1);

            bool ok = false;
            foreach(var att in card.atts)
            {
                if(att.attID == addAtt.attID)
                {
                    ok = true;
                    att.revise.BeOnAtt(addAtt.revise, 1);
                    break;
                }
            }
            if(!ok)
            {
                card.atts.Add(addAtt);
            }
        }
    }
    void OnItemAtt(CardBase _card, MsgBase _msg)
    {
        CGattItemBase_40 card = _card as CGattItemBase_40;
        MsgOnItem msg = _msg as MsgOnItem;

        foreach(AttAndRevise att in card.atts)
        {
            att.UseOnLive(card, msg.op);
        }
    }
    void ParseDescribe(CardBase _card, MsgBase _msg)
    {
        CGattItemBase_40 card = _card as CGattItemBase_40;
        MsgParseDescribe msg = _msg as MsgParseDescribe;

        if (msg.card != card && msg.card != card.parent) return;

        foreach (AttAndRevise att in card.atts)
        {
            msg.InsertFront(att.ToString());
        }
    }
}