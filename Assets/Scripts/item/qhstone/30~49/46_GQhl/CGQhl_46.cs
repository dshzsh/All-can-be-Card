using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQhl_46 : CGqhsbase_11
{

}
public class DGQhl_46 : DataBase, ICloneAble
{
    public int cnt;
    public float bulletPow;

    public float dirShift;
    public float posShift;
    public DGQhl_46() { }
    public DGQhl_46(DGQhl_46 other)
    {
        cnt = other.cnt;
        bulletPow = other.bulletPow;
        dirShift = other.dirShift;
        posShift = other.posShift;
    }
    object ICloneAble.Clone()
    {
        return new DGQhl_46(this);
    }
}
public class SGQhl_46 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, HandlerPriority.Before);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQhl_46 card = _card as CGQhl_46;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQhl_46 config = basicConfig as DGQhl_46;

        CGQChl_47 buff = CreateCard<CGQChl_47>();
        buff.config = new DGQhl_46(config);
        buff.config.bulletPow *= card.pow;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, buff);
    }
}