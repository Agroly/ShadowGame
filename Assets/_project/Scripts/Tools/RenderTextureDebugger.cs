using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class RenderTextureDebugger : MonoBehaviour
{
    [Header("RT")]
    [SerializeField] private int _rtWidth = 512;
    [SerializeField] private int _rtHeight = 512;
    [SerializeField] private int _depthBufferBits = 24;
    [SerializeField] private RenderTextureFormat _colorFormat = RenderTextureFormat.ARGB32;

    [Header("Optional")]
    [SerializeField] private RawImage _rawImage;
    [SerializeField] private GameObject _testCube;
    [SerializeField] private Renderer _wallRenderer;

    private Camera _camera;
    private RenderTexture _rt;
    private Texture2D _readbackTexture;
    private Material _unlitMaterial;
    private GameObject _wallQuad;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        StartCoroutine(TestRenderTexture());
    }

    private IEnumerator TestRenderTexture()
    {
        Debug.Log($"[RT-DEBUG] Step 1. Create RT {_rtWidth}x{_rtHeight}, color={_colorFormat}, depth={_depthBufferBits}");

        _rt = new RenderTexture(_rtWidth, _rtHeight, _depthBufferBits, _colorFormat);
        _rt.antiAliasing = 1;
        _rt.useMipMap = false;
        _rt.autoGenerateMips = false;
        _rt.Create();

        Debug.Log($"[RT-DEBUG] RT created={_rt.IsCreated()}, actualDepthStencilFormat={_rt.depthStencilFormat}");

        if (!_rt.IsCreated())
        {
            Debug.LogError("[RT-DEBUG] RT was not created");
            yield break;
        }

        _camera.targetTexture = _rt;
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = Color.magenta;

        Debug.Log($"[RT-DEBUG] Step 2. Camera target assigned={_camera.targetTexture != null}");

        if (_testCube == null)
        {
            _testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            _testCube.name = "[RT-DEBUG] Cube";
            _testCube.transform.position = _camera.transform.position + _camera.transform.forward * 2f;
            _testCube.transform.rotation = Quaternion.identity;

            var cubeRenderer = _testCube.GetComponent<Renderer>();
            if (cubeRenderer != null)
                cubeRenderer.material.color = Color.yellow;
        }

        Debug.Log($"[RT-DEBUG] Step 3. Test cube exists={_testCube != null}");

        yield return new WaitForEndOfFrame();

        Color pixel = ReadCenterPixel(_rt);
        Debug.Log($"[RT-DEBUG] Step 4. Center pixel after render={pixel}");

        if (_rawImage != null)
        {
            _rawImage.texture = _rt;
            Debug.Log("[RT-DEBUG] Step 5. RT assigned to RawImage");
        }

        if (_wallRenderer != null)
        {
            _wallRenderer.material.mainTexture = _rt;
            Debug.Log($"[RT-DEBUG] Step 6. RT assigned to wallRenderer, hasTexture={_wallRenderer.material.mainTexture != null}");
        }
        else
        {
            _wallQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _wallQuad.name = "[RT-DEBUG] WallQuad";
            _wallQuad.transform.position = new Vector3(0f, 0f, 3f);
            _wallQuad.transform.rotation = Quaternion.identity;

            Shader shader = Shader.Find("Unlit/Texture");
            if (shader == null)
            {
                Debug.LogError("[RT-DEBUG] Shader 'Unlit/Texture' not found");
            }
            else
            {
                _unlitMaterial = new Material(shader);
                _unlitMaterial.mainTexture = _rt;
                _wallQuad.GetComponent<Renderer>().material = _unlitMaterial;
                Debug.Log($"[RT-DEBUG] Step 6. Debug quad created, material texture assigned={_unlitMaterial.mainTexture != null}");
            }
        }

        yield return new WaitForEndOfFrame();

        pixel = ReadCenterPixel(_rt);
        Debug.Log($"[RT-DEBUG] Final center pixel={pixel}");
        Debug.Log($"[RT-DEBUG] Graphics API={SystemInfo.graphicsDeviceType}");
        Debug.Log($"[RT-DEBUG] Vendor={SystemInfo.graphicsDeviceVendor}");
        Debug.Log($"[RT-DEBUG] Supports RenderTextures={SystemInfo.supportsRenderTextures}");
        Debug.Log($"[RT-DEBUG] Supports RandomWrite for {_colorFormat}={SystemInfo.SupportsRandomWriteOnRenderTextureFormat(_colorFormat)}");

        Debug.Log("[RT-DEBUG] RESULT GUIDE:");
        Debug.Log("[RT-DEBUG] Magenta -> camera clears RT, but object may be invisible/not rendered");
        Debug.Log("[RT-DEBUG] Yellow-ish -> RT works and cube is rendered");
        Debug.Log("[RT-DEBUG] Black/zero -> RT or format/backend likely failed");
    }

    private Color ReadCenterPixel(RenderTexture rt)
    {
        if (rt == null)
            return Color.clear;

        if (_readbackTexture == null || _readbackTexture.width != rt.width || _readbackTexture.height != rt.height)
        {
            _readbackTexture = new Texture2D(rt.width, rt.height, TextureFormat.RGBA32, false);
        }

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        _readbackTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        _readbackTexture.Apply();

        Color pixel = _readbackTexture.GetPixel(rt.width / 2, rt.height / 2);

        RenderTexture.active = previous;
        return pixel;
    }

    private void OnDestroy()
    {
        if (_camera != null && _camera.targetTexture == _rt)
            _camera.targetTexture = null;

        if (_rt != null)
        {
            _rt.Release();
            Destroy(_rt);
        }

        if (_readbackTexture != null)
            Destroy(_readbackTexture);

        if (_unlitMaterial != null)
            Destroy(_unlitMaterial);

        if (_wallQuad != null)
            Destroy(_wallQuad);
    }
}