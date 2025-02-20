﻿Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Ambient ("Ambient Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Amount ("Amount", Range(0,1)) = 0.0
        _NormalTex ("Normal Map", 2D) = "bump" {}
    }
    SubShader
    {
        Tags {"RenderType"="Opaque"}
        LOD 200

        Pass {
            Tags { "RenderType"="Opaque"}
 
                Cull Front
                Fog { Mode Off }
                Lighting Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            half _Amount;
            fixed4 _Outline;
            struct v2f {
                float4 pos : SV_POSITION;
            };
            v2f vert (appdata_base v) {
                v2f o;
                v.vertex.xyz += v.normal * _Amount;
                o.pos = UnityObjectToClipPos (v.vertex);
                return o;
            }
            half4 frag (v2f i) : SV_Target
            {
                return _Outline;
            }
            ENDCG
        }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Toon

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _NormalTex;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed4 _Ambient;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 LightingToon (SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            half NdotL = dot(s.Normal, lightDir);
            if(NdotL >= 0.6){
                NdotL = 1;
            }
            else if(NdotL >= 0.5){
                NdotL = 0.3;
            }else{
                NdotL = 0.0;
            }
            fixed4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * NdotL * atten;
            c.a = s.Alpha;
            return c;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
            o.Albedo *= _Color * _Ambient;
            float3 normalMap = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
            o.Normal = normalMap.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
