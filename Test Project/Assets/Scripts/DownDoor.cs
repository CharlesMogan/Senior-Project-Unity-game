using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DownDoor : MonoBehaviour {
	WorldMaker.Room myRoom;

	void OnTriggerEnter(Collider other) {
	   	if(other.gameObject.tag == "Player"){
	   		myRoom.EnteredRoom();
	   	}
	}


	public void WhoIsMyRoom(WorldMaker.Room myRoom){
		this.myRoom = myRoom;
	}


}
