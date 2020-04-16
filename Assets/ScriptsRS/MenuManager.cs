using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    private bool isPause = false;
    private bool loadedCam = false;
    private Scene pause;
    private Scene currentScene;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isPause)
        {
            isPause = true;
            LoadSceneParameters p = new LoadSceneParameters();
            p.loadSceneMode = LoadSceneMode.Additive;
            
            Time.timeScale = 0;
            pause = SceneManager.LoadScene("Scenes/PauseMenu", p);
            
        }
        else if (isPause && Input.GetKeyDown(KeyCode.Escape))
        {
            onContinueButton();
        }
        else if (pause.isLoaded && !loadedCam && isPause)
        {
            GameObject[] glist = pause.GetRootGameObjects();
            bool o = false;
            for (int i = 0; i < glist.Length; i++)
            {
                GameObject obj = glist[i];
      
                if (obj.CompareTag("Canvas"))
                {
                    Button[] butList = obj.GetComponentsInChildren<Button>();
                    for (int j = 0; j < butList.Length; j++)
                    {
                        if (butList[j].CompareTag("Continue"))
                        {
                            butList[j].onClick.AddListener(onContinueButton);
                        }
                        else if (butList[j].CompareTag("Retry"))
                        {
                            butList[j].onClick.AddListener(onRetryButton);
                        }
                        
                    }
                }
            }

           
            loadedCam = true;
        }
       
    }
    public void onContinueButton()
    {
        Debug.Log("vvvvvv");
        isPause = false;
        SceneManager.UnloadSceneAsync(pause);
        Time.timeScale = 1;
        loadedCam = false;
     
    }

    public void onRetryButton()
    {
        
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(pause);
        isPause = false;
        loadedCam = false;
        string name = currentScene.name;
        LoadSceneParameters p = new LoadSceneParameters(LoadSceneMode.Single);
        SceneManager.LoadScene(currentScene.name, p);
    }
}
