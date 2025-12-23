using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMylcc_11 : Cmagicbase_17
{

}
public class DMylcc_11 : DataBase
{
    public float distance;
    public float moveTime;
}
public class SMylcc_11 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMylcc_11 card = _card as CMylcc_11;
        DMylcc_11 config = basicConfig as DMylcc_11;

        if (!TryGetCobj(card, out var cobj)) return;

        Vector3 dir = SMcc_10.GetSprintDir(card, msg.pos);

        CFmustMove_4 buff = CreateCard<CFmustMove_4>();
        buff.mustMoveVelocity = config.distance * msg.pow / config.moveTime * MyMath.V3zeroYNor(dir);
        Sbuff_35.GiveBuff(cobj, cobj, new MsgBeBuff(buff, config.moveTime));
        CFghost_3 buff2 = CreateCard<CFghost_3>();
        Sbuff_35.GiveBuff(cobj, cobj, new MsgBeBuff(buff2, config.moveTime));

        Sbuff_35.GiveBuff(cobj, cobj, new MsgBeBuff(CreateCard<CFwfsj_16>(), config.moveTime));
        Sbuff_35.GiveBuff(cobj, cobj, new MsgBeBuff(CreateCard<CFwfzd_17>(), config.moveTime));
    }
}