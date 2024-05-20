using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTime : MonoBehaviour
{
  public void PlayTime(int GameScene)
  {     // Loads the next Scene in the Queue 
        SceneManager.LoadScene(GameScene);

  }

  public void _QuitGame () 
  {
    Debug.Log("Game has been Quit");
    Application.Quit();
    
  }

    public void ReturnToMainMenu(int MenuScene)
    {
        // Returns to Main MenuScene
        SceneManager.LoadScene(MenuScene);



    }



}
