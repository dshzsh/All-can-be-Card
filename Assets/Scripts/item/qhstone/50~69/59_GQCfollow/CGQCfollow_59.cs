using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQCfollow_59 : CGqhsbase_11
{
    public Transform follow;

    public Vector3 oldPos;
}
public class DGQCfollow_59 : DataBase
{

}
public class SGQCfollow_59 : SGqhsbase_11
{
    public static void AddFollow(CardBase card, MsgBullet bmsg)
    {
        if (!TryGetCobj(card, out var cobj)) return;
        CGQCfollow_59 buff = CreateCard<CGQCfollow_59>(); buff.follow = cobj.obj.transform;
        buff.oldPos = cobj.obj.transform.position;
        bmsg.AddCard(buff);
    }
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.FixedUpdate, FixedUpdate);
    }
    void FixedUpdate(CardBase _card, MsgBase _msg)
    {
        CGQCfollow_59 card = _card as CGQCfollow_59;
        MsgUpdate msg = _msg as MsgUpdate;

        if (card.follow == null) return;
        if (!TryGetCobj(_card, out var cobj)) return;

        cobj.obj.transform.position += card.follow.transform.position - card.oldPos;
        card.oldPos = card.follow.transform.position;
    }
}