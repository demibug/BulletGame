﻿Shader "Terrain Grid System/Unlit Single Color Territory Gradient" {
 
Properties {
    [HideInInspector] _MainTex ("Texture", 2D) = "black" {}
    _Color ("Color", Color) = (1,1,1,1)
    _SecondColor ("Second Color", Color) = (1,1,1,1)
    _Offset ("Depth Offset", float) = -0.01  
    _NearClip ("Near Clip", Range(0, 1000.0)) = 25.0
    _FallOff ("FallOff", Range(1, 1000.0)) = 50.0
    _FarFadeDistance ("Far Fade Distance", Float) = 10000
    _FarFadeFallOff ("Far Fade FallOff", Float) = 50.0
    _CircularFadeDistance ("Circular Fade Distance", Float) = 250000
    _CircularFadeFallOff ("Circular Fade FallOff", Float) = 50.0
    _Thickness ("Thickness", Float) = 0.05
    _ZWrite("ZWrite", Int) = 0
    _SrcBlend("Src Blend", Int) = 5
    _DstBlend("Dst Blend", Int) = 10
	_StencilComp("Stencil Comp", Int) = 6 // not equal
	_StencilOp("Stencil Op", Int) = 2 // replace
	_StencilRef("Stencil Ref", Int) = 3 // 2 = cells, 4 = territories, 3 = anything else
    _AnimationSpeed("Anumaation Speed", Float) = 0
}

SubShader {
    Tags {
      "Queue"="Geometry+126"
      "RenderType"="Opaque"
      "DisableBatching"="True"
    }
    Blend [_SrcBlend] [_DstBlend]
  	ZWrite [_ZWrite]
  	Cull Off
    Stencil {
        Ref [_StencilRef]
        Comp [_StencilComp]
        Pass [_StencilOp]
    }

    Pass {
	   	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag				
		#pragma target 3.0
		#pragma multi_compile __ TGS_NEAR_CLIP_FADE
		#pragma multi_compile __ TGS_FAR_FADE
		#pragma multi_compile __ TGS_CIRCULAR_FADE
        #pragma multi_compile __ TGS_GRADIENT

		#include "UnityCG.cginc"
		#include "TGSCommon.cginc"

		struct appdata {
			float4 vertex : POSITION;
			half4 color  : COLOR;
            float2 uv     : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};


		struct v2f {
			float4 pos    : SV_POSITION;
			half4 color  : COLOR;
            float2 uv     : TEXCOORD0;
            #if TGS_GRADIENT
                fixed4 secondColor  : TEXCOORD1;
            #endif
			UNITY_VERTEX_OUTPUT_STEREO
		};

        float4 ComputePos(float4 v) {
            float4 vertex = UnityObjectToClipPos(v);
			#if UNITY_REVERSED_Z
				vertex.z -= _Offset;
			#else
				vertex.z += _Offset;
			#endif
            return vertex;
        }

        half4 _SecondColor;
        float _AnimationSpeed;

        v2f vert(appdata v) {
            v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_OUTPUT(v2f, o);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

            o.pos = ComputePos(v.vertex);

		    APPLY_FADE(_Color, v.vertex, o.pos, o.color)

            #if TGS_GRADIENT
               APPLY_FADE(_SecondColor, v.vertex, o.pos, o.secondColor)
           #endif

           o.uv = v.uv;

           return o;
 		}
		
		half4 frag(v2f i) : SV_Target {
            half4 color;
            #if TGS_GRADIENT
                color = lerp(i.color, i.secondColor, frac(saturate(i.uv.y) * 0.999 + _Time.y * _AnimationSpeed));
            #else
                color = i.color;
            #endif
			return color;
		}
		ENDCG
    }
            
 }
 Fallback Off
}
