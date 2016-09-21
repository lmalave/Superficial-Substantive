using UnityEngine;
using System.Collections;

public class SnapBase : MonoBehaviour {

	public string objectType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter (Collision col)
    {
       
        if(col.gameObject.name == "BigBox")
        {
	       col.gameObject.transform.parent = transform;
	       col.gameObject.transform.localPosition = new Vector3(0f,1f,0f);
	    }
	    Debug.Log("in SnapBase OnCollisionEnter, this: "+this.name+", col: "+ col.gameObject.name);
    }
}
