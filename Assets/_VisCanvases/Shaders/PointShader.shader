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
				float3 worldPos : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
			sampler3D _ColorData;
			sampler2D _MainTex;
			sampler2D _ColorMap;
			int _HasColorVariable;
			float4 _MainTex_ST;
			float4x4 _DataBoundsMatrixInv;
			
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
				float val = 0;

			int pointIndex = floor(i.uv.y +0.5);
			int cellIndex = floor(i.uv.x + 0.5);
			float3 dataSpace = WorldToDataSpace(i.worldPos);

				if(_HasColorVariable == 1) {
					float4 worldSpace = float4(i.worldPos,1);
					float4 canvasSpace = mul(_CanvasInverse,worldSpace);
					float4 innerSceneSpace = mul(_CanvasInnerSceneInverse,worldSpace);
					float4 dataSpace = mul(_DataBoundsMatrixInv,innerSceneSpace);
					float3 textureSpace = (dataSpace.xyz+0.5);
					val = tex3D (_ColorData, textureSpace).x/20.0;
				}

				float3 dataVal = GetData(1,floor(i.uv.x +0.5),floor(i.uv.y +0.5),WorldToDataSpace(i.worldPos));
				float3 normalizedDataVal = NormalizeData(1,dataVal);

				// sample the texture
				fixed4 col = tex2D(_ColorMap,float2(normalizedDataVal.x,0.5));

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);



			float3 opacityVal = NormalizeData(3,GetData(3,cellIndex,pointIndex,dataSpace));
			if(VariableIsAssigned(3)) {
				//col.rgb = float3(normalizedDataVal.x,normalizedDataVal.x,normalizedDataVal.x);
				StippleTransparency(i.vertex,_ScreenParams,opacityVal.x);

			}

				col = MarkBounds(i.worldPos,col);
				StippleCrop(i.worldPos,i.vertex,_ScreenParams);

				return col;
			}
			ENDCG
		}
	}
}
