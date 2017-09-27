using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : Health {
	public Text healthText;

	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		healthText.text =  health.ToString()+"/"+ maxHealth.ToString();
	}




}
