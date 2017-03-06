sampler s0; 

texture lightMask;
sampler lightSampler = sampler_state
{
	Texture = lightMask;
};

float4 PixelShaderLight(float2  coOrds: TEXCOORD0) : COLOR0
{
	float4 colour = tex2D(s0, coOrds); 

	float4 lightColour = tex2D(lightSampler, coOrds);

	return colour * lightColour;
}

technique Technique1
{
	pass Pass1
	{
		PixelShader = compile ps_2_0 PixelShaderLight();
	}
}