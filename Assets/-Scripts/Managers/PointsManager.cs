using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    [SerializeField] Transform pathParent;
    List<Vector3> positions = new List<Vector3>();

    private void Awake()
    {
        for (int i = 0; i < pathParent.childCount; i++)
        {
            Vector3 childPosition = pathParent.GetChild(i).position;
            positions.Add(childPosition);
        }
    }

    public Vector3 GetPosition(int index) => positions[index];
    public int positionsCount() => positions.Count;
}

