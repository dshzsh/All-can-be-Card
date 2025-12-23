using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class GiveTextInUI : MonoBehaviour
{
    private RectTransform textUI;
    public void Set(string text, Color color, float size)
    {
        textUI = UIBasic.GiveUI(UIBasic.textUI).GetComponent<RectTransform>();
        textUI.transform.SetParent(UIBasic.sceneShow.transform);
        TextMeshProUGUI tt = textUI.GetComponent<TextMeshProUGUI>();
        tt.text = text; tt.color= color; tt.fontSize *= size;
        textUI.transform.DOScale(1, 0.2f);
        Update();
    }
    private void Update()
    {
        if (textUI == null) return;

        textUI.position = UIBasic.WorldToUIPosition(transform.position);
    }
    private void OnDestroy()
    {
        if (textUI != null) GameObject.Destroy(textUI.gameObject);
    }
}
