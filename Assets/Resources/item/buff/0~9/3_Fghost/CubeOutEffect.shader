Shader "Custom/CubeOutEffect" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1,0,1,1) // 紫色
        _OutlineWidth("Outline Width", Range(0, 0.2)) = 0.1
    }
        SubShader{
            Tags { "Queue" = "Transparent" }

            // 描边Pass：渲染背面扩展的几何体
            Pass {
                Cull Front
                ZWrite Off
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                };

                float _OutlineWidth;
                float4 _OutlineColor;

                v2f vert(appdata v) {
                    v2f o;
                    // 沿法线方向扩展顶点
                    float3 normal = normalize(v.normal);
                    v.vertex.xyz += normal * _OutlineWidth;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    return _OutlineColor;
                }
                ENDCG
            }

            // 正常Pass：渲染物体本身
            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                };

                sampler2D _MainTex;

                v2f vert(appdata v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    return tex2D(_MainTex, i.uv);
                }
                ENDCG
            }
        }
}