Shader "Heightmap"
{
	Properties {
		_Color("Color",Color) = (1,1,1,1)
		_Distance("Distance",Float)=2
		_LineFat("LineFat",Range(0,1))=1
		_Correction("Correction",Range(0,1))=0.7
	}
	
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input
		{
			float3 worldPos;
			float3 worldNormal;
		};
		
		float4 _Color;
		int _Distance;
		float _LineFat;
		float _Correction;

		
		void surf(Input IN, inout SurfaceOutput o)
		{
			float  y  = IN.worldPos.y/_Distance;
			float heightFract = y - (int)y;
			
			
			
			
			if(heightFract < _LineFat)
			{
				if(IN.worldNormal.y<=_Correction)
				{
				 	o.Albedo = _Color.rgb;
				}
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