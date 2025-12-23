using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static SystemManager;
using static Unity.Burst.Intrinsics.X86;

public class CMxlxz_44 : Cmagicbase_17
{

}
public class DMxlxz_44 : DataBase
{
    public float damage;

    public float rotSpeed;
    public float baseCnt, addCntPerSec;
}
public class SMxlxz_44 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        bid = GetTypeId(typeof(CBcz_25));
    }
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        CGxlmk_12 charge = CreateCard<CGxlmk_12>();
        DMcharge config = DataManager.GetConfig<DMcharge>(id);

        charge.charge.max = config.chargeTime;
        AddComponent(_card, charge);
    }

    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMxlxz_44 card = _card as CMxlxz_44;
        DMxlxz_44 config = basicConfig as DMxlxz_44;

        float chargeTime = 0f;
        if (msg.TryGetTag<CchargeTime_44>(out var cc))
            chargeTime = cc.time;

        MsgBullet bmsg = new MsgBullet(msg, dirNoY:true);
        bmsg.damage = config.damage * Shealth_4.GetAttf(card, BasicAttID.atk);
        bmsg.isMelee = true;

        // 初始从固定一侧开始旋转
        bmsg.AddRotate(Quaternion.Euler(0, -90, 0));

        CGQCxj_43 xj = CreateCard<CGQCxj_43>();
        xj.rotAngle = 360;
        xj.rotSpeed = config.rotSpeed;
        bmsg.addCards.Add(xj);

        bmsg.time = (1 / config.rotSpeed) * (config.baseCnt + chargeTime * config.addCntPerSec);

        SGQCfollow_59.AddFollow(card, bmsg);

        Sbullet_10.GiveBullet(bid, bmsg);
    }
}