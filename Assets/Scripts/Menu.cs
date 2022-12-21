using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject optionsUI;
    [SerializeField] GameObject mainUI;
    [SerializeField] GameObject authorsUI;

    [SerializeField] GameObject mainPosition;
    [SerializeField] GameObject volumePosition;
    [SerializeField] GameObject authorsPosition;
    GameObject camera;

    bool isOptions = false;
    bool isAuthors = false;
    public enum State { Main, Settings, Authors}
    State current;

    // Start is called before the first frame update
    void Start()
    {
        current = State.Main;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOptions && !isAuthors)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, mainPosition.transform.position, 0.01f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, mainPosition.transform.rotation, 0.01f);
        }
        else if (isOptions)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, volumePosition.transform.position, 0.01f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, volumePosition.transform.rotation, 0.01f);
        }
        else if (isAuthors)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, authorsPosition.transform.position, 0.01f);
            camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, authorsPosition.transform.rotation, 0.01f);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        if (!isOptions) SwitchState(State.Settings);
        else SwitchState(State.Main);
    }

    public void OpenAuthors()
    {
        if (!isAuthors) SwitchState(State.Authors);
        else SwitchState(State.Main);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SwitchState(State state)
    {
        current = state;
        switch (state)
        {
            case State.Main:
                mainUI.SetActive(true);
                optionsUI.SetActive(false);
                authorsUI.SetActive(false);
                isOptions = false;
                isAuthors = false;
                break;
            case State.Settings:
                mainUI.SetActive(false);
                optionsUI.SetActive(true);
                authorsUI.SetActive(false);
                isOptions = true;
                isAuthors = false;
                break;
            case State.Authors:
                mainUI.SetActive(false);
                optionsUI.SetActive(false);
                authorsUI.SetActive(true);
                isOptions = false;
                isAuthors = true;
                break;
            default:
                break;
        }
    }

}
