using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQyl_69 : CGqhsbase_11
{

}
public class DGQyl_69 : DataBase
{
    public float damageRate;
    public float delay;
}
public class SGQyl_69 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQyl_69 card = _card as CGQyl_69;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQyl_69 config = basicConfig as DGQyl_69;

        CGQCldfj_68 add = CreateCard<CGQCldfj_68>();
        add.damageRate = config.damageRate * card.pow;
        add.onlyFirst = false;
        add.delay = config.delay;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, add);
    }
}