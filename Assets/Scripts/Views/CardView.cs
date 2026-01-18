using DG.Tweening;
using TMPro;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text type;
    [SerializeField] private TMP_Text rarity;
    [SerializeField] private SpriteRenderer imageSR;
    [SerializeField] private GameObject wrapper;
    [SerializeField] private LayerMask dropLayer;
    public CardData CardData { get; private set; }

    private Vector3 dragStartPos;
    private Quaternion dragStartRot;

    public void Setup(CardData data)
    {
        CardData = data;
        title.text = data?.Name ?? "No Title";
        description.text = data?.Desc ?? "No Description";
        if (data.CDTurns > 0)
            description.text += $"，CD: {data.CDTurns}";
        if (data.IsEcho)
            description.text += "，回响";
        type.text = data?.Type ?? "No Type";
        rarity.text = data?.Rarity switch
        {
            "1级：普通卡" => "普通",
            "2级：稀有卡" => "稀有",
            "3级：超级卡" => "超级",
            "4级：传说卡" => "传说",
            _ => "普通"
        };

        rarity.color = data.Rarity switch
        {
            "1级：普通卡" => Color.gray,
            "2级：稀有卡" => Color.blue,
            "3级：超级卡" => Color.magenta,
            "4级：传说卡" => Color.yellow,
            _ => Color.white
        };
        imageSR.sprite = data?.Image;
        
    }

    public void UpdatePositionRotation(Vector3 pos, Quaternion rot, float duration)
    {
        transform.DOMove(pos, duration);
        transform.DORotate(rot.eulerAngles, duration);
    }

    private void OnMouseEnter()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        wrapper.SetActive(false);
        Vector3 pos = new(transform.position.x, -2, 0);
        CardViewHoverSystem.Instance.Show(CardData, pos);
    }

    private void OnMouseExit()
    {
        if (!Interactions.Instance.PlayerCanHover()) return;
        CardViewHoverSystem.Instance.Hide();
        wrapper.SetActive(true);
    }

    private void OnMouseDown()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (CardData.ManualTargetEffect != null)
        {
            ManualTargetSystem.Instance.StartTargeting(transform.position);
        }
        else
        {
            Interactions.Instance.PlayerIsDragging = true;
            wrapper.SetActive(true);
            CardViewHoverSystem.Instance.Hide();
            dragStartPos = transform.position;
            dragStartRot = transform.rotation;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
        }
    }

    private void OnMouseDrag()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (CardData.ManualTargetEffect != null) return;
        transform.position = MouseUtil.GetMousePositionInWorldSpace(-1);
    }

    private void OnMouseUp()
    {
        if (!Interactions.Instance.PlayerCanInteract()) return;
        if (CardData.ManualTargetEffect != null)
        {
            EnemyView target = ManualTargetSystem.Instance.EndTargeting(MouseUtil.GetMousePositionInWorldSpace(-1));
            if (target != null)
            {
                PlayCardGA playCardGA = new(CardData, target);
                ActionSystem.Instance.Perform(playCardGA);
            }
        }
        else
        {
            if ( Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit, 10f, dropLayer))
            {
                PlayCardGA playCardGA = new(CardData);
                ActionSystem.Instance.Perform(playCardGA);
            }
            else
            {
                transform.position = dragStartPos;
                transform.rotation = dragStartRot;
            }

            Interactions.Instance.PlayerIsDragging = false;
        }
    }
}