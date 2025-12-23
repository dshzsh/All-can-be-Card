using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMxl_55 : Cmagicbase_17
{
    public int lj = 0;
}
public class DMxl_55 : DataBase
{
    public float force, damage, exDownSpeed;
    public float rangeUp, damageUp;
}
public class SMxl_55 : Smagicbase_17
{
    public int bid;
    public override void Init()
    {
        base.Init();
        // 技能效果的切换最好放在这里，这样子更兼容多重施法的效果
        AddHandle(MsgType.MyUseMagicAfter, MyUseMagicAfter);
        bid = GetTypeId(typeof(CBxl_31));
    }
    void MyUseMagicAfter(CardBase _card, MsgBase _msg)
    {
        CMxl_55 card = _card as CMxl_55;
        MsgMagicUse msg = _msg as MsgMagicUse;

        if(card.lj==0)
        {
            Smagic_14.RecoverMagicCd(msg.magic, time: 1, isPercent: true);
        }

        card.lj = 1 - card.lj; // 切换技能效果
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMxl_55 card = _card as CMxl_55;
        DMxl_55 config = basicConfig as DMxl_55;

        if (card.lj == 0)
            UseMagic0(card, msg, config);
        else
            UseMagic1(card, msg, config);
    }
    private void UseMagic0(CMxl_55 card, MsgMagicUse msg, DMxl_55 config)
    {
        if (!TryGetCobj(msg.live, out var cobj, true)) return;

        MyPhysics.GiveImpulseForce(cobj.obj.rbody, cobj.obj.transform.up, config.force * msg.pow);
        GameObject.Instantiate(DMty_16.jumpParPrefab, cobj.obj.transform.position, cobj.obj.transform.rotation);
    }
    private void UseMagic1(CMxl_55 card, MsgMagicUse msg, DMxl_55 config)
    {
        MsgBullet bmsg = new MsgBullet(msg, true);
        bmsg.id = bid;
        bmsg.damage = config.damage * Shealth_4.GetAttf(msg.live, BasicAttID.atk);

        CFxl_22 buff = CreateCard<CFxl_22>();
        buff.bmsg = bmsg;
        buff.exDownSpeed = config.exDownSpeed;
        buff.rangeUp = config.rangeUp;
        buff.damageUp = config.damageUp;
        Sbuff_35.GiveBuff(msg.live, msg.live, new MsgBeBuff(buff, Sbuff_35.BuffSpeTime.Inf));
        
    }
}