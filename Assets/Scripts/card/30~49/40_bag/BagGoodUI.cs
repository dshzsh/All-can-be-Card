using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagGoodUI : MonoBehaviour
{
    public GoodUI goodUI;
    public void SetCard(CardBase good, BagUI mybag = null)
    {
        goodUI.SetCard(good);
    }
}
