using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MNshop_3_obj : MonoBehaviour
{
    public ItemObj itemObj;
    public TextMeshPro text;

    public void Set(CardBase item, string costText)
    {
        itemObj.SetCard(item);
        text.text = costText;
        itemObj.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
    }
    public void SetClear()
    {
        itemObj.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }
}
