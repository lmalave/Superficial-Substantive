using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageQueueHandler : MonoBehaviour {

    public GameObject messageBox;
    [SerializeField] Text messageText;
    public float messageDisplayTime;
    public float messageTimeGap;

    private Queue messageQueue;
    private float messageDisplayCountdown;
    private float messageTimeGapCountdown;

    // Use this for initialization
    void Start() {
        messageQueue = new Queue();
    }

    // Update is called once per frame
    void Update() {
        if (messageBox.activeSelf)
        {
            if (messageDisplayCountdown < 0)
            {
                messageBox.SetActive(false);
            }
            else
            {
                messageDisplayCountdown = messageDisplayCountdown - Time.deltaTime;
            }
        }
        else
        {
            if (messageQueue.Count > 0)
            {
                Message message = (Message)messageQueue.Dequeue();
                messageText.text = message.text;
                messageBox.SetActive(true);
                messageDisplayCountdown = message.displayTime;
            }
        }
    }

    public void PushMessage(Message message)
    {
        //Debug.Log("about to push message onto queue: " + message.text);
       // Debug.Log("message queue: " + messageQueue);
        messageQueue.Enqueue(message);
    }

}

public class Message
{
    public string text;
    public float displayTime;
}
