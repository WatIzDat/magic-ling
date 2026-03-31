using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void OnPlayButtonClicked()
    {
        RunInfo.NewRun();

        SceneManager.LoadScene("SampleScene");
    }
}
