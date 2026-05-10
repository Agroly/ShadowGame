// ShadowCamera.cs — на объект-камеру, направленную на стену

using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class ShadowCamera : MonoBehaviour
{
    [SerializeField] private RenderTexture _shadowTexture;
    
    private Camera _cam;

    private void OnValidate()
    {
        _cam = GetComponent<Camera>();
        _cam.orthographic = true;
        _cam.clearFlags = CameraClearFlags.SolidColor;
        _cam.backgroundColor = Color.clear;
        _cam.cullingMask = LayerMask.GetMask("ShadowCaster"); // только ваш объект
        _cam.targetTexture = _shadowTexture;
    }
}