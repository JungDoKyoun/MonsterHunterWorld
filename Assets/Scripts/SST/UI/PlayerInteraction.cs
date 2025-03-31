using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform singleQuestPanel;
    public Transform createQuestPanel;
    public Transform joinQuestPanel;
    public Transform movePanel;
    public Transform boxPanel;

    public enum State
    {
        Hide,
        SingleQuest,
        CreateQuest,
        JoinQuest,
        Move,
        Box
    
    }

    private void Start()
    {
        //UIManager.Instance.AddUI(UIType.BoxSelectUI, boxPanel.gameObject);
        Debug.Log("?");
    }

    public void Show(State state)
    {
        singleQuestPanel.SetActive(state == State.SingleQuest);
        createQuestPanel.SetActive(state == State.CreateQuest);
        joinQuestPanel.SetActive(state == State.JoinQuest);
        movePanel.SetActive(state == State.Move);
        boxPanel.SetActive(state == State.Box);
    }
}