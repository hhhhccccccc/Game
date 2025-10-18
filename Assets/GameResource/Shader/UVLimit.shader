Shader "Custom/SpriteMask"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CutoffXL ("CutoffXL", Range(0,1)) = 0.5
        _CutoffXR ("CutoffXR", Range(0,1)) = 0.5
        _CutoffYD ("CutoffYD", Range(0,1)) = 0.5
        _CutoffYU ("CutoffYU", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _CutoffXL;
            float _CutoffXR;
            float _CutoffYD;
            float _CutoffYU;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 tex = tex2D(_MainTex, i.texcoord);
                if (i.texcoord.x < _CutoffXL) discard;
                if (i.texcoord.x > _CutoffXR) discard;
                if (i.texcoord.y < _CutoffYD) discard;
                if (i.texcoord.y > _CutoffYU) discard;
                return tex;
            }
            ENDCG
        }
    }
}