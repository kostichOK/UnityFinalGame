using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PointsManager : MonoBehaviour
{
    public Transform pathParent;
    public List<Vector3> positions = new List<Vector3>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Сбрасываем список точек
        positions.Clear();

        // Пытаемся найти новый PathParent по тегу
        if (pathParent == null)
        {
            GameObject commonPoint = GameObject.FindGameObjectWithTag("Points");
            if (commonPoint != null)
                pathParent = commonPoint.transform;
        }

        // Если нашли, заполняем positions
        if (pathParent != null)
        {
            for (int i = 0; i < pathParent.childCount; i++)
                positions.Add(pathParent.GetChild(i).position);
        }
        else
        {
            Debug.LogWarning("PathParent не найден!");
        }
    }

    public Vector3 GetPosition(int index) => positions[index];
    public int positionsCount() => positions.Count;
}

