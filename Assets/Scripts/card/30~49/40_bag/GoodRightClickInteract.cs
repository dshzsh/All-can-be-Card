using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MsgInteractItem : MsgBase
{
    public CardBase item;
    public CardBase live;
    public bool used = true;
    public bool close = false;
    public MsgInteractItem(CardBase item, CardBase live)
    {
        this.item = item;
        this.live = live;
    }
}
public class GoodRightClickInteract : MonoBehaviour, IPointerClickHandler
{
    GoodUI goodUI;
    private void Awake()
    {
        goodUI = GetComponent<GoodUI>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;
        if (goodUI == null || goodUI.good == null) return;
        MsgInteractItem msg = new MsgInteractItem(goodUI.good, SystemManager.GetMainPlayer());
        SystemManager.SendMsg(goodUI.good, MsgType.MyInteractItem, msg);
        SystemManager.SendMsg(msg.live, MsgType.InteractItem, msg);
    }
}
