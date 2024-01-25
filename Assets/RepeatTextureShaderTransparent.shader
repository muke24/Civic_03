Shader "Custom/RepeatTextureShaderTransparent" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _RepeatX ("Repeat X", Float) = 1.0
        _RepeatY ("Repeat Y", Float) = 1.0
    }
    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        AlphaTest Greater 0.5
        Cull Off

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target to enable more modern features
        #pragma target 3.0

        sampler2D _MainTex;
        float _RepeatX;
        float _RepeatY;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Repeat the texture coordinates
            float2 repeatedUV = float2(fmod(IN.uv_MainTex.x * _RepeatX, 1.0),
                                       fmod(IN.uv_MainTex.y * _RepeatY, 1.0));
            
            // Sample the texture with repeated UVs
            fixed4 c = tex2D(_MainTex, repeatedUV);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Transparent/Diffuse"
}
