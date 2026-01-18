using TMPro;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField] private TMP_Text mana;

    public void UpdateManaText(int curMana)
    {
        mana.text = curMana.ToString();
    }
}