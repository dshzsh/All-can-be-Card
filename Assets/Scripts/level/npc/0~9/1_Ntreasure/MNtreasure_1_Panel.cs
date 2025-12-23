using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MNtreasure_1_Panel : MonoBehaviour, IEscClose
{
    public static GameObject treasurePanelItemPrefab;
    public TextMeshProUGUI title;
    public RectTransform cardList;

    public int select = -1;

    CardBase treasure;
    CardBase live;

    bool getted = false;

    private void OnEnable()
    {
        SControl_3.AddNoControl(1);
        SControl_3.AddEscClose(this);
    }
    private void OnDisable()
    {
        SControl_3.AddNoControl(-1);
        SControl_3.RemoveEscClose(this);
    }
    public void Close()
    {
        if (!getted)
            SystemManager.SendMsg(treasure, SNtreasure_1.mTGetTreasure, new SNtreasure_1.MsgGetTreasure(live, -1));
        Destroy(gameObject);
    }
    public const float interval = 30;
    public const float PreseeSizeX = 400;
    public const float ContentSizeX = 1600;
    private void GiveTreasureItem(CardBase card, float pos, int selectID)
    {
        GameObject obj = GameObject.Instantiate(treasurePanelItemPrefab);
        obj.transform.SetParent(cardList.transform);
        obj.GetComponent<MNtreasure_1_PanelItem>().Set(card, this, selectID);
        obj.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(pos, 0);
    }
    public void Set(CardBase live, CardBase treasure, string title, List<CardBase> cards)
    {
        this.live = live;
        this.treasure = treasure;
        this.title.text = title;

        //从左边开始垒起，如果总大小大于则拓展，总大小小于则居中
        int maxItemCnt = (int)(Mathf.Floor(ContentSizeX / (PreseeSizeX + interval)) + MyMath.SmallFloat);
        float needSizeX = (PreseeSizeX + interval) * cards.Count - interval;
        if (cards.Count<=maxItemCnt)
        {
            //小于，居中策略
            float offsetX = (ContentSizeX - needSizeX) / 2f;
            for (int i = 0; i < cards.Count; i++)
                GiveTreasureItem(cards[i], offsetX + i * (interval + PreseeSizeX), i);
        }
        else
        {
            //大于，平铺策略
            for (int i = 0; i < cards.Count; i++)
                GiveTreasureItem(cards[i], i * (interval + PreseeSizeX), i);
            cardList.sizeDelta = new Vector2(needSizeX, cardList.sizeDelta.y);
        }
    }
    public void GetItem()
    {
        if (select == -1) return;
        if(treasure==null)
        {
            Close();
            return;
        }
        SystemManager.SendMsg(treasure, SNtreasure_1.mTGetTreasure, new SNtreasure_1.MsgGetTreasure(live, select));
        getted = true;
        Close();
    }
}
