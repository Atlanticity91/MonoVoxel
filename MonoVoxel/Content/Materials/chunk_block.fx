#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

/// --- VERTEX ---
matrix WorldTransform;

struct VertexShaderInput {
    float4 Position : POSITION0;
    float4 Metadata : TEXCOORD0;
};

struct VertexShaderOutput {
	float4 Position : SV_POSITION;
    float2 UV : TEXCOORD0;
    int BlockID : TEXCOORD1;
    int FaceID : TEXCOORD2;
    float4 Color : COLOR0;
};

VertexShaderOutput MainVS( in VertexShaderInput input ) {
	VertexShaderOutput output = (VertexShaderOutput)0;
    
    output.Position = mul(float4(input.Position), WorldTransform );
    output.UV       = float2( input.Metadata.x, input.Metadata.y );
    output.BlockID  = (int) input.Metadata.z;
    output.FaceID   = (int) input.Metadata.w;
    output.Color    = float4(1.0, 1.0, 1.0, 1.0);
    
	return output;
}

// --- FRAGMENT ---
Texture2D Diffuse : register(t0);
sampler2D DiffuseSampler = sampler_state { Texture = <Diffuse>; };

float4 MainPS(VertexShaderOutput input) : COLOR {
    return tex2D(DiffuseSampler, input.UV) * input.Color;
}

// --- ENTRY ---
technique BasicColorDrawing {
	pass P0 {
		VertexShader = compile VS_SHADERMODEL MainVS( );
		PixelShader = compile PS_SHADERMODEL MainPS( );
	}
};
