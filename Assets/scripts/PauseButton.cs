using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{


    void Start()
    {
       



        // Get the Button component and add the listener
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(GoToMainMenu);
        }
        else
        {
            Debug.LogError("Button component not found on this GameObject.");
        }
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
