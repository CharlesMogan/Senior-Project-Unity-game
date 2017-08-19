using UnityEngine;

public class Bullet : MonoBehaviour {
	//private Rigidbody rb;
	//private Transform bullet;
	public float bulletDamage;
	//public Health healthScript; 
	private  float timeOfCreation, timeOfDestruction;
	void Start () {
	//	rb = GetComponent<Rigidbody>();
	//	bullet = GetComponent<Transform>();
	}


	void Update(){
		/*if(timeOfDestruction <= Time.time){
			Debug.Log("destroy the bullet");
			Destroy(this);
		}*/
	}

	protected virtual void OnTriggerEnter(Collider other) {
       	if(other.gameObject.tag != "Friendly Bullet" && other.gameObject.tag != "Unfrendly Bullet"){
    		Destroy(this.gameObject);
    	}
    }

	
	// Update is called once per frame

}
