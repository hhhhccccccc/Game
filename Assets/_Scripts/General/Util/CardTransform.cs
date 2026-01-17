using UnityEngine;

public struct CardTransform
{
    public Vector3 pos;
    public Quaternion rot;

    public CardTransform(Vector3 position, Quaternion rotation)
    {
        pos = position;
        rot = rotation;
    }
}