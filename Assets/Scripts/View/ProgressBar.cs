using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ProgressBar : MonoBehaviour
{
    public SpriteRenderer Bar;
    private Material BarMaterial;
    public SpriteRenderer MaskBar;
    private Material MaskMaterial;

    private float CurrPct = 1;

    private LTDescr LTDescr;
    

    public void Init(float curr = 1.0f, float max = 1.0f)
    {
        if (Bar != null)
        {
            Bar.material = new Material(GameResource.UVLimitData.UVLimitShader);
            BarMaterial = Bar.material;
            BarMaterial.SetFloat(GameResource.UVLimitData.CutoffXl, 0f);
            BarMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, 1f);
            BarMaterial.SetFloat(GameResource.UVLimitData.CutoffYD, 0f);
            BarMaterial.SetFloat(GameResource.UVLimitData.CutoffYU, 1f);
        }
        if (MaskBar != null)
        {
            MaskBar.material = new Material(GameResource.UVLimitData.UVLimitShader);
            MaskMaterial = MaskBar.material;
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXl, 0.5f);
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, 0.5f);
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffYD, 0f);
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffYU, 1f);
        }
    }

    public void SetValue(float curr, float max, bool useTween = false, float duration = 0.5f)
    {
        var pct = curr / max;
        if (MaskBar == null)
        {
            if (useTween)
            {
                if (LTDescr != null)
                {
                    LTDescr.pause();
                    LTDescr = null;
                }
            
                LTDescr = LeanTween.value(gameObject, CurrPct, pct, duration)
                    .setEase(LeanTweenType.easeOutQuad)
                    .setOnUpdate((float value) =>
                    {
                        CurrPct = value;
                        BarMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, value);
                    }).setOnComplete(() =>
                    {
                        LTDescr = null;
                    });
            }
            else
            {
                BarMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, pct);
                CurrPct = pct;
            }
        }
        else
        {
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXl, pct);
            BarMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, pct);
            if (LTDescr != null)
            {
                LTDescr.pause();
                LTDescr = null;
            }
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, CurrPct);
            
            LTDescr = LeanTween.value(gameObject, CurrPct, pct, duration)
                .setEase(LeanTweenType.easeOutQuad)
                .setOnUpdate((float value) =>
                {
                    CurrPct = value;
                    MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, value);
                }).setOnComplete(() =>
                {
                    LTDescr = null;
                });
        }
    }
}

