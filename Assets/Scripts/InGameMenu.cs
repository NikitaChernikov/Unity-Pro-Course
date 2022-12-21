using UnityEngine.SceneManagement;
using UnityEngine;
public class InGameMenu : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
