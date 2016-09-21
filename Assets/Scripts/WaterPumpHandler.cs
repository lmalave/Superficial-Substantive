using UnityEngine;
using System.Collections;

public class WaterPumpHandler : MonoBehaviour {

    public string waterPumpName;
    public MessageQueueHandler messageQueue;
    public GameObject waterObject;
    public float pumpAngleThreshold;
    public float breakTime;
    public float pumpFailDisplayTime;


    private GameObject middle;
    private GameObject top;
    private GameObject handle;
    private GameObject spout;
    private HingeJoint handleHingeJoint;

    private ParticleSystem waterParticleSystem;
    private AudioSource waterSound;

    public bool middleIsAttached;
    public bool topIsAttached;
    public bool handleIsAttached;
    public bool spoutIsAttached;
    public bool baseIsAttached;
    private bool isCompleted;
    private bool isBroken;
    private float breakCountdown;
    private bool pumpFailMessageDisplayed;
    private float pumpFailDisplayCountdown;

	// Use this for initialization
	void Start () {
        middleIsAttached = false;
        topIsAttached = false;
        handleIsAttached = false;
        spoutIsAttached = false;
        baseIsAttached = true;
        isCompleted = false;
        waterParticleSystem = waterObject.GetComponent<ParticleSystem>();
        waterSound = this.gameObject.GetComponent<AudioSource>();
        breakCountdown = 0f;
        pumpFailMessageDisplayed = false;
        pumpFailDisplayCountdown = 0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (middleIsAttached && topIsAttached && handleIsAttached && spoutIsAttached && !isCompleted)
        {
            // Check if completed
            isCompleted = true;
            breakCountdown = breakTime;
            Message message = new Message();
            message.text = "Pump completed!!! Pump the handle to get water.";
            message.displayTime = 3f;
            messageQueue.PushMessage(message);
        }
        if (isCompleted && handleHingeJoint.angle < -pumpAngleThreshold)
        {  // check if handle is pumped
            if (!isBroken)
            { // check if pump is broken
                if (!waterParticleSystem.isPlaying)
                { // if handle pumped, pump is not broken, then start water flow if it isnt' flowing already
                    waterParticleSystem.Play();
                    waterSound.Play();
                }
            }
            else
            {  // handle is pumped but pump is broken
                if (!pumpFailMessageDisplayed)
                { // check if we're already displaying a fail message.  If not, then display it
                    pumpFailMessageDisplayed = true;
                    Message message = new Message();
                    message.text = "Can't pump! Need maintenance to fix pump!";
                    message.displayTime = pumpFailDisplayTime;
                    messageQueue.PushMessage(message);
                    pumpFailDisplayCountdown = pumpFailDisplayTime;
                }
            }
        }
        else
        {  // handle is not being pumped
            if (waterParticleSystem.isPlaying)
            {  // stop water if it's playing
                waterParticleSystem.Stop();
                waterSound.Stop();
            }
        }
        if (isCompleted && !isBroken)
        {   // if pump is completed but it's not broken yet, then continue counting down
            breakCountdown = breakCountdown - Time.deltaTime;
        }
        if (!isBroken && breakCountdown < 0)
        { // finished break countdown, so flag pump as broken 
            isBroken = true;
            Message message = new Message();
            message.text = "Pump is broken!  Need maintenance staff!";
            message.displayTime = 3f;
            messageQueue.PushMessage(message);
        }
        if (pumpFailMessageDisplayed)
        {  // if pump fail message is being displayed then count down 
            pumpFailDisplayCountdown = pumpFailDisplayCountdown - Time.deltaTime;
        }
        if (pumpFailDisplayCountdown < 0)
        {   // already displayed pump fail message, so can clear flag so it can be displayed again if user attempts to pump again
            pumpFailMessageDisplayed = false;
        }
    }

    public void AttachPart(GameObject partObject)
    {
        NVRInteractableItemSnappable part = partObject.GetComponent<NVRInteractableItemSnappable>();
        if (part.objectType == "middle")
        {
            middleIsAttached = true;
        }
        else if (part.objectType == "top")
        {
            topIsAttached = true;
        }
        else if (part.objectType == "handle")
        {
            handleHingeJoint = part.GetComponentInChildren<HingeJoint>();
            handleIsAttached = true;
        }
        else if (part.objectType == "spout")
        {
            spoutIsAttached = true;
        }
    }
}
