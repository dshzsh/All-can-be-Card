using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class Chk3_24 : Cmagicbase_17
{
    
}

public class Shk3_24: Smagicbase_17
{
    public override void Init()
    {
        base.Init();
    }
    public override void MyUseMagic(CardBase _card, MsgMagicUse msg)
    {
        Chk3_24 card = _card as Chk3_24;

    }
}