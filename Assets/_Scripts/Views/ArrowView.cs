using UnityEngine;

public class ArrowView : MonoBehaviour
{
    [SerializeField] private GameObject arrowHead;
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3 startPos;

    private void Update()
    {
        Vector3 mousePos = MouseUtil.GetMousePositionInWorldSpace();
        Vector3 direction = (arrowHead.transform.position - startPos).normalized;
        lineRenderer.SetPosition(1, mousePos - direction * 0.5f);
        arrowHead.transform.position = mousePos;
        arrowHead.transform.right = direction;
    }

    public void SetupArrow(Vector3 startPosition)
    {
        startPos = startPosition;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, MouseUtil.GetMousePositionInWorldSpace());
    }
}