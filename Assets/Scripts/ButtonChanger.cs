
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonChanger : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] int buttonNum;
    public GameObject npc;


    public void SetNpc(GameObject npc)
    {
        this.npc = npc;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        npc.GetComponent<Dialogue>().AnswerClicked(buttonNum);
    }
}
