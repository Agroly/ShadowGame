using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Editor window: выбирает фотографии на доске и соединяет их линиями (UI Image через Mesh),
/// без использования scale. Линия строится как quad через CanvasRenderer + MeshFilter-подход
/// на основе RectTransform с нулевым scale.
/// </summary>
public class PhotoConnectorEditor : EditorWindow
{
    private List<GameObject> selectedPhotos = new List<GameObject>();
    private GameObject lineContainer;
    private string lineName = "Thread";
    private float lineWidth = 6f;
    private Color lineColor = new Color(0.6f, 0.3f, 0.1f, 1f);

    [MenuItem("Tools/Photo Board/Connect Photos with Thread")]
    public static void ShowWindow()
    {
        var window = GetWindow<PhotoConnectorEditor>("Photo Connector");
        window.minSize = new Vector2(320, 400);
    }

    private void OnGUI()
    {
        GUILayout.Label("📌 Thread Connector", EditorStyles.boldLabel);
        EditorGUILayout.Space(4);

        // --- Настройки линии ---
        EditorGUILayout.LabelField("Line Settings", EditorStyles.boldLabel);
        lineName = EditorGUILayout.TextField("Name Prefix", lineName);
        lineWidth = EditorGUILayout.FloatField("Line Width (px)", lineWidth);
        lineColor = EditorGUILayout.ColorField("Line Color", lineColor);
        EditorGUILayout.Space(4);

        // --- Контейнер ---
        lineContainer = (GameObject)EditorGUILayout.ObjectField(
            "Line Container (Canvas)", lineContainer, typeof(GameObject), true);
        EditorGUILayout.HelpBox(
            "Укажи GameObject внутри Canvas (например, пустой Panel), куда будут добавляться линии.",
            MessageType.Info);

        EditorGUILayout.Space(8);

        // --- Список фотографий ---
        EditorGUILayout.LabelField("Selected Photos (order matters)", EditorStyles.boldLabel);

        for (int i = 0; i < selectedPhotos.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            selectedPhotos[i] = (GameObject)EditorGUILayout.ObjectField(
                $"Photo {i + 1}", selectedPhotos[i], typeof(GameObject), true);
            if (GUILayout.Button("✕", GUILayout.Width(24)))
            {
                selectedPhotos.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space(4);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+ Add Photo"))
            selectedPhotos.Add(null);

        if (GUILayout.Button("Clear List"))
            selectedPhotos.Clear();
        EditorGUILayout.EndHorizontal();

        // --- Кнопка «взять из выделения» ---
        EditorGUILayout.Space(4);
        if (GUILayout.Button("↓ Use Scene Selection as List"))
        {
            selectedPhotos.Clear();
            foreach (var go in Selection.gameObjects)
                selectedPhotos.Add(go);
        }

        EditorGUILayout.Space(12);

        // --- Основная кнопка ---
        GUI.enabled = selectedPhotos.Count >= 2 && lineContainer != null;
        if (GUILayout.Button("▶  Connect Photos", GUILayout.Height(36)))
            ConnectPhotos();
        GUI.enabled = true;

        EditorGUILayout.Space(4);
        if (GUILayout.Button("🗑  Remove All Lines in Container"))
            RemoveAllLines();
    }

    // ─────────────────────────────────────────────
    // Главная логика
    // ─────────────────────────────────────────────
    private void ConnectPhotos()
    {
        // Проверяем, что у каждой фотографии есть pinPoint
        List<RectTransform> pins = new List<RectTransform>();
        foreach (var photo in selectedPhotos)
        {
            if (photo == null)
            {
                Debug.LogError("PhotoConnector: один из слотов пустой!");
                return;
            }
            var pin = FindPinPoint(photo);
            if (pin == null)
            {
                Debug.LogError($"PhotoConnector: у '{photo.name}' нет дочернего объекта с 'pinPoint' в имени!");
                return;
            }
            pins.Add(pin);
        }

        Canvas canvas = lineContainer.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("PhotoConnector: lineContainer должен быть внутри Canvas!");
            return;
        }

        Undo.SetCurrentGroupName("Connect Photos");
        int group = Undo.GetCurrentGroup();

        for (int i = 0; i < pins.Count - 1; i++)
        {
            CreateUILine(pins[i], pins[i + 1], canvas, i);
        }

        Undo.CollapseUndoOperations(group);
        Debug.Log($"PhotoConnector: создано {pins.Count - 1} линий.");
    }

    // ─────────────────────────────────────────────
    // Создание одной линии как UI Image (без scale)
    // ─────────────────────────────────────────────
    private void CreateUILine(RectTransform pinA, RectTransform pinB, Canvas canvas, int index)
    {
        // Переводим мировые позиции в локальные координаты контейнера
        RectTransform containerRT = lineContainer.GetComponent<RectTransform>();

        Vector2 posA = WorldToLocalCanvas(pinA.position, containerRT, canvas);
        Vector2 posB = WorldToLocalCanvas(pinB.position, containerRT, canvas);

        Vector2 dir = posB - posA;
        float length = dir.magnitude;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // Создаём GameObject
        string goName = $"{lineName}_{index + 1}_{pinA.parent?.name}_to_{pinB.parent?.name}";
        GameObject lineGO = new GameObject(goName, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        Undo.RegisterCreatedObjectUndo(lineGO, "Create Line");
        lineGO.transform.SetParent(lineContainer.transform, false);

        // RectTransform: ставим pivot в левый центр, позицию — в точку A
        RectTransform rt = lineGO.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0f, 0.5f);       // левый центр — точка начала
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = posA;

        // Размер: ширина = длина линии, высота = толщина. Scale НЕ трогаем.
        rt.sizeDelta = new Vector2(length, lineWidth);

        // Поворот
        rt.localRotation = Quaternion.Euler(0f, 0f, angle);

        // Image
        Image img = lineGO.GetComponent<Image>();
        img.color = lineColor;
        img.raycastTarget = false;
        // Спрайт не назначаем — пользователь назначит текстуру нити сам через Image.sprite

        EditorUtility.SetDirty(lineGO);
    }

    // ─────────────────────────────────────────────
    // Утилиты
    // ─────────────────────────────────────────────

    /// <summary>
    /// Переводит мировую позицию в локальные координаты RectTransform контейнера.
    /// Работает и для Screen Space Overlay, и для World Space / Camera Canvas.
    /// </summary>
    private Vector2 WorldToLocalCanvas(Vector3 worldPos, RectTransform containerRT, Canvas canvas)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Overlay: мировая позиция IS экранная
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                containerRT, screenPos, null, out Vector2 local);
            return local;
        }
        else
        {
            Camera cam = canvas.worldCamera ?? Camera.main;
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(cam, worldPos);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                containerRT, screenPos, cam, out Vector2 local);
            return local;
        }
    }

    /// <summary>
    /// Ищет дочерний объект с «pinpoint» (регистронезависимо) в имени.
    /// </summary>
    private RectTransform FindPinPoint(GameObject photo)
    {
        foreach (Transform child in photo.GetComponentsInChildren<Transform>(true))
        {
            if (child == photo.transform) continue;
            if (child.name.ToLower().Contains("pinpoint") || child.name.ToLower().Contains("pin_point"))
                return child.GetComponent<RectTransform>();
        }
        return null;
    }

    private void RemoveAllLines()
    {
        if (lineContainer == null) return;

        var children = new List<GameObject>();
        foreach (Transform child in lineContainer.transform)
            children.Add(child.gameObject);

        Undo.SetCurrentGroupName("Remove All Lines");
        foreach (var child in children)
            Undo.DestroyObjectImmediate(child);
    }
}