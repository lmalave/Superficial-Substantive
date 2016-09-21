using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageReceiver : MonoBehaviour {

    public float firstMessageTime;
    public float messageInterval;
    public float messageIntervalSpread;
    public bool messagePause;
    public float messageDisplayTime;
    public List<string>  messageText;
    [SerializeField] MessageQueueHandler messageQueueHandler;


    private int messageIndex;
    private float messageCountdown;

	// Use this for initialization
	void Start () {
        messageCountdown = firstMessageTime;
        messageIndex = 0;
        messagePause = false;
    }

    // Update is called once per frame
    void Update () {
        if (!messagePause)
        {
            messageCountdown = messageCountdown - Time.deltaTime;
                if (messageCountdown < 0)
                {
                    Debug.Log("about to create new message to push onto queue");
                    Message message = new Message();
                    message.text = messageText[messageIndex];
                    message.displayTime = messageDisplayTime;
                    messageQueueHandler.PushMessage(message);
                    messageCountdown = GetCountDownValue(messageInterval);
                    messageIndex++;
                    if (messageIndex >= messageText.Count)
                    {
                        messageIndex = 0;
                    }
                }
        }
    }

    private float GetCountDownValue(float interval)
    {
        return Random.Range(interval * (1 - messageIntervalSpread), interval * (1 + messageIntervalSpread));
    }
}
