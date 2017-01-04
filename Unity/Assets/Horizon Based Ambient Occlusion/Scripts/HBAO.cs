using UnityEngine;
using System;

[ExecuteInEditMode, AddComponentMenu("Image Effects/HBAO")]
[RequireComponent(typeof(Camera))]
public class HBAO : MonoBehaviour
{
    public enum Preset
    {
        FastestPerformance,
        FastPerformance,
        Normal,
        HighQuality,
        HighestQuality,
        Custom
    }

    public enum Quality
    {
        Lowest,
        Low,
        Medium,
        High,
        Highest
    }

    public enum Resolution
    {
        Full,
        Half,
        Quarter
    }

    public enum Deinterleaving
    {
        Disabled,
        _2x,
        _4x
    }

    public enum DisplayMode
    {
        Normal,
        AOOnly,
        ColorBleedingOnly,
        SplitWithoutAOAndWithAO,
        SplitWithAOAndAOOnly,
        SplitWithoutAOAndAOOnly
    }

    public enum Blur
    {
        None,
        Narrow,
        Medium,
        Wide,
        ExtraWide
    }

    public enum NoiseType
    {
        Random,
        Dither
    }

    public enum PerPixelNormals
    {
        GBuffer,
        Camera,
        Reconstruct
    }

    public Texture2D noiseTex;
    public Mesh quadMesh;
    public Shader hbaoShader = null;

    [Serializable]
    public struct Presets
    {
        public Preset preset;

        [SerializeField]
        public static Presets defaultPresets
        {
            get
            {
                return new Presets
                {
                    preset = Preset.Normal
                };
            }
        }
    }

    [Serializable]
    public struct GeneralSettings
    {
        [Tooltip("The quality of the AO.")]
        [Space(6)]
        public Quality quality;

        [Tooltip("The deinterleaving factor.")]
        public Deinterleaving deinterleaving;

        [Tooltip("The resolution at which the AO is calculated.")]
        public Resolution resolution;

        [Tooltip("The type of noise to use.")]
        [Space(10)]
        public NoiseType noiseType;

        [Tooltip("The way the AO is displayed on screen.")]
        [Space(10)]
        public DisplayMode displayMode;

        [SerializeField]
        public static GeneralSettings defaultSettings
        {
            get
            {
                return new GeneralSettings
                {
                    quality = Quality.Medium,
                    deinterleaving = Deinterleaving.Disabled,
                    resolution = Resolution.Full,
                    noiseType = NoiseType.Dither,
                    displayMode = DisplayMode.Normal
                };
            }
        }
    }

    [Serializable]
    public struct AOSettings
    {
        [Tooltip("AO radius: this is the distance outside which occluders are ignored.")]
        [Space(6), Range(0, 2)]
        public float radius;

        [Tooltip("Maximum radius in pixels: this prevents the radius to grow too much with close-up " +
                  "object and impact on performances.")]
        [Range(32, 256)]
        public float maxRadiusPixels;

        [Tooltip("For low-tessellated geometry, occlusion variations tend to appear at creases and " +
                 "ridges, which betray the underlying tessellation. To remove these artifacts, we use " +
                 "an angle bias parameter which restricts the hemisphere.")]
        [Range(0, 0.5f)]
        public float bias;

        [Tooltip("This value allows to scale up the ambient occlusion values.")]
        [Range(0, 10)]
        public float intensity;

        [Tooltip("This value allows to attenuate ambient occlusion depending on final color luminance.")]
        [Range(0, 1)]
        public float luminanceInfluence;

        [Tooltip("The max distance to display AO.")]
        public float maxDistance;

        [Tooltip("The distance before max distance at which AO start to decrease.")]
        public float distanceFalloff;

        [Tooltip("The type of per pixel normals to use.")]
        [Space(10)]
        public PerPixelNormals perPixelNormals;

        [Tooltip("This setting allow you to set the base color if the AO, the alpha channel value is unused.")]
        [Space(10)]
        public Color baseColor;

        [SerializeField]
        public static AOSettings defaultSettings
        {
            get
            {
                return new AOSettings
                {
                    radius = 0.8f,
                    maxRadiusPixels = 128f,
                    bias = 0.05f,
                    intensity = 1f,
                    luminanceInfluence = 0f,
                    maxDistance = 150f,
                    distanceFalloff = 50f,
                    perPixelNormals = PerPixelNormals.GBuffer,
                    baseColor = Color.black
                };
            }
        }
    }

    [Serializable]
    public struct ColorBleedingSettings
    {
        [Space(6)]
        public bool enabled;

        [Tooltip("This value allows to control the saturation of the color bleeding.")]
        [Space(10), Range(0, 4)]
        public float saturation;

        [Tooltip("This value allows to scale the contribution of the color bleeding samples.")]
        [Range(0, 32)]
        public float albedoMultiplier;

