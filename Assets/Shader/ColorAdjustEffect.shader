// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

//Shader's directory
Shader "Custom/ColorAdjustEffect"
{
	//Attribute block, the attributes used by shader, can be adjusted directly in the Inspector panel
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Brightness("Brightness", Float) = 1
		_Saturation("Saturation", Float) = 1
		_Contrast("Contrast", Float) = 1
	}

		//Each shader has a Subshaer, and there is a parallel relationship between each subshaer. Only one subshader can run, mainly for different hardware
			SubShader
		{
			//The real job is Pass, there may be different passes in a shader, you can execute multiple passes
			Pass
				{
				//Set some rendering states, not explained in detail here
				ZTest Always Cull Off ZWrite Off

				CGPROGRAM
			//The content in Properties is only for the Inspector panel, the real statement here, pay attention to the consistency with the above
			sampler2D _MainTex;
			half _Brightness;
			half _Saturation;
			half _Contrast;

			//vert and frag functions
			#pragma vertex vert
			#pragma fragment frag
			#include "Lighting.cginc"

			//Parameters passed into the pixel shader from the vertex shader
			struct v2f
			{
				float4 pos: SV_POSITION;//Vertex position
				half2 uv: TEXCOORD0;//UV coordinates
			};

			//vertex shader
			//appdata_img: vertex shader input with position and a texture coordinate
			v2f vert(appdata_img v)
			{
				v2f o;
				//Turn from own space to projection space
				o.pos = UnityObjectToClipPos(v.vertex);
				//The uv coordinates are assigned to output
				o.uv = v.texcoord;
				return o;
			}

			//fragment shader
			fixed4 frag(v2f i) : SV_Target
			{
				//Sampling from _MainTex according to uv coordinates
				fixed4 renderTex = tex2D(_MainTex, i.uv);
			//brigtness brightness is directly multiplied by a factor, which is RGB overall scaling, adjust brightness
			fixed3 finalColor = renderTex * _Brightness;
			//saturation saturation: first calculate the lowest saturation value under the same brightness according to the formula:
			fixed gray = 0.2125 * renderTex.r + 0.7154 * renderTex.g + 0.0721 * renderTex.b;
			fixed3 grayColor = fixed3(gray, gray, gray);
			//The difference between the image with the lowest saturation and the original image according to Saturation
			finalColor = lerp(grayColor, finalColor, _Saturation);
			//contrast: first calculate the lowest contrast value
			fixed3 avgColor = fixed3(0.5, 0.5, 0.5);
			//According to Contrast, the difference between the image with the lowest contrast and the original image
			finalColor = lerp(avgColor, finalColor, _Contrast);
			//Return the result, the alpha channel remains unchanged
			return fixed4(finalColor, renderTex.a);
		}
			ENDCG
}
		}
			//Safety measures to prevent shader failure
			FallBack Off
}