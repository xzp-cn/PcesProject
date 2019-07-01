// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.28 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.28;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:0,lgpr:1,limd:0,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:32803,y:32680,varname:node_3138,prsc:2|emission-9023-OUT;n:type:ShaderForge.SFN_Tex2d,id:5095,x:31848,y:32767,ptovrint:False,ptlb:diffuse,ptin:_diffuse,varname:node_5095,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:4976,x:32147,y:32644,varname:node_4976,prsc:2|A-7716-RGB,B-5095-RGB;n:type:ShaderForge.SFN_Color,id:7716,x:31859,y:32585,ptovrint:False,ptlb:diffuse_color,ptin:_diffuse_color,varname:node_7716,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:8518,x:31634,y:33094,ptovrint:False,ptlb:emission_tex,ptin:_emission_tex,varname:node_8518,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Add,id:9023,x:32596,y:32680,varname:node_9023,prsc:2|A-4976-OUT,B-7739-OUT;n:type:ShaderForge.SFN_Time,id:1158,x:31602,y:33386,varname:node_1158,prsc:2;n:type:ShaderForge.SFN_Multiply,id:1752,x:31862,y:33111,varname:node_1752,prsc:2|A-8518-R,B-3489-OUT;n:type:ShaderForge.SFN_Cos,id:8838,x:31919,y:33284,varname:node_8838,prsc:2|IN-8637-OUT;n:type:ShaderForge.SFN_Multiply,id:143,x:32087,y:32984,varname:node_143,prsc:2|A-5095-R,B-1752-OUT;n:type:ShaderForge.SFN_Multiply,id:7739,x:32272,y:32895,varname:node_7739,prsc:2|A-4298-RGB,B-143-OUT;n:type:ShaderForge.SFN_Color,id:4298,x:32087,y:32826,ptovrint:False,ptlb:emission_tex_color,ptin:_emission_tex_color,varname:node_4298,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_ConstantClamp,id:3489,x:32294,y:33268,varname:node_3489,prsc:2,min:0,max:1|IN-384-OUT;n:type:ShaderForge.SFN_Multiply,id:8637,x:31756,y:33319,varname:node_8637,prsc:2|A-3396-OUT,B-1158-T;n:type:ShaderForge.SFN_ValueProperty,id:3396,x:31602,y:33319,ptovrint:False,ptlb:time_value,ptin:_time_value,varname:node_3396,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Abs,id:384,x:32101,y:33278,varname:node_384,prsc:2|IN-8838-OUT;proporder:5095-7716-8518-4298-3396;pass:END;sub:END;*/

Shader "Jan_shader/ModelEmission" {
    Properties {
        _diffuse ("diffuse", 2D) = "white" {}
        _diffuse_color ("diffuse_color", Color) = (1,1,1,1)
        _emission_tex ("emission_tex", 2D) = "bump" {}
        _emission_tex_color ("emission_tex_color", Color) = (1,1,1,1)
        _time_value ("time_value", Float ) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #include "UnityCG.cginc"
            uniform float4 _TimeEditor;
            uniform sampler2D _diffuse; uniform float4 _diffuse_ST;
            uniform float4 _diffuse_color;
            uniform sampler2D _emission_tex; uniform float4 _emission_tex_ST;
            uniform float4 _emission_tex_color;
            uniform float _time_value;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float4 _diffuse_var = tex2D(_diffuse,TRANSFORM_TEX(i.uv0, _diffuse));
                float4 _emission_tex_var = tex2D(_emission_tex,TRANSFORM_TEX(i.uv0, _emission_tex));
                float4 node_1158 = _Time + _TimeEditor;
                float3 emissive = ((_diffuse_color.rgb*_diffuse_var.rgb)+(_emission_tex_color.rgb*(_diffuse_var.r*(_emission_tex_var.r*clamp(abs(cos((_time_value*node_1158.g))),0,1)))));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    //FallBack "Diffuse"
    //CustomEditor "ShaderForgeMaterialInspector"
}
