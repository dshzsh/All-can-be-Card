using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMxljr_36 : Cmagicbase_17
{

}
public class DMxljr_36 : DataBase
{
    
}
public class SMxljr_36 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMxljr_36 card = _card as CMxljr_36;
        DMxljr_36 config = basicConfig as DMxljr_36;

        Sysw_26.MsgSummonYsw smsg = new Sysw_26.MsgSummonYsw(msg);
        smsg.id = GetTypeId(typeof(CExljr_4));
        smsg.team = Slive_19.Team.enemy;
        Sysw_26.SummonYsw(smsg);
    }
}