        [SerializeField]
        public static ColorBleedingSettings defaultSettings
        {
            get
            {
                return new ColorBleedingSettings
                {
                    enabled = false,
                    saturation = 1f,
                    albedoMultiplier = 4f
                };
            }
        }
    }

    [Serializable]
    public struct BlurSettings
    {
        [Tooltip("The type of blur to use.")]
        [Space(6)]
        public Blur amount;

        [Tooltip("This parameter controls the depth-dependent weight of the bilateral filter, to " +
                 "avoid bleeding across edges. A zero sharpness is a pure Gaussian blur. Increasing " +
                 "the blur sharpness removes bleeding by using lower weights for samples with large " +
                 "depth delta from the current pixel.")]
        [Space(10), Range(0, 16)]
        public float sharpness;

        [Tooltip("Is the blur downsampled.")]
        public bool downsample;

        [SerializeField]
        public static BlurSettings defaultSettings
        {
            get
            {
                return new BlurSettings
                {
                    amount = Blur.Medium,
                    sharpness = 8f,
                    downsample = false
                };
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class SettingsGroup : Attribute { }

    [SerializeField, SettingsGroup]
    private Presets m_Presets = Presets.defaultPresets;
    public Presets presets
    {
        get { return m_Presets; }
        set { m_Presets = value; }
    }

    [SerializeField, SettingsGroup]
    private GeneralSettings m_GeneralSettings = GeneralSettings.defaultSettings;
    public GeneralSettings generalSettings
    {
        get { return m_GeneralSettings; }
        set { m_GeneralSettings = value; }
    }

    [SerializeField, SettingsGroup]
    private AOSettings m_AOSettings = AOSettings.defaultSettings;
    public AOSettings aoSettings
    {
        get { return m_AOSettings; }
        set { m_AOSettings = value; }
    }

    [SerializeField, SettingsGroup]
    private ColorBleedingSettings m_ColorBleedingSettings = ColorBleedingSettings.defaultSettings;
    public ColorBleedingSettings colorBleedingSettings
    {
        get { return m_ColorBleedingSettings; }
        set { m_ColorBleedingSettings = value; }
    }

    [SerializeField, SettingsGroup]
    private BlurSettings m_BlurSettings = BlurSettings.defaultSettings;
    public BlurSettings blurSettings
    {
        get { return m_BlurSettings; }
        set { m_BlurSettings = value; }
    }

    private static class MersenneTwister
    {
        // Mersenne-Twister random numbers in [0,1).
        public static float[] Numbers = new float[] {
            0.463937f,0.340042f,0.223035f,0.468465f,0.322224f,0.979269f,0.031798f,0.973392f,0.778313f,0.456168f,0.258593f,0.330083f,0.387332f,0.380117f,0.179842f,0.910755f,
            0.511623f,0.092933f,0.180794f,0.620153f,0.101348f,0.556342f,0.642479f,0.442008f,0.215115f,0.475218f,0.157357f,0.568868f,0.501241f,0.629229f,0.699218f,0.707733f
        };
    }

    private static class Pass
    {
        public const int AO_LowestQuality = 0;
        public const int AO_LowQuality = 1;
        public const int AO_MediumQuality = 2;
        public const int AO_HighQuality = 3;
        public const int AO_HighestQuality = 4;
        public const int AO_Deinterleaved_LowestQuality = 5;
        public const int AO_Deinterleaved_LowQuality = 6;
        public const int AO_Deinterleaved_MediumQuality = 7;
        public const int AO_Deinterleaved_HighQuality = 8;
        public const int AO_Deinterleaved_HighestQuality = 9;

        public const int Depth_Deinterleaving_2x2 = 10;
        public const int Depth_Deinterleaving_4x4 = 11;
        public const int Normals_Deinterleaving_2x2 = 12;
        public const int Normals_Deinterleaving_4x4 = 13;

        public const int Atlas = 14;

        public const int Reinterleaving_2x2 = 15;
        public const int Reinterleaving_4x4 = 16;

        public const int Blur_X_Narrow = 17;
        public const int Blur_X_Medium = 18;
        public const int Blur_X_Wide = 19;
        public const int Blur_X_ExtraWide = 20;
        public const int Blur_Y_Narrow = 21;
        public const int Blur_Y_Medium = 22;
        public const int Blur_Y_Wide = 23;
        public const int Blur_Y_ExtraWide = 24;

        public const int Composite = 25;
        public const int Debug_AO_Only = 26;
        public const int Debug_ColorBleeding_Only = 27;
        public const int Debug_Split_WithoutAO_WithAO = 28;
        public const int Debug_Split_WithAO_AOOnly = 29;
        public const int Debug_Split_WithoutAO_AOOnly = 30;
    }

    private class RenderTarget
    {
        public bool orthographic;
        public bool hdr;
        public int width;
        public int height;
        public int fullWidth;
        public int fullHeight;
        public int layerWidth;
        public int layerHeight;
        public int downsamplingFactor;
        public int deinterleavingFactor;
        public int blurDownsamplingFactor;
    }

    private Quality _quality;
    private NoiseType _noiseType;
    private RenderTarget _renderTarget;
    private const int NUM_MRTS = 4;
    private RenderTexture[] _mrtTexDepth = new RenderTexture[4 * NUM_MRTS];
    private RenderTexture[] _mrtTexNrm = new RenderTexture[4 * NUM_MRTS];
    private RenderTexture[] _mrtTexAO = new RenderTexture[4 * NUM_MRTS];
    private RenderBuffer[][] _mrtRB = new RenderBuffer[NUM_MRTS][] { new RenderBuffer[4], new RenderBuffer[4], new RenderBuffer[4], new RenderBuffer[4] };
    private RenderBuffer[][] _mrtRBNrm = new RenderBuffer[NUM_MRTS][] { new RenderBuffer[4], new RenderBuffer[4], new RenderBuffer[4], new RenderBuffer[4] };
    private Vector4[] _jitter = new Vector4[4 * NUM_MRTS];

    private string[] _hbaoShaderKeywords = new string[3];
    private Material _hbaoMaterial;
    private Camera _hbaoCamera;
    private int[] _numSampleDirections = new int[] { 3, 4, 6, 8, 8 }; // LOWEST, LOW, MEDIUM, HIGH, HIGHEST (highest uses more steps)

    void OnEnable()
    {
        if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures || !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
        {
            Debug.LogWarning("HBAO shader is not supported on this platform.");
            this.enabled = false;
            return;
        }

        if (hbaoShader != null && !hbaoShader.isSupported)
        {
            Debug.LogWarning("HBAO shader is not supported on this platform.");
            this.enabled = false;
            return;
        }

        if (hbaoShader == null)
        {
            return;
        }

        CreateMaterial();

        _hbaoCamera.depthTextureMode |= DepthTextureMode.Depth;
        if (aoSettings.perPixelNormals == PerPixelNormals.Camera)
            _hbaoCamera.depthTextureMode |= DepthTextureMode.DepthNormals;
    }

    void OnDisable()
    {
        if (_hbaoMaterial != null)
            DestroyImmediate(_hbaoMaterial);
        if (noiseTex != null)
            DestroyImmediate(noiseTex);
        if (quadMesh != null)
            DestroyImmediate(quadMesh);
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (hbaoShader == null || _hbaoCamera == null)
        {
            Graphics.Blit(source, destination);
            return;
        }

        _hbaoCamera.depthTextureMode |= DepthTextureMode.Depth;
        if (aoSettings.perPixelNormals == PerPixelNormals.Camera)
            _hbaoCamera.depthTextureMode |= DepthTextureMode.DepthNormals;

        CheckParameters();
        UpdateShaderProperties();
        UpdateShaderKeywords();

        if (generalSettings.deinterleaving == Deinterleaving._2x)
            RenderHBAODeinterleaved2x(source, destination);
        else if (generalSettings.deinterleaving == Deinterleaving._4x)
            RenderHBAODeinterleaved4x(source, destination);
        else
            RenderHBAO(source, destination);
    }

    private void RenderHBAO(RenderTexture source, RenderTexture destination)
    {
        RenderTexture rt = RenderTexture.GetTemporary(_renderTarget.fullWidth / _renderTarget.downsamplingFactor, _renderTarget.fullHeight / _renderTarget.downsamplingFactor);

        // AO rt clear
        RenderTexture lastActive = RenderTexture.active;
        RenderTexture.active = rt;
        GL.Clear(false, true, Color.white);
        RenderTexture.active = lastActive;

        Graphics.Blit(source, rt, _hbaoMaterial, GetAoPass()); // hbao
        if (blurSettings.amount != Blur.None)
        {
            RenderTexture rtBlur = RenderTexture.GetTemporary((_renderTarget.fullWidth / _renderTarget.downsamplingFactor) / _renderTarget.blurDownsamplingFactor,
                                                              (_renderTarget.fullHeight / _renderTarget.downsamplingFactor) / _renderTarget.blurDownsamplingFactor);
            Graphics.Blit(rt, rtBlur, _hbaoMaterial, GetBlurXPass()); // blur X
            rt.DiscardContents();
            Graphics.Blit(rtBlur, rt, _hbaoMaterial, GetBlurYPass()); // blur Y
            RenderTexture.ReleaseTemporary(rtBlur);
        }
        _hbaoMaterial.SetTexture("_HBAOTex", rt);
        Graphics.Blit(source, destination, _hbaoMaterial, GetFinalPass()); // final pass
        RenderTexture.ReleaseTemporary(rt);
    }

    private void RenderHBAODeinterleaved2x(RenderTexture source, RenderTexture destination)
    {
        RenderTexture lastActive = RenderTexture.active;

        // initialize render textures & buffers
        for (int i = 0; i < NUM_MRTS; i++)
        {
            _mrtTexDepth[i] = RenderTexture.GetTemporary(_renderTarget.layerWidth, _renderTarget.layerHeight, 0, RenderTextureFormat.RFloat);
            _mrtTexNrm[i] = RenderTexture.GetTemporary(_renderTarget.layerWidth, _renderTarget.layerHeight, 0, RenderTextureFormat.ARGB2101010);
            _mrtTexAO[i] = RenderTexture.GetTemporary(_renderTarget.layerWidth, _renderTarget.layerHeight);
            _mrtRB[0][i] = _mrtTexDepth[i].colorBuffer;
            _mrtRBNrm[0][i] = _mrtTexNrm[i].colorBuffer;
            // AO rt clear
            RenderTexture.active = _mrtTexAO[i];
            GL.Clear(false, true, Color.white);
        }

        // deinterleave depth & normals 2x2
        _hbaoMaterial.SetVector("_Deinterleaving_Offset00", new Vector2(0, 0));
        _hbaoMaterial.SetVector("_Deinterleaving_Offset10", new Vector2(1, 0));
        _hbaoMaterial.SetVector("_Deinterleaving_Offset01", new Vector2(0, 1));
        _hbaoMaterial.SetVector("_Deinterleaving_Offset11", new Vector2(1, 1));
        Graphics.SetRenderTarget(_mrtRB[0], _mrtTexDepth[0].depthBuffer);
        _hbaoMaterial.SetPass(Pass.Depth_Deinterleaving_2x2);
        Graphics.DrawMeshNow(quadMesh, Matrix4x4.identity); // outputs 4 render textures
        Graphics.SetRenderTarget(_mrtRBNrm[0], _mrtTexNrm[0].depthBuffer);
        _hbaoMaterial.SetPass(Pass.Normals_Deinterleaving_2x2);
        Graphics.DrawMeshNow(quadMesh, Matrix4x4.identity); // outputs 4 render textures

        RenderTexture.active = lastActive;

        // calculate AO on each layer
        for (int i = 0; i < NUM_MRTS; i++)
        {
            _hbaoMaterial.SetTexture("_DepthTex", _mrtTexDepth[i]);
            _hbaoMaterial.SetTexture("_NormalsTex", _mrtTexNrm[i]);
            _hbaoMaterial.SetVector("_Jitter", _jitter[i]);
            Graphics.Blit(source, _mrtTexAO[i], _hbaoMaterial, GetAoDeinterleavedPass());
            _mrtTexDepth[i].DiscardContents();
            _mrtTexNrm[i].DiscardContents();
        }

        // build atlas
        RenderTexture rt1 = RenderTexture.GetTemporary(_renderTarget.fullWidth, _renderTarget.fullHeight);
        for (int i = 0; i < NUM_MRTS; i++)
        {
            _hbaoMaterial.SetVector("_LayerOffset", new Vector2((i & 1) * _renderTarget.layerWidth, (i >> 1) * _renderTarget.layerHeight));
            Graphics.Blit(_mrtTexAO[i], rt1, _hbaoMaterial, Pass.Atlas);
            RenderTexture.ReleaseTemporary(_mrtTexAO[i]);
            RenderTexture.ReleaseTemporary(_mrtTexNrm[i]);
            RenderTexture.ReleaseTemporary(_mrtTexDepth[i]);
        }

        // reinterleave
        RenderTexture rt2 = RenderTexture.GetTemporary(_renderTarget.fullWidth, _renderTarget.fullHeight);
        Graphics.Blit(rt1, rt2, _hbaoMaterial, Pass.Reinterleaving_2x2);
        rt1.DiscardContents();

        if (blurSettings.amount != Blur.None)
        {
            if (blurSettings.downsample)
            {
                RenderTexture rtBlur = RenderTexture.GetTemporary(_renderTarget.fullWidth / _renderTarget.blurDownsamplingFactor, _renderTarget.fullHeight / _renderTarget.blurDownsamplingFactor);
                Graphics.Blit(rt2, rtBlur, _hbaoMaterial, GetBlurXPass()); // blur X
                rt2.DiscardContents();
                Graphics.Blit(rtBlur, rt2, _hbaoMaterial, GetBlurYPass()); // blur Y
                RenderTexture.ReleaseTemporary(rtBlur);
            }
            else
            {
                Graphics.Blit(rt2, rt1, _hbaoMaterial, GetBlurXPass()); // blur X
                rt2.DiscardContents();
                Graphics.Blit(rt1, rt2, _hbaoMaterial, GetBlurYPass()); // blur Y
            }
        }

        RenderTexture.ReleaseTemporary(rt1);

        _hbaoMaterial.SetTexture("_HBAOTex", rt2);
        Graphics.Blit(source, destination, _hbaoMaterial, GetFinalPass()); // final pass

        RenderTexture.ReleaseTemporary(rt2);
    }

    private void RenderHBAODeinterleaved4x(RenderTexture source, RenderTexture destination)
    {
        RenderTexture lastActive = RenderTexture.active;

        // initialize render textures & buffers
        for (int i = 0; i < 4 * NUM_MRTS; i++)
        {
            _mrtTexDepth[i] = RenderTexture.GetTemporary(_renderTarget.layerWidth, _renderTarget.layerHeight, 0, RenderTextureFormat.RFloat);
            _mrtTexNrm[i] = RenderTexture.GetTemporary(_renderTarget.layerWidth, _renderTarget.layerHeight, 0, RenderTextureFormat.ARGB2101010);
            _mrtTexAO[i] = RenderTexture.GetTemporary(_renderTarget.layerWidth, _renderTarget.layerHeight);
            // AO rt clear
            RenderTexture.active = _mrtTexAO[i];
            GL.Clear(false, true, Color.white);
        }
        for (int i = 0; i < NUM_MRTS; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                _mrtRB[i][j] = _mrtTexDepth[j + NUM_MRTS * i].colorBuffer;
                _mrtRBNrm[i][j] = _mrtTexNrm[j + NUM_MRTS * i].colorBuffer;
            }
        }

        // deinterleave depth & normals 4x4
        for (int i = 0; i < NUM_MRTS; i++)
        {
            int offsetX = (i & 1) << 1;
            int offsetY = (i >> 1) << 1;
            _hbaoMaterial.SetVector("_Deinterleaving_Offset00", new Vector2(offsetX + 0, offsetY + 0));
            _hbaoMaterial.SetVector("_Deinterleaving_Offset10", new Vector2(offsetX + 1, offsetY + 0));
            _hbaoMaterial.SetVector("_Deinterleaving_Offset01", new Vector2(offsetX + 0, offsetY + 1));
            _hbaoMaterial.SetVector("_Deinterleaving_Offset11", new Vector2(offsetX + 1, offsetY + 1));
            Graphics.SetRenderTarget(_mrtRB[i], _mrtTexDepth[NUM_MRTS * i].depthBuffer);
            _hbaoMaterial.SetPass(Pass.Depth_Deinterleaving_4x4);
            Graphics.DrawMeshNow(quadMesh, Matrix4x4.identity); // outputs 4 render textures
            Graphics.SetRenderTarget(_mrtRBNrm[i], _mrtTexNrm[NUM_MRTS * i].depthBuffer);
            _hbaoMaterial.SetPass(Pass.Normals_Deinterleaving_4x4);
            Graphics.DrawMeshNow(quadMesh, Matrix4x4.identity); // outputs 4 render textures
        }

        RenderTexture.active = lastActive;

        // calculate AO on each layer
        for (int i = 0; i < 4 * NUM_MRTS; i++)
        {
            _hbaoMaterial.SetTexture("_DepthTex", _mrtTexDepth[i]);
            _hbaoMaterial.SetTexture("_NormalsTex", _mrtTexNrm[i]);
            _hbaoMaterial.SetVector("_Jitter", _jitter[i]);
            Graphics.Blit(source, _mrtTexAO[i], _hbaoMaterial, GetAoDeinterleavedPass());
            _mrtTexDepth[i].DiscardContents();
            _mrtTexNrm[i].DiscardContents();
        }

        // build atlas
        RenderTexture rt1 = RenderTexture.GetTemporary(_renderTarget.fullWidth, _renderTarget.fullHeight);
        for (int i = 0; i < 4 * NUM_MRTS; i++)
        {
            _hbaoMaterial.SetVector("_LayerOffset", new Vector2(((i & 1) + (((i & 7) >> 2) << 1)) * _renderTarget.layerWidth, (((i & 3) >> 1) + ((i >> 3) << 1)) * _renderTarget.layerHeight));
            Graphics.Blit(_mrtTexAO[i], rt1, _hbaoMaterial, Pass.Atlas);
            RenderTexture.ReleaseTemporary(_mrtTexAO[i]);
            RenderTexture.ReleaseTemporary(_mrtTexNrm[i]);
            RenderTexture.ReleaseTemporary(_mrtTexDepth[i]);
        }

        // reinterleave
        RenderTexture rt2 = RenderTexture.GetTemporary(_renderTarget.fullWidth, _renderTarget.fullHeight);
        Graphics.Blit(rt1, rt2, _hbaoMaterial, Pass.Reinterleaving_4x4);
        rt1.DiscardContents();

        if (blurSettings.amount != Blur.None)
        {
            if (blurSettings.downsample)
            {
                RenderTexture rtBlur = RenderTexture.GetTemporary(_renderTarget.fullWidth / _renderTarget.blurDownsamplingFactor, _renderTarget.fullHeight / _renderTarget.blurDownsamplingFactor);
                Graphics.Blit(rt2, rtBlur, _hbaoMaterial, GetBlurXPass()); // blur X
                rt2.DiscardContents();
                Graphics.Blit(rtBlur, rt2, _hbaoMaterial, GetBlurYPass()); // blur Y
                RenderTexture.ReleaseTemporary(rtBlur);
            }
            else
            {
                Graphics.Blit(rt2, rt1, _hbaoMaterial, GetBlurXPass()); // blur X
                rt2.DiscardContents();
                Graphics.Blit(rt1, rt2, _hbaoMaterial, GetBlurYPass()); // blur Y
            }
        }

        RenderTexture.ReleaseTemporary(rt1);

        _hbaoMaterial.SetTexture("_HBAOTex", rt2);
        Graphics.Blit(source, destination, _hbaoMaterial, GetFinalPass()); // final pass

        RenderTexture.ReleaseTemporary(rt2);
    }

    private void CreateMaterial()
    {
        if (_hbaoMaterial == null)
        {
            _hbaoMaterial = new Material(hbaoShader);
            _hbaoMaterial.hideFlags = HideFlags.HideAndDontSave;

            _hbaoCamera = GetComponent<Camera>();
        }

        if (quadMesh != null)
            DestroyImmediate(quadMesh);

        quadMesh = new Mesh();
        quadMesh.vertices = new Vector3[] {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3( 0.5f,  0.5f, 0),
            new Vector3( 0.5f, -0.5f, 0),
            new Vector3(-0.5f,  0.5f, 0)
        };
        quadMesh.uv = new Vector2[] {
            new Vector2(0, 0),
            new Vector2(1, 1),
            new Vector2(1, 0),
            new Vector2(0, 1)
        };
        quadMesh.triangles = new int[] { 0, 1, 2, 1, 0, 3 };

        _renderTarget = new RenderTarget();
    }

    private void UpdateShaderProperties()
    {
        _renderTarget.orthographic = _hbaoCamera.orthographic;
        _renderTarget.hdr = _hbaoCamera.hdr;
        _renderTarget.width = _hbaoCamera.pixelWidth;
        _renderTarget.height = _hbaoCamera.pixelHeight;
        _renderTarget.downsamplingFactor = generalSettings.resolution == Resolution.Full ? 1 : generalSettings.resolution == Resolution.Half ? 2 : 4;
        _renderTarget.deinterleavingFactor = GetDeinterleavingFactor();
        _renderTarget.blurDownsamplingFactor = blurSettings.downsample ? 2 : 1;

        float tanHalfFovY = Mathf.Tan(0.5f * _hbaoCamera.fieldOfView * Mathf.Deg2Rad);
        float invFocalLenX = 1.0f / (1.0f / tanHalfFovY * (_renderTarget.height / (float)_renderTarget.width));
        float invFocalLenY = 1.0f / (1.0f / tanHalfFovY);
        _hbaoMaterial.SetVector("_UVToView", new Vector4(2.0f * invFocalLenX, -2.0f * invFocalLenY, -1.0f * invFocalLenX, 1.0f * invFocalLenY));
        _hbaoMaterial.SetMatrix("_WorldToCameraMatrix", _hbaoCamera.worldToCameraMatrix);

        if (generalSettings.deinterleaving != Deinterleaving.Disabled)
        {
            _renderTarget.fullWidth = _renderTarget.width + (_renderTarget.width % _renderTarget.deinterleavingFactor == 0 ? 0 : _renderTarget.deinterleavingFactor - (_renderTarget.width % _renderTarget.deinterleavingFactor));
            _renderTarget.fullHeight = _renderTarget.height + (_renderTarget.height % _renderTarget.deinterleavingFactor == 0 ? 0 : _renderTarget.deinterleavingFactor - (_renderTarget.height % _renderTarget.deinterleavingFactor));
            _renderTarget.layerWidth = _renderTarget.fullWidth / _renderTarget.deinterleavingFactor;
            _renderTarget.layerHeight = _renderTarget.fullHeight / _renderTarget.deinterleavingFactor;

            _hbaoMaterial.SetVector("_FullRes_TexelSize", new Vector4(1.0f / _renderTarget.fullWidth, 1.0f / _renderTarget.fullHeight, _renderTarget.fullWidth, _renderTarget.fullHeight));
            _hbaoMaterial.SetVector("_LayerRes_TexelSize", new Vector4(1.0f / _renderTarget.layerWidth, 1.0f / _renderTarget.layerHeight, _renderTarget.layerWidth, _renderTarget.layerHeight));
            _hbaoMaterial.SetVector("_TargetScale", new Vector4(_renderTarget.fullWidth / (float)_renderTarget.width, _renderTarget.fullHeight / (float)_renderTarget.height, 1.0f / (_renderTarget.fullWidth / (float)_renderTarget.width), 1.0f / (_renderTarget.fullHeight / (float)_renderTarget.height)));
        }
        else
        {
            _renderTarget.fullWidth = _renderTarget.width;
            _renderTarget.fullHeight = _renderTarget.height;
            if (generalSettings.resolution == Resolution.Half && aoSettings.perPixelNormals == PerPixelNormals.Reconstruct)
                _hbaoMaterial.SetVector("_TargetScale", new Vector4((_renderTarget.width + 0.5f) / _renderTarget.width, (_renderTarget.height + 0.5f) / _renderTarget.height, 1f, 1f));
            else
                _hbaoMaterial.SetVector("_TargetScale", new Vector4(1f, 1f, 1f, 1f));
        }

        if (noiseTex == null || _quality != generalSettings.quality || _noiseType != generalSettings.noiseType)
        {
            if (noiseTex != null)
                DestroyImmediate(noiseTex);

            float noiseTexSize = generalSettings.noiseType == NoiseType.Dither ? 4 : 64;
            CreateRandomTexture((int)noiseTexSize);
        }

        _quality = generalSettings.quality;
        _noiseType = generalSettings.noiseType;

        _hbaoMaterial.SetTexture("_NoiseTex", noiseTex);
        _hbaoMaterial.SetFloat("_NoiseTexSize", _noiseType == NoiseType.Dither ? 4 : 64);
        _hbaoMaterial.SetFloat("_Radius", aoSettings.radius * 0.5f * (_renderTarget.height / (tanHalfFovY * 2.0f)) / _renderTarget.deinterleavingFactor);
        _hbaoMaterial.SetFloat("_MaxRadiusPixels", aoSettings.maxRadiusPixels / _renderTarget.deinterleavingFactor);
        _hbaoMaterial.SetFloat("_NegInvRadius2", -1.0f / (aoSettings.radius * aoSettings.radius));
        _hbaoMaterial.SetFloat("_AngleBias", aoSettings.bias);
        _hbaoMaterial.SetFloat("_AOmultiplier", 2.0f * (1.0f / (1.0f - aoSettings.bias)));
        _hbaoMaterial.SetFloat("_Intensity", aoSettings.intensity);
        _hbaoMaterial.SetFloat("_LuminanceInfluence", aoSettings.luminanceInfluence);
        _hbaoMaterial.SetFloat("_MaxDistance", aoSettings.maxDistance);
        _hbaoMaterial.SetFloat("_DistanceFalloff", aoSettings.distanceFalloff);
        _hbaoMaterial.SetColor("_BaseColor", aoSettings.baseColor);
        _hbaoMaterial.SetFloat("_ColorBleedSaturation", colorBleedingSettings.saturation);
        _hbaoMaterial.SetFloat("_AlbedoMultiplier", colorBleedingSettings.albedoMultiplier);
        _hbaoMaterial.SetFloat("_BlurSharpness", blurSettings.sharpness);
    }

    private void UpdateShaderKeywords()
    {
        _hbaoShaderKeywords[0] = colorBleedingSettings.enabled? "COLOR_BLEEDING_ON" : "__";

        if (_renderTarget.orthographic)
            _hbaoShaderKeywords[1] = "ORTHOGRAPHIC_PROJECTION_ON";
        else
            _hbaoShaderKeywords[1] = IsDeferredShading() ? "DEFERRED_SHADING_ON" : "__";

        _hbaoShaderKeywords[2] = aoSettings.perPixelNormals == PerPixelNormals.Camera? "NORMALS_CAMERA" : aoSettings.perPixelNormals == PerPixelNormals.Reconstruct? "NORMALS_RECONSTRUCT" : "__";

        _hbaoMaterial.shaderKeywords = _hbaoShaderKeywords;
    }

    private void CheckParameters()
    {
        if (!IsDeferredShading() && aoSettings.perPixelNormals == PerPixelNormals.GBuffer)
            m_AOSettings.perPixelNormals = PerPixelNormals.Camera;

        if (generalSettings.deinterleaving != Deinterleaving.Disabled && SystemInfo.supportedRenderTargetCount < 4)
            m_GeneralSettings.deinterleaving = Deinterleaving.Disabled;
    }

    private bool IsDeferredShading()
    {
        return _hbaoCamera.actualRenderingPath == RenderingPath.DeferredShading;
    }

    private int GetDeinterleavingFactor()
    {
        switch (generalSettings.deinterleaving)
        {
            case Deinterleaving._2x:
                return 2;
            case Deinterleaving._4x:
                return 4;
            case Deinterleaving.Disabled:
            default:
                return 1;
        }
    }

    private int GetAoPass()
    {
        switch (generalSettings.quality)
        {
            case Quality.Lowest:
                return Pass.AO_LowestQuality;
            case Quality.Low:
                return Pass.AO_LowQuality;
            case Quality.Medium:
                return Pass.AO_MediumQuality;
            case Quality.High:
                return Pass.AO_HighQuality;
            case Quality.Highest:
                return Pass.AO_HighestQuality;
            default:
                return Pass.AO_MediumQuality;
        }
    }

    private int GetAoDeinterleavedPass()
    {
        switch (generalSettings.quality)
        {
            case Quality.Lowest:
                return Pass.AO_Deinterleaved_LowestQuality;
            case Quality.Low:
                return Pass.AO_Deinterleaved_LowQuality;
            case Quality.Medium:
                return Pass.AO_Deinterleaved_MediumQuality;
            case Quality.High:
                return Pass.AO_Deinterleaved_HighQuality;
            case Quality.Highest:
                return Pass.AO_Deinterleaved_HighestQuality;
            default:
                return Pass.AO_Deinterleaved_MediumQuality;
        }
    }

    private int GetBlurXPass()
    {
        switch (blurSettings.amount)
        {
            case Blur.Narrow:
                return Pass.Blur_X_Narrow;
            case Blur.Medium:
                return Pass.Blur_X_Medium;
            case Blur.Wide:
                return Pass.Blur_X_Wide;
            case Blur.ExtraWide:
                return Pass.Blur_X_ExtraWide;
            default:
                return Pass.Blur_X_Medium;
        }
    }

    private int GetBlurYPass()
    {
        switch (blurSettings.amount)
        {
            case Blur.Narrow:
                return Pass.Blur_Y_Narrow;
            case Blur.Medium:
                return Pass.Blur_Y_Medium;
            case Blur.Wide:
                return Pass.Blur_Y_Wide;
            case Blur.ExtraWide:
                return Pass.Blur_Y_ExtraWide;
            default:
                return Pass.Blur_Y_Medium;
        }
    }

    private int GetFinalPass()
    {
        switch (generalSettings.displayMode)
        {
            case DisplayMode.Normal:
                return Pass.Composite;
            case DisplayMode.AOOnly:
                return Pass.Debug_AO_Only;
            case DisplayMode.ColorBleedingOnly:
                return Pass.Debug_ColorBleeding_Only;
            case DisplayMode.SplitWithoutAOAndWithAO:
                return Pass.Debug_Split_WithoutAO_WithAO;
            case DisplayMode.SplitWithAOAndAOOnly:
                return Pass.Debug_Split_WithAO_AOOnly;
            case DisplayMode.SplitWithoutAOAndAOOnly:
                return Pass.Debug_Split_WithoutAO_AOOnly;
            default:
                return Pass.Composite;
        }
    }

    private void CreateRandomTexture(int size)
    {
        noiseTex = new Texture2D(size, size, TextureFormat.RGB24, false, true);
        noiseTex.filterMode = FilterMode.Point;
        noiseTex.wrapMode = TextureWrapMode.Repeat;
        int z = 0;
        for (int x = 0; x < size; ++x)
        {
            for (int y = 0; y < size; ++y)
            {
                float r1 = generalSettings.noiseType == NoiseType.Dither ? MersenneTwister.Numbers[z++] : UnityEngine.Random.Range(0.0f, 1.0f);
                float r2 = generalSettings.noiseType == NoiseType.Dither ? MersenneTwister.Numbers[z++] : UnityEngine.Random.Range(0.0f, 1.0f);
                float angle = 2.0f * Mathf.PI * r1 / _numSampleDirections[GetAoPass()];
                Color color = new Color(Mathf.Cos(angle), Mathf.Sin(angle), r2);
                noiseTex.SetPixel(x, y, color);
            }
        }
        noiseTex.Apply();

        for (int i = 0, j = 0; i < _jitter.Length; ++i)
        {
            float r1 = MersenneTwister.Numbers[j++];
            float r2 = MersenneTwister.Numbers[j++];
            float angle = 2.0f * Mathf.PI * r1 / _numSampleDirections[GetAoPass()];
            _jitter[i] = new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), r2, 0);
        }
    }

