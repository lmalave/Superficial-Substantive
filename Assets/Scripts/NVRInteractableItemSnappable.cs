using UnityEngine;
using System.Collections;
using NewtonVR; 

public class NVRInteractableItemSnappable : MonoBehaviour {


	public string objectType;
	public string snapToObjectType;
	public Vector3 snapToPosition;
	public Vector3 snapToRotationEulerAngles;
    public GameObject solidObjectPrefab;
    public bool isGhost;
    public bool isNoJoint;
    [SerializeField] WaterPumpHandler waterPumpHandler;

	private bool isSnapped = false;
    private bool handleAttached = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (isSnapped)
		{
			this.transform.localPosition = snapToPosition;
		    this.transform.localRotation = Quaternion.Euler(snapToRotationEulerAngles);
		}
	}

	void OnCollisionEnter (Collision col)
    {
        NVRInteractableItemSnappable colObject = col.gameObject.GetComponent<NVRInteractableItemSnappable>();

        if (!this.isGhost && colObject != null && !isNoJoint)
        {
            Debug.Log(this.gameObject.name + " is colliding with " + col.gameObject.name);

            if (colObject.objectType == this.objectType && colObject.isGhost)
            {
                if ((this.snapToObjectType == "middle" && (colObject.objectType == "top" || colObject.objectType == "spout") && colObject.waterPumpHandler.middleIsAttached)
                    || (colObject.snapToObjectType == "top" && this.objectType == "handle" && colObject.waterPumpHandler.topIsAttached)
                    || (colObject.snapToObjectType == "base" && this.objectType == "middle" && colObject.waterPumpHandler.baseIsAttached))
                {
                    this.transform.localScale = Vector3.zero;
                    Destroy(this.gameObject, 1);
                    //col.GetComponent<Renderer>.Material = solidMaterial;
                    Vector3 solidPosition = col.transform.position;
                    Quaternion solidRotation = col.transform.localRotation;
                    GameObject solidPrefab = colObject.solidObjectPrefab;
                    // Destroy(col.gameObject);
                    col.gameObject.SetActive(false);
                    if (solidPrefab)
                    {
                        GameObject solidObject = Instantiate(solidPrefab, solidPosition, solidRotation) as GameObject;
                        Material mat = this.gameObject.GetComponentsInChildren<Renderer>()[0].material;
                        foreach (Renderer renderer in solidObject.GetComponentsInChildren<Renderer>())
                        {
                            renderer.material = mat;
                        }
                        colObject.waterPumpHandler.AttachPart(solidObject);
                        isSnapped = true;
                    }
                    else
                    {
                        Debug.Log("could not find solidObjectPrefab for: " + col.gameObject.name);
                    }
                }
            }
            /*      if (this.objectType == "handle")
                  {
                      GameObject handle = GameObject.Find("handle");
                      handle.AddComponent<HingeJoint>();
                      handle.GetComponent<HingeJoint>().connectedBody = this.gameObject.GetComponent<Rigidbody>();
                  } */
        }
       // Debug.Log("in SnapBase OnCollisionEnter, this: "+this.name+", col: "+ col.gameObject.name);
    }
}