using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMcz_45 : Cmagicbase_17
{

}
public class DMcz_45 : DataBase
{
    public float distance;
    public float moveTime;
    public float damage;
}
public class SMcz_45 : Smagicbase_17
{
    public static void GiveCollisonBullet(CardBase card, MsgBullet bmsg, float moveTime)
    {
        if(!TryGetCobj(card, out var cobj)) return;
        float height = cobj.obj.transform.localScale.y * cobj.height;
        bmsg.time = moveTime;
        bmsg.exScale = cobj.obj.transform.localScale;bmsg.exScale.y = height;
        bmsg.exScale *= 1.05f;// 稍微大一点方便碰撞
        bmsg.initPos = cobj.obj.transform.position + new Vector3(0, height / 2, 0);// 要求是此物体的中心，不是默认的center
        SGQCfollow_59.AddFollow(card, bmsg);
    }
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMcz_45 card = _card as CMcz_45;
        DMcz_45 config = basicConfig as DMcz_45;

        Vector3 dir = SMcc_10.GetSprintDir(card, msg.pos);

        SMcc_10.GiveSprint(card, config.distance * msg.pow, config.moveTime, dir, true);

        MsgBullet bmsg = new MsgBullet(msg, true);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        GiveCollisonBullet(card, bmsg, config.moveTime);
        Sbullet_10.GiveBullet(SBzft_26.bid, bmsg);
    }
}