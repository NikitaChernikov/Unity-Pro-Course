using UnityEngine;

public class RandomSkinChanger : MonoBehaviour
{
    [SerializeField] private GameObject[] skins;
    [SerializeField] private GameObject[] heads;

    // Start is called before the first frame update
    void Start()
    {
        skins[Random.Range(0, skins.Length)].SetActive(true);
        heads[Random.Range(0, skins.Length)].SetActive(true);
    }

    
}
