using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UIBar : MonoBehaviour
{
    public RectTransform BackGround;
    public RectTransform Bar;
    public Image MaskBar;
    private Material MaskMaterial;
    private float Width;

    private float CurrPct = 1;
    private Vector2 CurrSizeDelta = new Vector2();

    private LTDescr LTDescr;

    public void Init()
    {
        Width = BackGround.sizeDelta.x;
        CurrSizeDelta = Bar.sizeDelta;
        if (MaskBar != null)
        {
            MaskBar.material = new Material(GameResource.UVLimitData.UVLimitShader);
            MaskMaterial = MaskBar.material;
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXl, 0.5f);
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXR, 0.5f);
        }
        SetValue(1, 1);
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
                        CurrSizeDelta.x = Width * value;
                        Bar.sizeDelta = CurrSizeDelta;
                    }).setOnComplete(() =>
                    {
                        LTDescr = null;
                    });
            }
            else
            {
                CurrSizeDelta.x = Width * pct;
                Bar.sizeDelta = CurrSizeDelta;
                CurrPct = pct;
            }
        }
        else
        {
            CurrSizeDelta.x = Width * pct;
            Bar.sizeDelta = CurrSizeDelta;
            MaskMaterial.SetFloat(GameResource.UVLimitData.CutoffXl, pct);
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

