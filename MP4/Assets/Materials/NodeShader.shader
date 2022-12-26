Shader "Unlit/NodeShader"
{
	Properties
	{
		 _MainTex ("Texture", 2D) = "white" {}
		// we want to assign any color seamlessly through the editor
		// use this as default value
		// MyColor("ColorLabel", Color) = (0.2, 0.2, 0.8, 1) 
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
            float4x4 MyTRSMatrix; // use our own TRSMatrix
            fixed4 MyColor;

			
			v2f vert (appdata v)
			{
				v2f o;
                o.vertex = mul(MyTRSMatrix, v.vertex); // allow Unity to use our own matrix
                o.vertex = mul(UNITY_MATRIX_VP, o.vertex); // allow us to do camera + transform with this matrix.
				o.uv = TRANSFORM_TEX(v.uv, _MainTex); // transfomr the texture to move in line with the vertices.

                // o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                col += MyColor;
				return col;
			}
			ENDCG
		}
	}
}
