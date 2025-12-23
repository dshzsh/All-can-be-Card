Shader "Custom/URPXZGrid"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _GridColor ("Grid Color", Color) = (0.5,0.5,0.5,1)
        _GridSpacing ("Grid Spacing", Float) = 1.0
        _GridThickness ("Grid Thickness", Float) = 0.01
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fog

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 texcoord : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 gridUV : TEXCOORD2;
                float fogCoord : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _GridColor;
                float _GridSpacing;
                float _GridThickness;
                float _Smoothness;
                float _Metallic;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);
                
                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                
                // 使用世界坐标的XZ平面作为网格UV
                output.gridUV = vertexInput.positionWS.xz;
                
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 网格计算
                float2 gridUV = input.gridUV / _GridSpacing;
                float2 grid = abs(frac(gridUV - 0.5) - 0.5);
                float gridLines = 1.0 - smoothstep(0.0, _GridThickness, min(grid.x, grid.y));
                
                // 基础颜色与网格颜色混合
                half4 color = lerp(_BaseColor, _GridColor, gridLines);

                // 光照计算
                InputData lightingInput = (InputData)0;
                lightingInput.positionWS = input.positionWS;
                lightingInput.normalWS = normalize(input.normalWS);
                lightingInput.viewDirectionWS = GetWorldSpaceNormalizeViewDir(input.positionWS);
                lightingInput.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
                lightingInput.fogCoord = input.fogCoord;
                lightingInput.vertexLighting = half3(0,0,0);
                lightingInput.bakedGI = half3(0,0,0);

                SurfaceData surfaceData;
                surfaceData.albedo = color.rgb;
                surfaceData.alpha = color.a;
                surfaceData.emission = half3(0,0,0);
                surfaceData.metallic = _Metallic;
                surfaceData.occlusion = 1.0;
                surfaceData.smoothness = _Smoothness;
                surfaceData.specular = half3(0,0,0);
                surfaceData.clearCoatMask = 0.0;
                surfaceData.clearCoatSmoothness = 0.0;
                surfaceData.normalTS = half3(0,0,1);

                // 应用光照
                half4 finalColor = UniversalFragmentPBR(lightingInput, surfaceData);
                
                return finalColor;
            }
            ENDHLSL
        }

        // 阴影投射Pass
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }

        // 深度Only Pass (用于深度纹理)
        Pass
        {
            Name "DepthOnly"
            Tags{"LightMode" = "DepthOnly"}

            ZWrite On
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/LitInput.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
            ENDHLSL
        }
    }
}