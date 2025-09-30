using UnityEngine;
using System.Collections.Generic;

public class Interactble : MonoBehaviour
{
    public string requiredKeyName;
    public Animator animator;
    public GameObject[] artifactsInside;

    private bool isOpen = false;

    // Сохраняем, какие артефакты уже забрали
    private static HashSet<string> pickedArtifacts = new HashSet<string>();

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("OpenChest");

        // Делаем все артефакты доступными, кроме уже взятых
        foreach (var artifact in artifactsInside)
        {
            if (artifact != null && !pickedArtifacts.Contains(artifact.name))
                artifact.SetActive(true);
        }
    }

    // Вызывается артефактом при взятии игроком
    public void PickArtifact(string artifactName)
    {
        if (!string.IsNullOrEmpty(artifactName) && !pickedArtifacts.Contains(artifactName))
        {
            pickedArtifacts.Add(artifactName);
            // Сразу скрываем артефакт на сцене
            foreach (var artifact in artifactsInside)
            {
                if (artifact != null && artifact.name == artifactName)
                {
                    artifact.SetActive(false);
                    break;
                }
            }
        }
    }

    // Дополнительно можно вызвать, чтобы проверить состояние сундука при загрузке сцены
    public void RefreshArtifacts()
    {
        foreach (var artifact in artifactsInside)
        {
            if (artifact != null)
                artifact.SetActive(!pickedArtifacts.Contains(artifact.name));
        }
    }
}