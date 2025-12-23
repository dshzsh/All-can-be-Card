using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MInteractBoxUI : MonoBehaviour,IEscClose
{
    public MLiveAllItemUI mLiveAllItemUI;
    public BoxUI forgeUI;
    public TextMeshProUGUI title;
    public Button button;
    public TextMeshProUGUI buttonText;

    CardBase live, container;
    string use_string, used_string = "";
    private void Set(CardBase live, CardBase container, string title)
    {
        this.live = live;
        this.container = container;

        mLiveAllItemUI.Set(live);
        this.title.text = title;
        forgeUI.SetBox(container);
    }
    public void SetWithUse(CardBase live, CardBase container, string title, bool used, string used_string, string use_string="确认")
    {
        Set(live, container, title);

        this.used_string = used_string;
        this.use_string = use_string;
        button.interactable = !used;
        if (used)
            buttonText.text = used_string;
        else buttonText.text = use_string;
    }
    public void Button()
    {
        MsgInteractItem imsg = new MsgInteractItem(container, live);
        SystemManager.SendMsg(container, MsgType.MyInteractItem, imsg);
        SetWithUse(live, container, this.title.text, imsg.used, used_string, use_string);
        if(imsg.close)
        {
            Close();
        }
    }
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
        Destroy(gameObject);
    }
}
