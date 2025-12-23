using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreSeeGoodUI : MonoBehaviour
{
    public GoodUI goodUI;
    public void SetCard(CardBase good)
    {
        goodUI.SetCard(good);
    }
    public void Click()
    {
        GoodPreSee.SetPreSee(goodUI.good);
    }
}
