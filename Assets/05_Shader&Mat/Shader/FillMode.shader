Shader "Custom/FilledRadialShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill Percentage", Range(0, 1)) = 1.0
        _Color ("Color", Color) = (1, 1, 1, 1)
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Fill;
            fixed4 _Color;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float2 dir = center - i.uv;
                float angle = atan2(dir.y, dir.x);
                float fill = saturate(1.0 - angle / (2 * 3.14159) - (_Fill + 0.5));
                return fill > 0.0 ? half4(0,0,0,0) : tex2D(_MainTex, i.uv) * _Color;
            }
            ENDCG
        }
    }
}
