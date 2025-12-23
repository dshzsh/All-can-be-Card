Shader "Universal Render Pipeline/Custom/FresnelGlowTransparent"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _GlowColor("Glow Color", Color) = (1,1,1,1)
        _GlowPower("Glow Power", Range(0,10)) = 1
        _FresnelPower("Fresnel Power", Range(0,10)) = 1
        _Transparency("Transparency", Range(0,1)) = 0.5
        _OffsetAmount("Offset Amount", Range(0, 0.1)) = 0.01
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
        }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 worldViewDir : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _Color;
                float4 _GlowColor;
                float _GlowPower;
                float _FresnelPower;
                float _Transparency;
                float _OffsetAmount;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                // 沿着法线方向向外偏移顶点位置
                input.positionOS.xyz += input.normalOS * _OffsetAmount;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionHCS = vertexInput.positionCS;

                // 计算世界空间法线
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                output.worldNormal = normalInput.normalWS;

                // 计算世界空间视角方向
                float3 worldPos = vertexInput.positionWS;
                output.worldViewDir = GetWorldSpaceNormalizeViewDir(worldPos);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // 归一化向量
                float3 worldNormal = normalize(input.worldNormal);
                float3 worldViewDir = normalize(input.worldViewDir);

                // 计算菲涅尔项
                float fresnelTerm = pow(1 - saturate(dot(worldNormal, worldViewDir)), _FresnelPower);
                
                // 计算发光颜色
                half3 emission = _GlowColor.rgb * _GlowPower * fresnelTerm;
                
                // 组合最终颜色
                half4 color = half4(emission, _Transparency);
                
                return color;
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Simple Lit"
}