using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MLTthingbase_1_panel : MonoBehaviour
{
    public TextMeshProUGUI title, content;
    public GameObject choose;
    public CLTthingbase_1 callBackCard;
    public CardBase live;

    public void Set(CLTthingbase_1 card, string title, string content, List<string> choice)
    {
        gameObject.SetActive(false);
        callBackCard = card;
        this.title.text = title;
        this.content.text = content;
        live = callBackCard.live;

        // 根据choice的数量给予对应数目的选项
        const float yOffset = -180;
        List<GameObject> buttons = new List<GameObject>();
        buttons.Add(choose);
        for(int i=1;i<choice.Count;i++)
        {
            GameObject button = GameObject.Instantiate(choose, choose.transform.parent);
            button.transform.position += i * new Vector3(0, yOffset);
            buttons.Add(button);
        }
        for (int i = 0; i < choice.Count; i++)
        {
            buttons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = choice[i];
            int ii = i;
            buttons[i].GetComponent<Button>().onClick.AddListener(() => Choose(ii));
        }
        gameObject.SetActive(true);
    }
    public void Choose(int choose)
    {
        if(!CardManager.CardValid(callBackCard))
        {
            callBackCard = null;
            return;
        }
        SystemManager.SendMsg(callBackCard, SLTthingbase_1.mTThingChoose, new SLTthingbase_1.MsgThingChoose(
            live, choose));
        Destroy(gameObject);
    }
    private void OnEnable()
    {
        SControl_3.AddNoControl(1);
    }
    private void OnDisable()
    {
        SControl_3.AddNoControl(-1);
    }
}
