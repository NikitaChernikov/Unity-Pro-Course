using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour
{
    GameObject dialogue;
    public DialogueNode[] node;
    Text npc;
    Text[] textButtons;
    GameObject[] buttons;
    [SerializeField] public List<GameObject> answerButtons = new List<GameObject>();

    [SerializeField] GameObject hiButton;

    [SerializeField] GameObject colorTarget; // здесь хранится наш префаб
    [System.NonSerialized] public GameObject target; // объект, в котором будет хранится наш префаб пока мы не выполним задание
    bool isInstantiate; // обозначаем активирован ли наш префаб

    int currentNode = 0;
    private void Awake()
    {
        npc = GameObject.FindGameObjectWithTag("TextNPC").GetComponent<Text>();
        dialogue = GameObject.FindGameObjectWithTag("Dialogue");
        buttons = GameObject.FindGameObjectsWithTag("QuestButton");
        textButtons = new Text[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            textButtons[i] = buttons[i].transform.GetChild(0).GetComponent<Text>();
        }
    }

    private void Start()
    {
        dialogue.SetActive(false);
    }
    public void AnswerClicked(int button)
    {
        
        if (node[currentNode].PlayerAnswer[button].SpeakEnd)
        {
            dialogue.SetActive(false);
        }
        if (node[currentNode].PlayerAnswer[button].questValue > 0)
        {
            PlayerPrefs.SetInt(node[currentNode].PlayerAnswer[button].questName,
                    node[currentNode].PlayerAnswer[button].questValue);
        }
        if (node[currentNode].PlayerAnswer[button].getMoney > 0)
        {
            FindObjectOfType<PlayerController>().GetMoney(node[currentNode].PlayerAnswer[button].getMoney);
        }
        if (node[currentNode].PlayerAnswer[button].target != null)
        {
            if (!isInstantiate)
            {
                target = Instantiate(colorTarget);
                isInstantiate = true;
            }
            target.transform.position = node[currentNode].PlayerAnswer[button].target.transform.position;
        }
        if (node[currentNode].PlayerAnswer[button].destroyTarget)
        {
            Destroy(target);
        }
        currentNode = node[currentNode].PlayerAnswer[button].ToNode;
        Refresh();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            hiButton.SetActive(true);
            
        }
    }

    public void StartDialogue()
    {
        dialogue.SetActive(true);
        currentNode = 0;
        Refresh();
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            hiButton.SetActive(false);
            dialogue.SetActive(false);
        }
    }
    public void Refresh()
    {
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].SetActive(false);
            answerButtons[i].gameObject.SetActive(false);
        }
        answerButtons.Clear();
        npc.text = node[currentNode].NpcText;
        for (int i = 0; i < node[currentNode].PlayerAnswer.Length; i++)
        {
            buttons[i].SetActive(false);
            buttons[i].gameObject.SetActive(false);

            if (node[currentNode].PlayerAnswer[i].questName == "" ||
                    node[currentNode].PlayerAnswer[i].needQuestValue ==
                        PlayerPrefs.GetInt(node[currentNode].PlayerAnswer[i].questName))
            {
                answerButtons.Add(buttons[i]);
                textButtons[i].text = node[currentNode].PlayerAnswer[i].Text;
            }
        }
        for (int i = 0; i < answerButtons.Count; i++)
        {
            answerButtons[i].SetActive(true);
            answerButtons[i].gameObject.SetActive(true);
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    public string NpcText;
    public Answer[] PlayerAnswer;
}

[System.Serializable]
public class Answer
{
    public string Text;
    public int ToNode;
    public int questValue;
    public int needQuestValue;
    public string questName;
    public bool SpeakEnd;
    public int getMoney;
    public bool destroyTarget; // если true, то уничтожим particle
    public GameObject target; // ссылка на объект к которому переместится Particle
}