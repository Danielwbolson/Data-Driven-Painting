﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/PointShader"
{
	Properties
	{
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
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			#include "CanvasSupport.cginc"
			#include "VariableSupport.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv3 : TEXCOORD3;
				float3 worldPos : TEXCOORD2;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
			sampler3D _ColorData;
			sampler2D _MainTex;
			sampler2D _ColorMap;
			int _HasColorVariable;
			float4 _MainTex_ST;
			float4x4 _DataBoundsMatrixInv;
			int _useColormap;
			int _flipColormap;
			float4 _Color;

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			
			fixed4 frag (v2f i) : SV_Target
			{

				int cellIndex = floor(i.uv.x + 0.5);
				int pointIndex = floor(i.uv.y +0.5);

				float3 dataSpace = WorldToDataSpace(i.worldPos);

				fixed4 c = float4(1,1,1,1);;

				if(VariableIsAssigned(1)){
					float3 dataVal =  NormalizeData(1,GetData(1,cellIndex,pointIndex,WorldToDataSpace(i.worldPos)));
					float colormapU = dataVal.x;
					if(_flipColormap)
						colormapU = 1-colormapU;


					if(_useColormap == 1)
						c.rgb = tex2D(_ColorMap,float2(colormapU,0.5)).rgb;
					else
						c.rgb = _Color.rgb;


				} else {
					if(_useColormap == 1)
						c.rgb = tex2D(_ColorMap,float2(_flipColormap==1?0:1,0.5)).rgb;
					else
						c.rgb *= _Color.rgb;
				}

				if(VariableIsAssigned(3)) {
					float3 opacityVal = NormalizeData(1,GetData(1,cellIndex,pointIndex,WorldToDataSpace(i.worldPos)));
					StippleTransparency(i.vertex,_ScreenParams,opacityVal.x);
				}


				c = MarkBounds(i.worldPos,c);
				StippleCrop(i.worldPos,i.vertex,_ScreenParams);
				UNITY_APPLY_FOG(i.fogCoord, c);
				UNITY_OPAQUE_ALPHA(c.a);
				return c;
			}
			ENDCG
		}
	}
}
