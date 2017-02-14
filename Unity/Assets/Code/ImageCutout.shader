Shader "Custom/ImageCutout"
{
	Properties
	{
		_CenterPoint("Center", Vector) = (0,0,0,0) 
		_MainTex ("Texture", 2D) = "white" {}
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 screenSpace: TEXCOORD1; 
			};

			sampler2D _MainTex;
			fixed4 _CenterPoint; 
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenSpace = ComputeScreenPos(o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float2 offset = i.screenSpace.xy / i.screenSpace.w - _CenterPoint.xy;
				fixed4 col = tex2D(_MainTex, 1 - offset + _CenterPoint.xy * _CenterPoint.z);
				return col;
			}
			ENDCG
		}
	}
}
