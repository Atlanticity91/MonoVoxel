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
    float AO : TEXCOORD3;
};

static const float uv_size = 1.0 / 16.0;
static const float ambient_occlusion[ 6 ] = { 
    1.0, 0.5, 
    0.5, 0.8, 
    0.5, 0.8
};

VertexShaderOutput MainVS( in VertexShaderInput input ) {
	VertexShaderOutput output = (VertexShaderOutput)0;
    
    output.Position = mul(float4(input.Position), WorldTransform );
    output.UV       = float2( input.Metadata.x, input.Metadata.y );
    output.BlockID  = (int) input.Metadata.z;
    output.FaceID   = (int) input.Metadata.w;
    output.AO       = ambient_occlusion[ output.FaceID ];
    
	return output;
}

// --- FRAGMENT ---
Texture2D Texture;
sampler2D TextureSampler = sampler_state {
    Texture = <Texture>;
    MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
    ADDRESSU = Wrap;
    ADDRESSV = Wrap;
};

float4 MainPS(VertexShaderOutput input) : COLOR {
    float3 color = tex2D( TextureSampler, input.UV ).rgb;
    
    return float4( color, input.AO );
}

// --- ENTRY ---
technique BasicColorDrawing {
	pass P0 {
		VertexShader = compile VS_SHADERMODEL MainVS( );
		PixelShader = compile PS_SHADERMODEL MainPS( );
	}
};
