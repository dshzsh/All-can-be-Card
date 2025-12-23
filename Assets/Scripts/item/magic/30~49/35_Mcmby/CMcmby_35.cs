using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMcmby_35 : Cmagicbase_17
{

}
public class DMcmby_35 : DataBase
{
    public DYswAtt yswAtt;
}
public class SMcmby_35 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMcmby_35 card = _card as CMcmby_35;
        DMcmby_35 config = basicConfig as DMcmby_35;

        Sysw_26.MsgSummonYsw smsg = new Sysw_26.MsgSummonYsw(msg, config.yswAtt);
        smsg.id = GetTypeId(typeof(CEcmby_6));
        Sysw_26.SummonYsw(smsg);
    }
}