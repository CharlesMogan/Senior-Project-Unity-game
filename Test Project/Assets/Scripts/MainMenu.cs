using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {



	public void	StartStandardGame (){
		SceneManager.LoadScene(1);
		
	}


	public void	Quit (){
		Application.Quit();
		Debug.Log("game is quit");
	}



}