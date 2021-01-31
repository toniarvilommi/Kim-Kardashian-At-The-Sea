Shader "Unlit/WaterToon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DepthGradientShallow("Depth Gradient Shallow", Color) = (0.325, 0.807, 0.971, 0.725)
        _DepthGradientDeep("Depth Gradient Deep", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 50
        _SurfaceNoise("Surface Noise", 2D) = "white" {}
        _SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777

        _WaveHighColor("WaveHighColor", Color) = (0.086, 0.407, 1, 0.749)
        _WaveHighFloat("WaveHighFloat", float) = 1

        _FoamDistance("Foam Distance", Float) = 0.4
        _Position("Position", Vector) = (0,0,0)
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 noiseUV : TEXCOORD2;
                float4 screenPosition : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _DepthGradientShallow;
            float4 _DepthGradientDeep;

            float _DepthMaxDistance;

            sampler2D _CameraDepthTexture;

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;

            float _SurfaceNoiseCutoff;

            float4 _WaveHighColor;
            float _WaveHighFloat;

            float _FoamDistance;

            float4 _Position;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.noiseUV = v.vertex;
                return o;
            }

            float random (in float2 st) {
                return frac(sin(dot(st.xy,
                                    float2(12.9898,78.233)))
                            * 43758.5453123);
            }

            // 2D Noise based on Morgan McGuire @morgan3d
            // https://www.shadertoy.com/view/4dS3Wd
            float noise (in float2 st) {
                float2 i = floor(st);
                float2 f = frac(st);

                // Four corners in 2D of a tile
                float a = random(i);
                float b = random(i + float2(1.0, 0.0));
                float c = random(i + float2(0.0, 1.0));
                float d = random(i + float2(1.0, 1.0));

                // Smooth Interpolation

                // Cubic Hermine Curve.  Same as SmoothStep()
                float2 u = f*f*(3.0-2.0*f);
                // u = smoothstep(0.,1.,f);

                // Mix 4 coorners percentages
                return lerp(a, b, u.x) +
                        (c - a)* u.y * (1.0 - u.x) +
                        (d - b) * u.x * u.y;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float existingDepth01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPosition)).r;
                float existingDepthLinear = LinearEyeDepth(existingDepth01);
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float depthDifference = existingDepthLinear - i.screenPosition.w;
                
                float waterDepthDifference01 = saturate(depthDifference / _DepthMaxDistance);
                float4 waterColor = lerp(_DepthGradientShallow, lerp(_DepthGradientDeep, _WaveHighColor, i.noiseUV.y/_WaveHighFloat), waterDepthDifference01);

                float2 pos = i.noiseUV.xz+_Position.xz;
                pos.x = pos.x;
                pos.y = pos.y;
                
                float noise2 = noise(pos);

                float foamDepthDifference01 = saturate(depthDifference / _FoamDistance);
                float surfaceNoiseCutoff = foamDepthDifference01 * _SurfaceNoiseCutoff;

                float surfaceNoise = noise(pos) > _SurfaceNoiseCutoff ? 0.1 : 0;

                //return float4(noise2, noise2, noise2, 1.0);
                // apply fog

                waterColor = waterColor + surfaceNoise;
                UNITY_APPLY_FOG(i.fogCoord, waterColor);

                return waterColor;
            }
            ENDCG
        }
    }
}
