using UnityEngine;

public static class GameResource
{
    public static class UVLimitData
    {
        public static Shader UVLimitShader;
        public static readonly int CutoffXl = Shader.PropertyToID("_CutoffXL");
        public static readonly int CutoffXR = Shader.PropertyToID("_CutoffXR");
        public static readonly int CutoffYD = Shader.PropertyToID("_CutoffYD");
        public static readonly int CutoffYU = Shader.PropertyToID("_CutoffYU");
    }
}
