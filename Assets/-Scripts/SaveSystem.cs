using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem
{
    // Здесь будут храниться сохранённые позиции (ID объекта → позиция)
    private static Dictionary<string, Vector3> savedPositions = new Dictionary<string, Vector3>();

    public static void SavePosition(Item obj, Vector3 pos)
    {
        savedPositions[obj.objectID] = pos;
    }

    public static bool TryGetSavedPosition(Item obj, out Vector3 pos)
    {
        return savedPositions.TryGetValue(obj.objectID, out pos);
    }
}
