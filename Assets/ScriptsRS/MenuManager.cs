using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    enum loadedType
    {
        TYPE_LEVEL,
        TYPE_MAIN,
        TYPE_NONE,
    }
    private bool isPause = false;
    private bool loadedCam = false;
    private bool loadedMain = false;
    private bool loadingScene = false;
    private bool hasToGoToLevelMenu = false;
    private AsyncOperation op;
    private Scene pause;
    private Scene currentScene;
    private Animator mainAnim;
    private loadedType type = loadedType.TYPE_MAIN;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        op = null;
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        if (op != null && op.isDone)
        {
            op = null;
        }
        if (type == loadedType.TYPE_MAIN && !loadedMain && SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Canvas");
            mainAnim = obj.GetComponent<Animator>();
            if (hasToGoToLevelMenu)
            {
                hasToGoToLevelMenu = false;
                onLevelsButton();
            }
            
            Button[] butList = obj.GetComponentsInChildren<Button>();
            for (int j = 0; j < butList.Length; j++)
            {
                if (butList[j].name == "Levels")
                {
                    butList[j].onClick.AddListener(onLevelsButton);
                }
                else if (butList[j].name == "Controls")
                {
                    butList[j].onClick.AddListener(onControlsButton);
                }
                else if (butList[j].name == "Credits")
                {
                    butList[j].onClick.AddListener(onCreditsButton);
                }
                else if (butList[j].name == "Exit")
                {
                    butList[j].onClick.AddListener(onExitButton);
                }
                else if (butList[j].name == "Back Button")
                {
                    butList[j].onClick.AddListener(onBackButton);
                }
                else if (butList[j].name == "Level 1")
                {
                    butList[j].onClick.AddListener(launchLevel1);
                }
                else if (butList[j].name == "Level 2")
                {
                    butList[j].onClick.AddListener(launchLevel2);
                }
                else if (butList[j].name == "Level 3")
                {
                    butList[j].onClick.AddListener(launchLevel3);
                }
                else if (butList[j].name == "Level 4")
                {
                    butList[j].onClick.AddListener(launchLevel4);
                }
                else if (butList[j].name == "Level 5")
                {
                    butList[j].onClick.AddListener(launchLevel5);
                }
                        
            }
            Debug.Log(butList.Length);
            loadedMain = true;
            
        }

        if (loadedMain && type == loadedType.TYPE_MAIN)
        {
            Debug.Log("Main already loaded");
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && !isPause && type == loadedType.TYPE_LEVEL)
        {
            isPause = true;
            LoadSceneParameters p = new LoadSceneParameters();
            p.loadSceneMode = LoadSceneMode.Additive;
            
            Time.timeScale = 0;
            pause = SceneManager.LoadScene("Scenes/PauseMenu", p);
            
        }
        else if (isPause && Input.GetKeyDown(KeyCode.Escape)&& type == loadedType.TYPE_LEVEL)
        {
            onContinueButton();
        }
        else if (pause.isLoaded && isPause)
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
                        if (butList[j].name == "Continue" || butList[j].name == "Back")
                        {
                            
                            butList[j].onClick.AddListener(onContinueButton);
                        }
                        else if (butList[j].name == "Retry")
                        {
                            butList[j].onClick.AddListener(onRetryButton);
                        }
                        else if (butList[j].name == "Levels")
                        {
                            butList[j].onClick.AddListener(onLevelsButton);
                        }
                        else if (butList[j].name == "Menu")
                        {
                            butList[j].onClick.AddListener(onMenuButton);
                        }

                    }
                }
            }

           
            loadedCam = true;
        }
       
    }
    public void onContinueButton()
    {
        if(!isPause)
            return;
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
        SceneManager.UnloadSceneAsync(name);
        if(op == null)
        {
            op = SceneManager.LoadSceneAsync(name);
            SceneManager.UnloadSceneAsync(name);
        }
        else if (op.isDone)
        {
            op = null;
        }
    }

    public void onLevelsButton()
    {
        
        if(type == loadedType.TYPE_MAIN && mainAnim	!= null)
            mainAnim.SetBool("PressedLevels", true);
        else
        {
            string name = currentScene.name;
            if(op == null)
            {
                op = SceneManager.LoadSceneAsync(1);
                SceneManager.UnloadSceneAsync(name);
            }
            
            type = loadedType.TYPE_MAIN	;
            Time.timeScale = 1;
            loadedMain = false;
            isPause = false;
            
            hasToGoToLevelMenu = true;
        }
    }

    public void onMenuButton()
    {
        if (type == loadedType.TYPE_MAIN)
        {
            return;
        }
        else
        {
            string name = currentScene.name;
            if(op == null)
            {
                op = SceneManager.LoadSceneAsync(1);
                SceneManager.UnloadSceneAsync(name);
            }
            else if (op.isDone)
            {
                op = null;
            }
            Time.timeScale = 1;
            loadedMain = false;
            isPause = false;
            type = loadedType.TYPE_MAIN	;
            hasToGoToLevelMenu = false;
        }
    }
    public void onControlsButton()
    {
        mainAnim.SetBool("PressedControls", true);
    }
    public void onCreditsButton()
    {
        mainAnim.SetBool("PressedCredits", true);
    }

    public void onBackButton()
    {
        mainAnim.SetBool("Goback", true);
    }

    public void launchLevel1()
    {
        type = loadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(2);
        loadedMain = false;
    }
    public void launchLevel2()
    {
        type = loadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(3);
        loadedMain = false;
    }
    public void launchLevel3()
    {
        type = loadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(4);
        loadedMain = false;
    }
    public void launchLevel4()
    {
        type = loadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(5);
        loadedMain = false;
    }
    public void launchLevel5()
    {
        type = loadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(6);
        loadedMain = false;
    }
    public void onExitButton()
    {
        Application.Quit();
    }
}
