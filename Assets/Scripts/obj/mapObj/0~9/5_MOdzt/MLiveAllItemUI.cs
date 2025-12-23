using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MLiveAllItemUI : MonoBehaviour
{
    public BoxUI bag, magic;
    public void Set(CardBase live)
    {
        if (!CardManager.TryGetCobj(live, out var cobj)) return;

        if (cobj.myBag != null)
            bag.SetBox(cobj.myBag);
        else bag.gameObject.SetActive(false);

        if (cobj.myMagic != null)
            magic.SetBox(cobj.myMagic);
        else magic.gameObject.SetActive(false);
    }
}
