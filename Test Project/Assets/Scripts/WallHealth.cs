using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHealth : Health {

protected override void Die(){
		Destroy(this.gameObject);
	}
}
