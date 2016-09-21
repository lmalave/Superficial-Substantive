using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DonationReceiver : MonoBehaviour {

    public float firstSpawnTime;
    public float spawnInterval;
    public float spawnIntervalSpread;
    public List<string> messageText;
    public float messageDisplayTime;
    public GameObject spawnPrefab;
    public Vector3 spawnPosition;
    public float spawnRadius;
    private int messageIndex;
    private bool isFirstObject;
    [SerializeField] MessageQueueHandler messageQueueHandler;

    private float spawnCountdown;
    private List<string> colorList = new List<string> { "Red", "Green", "Blue", "Yellow" };

    // Use this for initialization
    void Start () {
        spawnCountdown = firstSpawnTime;
        messageIndex = 0;
        isFirstObject = true;
    }

    // Update is called once per frame
    void Update()
    {
        spawnCountdown = spawnCountdown - Time.deltaTime;
        if (spawnCountdown < 0)
        {
            Message message = new Message();
            message.text = messageText[messageIndex];
            message.displayTime = messageDisplayTime;
            //Debug.Log("spawning object: " + spawnPrefab.name);
            if (isFirstObject)
            {
                messageQueueHandler.PushMessage(message);
                isFirstObject = false;
            }

            messageIndex++;
            if (messageIndex >= messageText.Count)
            {
                messageIndex = 0;
            }
            spawnCountdown = spawnInterval;

            GameObject newObject = GameObject.Instantiate(spawnPrefab, new Vector3(spawnPosition.x + Random.Range(0f, spawnRadius), spawnPosition.y, spawnPosition.z + Random.Range(0f, spawnRadius)), Quaternion.identity) as GameObject;
            CheckAndApplyColor(message, newObject);
        }
    }

    private float GetCountDownValue(float interval)
    {
        return Random.Range(interval * spawnIntervalSpread, interval * spawnIntervalSpread);
    }

    private void CheckAndApplyColor(Message message, GameObject gameObject)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (string color in colorList)
        {
            if (message.text.Contains(color))
            {
                Material mat = Resources.Load("Materials/" + color + "Material", typeof(Material)) as Material;
                foreach (Renderer renderer in renderers)
                {
                    renderer.material = mat;
                }
            }
        }
    }
}
