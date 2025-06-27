using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public GameObject mapButton;
    [SerializeField] ButtonsSO buttonSO; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mapButton.SetActive(true);
            buttonSO.mapActive = true;
            buttonSO.closeMap = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mapButton.SetActive(false);
            buttonSO.mapActive = false;
            buttonSO.closeMap = true;
        }
    }
}
