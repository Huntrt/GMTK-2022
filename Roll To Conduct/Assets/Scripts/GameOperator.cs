using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOperator : MonoBehaviour
{
	[Header("Gameplay")]
	public bool paused;
	public GameObject pauseMenu;

	//Set this class to singleton
	public static GameOperator i {get{if(_i==null){_i = GameObject.FindObjectOfType<GameOperator>();}return _i;}} static GameOperator _i;
	
	public void PauseToggle()
	{
		//Toggle the pause menu
		pauseMenu.SetActive(!pauseMenu.activeInHierarchy);
		//Toggle pause
		paused = !paused;
	}
    public void LoadSceneIndex(int i) {SceneManager.LoadScene(i, LoadSceneMode.Single);}
    public void QuitGame()  {Application.Quit();}
}