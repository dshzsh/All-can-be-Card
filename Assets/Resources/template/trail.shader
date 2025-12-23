Shader "Custom/TrailContactFade_BuiltIn"
{
    Properties
    {
        _MainColor ("Main Color", Color) = (1,1,1,1)
        _FadeDistance ("Fade Distance", Range(0.01, 1)) = 0.1
        _BaseAlpha ("Base Alpha", Range(0, 1)) = 0.8
        _TintColor ("Tint Color", Color) = (1,1,1,1)
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
        }
        
        LOD 100
        
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_instancing
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;  // 添加颜色属性
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
                float eyeDepth : TEXCOORD1;
                fixed4 color : COLOR;  // 传递颜色到片元着色器
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            fixed4 _MainColor;
            fixed _FadeDistance;
            fixed _BaseAlpha;
            
            #ifdef UNITY_INSTANCING_ENABLED
                UNITY_INSTANCING_BUFFER_START(Props)
                    UNITY_DEFINE_INSTANCED_PROP(fixed4, _TintColor)
                UNITY_INSTANCING_BUFFER_END(Props)
            #else
                fixed4 _TintColor = fixed4(1,1,1,1);
            #endif
            
            sampler2D _CameraDepthTexture;
            
            v2f vert (appdata v)
            {
                v2f o;
                
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);
                
                // 计算眼空间深度
                COMPUTE_EYEDEPTH(o.eyeDepth);
                
                // 传递顶点颜色（包含粒子系统的startColor）
                o.color = v.color;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                
                // 采样深度纹理
                float2 screenUV = i.screenPos.xy / i.screenPos.w;
                float sceneDepth = LinearEyeDepth(tex2D(_CameraDepthTexture, screenUV).r);
                
                // 计算深度差异
                float depthDifference = sceneDepth - i.eyeDepth;
                
                // 根据深度差异计算透明度
                fixed alpha = _BaseAlpha;
                
                if (depthDifference > 0 && depthDifference < _FadeDistance)
                {
                    alpha = lerp(0, _BaseAlpha, depthDifference / _FadeDistance);
                }
                else if (depthDifference <= 0)
                {
                    alpha = 0;
                }
                
                // 组合颜色：使用粒子颜色与主颜色的混合
                #ifdef UNITY_INSTANCING_ENABLED
                    fixed4 tintColor = UNITY_ACCESS_INSTANCED_PROP(Props, _TintColor);
                #else
                    fixed4 tintColor = _TintColor;
                #endif
                
                fixed4 finalColor = _MainColor * i.color;
                finalColor.a *= alpha;
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    FallBack "Particles/Alpha Blended"
}