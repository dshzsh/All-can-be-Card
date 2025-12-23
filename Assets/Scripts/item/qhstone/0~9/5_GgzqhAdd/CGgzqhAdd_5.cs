using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static CardManager;
using static SystemManager;

public class CGgzqhAdd_5 : CGqhsbase_11
{
    public float angleRate;

    public Clive_19 mb;
    public float time = MyTool.BigFloat;
}
public class DGgzqhAdd_5 : DataBase
{
    public float findEnemyInterval;
    public float baseGzAngle;

    public static float _baseGzAngle;
    public override void Init(int id)
    {
        _baseGzAngle = baseGzAngle;
    }
}
public class SGgzqhAdd_5 : SGqhsbase_11
{
    public static void AddGz(MsgBullet bmsg, float pow = 1)
    {
        CGgzqhAdd_5 addmk = CreateCard<CGgzqhAdd_5>();
        addmk.angleRate = DGgzqhAdd_5._baseGzAngle * pow;
        bmsg.AddCard(addmk);
    }

    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.Update, Update);
    }
    void Update(CardBase _card, MsgBase _msg)
    {
        CGgzqhAdd_5 card = _card as CGgzqhAdd_5;
        MsgUpdate msg = _msg as MsgUpdate;
        DGgzqhAdd_5 config = basicConfig as DGgzqhAdd_5;

        if (!TryGetCobj(card, out var cobj, true)) return;

        //索敌
        if (MyTool.IntervalTime(config.findEnemyInterval, ref card.time, msg.time))
        {
            card.mb = Slive_19.FindLive(cobj);
        }

        if (!CardValid(card.mb)) { card.mb = null; return; }

        Vector3 dir = cobj.obj.rbody.velocity;
        dir = Vector3.RotateTowards(dir, card.mb.obj.Center - cobj.obj.Center, card.angleRate * Mathf.Deg2Rad * msg.time, 0);
        cobj.obj.rbody.velocity = dir;
    }
}