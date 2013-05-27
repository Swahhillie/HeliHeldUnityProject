Shader "Heightmap"
{
	Properties {
		_Color("Color",Color) = (1,1,1,1)
		_Distance("Distance",Float)=2
	}
	
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input
		{
			float3 worldPos;
		};
		
		float4 _Color;
		int _Distance;
		sampler2D _MainTex;

		
		void surf(Input IN, inout SurfaceOutput o)
		{
			int x = (int)IN.worldPos.y%_Distance;		
			if(x==0)
			{
				o.Albedo = _Color.rgb;
			}
			else
			{
				o.Albedo = 0;
			}

			
			//half rim = 1.0 - dot(normalize(IN.viewDir),o.Normal);
			//o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}