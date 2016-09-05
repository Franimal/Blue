// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/Ocean Shader" {
	Properties{
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_Alpha("Alpha", Range(0, 1)) = 0.7
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	}
		SubShader{
		Tags{"RenderQueue"="Transparent" "RenderType"="Transparent"}
		LOD 250
		ZWrite On

		CGPROGRAM
		#pragma surface surf MobileBlinnPhong alpha vertex:vert exclude_path:prepass nolightmap noforwardadd halfasview interpolateview nofog
		
		inline fixed4 LightingMobileBlinnPhong(SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
	{
		fixed diff = max(0, dot(s.Normal, lightDir));
		fixed nh = max(0, dot(s.Normal, halfDir));
		fixed spec = pow(nh, s.Specular * 128) * s.Gloss;

		fixed4 c;
		c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * atten;
		c.a = s.Alpha;
		//UNITY_OPAQUE_ALPHA(c.a);
		return c;
	}

	sampler2D _MainTex;
	sampler2D _BumpMap;
	half _Shininess;
	half _Alpha;

	struct Input {
		float2 uv_MainTex;
	};
	
	void vert(inout appdata_full v) {
		
		//Apply horizon effect
		v.vertex.y -= 0.002 * (v.vertex.x + 70) * (v.vertex.x + 70);
		if (v.vertex.x > 0) {
			//v.vertex.y -= 0.004 * v.vertex.x * v.vertex.x;
		}
		v.vertex.y -= 0.0001 * v.vertex.z * v.vertex.z;

		//Apply waves
		float swell = _Time.y + v.vertex.x * 40.0;
		float chop = _Time.y * 0.8 + v.vertex.z * v.vertex.x * 20.0;
		v.vertex.y += sin(swell) * 0.5;
		v.vertex.y += sin(chop) * 0.1;
	}

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = tex.rgb;
		o.Gloss = tex.a;
		o.Alpha = _Alpha;
		o.Specular = _Shininess;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
	}
	ENDCG
	}

		FallBack "Mobile/VertexLit"
}