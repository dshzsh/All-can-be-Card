using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGql_52 : Citem_33
{

}
public class DGql_52 : DataBase
{
    public float baseOdds, ljBaseOdds;
    public float delay;
}
public class SGql_52 : Sitem_33
{
    public int tid;
    public static bool IsLj(CardBase bullet)
    {
        return bullet.id == SBlj_2.bid;
    }
    public override void Init()
    {
        base.Init();
        tid = GetTypeId(typeof(CTTql_4));
        // 这之后的附加的信息也能吃到一遍create，所以优先级放高
        AddHandle(MsgType.CreateBullet, CreateBullet, HandlerPriority.Before);
    }
    void CreateBullet(CardBase _card, MsgBase _msg)
    {
        CGql_52 card = _card as CGql_52;
        MsgBulletCreate msg = _msg as MsgBulletCreate;
        DGql_52 config = basicConfig as DGql_52;

        if (msg.msg.HaveTag(tid)) return;

        float baseOdds = config.baseOdds;
        if (IsLj(msg.bullet)) baseOdds = config.ljBaseOdds;

        float odds = SGAluck_13.LuckedOdds(card, baseOdds * card.pow);
        if (!MyRandom.RandPer(odds)) return;

        MsgBullet bmsg = new MsgBullet(msg.msg);
        bmsg.AddTag(CreateCard<CTTql_4>());
        Sbullet_10.GiveBullet(msg.bullet.id, bmsg, config.delay);
    }
}