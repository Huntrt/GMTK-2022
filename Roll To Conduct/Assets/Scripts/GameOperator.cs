using UnityEngine.SceneManagement;
using UnityEngine;

public class GameOperator : MonoBehaviour
{
    public void LoadSceneIndex(int i) {SceneManager.LoadScene(i, LoadSceneMode.Single);}
    public void QuitGame()  {Application.Quit();}
}