    public void ApplyPreset(Preset preset)
    {
        if (preset == Preset.Custom)
        {
            m_Presets.preset = preset;
            return;
        }

        DisplayMode displayMode = generalSettings.displayMode;

        m_GeneralSettings = GeneralSettings.defaultSettings;
        m_AOSettings = AOSettings.defaultSettings;
        m_ColorBleedingSettings = ColorBleedingSettings.defaultSettings;
        m_BlurSettings = BlurSettings.defaultSettings;

        m_GeneralSettings.displayMode = displayMode;

        switch (preset)
        {
            case Preset.FastestPerformance:
                m_GeneralSettings.quality = Quality.Lowest;
                m_AOSettings.radius = 0.5f;
                m_AOSettings.maxRadiusPixels = 64.0f;
                m_BlurSettings.amount = Blur.ExtraWide;
                m_BlurSettings.downsample = true;
                break;
            case Preset.FastPerformance:
                m_GeneralSettings.quality = Quality.Low;
                m_AOSettings.radius = 0.5f;
                m_AOSettings.maxRadiusPixels = 64.0f;
                m_BlurSettings.amount = Blur.Wide;
                m_BlurSettings.downsample = true;
                break;
            case Preset.HighQuality:
                m_GeneralSettings.quality = Quality.High;
                m_AOSettings.radius = 1.0f;
                break;
            case Preset.HighestQuality:
                m_GeneralSettings.quality = Quality.Highest;
                m_AOSettings.radius = 1.2f;
                m_AOSettings.maxRadiusPixels = 256.0f;
                m_BlurSettings.amount = Blur.Narrow;
                break;
            case Preset.Normal:
            default:
                break;
        }

        m_Presets.preset = preset;
    }
}
