using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodSelectUI : MonoBehaviour
{
    private static List<GoodSelectUI> list = new List<GoodSelectUI>();
    public bool resizeByContent = false;
    public TMPro.TextMeshProUGUI goodName;
    public GoodUI goodUI;
    public GoodSelectFit goodSelectFit;
    public UnityEngine.UI.Image rareImage;

    private CardBase good;

    public static void RefreshAll()
    {
        List<GoodSelectUI> tempList = new List<GoodSelectUI>(list);
        foreach (GoodSelectUI goodSelect in tempList)
        {
            goodSelect.SetCard(goodSelect.good);
        }
    }
    private void OnEnable()
    {
        list.Add(this);
    }
    private void OnDisable()
    {
        list.Remove(this);
    }
    public void SetCard(CardBase good)
    {
        this.good = good;
        gameObject.SetActive(false);
        if (CardManager.CardValid(good) == false)
            good = new CNull_0();
        Color ncolor = Sitem_33.RareColor.GetRareColor(good);ncolor.a = 0.25f;
        rareImage.color = ncolor;
        goodName.text = DataManager.GetName(good.id);
        goodUI.SetCard(good);
        goodSelectFit.Set(DataManager.GetDescirbe(good), resizeByContent);
        gameObject.SetActive(true);
    }
}
