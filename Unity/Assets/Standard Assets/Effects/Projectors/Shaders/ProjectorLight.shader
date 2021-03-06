// Upgrade NOTE: replaced '_Projector' with 'unity_Projector'
// Upgrade NOTE: replaced '_ProjectorClip' with 'unity_ProjectorClip'

Shader "Projector/LightNoBack" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_ShadowTex ("Cookie", 2D) = "" {}
		_FalloffTex ("FallOff", 2D) = "" {}
		_Intensity ("Intensity", Range(0, 10)) = 1	
		_Normal("ProjectorNormal", Vector) =  (0,0,0,0)
	}
	
	Subshader {
		Tags {"Queue"="Transparent"}
		Pass {
			ZWrite Off
			ColorMask RGB
			Blend DstColor One
			Offset -1, -1
	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			
			struct v2f {
				float4 uvShadow : TEXCOORD0;
				float4 uvFalloff : TEXCOORD1;
				UNITY_FOG_COORDS(2)
				float4 pos : SV_POSITION;
				float a : TEXCOORD2; 
			};
			struct io {
				float4 vertex: POSITION;
				float3 normal : NORMAL;
			};
			float4x4 unity_Projector;
			float4x4 unity_ProjectorClip;
			fixed4 _Normal; 
			
			v2f vert (io i)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, i.vertex);
				o.uvShadow = mul (unity_Projector, i.vertex);
				o.uvFalloff = mul (unity_ProjectorClip, i.vertex);
				o.a = dot(i.normal, _Normal.xyz);
				o.a = (o.a < 0) ? 1 : 0;
				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}
			
			fixed4 _Color;
			sampler2D _ShadowTex;
			sampler2D _FalloffTex;
			float _Intensity;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 texS = tex2Dproj (_ShadowTex, UNITY_PROJ_COORD(i.uvShadow));
				texS.rgb *= _Color.rgb;
				texS.a = 1.0-texS.a;
	
				fixed4 texF = tex2Dproj (_FalloffTex, UNITY_PROJ_COORD(i.uvFalloff));
				fixed4 res = texS * texF.a * i.a * _Intensity;

				UNITY_APPLY_FOG_COLOR(i.fogCoord, res, fixed4(0,0,0,0));
				return res;
			}
			ENDCG
		}
	}
}
