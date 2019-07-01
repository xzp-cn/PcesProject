// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Jan_shader/GlowAdditiveSimple(OutLine)" {
	Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_CoreColor ("Core Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Particle Texture", 2D) = "white" {}
	_TintStrength ("Tint Color Strength", Range(0, 5)) = 1
	_CoreStrength ("Core Color Strength", Range(0, 8)) = 1
	_CutOutLightCore ("CutOut Light Core", Range(-1, 1)) = 0.5
	
	_LineSize("OutlineSize", range(0, 1.5)) = 0.1
	_LineColor("LineColor", Color) = (0,0,0,1)
}

Category {
	Lighting Off 
	ZWrite On 
	
	SubShader {
		Pass {
			Tags{
				"LightMode" = "Always"
			}
			Cull Off
			ZWrite Off
			ZTest Always
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			half _LineSize;
			fixed4 _LineColor;
			struct v2f{
				fixed4 pos:SV_POSITION;
				fixed4 color: COLOR;
			};

			v2f vert(appdata_full v){
				v2f o;
				o.pos=UnityObjectToClipPos(v.vertex);
				half3 norm = mul((half3x3)UNITY_MATRIX_IT_MV, v.normal);
				half2 offset=TransformViewToProjection(norm.xy);
				o.pos.xy += offset*_LineSize;
				o.color = _LineColor;
				return o;
			}
			fixed4 frag(v2f i):COLOR {
				return i.color;
			}

			ENDCG
		}
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
		
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;
			fixed4 _CoreColor;
			half _CutOutLightCore;
			half _TintStrength;
			half _CoreStrength;
			
			struct appdata_t {
				half4 vertex : POSITION;
				fixed4 color : COLOR;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f {
				half4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};
			
			half4 _MainTex_ST;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : COLOR
			{
				fixed4 tex = tex2D(_MainTex, i.texcoord);
				fixed4 col = (_TintColor * tex.g * _TintStrength + tex.r * _CoreColor * _CoreStrength - _CutOutLightCore)*tex; 
				return clamp(col, 0, 255);
			}
			ENDCG 
		}
	}
}
}
