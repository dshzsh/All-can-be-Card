using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCsetParent_50 : CGqhsbase_11
{
    public Transform transform;
}
public class DGQCsetParent_50 : DataBase
{

}
public class SGQCsetParent_50 : SGqhsbase_11
{
    public static void AddParent(CardBase card, MsgBullet bmsg)
    {
        if (!TryGetCobj(card, out var cobj)) return;
        CGQCsetParent_50 buff = CreateCard<CGQCsetParent_50>(); buff.transform = cobj.obj.transform;
        bmsg.AddCard(buff);
    }

    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.BulletStart, BulletStart, HandlerPriority.Lowest);
    }
    void BulletStart(CardBase _card, MsgBase _msg)
    {
        CGQCsetParent_50 card = _card as CGQCsetParent_50;
        MsgOnItem msg = _msg as MsgOnItem;
        DGQCsetParent_50 config = basicConfig as DGQCsetParent_50;

        if (!TryGetCobj(card, out var obj))
            return;

        obj.obj.transform.SetParent(card.transform);
    }
}