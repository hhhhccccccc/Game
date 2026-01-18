using UnityEngine;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour
{
    [SerializeField] private Image image;
    public PerkData PerkData { get; private set; }

    public void Setup(PerkData perkData)
    {
        PerkData = perkData;
        image.sprite = perkData.Image;
    }
}