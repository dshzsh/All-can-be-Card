using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MNtreasure_1_PanelItem : MonoBehaviour
{
    public GoodSelectUI goodSelectUI;
    public GameObject selectHighLight;
    int selectID;
    MNtreasure_1_Panel panel;
    bool onSelect = false;

    public void Set(CardBase card, MNtreasure_1_Panel panel, int selectID)
    {
        goodSelectUI.SetCard(card);
        this.panel = panel;
        this.selectID = selectID;
    }
    public void Select()
    {
        panel.select = selectID;
    }
    private void Update()
    {
        if (panel.select == selectID && onSelect == false)
        {
            onSelect = true;
            //高亮显示
            selectHighLight.SetActive(true);
        }
        else if (panel.select != selectID && onSelect == true)
        {
            onSelect = false;
            //取消高亮显示
            selectHighLight.SetActive(false);
        }
    }
}
