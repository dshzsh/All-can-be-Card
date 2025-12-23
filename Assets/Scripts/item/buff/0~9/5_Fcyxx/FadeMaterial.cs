using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMaterial : MonoBehaviour
{
    public static int alphaID = Shader.PropertyToID("_alpha");
    private Renderer[] mrs;
    private float timeMax;
    private float fadeTime;
    public void Set(MeshRenderer mr, float timeMax, float fadeTime)
    {
        this.mrs = new Renderer[1] { mr };
        this.timeMax = timeMax;
        this.fadeTime = fadeTime;
    }
    public void Set(float timeMax, float fadeTime)
    {
        this.timeMax = timeMax;
        this.fadeTime = fadeTime;
        mrs = GetComponentsInChildren<Renderer>();
        //Debug.Log(mrs.Length);
    }
    private void Update()
    {
        timeMax -= Time.deltaTime;
        if(timeMax<fadeTime)
        {
            foreach(Renderer mr in mrs)
            {
                Material[] materials = mr.materials;
                Material nm = materials[0]; nm.SetFloat(alphaID, timeMax / fadeTime);
                //Debug.Log(nm.GetFloat(alphaID));
                for (int i = 0; i < materials.Length; i++)
                {
                    materials[i] = nm;
                }
                mr.materials = materials;
            }
        }
    }
}
