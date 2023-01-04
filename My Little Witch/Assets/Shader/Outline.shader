Shader "Universal Render Pipeline/Outline"
{
    Properties
    {
        _OutlineColor("Outline Color", Color) = (1,0,0,1)
        _Outline("Outline width", Range(0, 1)) = .1
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Inputs
        {
            float2 uv_MainTex;
            float3 normal;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;
        float _Outline;
        float4 _OutlineColor;

        void surf(Inputs IN, inout SurfaceOutputStandard o)
        {
            o.Normal = IN.normal;
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _OutlineColor;
        }

        ENDCG

        Pass
        {
            Name "OUTLINE"
            Tags {"LightMode" = "Always" }
            ZTest Always
            ZWrite On
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fwdadd

            struct appdata
            {
                float3 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;

                v.vertex *= (1 + _Outline);

                o.pos = UnityObjectToClipPos(v.vertex);

                o.color = _OutlineColor;
                return o;
            }

            half4 frag(v2f i) : COLOR
            {
                return i.color;
            }
            ENDCG
        }
    }
}