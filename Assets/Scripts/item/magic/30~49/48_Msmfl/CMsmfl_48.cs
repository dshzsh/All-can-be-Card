using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMsmfl_48 : Cmagicbase_17
{

}
public class DMsmfl_48 : DataBase
{
    public float CostHealthPerMana;
    public float recoverManaRate;
}
public class SMsmfl_48 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMsmfl_48 card = _card as CMsmfl_48;
        DMsmfl_48 config = basicConfig as DMsmfl_48;

        float recover = config.recoverManaRate * msg.pow * Shealth_4.GetAttf(card, BasicAttID.manaMax);
        SendMsg(msg.live, MsgType.RestoreMana, new MsgRestoreMana(recover));
        SendMsg(msg.live, MsgType.CostHealth, new MsgCostHealth(recover * config.CostHealthPerMana));
    }
}