using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishRace : MonoBehaviour
{
    [SerializeField] Text finishText;
    [SerializeField] GameObject exitRaceButton;
    [SerializeField] GameObject restartRaceButton;
    [SerializeField] int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyCar"))
        {
            count++;
        }
        if (other.CompareTag("Car"))
        {
            count++;
            if (count == 1)
            {
                finishText.text = count + "st PLACE";
            }
            else
            {
                finishText.text = count + "nd PLACE";
            }
            exitRaceButton.SetActive(true);
            restartRaceButton.SetActive(true);
        }
    }

    public void ExitRace()
    {
        SceneManager.LoadScene(1);
    }
    public void TryAgain()
    {
        SceneManager.LoadScene(2);
    }
}
