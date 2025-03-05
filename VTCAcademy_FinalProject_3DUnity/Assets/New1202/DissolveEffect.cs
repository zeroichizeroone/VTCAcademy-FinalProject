using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    public int matIndex = 0;
    public float time;
    public float from;
    public float to;

    public AnimationCurve curve;
    private void OnEnable()
    {
        DoAppear();
    }
    public void DoAppear()
    {
        StartCoroutine(DoDissolveAction(time, from, to));
    }
    IEnumerator DoDissolveAction(float time, float from, float to)
    {
        float crrTime = 0;
        Material[] mats = new Material[0];
        if (this.GetComponent<SkinnedMeshRenderer>() != null)
            mats = this.GetComponent<SkinnedMeshRenderer>().materials;
        if (this.GetComponent<MeshRenderer>() != null)
            mats = this.GetComponent<MeshRenderer>().materials;
        foreach (Material mat in mats)
        {
            if (mat.HasFloat("_Cutoff"))
                mat.SetFloat("_Cutoff", from);
        }
        while (crrTime <= time)
        {
            foreach (Material mat in mats)
            {
                if (mat.HasFloat("_Cutoff"))
                {
                    float p = crrTime / time;
                    p = curve.Evaluate(p);
                    float value = (to - from) * p + from;
                    mat.SetFloat("_Cutoff", value);
                }

                yield return null;
            }
            if (this.GetComponent<SkinnedMeshRenderer>() != null)
                this.GetComponent<SkinnedMeshRenderer>().materials = mats;
            yield return new WaitForSeconds(0.05f);
            crrTime += 0.05f;
        }
    }
}
