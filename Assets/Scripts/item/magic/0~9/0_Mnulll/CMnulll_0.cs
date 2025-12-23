using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class CMnulll_0 : Cmagicbase_17
{

}
public class DMnulll_0 : DataBase
{
    
}
public class SMnulll_0 : Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        CMnulll_0 card = _card as CMnulll_0;
        DMnulll_0 config = DataManager.GetConfig<DMnulll_0>(id);

        
    }
}