#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

float region1Angle;
float4 region1;
float4 region2;

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    float2 deltaFromCenter = input.TextureCoordinates - 0.5f;
    float angle = atan2(deltaFromCenter.y, deltaFromCenter.x);

    float4 selectedRegion = lerp(region2, region1, step(region1Angle, angle));
    
    return tex2D(SpriteTextureSampler, input.TextureCoordinates) * float4(selectedRegion.rgb, 1);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};