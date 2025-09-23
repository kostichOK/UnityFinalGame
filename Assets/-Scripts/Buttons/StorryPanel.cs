using UnityEngine;
using UnityEngine.UI;

public class StoryPanel : MonoBehaviour
{
    public GameObject storyPanel;  
    public Text storyText;         

    [TextArea(5, 10)]
    public string storyContent = "Тут можна написати сюжет гри...";

    private void Start()
    {
       
        storyPanel.SetActive(false);

        if (storyText != null)
            storyText.text = storyContent;
    }
        
    public void OpenStory()
    {
        storyPanel.SetActive(true);
    }

    public void CloseStory()
    {
        storyPanel.SetActive(false);
    }
}
