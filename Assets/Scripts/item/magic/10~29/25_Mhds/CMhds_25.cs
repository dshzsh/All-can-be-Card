using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMhds_25 : Cmagicbase_17
{

}
public class DMhds_25 : DataBase
{
    public float armor;
    public string onArmor;

    public GameObject onArmorPrefab;

    public override void Init(int id)
    {
        onArmorPrefab = DataManager.LoadResource<GameObject>(id, onArmor);
    }
}
public class SMhds_25 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMhds_25 card = _card as CMhds_25;
        DMhds_25 config = basicConfig as DMhds_25;

        SFhd_9.GiveArmor(card, msg.live, config.armor * Shealth_4.GetAttf(card, BasicAttID.def) * msg.pow);

        GiveInstantTx(card, config.onArmorPrefab);
    }
}