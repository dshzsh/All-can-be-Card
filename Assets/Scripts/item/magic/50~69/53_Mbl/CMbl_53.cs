using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMbl_53 : Cmagicbase_17
{

}
public class DMbl_53 : DataBase
{
    public float distance, moveTime;
    public float damage, cdRecover;
}
public class SMbl_53 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.CreateBullet, CreateBullet, HandlerPriority.After);
    }
    void CreateBullet(CardBase _card, MsgBase _msg)
    {
        CMbl_53 card = _card as CMbl_53;
        MsgBulletCreate msg = _msg as MsgBulletCreate;
        DMbl_53 config = basicConfig as DMbl_53;

        if (!SGql_52.IsLj(msg.bullet)) return;

        Smagic_14.RecoverMagicCd(card, config.cdRecover * card.pow, false);
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMbl_53 card = _card as CMbl_53;
        DMbl_53 config = basicConfig as DMbl_53;

        // 冲刺
        Vector3 dir = SMcc_10.GetSprintDir(card, msg.pos);
        SMcc_10.GiveSprint(card, config.distance * msg.pow, config.moveTime, dir);

        // 寻找落雷目标
        if (!TryGetCobj(card, out var cobj)) return;
        Vector3 pos = cobj.obj.Center;
        Clive_19 mb = Slive_19.FindLive(cobj, findLiveMaxDis: msg.mdata.useDis);
        if (mb != null) pos = mb.obj.Center;

        MsgBullet bmsg = new MsgBullet(msg);
        bmsg.initPos = pos;
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        Sbullet_10.GiveBullet(SBlj_2.bid, bmsg);
    }
}