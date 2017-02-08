Shader "Hidden/BGGradient"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color1("Top Color", Color) = (1,1,1,1)
		_Color2("Bottom Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _CameraDepthTexture;
			uniform half4 _MainTex_TexelSize;
			fixed4 _Color1; 
			fixed4 _Color2; 
			struct input
			{
				float4 pos : POSITION;
				half2 uv : TEXCOORD0;
			};

			struct output
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 depthUV : TEXCOORD1; 
			};


			output vert(input i)
			{
				output o;
				o.pos = mul(UNITY_MATRIX_MVP, i.pos);
				o.uv = i.uv; 
				o.depthUV = i.uv;
				#if UNITY_UV_STARTS_AT_TOP
				o.depthUV.y = 1 - o.depthUV.y;
				#endif
				return o; 
			}

			fixed4 frag(output o) : COLOR
			{
				//fixed4 depth = tex2D(_CameraDepthTexture, o.uv);
				float depth = UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, o.depthUV));
				if(depth > 0){
					return tex2D(_MainTex, o.uv); 
				}
				return o.uv.y * _Color1 + (1 - o.uv.y) * _Color2;
			}

			ENDCG
		}
	}
}