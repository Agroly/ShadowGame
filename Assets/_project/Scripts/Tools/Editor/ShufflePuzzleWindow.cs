using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PrefabShufflerEditor : EditorWindow
{
    [MenuItem("Tools/Prefab Shuffler")]
    public static void ShowWindow()
    {
        GetWindow<PrefabShufflerEditor>("Prefab Shuffler");
    }

    private void OnGUI()
    {
        GUILayout.Label("Перемешивание Piece в Префабе", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        PrefabStage prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

        if (prefabStage == null)
        {
            EditorGUILayout.HelpBox("Пожалуйста, откройте префаб в режиме просмотра (Prefab Mode), чтобы использовать этот инструмент.", MessageType.Warning);
            return;
        }

        if (GUILayout.Button("Перемешать и повернуть Piece", GUILayout.Height(40)))
        {
            ShufflePrefabContents(prefabStage);
        }
    }

    private void ShufflePrefabContents(PrefabStage prefabStage)
    {
        GameObject root = prefabStage.prefabContentsRoot;
        
        List<Transform> pieces = new List<Transform>();

        // Собираем все Piece, которые лежат внутри слотов
        // Иерархия: Root (Grid) -> Parent -> Slots -> Piece
        foreach (Transform child in root.transform) 
        {
            foreach (Transform slot in child) 
            {
                if (slot.childCount > 0)
                {
                    pieces.Add(slot.GetChild(0)); 
                }
            }
        }

        if (pieces.Count <= 1)
        {
            Debug.LogWarning("Найдено слишком мало объектов Piece для перемешивания.");
            return;
        }

        // Перед изменением позиций регистрируем ВСЕ Piece в системе Undo
        foreach (Transform piece in pieces)
        {
            Undo.RecordObject(piece, "Shuffle Piece Transform");
        }

        // Вместо перемешивания самих объектов, мы перемешаем их ПАРАМЕТРЫ (позиции и повороты)
        // Создаем список исходных локальных позиций (хотя они обычно Vector3.zero, если привязаны к слотам)
        // И самое главное — нам нужно перемешать их глобальные/локальные связи относительно слотов.
        
        // Для этого алгоритмом Фишера-Йетса перемешаем массив ссылок на слоты, 
        // чтобы назначить их новые позиции.
        
        List<Transform> targetSlots = new List<Transform>(pieces.Count);
        foreach (Transform piece in pieces)
        {
            targetSlots.Add(piece.parent); // Запоминаем текущие слоты
        }

        // Перемешиваем слоты
        for (int i = targetSlots.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Transform temp = targetSlots[i];
            targetSlots[i] = targetSlots[randomIndex];
            targetSlots[randomIndex] = temp;
        }

        // Применяем новые позиции (переносим Piece на координаты новых слотов) и крутим
        for (int i = 0; i < pieces.Count; i++)
        {
            Transform piece = pieces[i];
            Transform targetSlot = targetSlots[i];

            // На случай, если в будущем вы решите всё-таки сменить родителя, 
            // этот метод сделает это без вызова ошибок, но теперь мы также принудительно двигаем объект
            Undo.SetTransformParent(piece, targetSlot, "Shuffle Piece Parent");
            
            // Жестко ставим в локальный ноль нового слота
            piece.localPosition = Vector3.zero;

            // Генерируем случайный поворот по Z кратный 90
            int randomAngleMultiplier = Random.Range(0, 4);
            float targetAngle = randomAngleMultiplier * 90f;
            piece.localRotation = Quaternion.Euler(0, 0, targetAngle);

            // Говорим Unity, что объект изменен
            EditorUtility.SetDirty(piece.gameObject);
        }

        // Принудительно маркируем сцену префаба как измененную
        EditorSceneManager.MarkSceneDirty(prefabStage.scene);

        Debug.Log($"Успешно перемешано и повернуто объектов: {pieces.Count}. Не забудьте сохранить префаб!");
    }
}