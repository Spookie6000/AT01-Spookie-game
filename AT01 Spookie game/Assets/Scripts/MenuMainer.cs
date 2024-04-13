using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMainer : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuObject;

    private void OnEnable()
    {
        EventManger.pauseGameEvent += TogglePauseMenu;
    }

    private void OnDestroy()
    {
        EventManger.pauseGameEvent -= TogglePauseMenu;
    }


    // Start is called before the first frame update

    void Start()
    {
        pauseMenuObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TogglePauseMenu(bool toggle)
    {
        pauseMenuObject.SetActive(toggle);
    }

    public void resumeGame()
    {
        EventManger.pauseGameEvent(false);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT THE GAME");
        Application.Quit();
    }


}
