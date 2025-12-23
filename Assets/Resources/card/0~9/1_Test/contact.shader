Shader "Custom/TransparentContactPattern"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _PatternTex ("Pattern (RGB)", 2D) = "white" {}
        _Color ("Base Color", Color) = (1,1,1,0.5)
        _PatternColor ("Pattern Color", Color) = (1,1,1,1)
        _ContactThreshold ("Contact Threshold", Range(0.0, 0.5)) = 0.05
        _PatternStrength ("Pattern Strength", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            Cull Off
            ZWrite Off
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _PatternTex;
            sampler2D _CameraDepthTexture;

            fixed4 _Color;
            fixed4 _PatternColor;
            float _ContactThreshold;
            float _PatternStrength;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            float LinearEyeDept(float d)
            {
                return 1.0 / (_ZBufferParams.z * d + _ZBufferParams.w);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 当前透明物体的基础颜色
                fixed4 baseCol = tex2D(_MainTex, i.uv) * _Color;

                // 采样场景深度
                float sceneDepth = LinearEyeDept(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));

                // 当前片元深度（世界空间 Eye Depth）
                float myDepth = LinearEyeDept(i.screenPos.z / i.screenPos.w);

                // 深度差
                float diff = abs(sceneDepth - myDepth);

                // 接触区域强度
                float contactMask = saturate(1.0 - diff / _ContactThreshold);

                // 花纹采样
                fixed4 patternCol = tex2D(_PatternTex, i.uv) * _PatternColor;

                // 混合花纹
                baseCol = lerp(baseCol, patternCol, contactMask * _PatternStrength);

                return baseCol;
            }
            ENDCG
        }
    }
}
