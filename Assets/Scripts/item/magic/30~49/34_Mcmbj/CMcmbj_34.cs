using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CardManager;
using static DataManager;
using static SystemManager;

public class CMcmbj_34 : Cmagicbase_17
{

}
public class DYswAtt: IParseStringWithPow
{
    public float aPow, dPow, hPow;
    public float time;
    public DYswAtt WithPow(float pow)
    {
        return new DYswAtt()
        {
            aPow = this.aPow * pow,
            dPow = this.dPow * pow,
            hPow = this.hPow * pow,
            time = this.time
        };
    }

    string IParseStringWithPow.ToStringWithPow(CardBase card, float pow)
    {
        string ans = $"召唤物属性：\n{GetHealthText(hPow * pow, BasicAttID.healthMax, card)}\n" +
            $"{GetHealthText(aPow * pow, BasicAttID.atk, card)}\n" +
            $"{GetHealthText(dPow * pow, BasicAttID.def, card)}\n";
        if(time > 0)
        {
            ans += $"持续{GetFloatText(time)}s";
        }
        return ans;
    }
}
public class DMcmbj_34 : DataBase
{
    public DYswAtt yswAtt;
}
public class SMcmbj_34 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMcmbj_34 card = _card as CMcmbj_34;
        DMcmbj_34 config = basicConfig as DMcmbj_34;

        Sysw_26.MsgSummonYsw smsg = new Sysw_26.MsgSummonYsw(msg, config.yswAtt);
        smsg.id = SEcmbj_5.eid;
        Sysw_26.SummonYsw(smsg);
    }
}