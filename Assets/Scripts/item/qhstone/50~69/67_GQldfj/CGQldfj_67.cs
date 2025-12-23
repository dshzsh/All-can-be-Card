using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CGQldfj_67 : CGqhsbase_11
{

}
public class DGQldfj_67 : DataBase
{
    public float damageRate;
}
public class SGQldfj_67 : SGqhsbase_11
{
    public override void Init()
    {
        base.Init();
        AddHandle(MsgType.MyUseMagic, MyUseMagic, priorityQhstoneMagicUse);
    }
    void MyUseMagic(CardBase _card, MsgBase _msg)
    {
        CGQldfj_67 card = _card as CGQldfj_67;
        MsgMagicUse msg = _msg as MsgMagicUse;
        DGQldfj_67 config = basicConfig as DGQldfj_67;

        CGQCldfj_68 add = CreateCard<CGQCldfj_68>();
        add.damageRate = config.damageRate * card.pow;
        add.onlyFirst = true;
        msg.AddMk(MsgMagicUse.AddCardType.bullet, add);
    }
}