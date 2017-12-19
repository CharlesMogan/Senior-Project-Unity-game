using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	private Transform player;
	public Vector3 offset;

	// Update is called once per frame
	void Update () {
		if(player == null){
			player = GameObject.FindWithTag("Player").transform;
		}else{
			transform.position = player.position + offset;
		}
	}
}
