using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPanel : MonoBehaviour
{
    public static GameObject magicKeyObj;

    public static float useAlpha = 0.4f;

    public GameObject panel;

    public List<MagicKey> magicKeys = new();
    public List<bool> cdInTM = new();
    Cmagic_14 myMagic;
    CardBase cNullMagic;
    public void SetMagic(Cmagic_14 myMagic)
    {
        this.myMagic = myMagic;
    }
    private bool ShowCanUse(float cdPar, int key)
    {
        return cdPar < MyTool.SmallFloat && Smagic_14.CanUseMagic(myMagic, key);
    }
    private void Update()
    {
        if (myMagic == null) return;
        if (cNullMagic == null) cNullMagic = CardManager.CreateCard<CMnulll_0>();
        while(myMagic.holdMax>magicKeys.Count)
        {
            magicKeys.Add(GameObject.Instantiate(magicKeyObj, panel.transform).GetComponent<MagicKey>());
            cdInTM.Add(false);
        }
        
        for (int i = 0; i < myMagic.holdMax; i++)
        {
            if(myMagic.magics.Count<=i)
            {
                magicKeys[i].transform.localScale = Vector3.zero;
                magicKeys[i].goodUI.SetCard(null);
                magicKeys[i].goodImage.color = Color.white;
            }
            float cdPar = myMagic.cdBearMax[i] == 0 ? 0 : (myMagic.cdBear[i] / myMagic.cdBearMax[i]);
            if (cdPar < 0) cdPar = 0;
            magicKeys[i].cd.transform.localScale = new Vector3(1, cdPar, 1);
            magicKeys[i].goodUI.SetCard(myMagic.magics[i]);

            bool canUse = ShowCanUse(cdPar, i);
            if (!canUse && !cdInTM[i])
            {
                magicKeys[i].canvasGroup.alpha = useAlpha;
                cdInTM[i] = true;
            }
            if (canUse && cdInTM[i])
            {
                magicKeys[i].canvasGroup.alpha = 1;
                cdInTM[i] = false;
            }

            if (!magicKeys[i].gameObject.activeSelf)
                magicKeys[i].gameObject.SetActive(true);
        }
        for (int i = myMagic.holdMax; i < magicKeys.Count; i++)
        {
            magicKeys[i].gameObject.SetActive(false);
        }
    }
}
