using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQhlqx_48 : CGqhsbase_11
{

}
public class DGQhlqx_48 : DataBase, ICloneAble
{
    public int cnt;
    public float bulletPow;

    public float posShift;
    public DGQhlqx_48() { }
    public DGQhlqx_48(DGQhlqx_48 other)
    {
        this.cnt = other.cnt;
        this.bulletPow = other.bulletPow;
        this.posShift = other.posShift;
    }
    object ICloneAble.Clone()
    {
        return new DGQhlqx_48(this);
    }
}
public class SGQhlqx_48 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQhlqx_48 card = _card as CGQhlqx_48;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQhlqx_48 config = basicConfig as DGQhlqx_48;

        CGQChlqx_49 buff = CreateCard<CGQChlqx_49>();
        buff.usePos = msg.pos;
        buff.config = new DGQhlqx_48(config);
        buff.config.bulletPow *= card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}