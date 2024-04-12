Shader "Hidden/Custom/CustomEffect"
{
	Properties 
	{
	    _MainTex ("Main Texture", 2D) = "white" {}
	    _Palette("_Palette", 2D) = "white" {}
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline" }
		
		Pass
		{
            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            
			#pragma vertex vert
			#pragma fragment frag
			
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            TEXTURE2D(_Palette);
            SAMPLER(sampler_Palette);

            float _ColorCount;
            float4 _BaseColor;
            
            struct Attributes
            {
                float4 positionOS       : POSITION;
                float2 uv               : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv        : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            
            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = input.uv;
                
                return output;
            }
            
            float4 frag (Varyings i) : SV_Target 
            {
				float4 color = SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);
                //return color;
                float2 uvPalette= (0,0);
                float4 col = (1,1,1,1);
                float dist = 10000000.0;

                for (int i = 0; i < _ColorCount; i++) {
                    uvPalette.x += (1/float(_ColorCount));
	   				float4 c = SAMPLE_TEXTURE2D(_Palette,sampler_Palette, uvPalette);			
	   				float d = distance(color, c);

	   				if (d < dist) {
	   					dist = d;
	   					col = c;
	   				}
	   			}
                return col*_BaseColor;
            }
			ENDHLSL
		}
	} 
	FallBack "Diffuse"
}