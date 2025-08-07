using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent Agent;
    public PointsManager pointsManager;
    int currentPositionIndex = 0;

    private void Start()
    {
        Init(pointsManager);
    }

    public void Init(PointsManager pointsManager)
    {
        Agent = GetComponent<NavMeshAgent>();
        this.pointsManager = pointsManager;
        transform.position = pointsManager.GetPosition(0);
        Agent.SetDestination(pointsManager.GetPosition(0));
    }

    private void Update() 
    {
        if (pointsManager == null || pointsManager.positionsCount() == 0) return;

        // Если почти дошёл до цели — идём к следующей
        if (!Agent.pathPending && Agent.remainingDistance <= 0.2f)
        {
            currentPositionIndex++;

            if (currentPositionIndex >= pointsManager.positionsCount())
            {
                currentPositionIndex = 0;
            }

            Agent.SetDestination(pointsManager.GetPosition(currentPositionIndex));
        }
    }
}
