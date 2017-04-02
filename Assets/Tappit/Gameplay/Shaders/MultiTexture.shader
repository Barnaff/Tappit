Shader "Custom/MultiTexture" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0


		_BlendTex ("Blended Texture", 2D) = "white" {}
		_BlendMask("Blend Mask", 2D) = "black" {}
		_BlendColor("Blend Color", Color) = (1,1,1,1)
		_BlendAlpha("Blend Alpha", Range(0,1)) = 0
		_moveSpeedX("Move Speed X", float) = 0
		_moveSpeedY("Move Speed Y", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
			
			Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0


		sampler2D _MainTex;
		sampler2D _BlendTex;
		sampler2D _BlendMask;
		float _BlendAlpha;
		float _moveSpeedX;
		float _moveSpeedY;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _BlendColor;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			
			// move the blended thexute offset
			float2 offset = float2(_Time.x * _moveSpeedX, _Time.x * _moveSpeedY);


			// Blend the color of the main texture with the blended texture
			fixed blendAlpha = _BlendAlpha * tex2D(_BlendMask, IN.uv_MainTex).a;
			fixed4 c = (tex2D(_MainTex, IN.uv_MainTex) * _Color + blendAlpha * tex2D(_BlendTex, IN.uv_MainTex + offset)) * _BlendColor;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

		}
		ENDCG
	}
	FallBack "Diffuse"
}
