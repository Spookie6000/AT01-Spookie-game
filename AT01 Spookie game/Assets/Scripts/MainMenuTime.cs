using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTime : MonoBehaviour
{
  public void PlayTime()
  {     // Loads the next Scene in the Queue 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

  }

  public void _QuitGame () 
  {
    Debug.Log("Game has been Quit");
    Application.Quit();
    
  }

}
