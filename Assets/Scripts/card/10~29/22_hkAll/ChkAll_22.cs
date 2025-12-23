using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SystemManager;
using static CardManager;

public class ChkAll_22 : Chk_18
{

}
public class ShkAll_22 : Shk_18
{
    public override void Create(CardBase _card)
    {
        base.Create(_card);
        Sdld_21.AddDld(_card, 20, 24);
    }
}