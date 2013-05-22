Shader "Rimlight"
{
	Properties {
		_ColorTint("Color",Color) = (1,1,1,1)
		_MainTex("MainTexture",2D)="white"{}
		//_BumpMap("Bump Map",2D) = "bump"{}
		_RimColor("RimColor",Color) = (1,1,1,1)
		_RimPower("RimPower",Range(0.2,10.0))= 3.0
	}
	
	SubShader
	{
		Tags {"RenderType" = "Opaque"}
		
		CGPROGRAM
		#pragma surface surf Lambert
		
		struct Input
		{
			float4 color : COLOR;
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};
		
		float4 _ColorTint;
		sampler2D _MainTex;
		//sampler2D _BumpMap;
		float4 _RimColor;
		float _RimPower;
		
		void surf(Input IN, inout SurfaceOutput o)
		{
			IN.color = _ColorTint;
			o.Albedo = tex2D(_MainTex,IN.uv_MainTex).rgb * IN.color;
			//o.normal = UnpackNormal(text2D(_BumpMap,IN.uv_BumpMap));
			
			half rim = 1.0 - dot(normalize(IN.viewDir),o.Normal);
			o.Emission = _RimColor.rgb * pow(rim, _RimPower);
			
		}
		
		ENDCG
	}
	FallBack "Diffuse"
}