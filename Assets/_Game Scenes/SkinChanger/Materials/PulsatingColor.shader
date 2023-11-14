Shader "Custom/PulsatingGradientShader" {
    Properties {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _Speed ("Speed", Range(0, 10)) = 1
    }
 
    SubShader {
        Tags {"Queue"="Transparent" "RenderType"="Opaque"}
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float4 color : COLOR;
            };
            
            float4 _Color1;
            float4 _BaseColor;
            float _Speed;
            
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                o.color = _BaseColor;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target {
                float t = sin(dot(i.vertex.xyz, i.worldNormal) * _Speed + _Time.y * _Speed);
                float factor = (dot(normalize(_WorldSpaceCameraPos - i.vertex.xyz), i.worldNormal) + 1) / 2;
                fixed4 gradientColor = lerp(_BaseColor, _Color1, factor);
                fixed4 baseColor = _BaseColor;
                fixed4 finalColor = baseColor + (gradientColor - baseColor) * t;
                finalColor.a = baseColor.a;
                return finalColor;
            }
            
            ENDCG
        }
    }
    FallBack "Diffuse"
}
