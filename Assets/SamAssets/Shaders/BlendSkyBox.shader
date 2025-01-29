Shader "Custom/ProceduralSkyWithStars"
{
    Properties
    {
        _SkyColor ("Sky Color", Color) = (0.5,0.5,1,1)
        _HorizonColor ("Horizon Color", Color) = (1,0.5,0,1)
        _StarTexture ("Star Texture", 2D) = "white" {}
        _BlendFactor ("Blend Factor", Range(0, 1)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            sampler2D _StarTexture;
            float4 _SkyColor;
            float4 _HorizonColor;
            float _BlendFactor;

            struct appdata { float4 vertex : POSITION; };
            struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy * 0.5 + 0.5; // Simple UV
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float gradient = i.uv.y;
                float4 sky = lerp(_HorizonColor, _SkyColor, gradient);
                float4 stars = tex2D(_StarTexture, i.uv) * _BlendFactor;
                return sky + stars;
            }
            ENDCG
        }
    }
}
