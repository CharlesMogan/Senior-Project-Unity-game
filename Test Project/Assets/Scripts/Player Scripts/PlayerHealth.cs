using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : Health {
	public Text healthText;
	
	void Start () {
		healthText.text =  health.ToString()+"/"+ maxHealth.ToString();
	}

	protected override void Die(){
		Debug.Log("you died");
	}

	public override void TakeDamage(float damage){
		base.TakeDamage(damage);
		healthText.text =  health.ToString()+"/"+ maxHealth.ToString();	
	}


}
