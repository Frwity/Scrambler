using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;




public class MenuManager : MonoBehaviour
{

    // Start is called before the first frame update
    enum LoadedType
    {
        TYPE_LEVEL,
        TYPE_MAIN,
        TYPE_NONE,
    }

    private bool isPause = false;
    private bool loadedMain = false;
    private bool hasToGoToLevelMenu = false;
    private AsyncOperation op;
    private Scene pause;
    private Scene end;
    private Scene currentScene;
    private Animator mainAnim;
    [HideInInspector]public bool levelEnded = false;
    private bool EndMenuLoaded = false;
    private pointer cursor;
    private LoadedType type = LoadedType.TYPE_MAIN;

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        op = null;
    }

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
            Destroy	(this.gameObject);
        }
            if (op != null && op.isDone)
        {
            op = null;
        }
        if (type == LoadedType.TYPE_MAIN && !loadedMain && SceneManager.GetSceneByBuildIndex(1).isLoaded)
        {
            EndMenuLoaded = false;
            levelEnded = false;
            GameObject obj = GameObject.FindGameObjectWithTag("Canvas");
            cursor = obj.GetComponentInChildren<pointer>();
            mainAnim = obj.GetComponent<Animator>();
            if (hasToGoToLevelMenu)
            {
                hasToGoToLevelMenu = false;
                OnLevelsButton();
            }
            
            Button[] butList = obj.GetComponentsInChildren<Button>();
            for (int j = 0; j < butList.Length; j++)
            {
                if (butList[j].name == "Levels")
                {
                    butList[j].onClick.AddListener(OnLevelsButton);
                }
                else if (butList[j].name == "Controls")
                {
                    butList[j].onClick.AddListener(OnControlsButton);
                }
                else if (butList[j].name == "Credits")
                {
                    butList[j].onClick.AddListener(OnCreditsButton);
                }
                else if (butList[j].name == "Exit")
                {
                    butList[j].onClick.AddListener(OnExitButton);
                }
                else if (butList[j].name == "Back Button")
                {
                    butList[j].onClick.AddListener(OnBackButton);
                }
                else if (butList[j].name == "Next Button (1)")
                {
                    butList[j].onClick.AddListener(OnPrevButton);
                }
                else if (butList[j].name == "Next Button")
                {
                    butList[j].onClick.AddListener(OnNextButton);
                }
                else if (butList[j].name == "Level 1")
                {
                    butList[j].onClick.AddListener(LaunchLevel1);
                }
                else if (butList[j].name == "Level 2")
                {
                    butList[j].onClick.AddListener(LaunchLevel2);
                }
                else if (butList[j].name == "Level 3")
                {
                    butList[j].onClick.AddListener(LaunchLevel3);
                }
                else if (butList[j].name == "Level 4")
                {
                    butList[j].onClick.AddListener(LaunchLevel4);
                }
                else if (butList[j].name == "Level 5")
                {
                    butList[j].onClick.AddListener(LaunchLevel5);
                }
                        
            }
            Debug.Log(butList.Length);
            loadedMain = true;
            
        }

        if (loadedMain && type == LoadedType.TYPE_MAIN)
        {
           
            
            Debug.Log("Main already loaded");
            return;
        }
        if (Input.GetAxisRaw("start") == 1 && !isPause && type == LoadedType.TYPE_LEVEL && !levelEnded)
        {
            isPause = true;
            LoadSceneParameters p = new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Additive
            };

            Time.timeScale = 0;
            pause = SceneManager.LoadScene("Scenes/PauseMenu", p);
            
        }
        else if (isPause && Input.GetKeyDown(KeyCode.Escape)&& type == LoadedType.TYPE_LEVEL)
        {
            OnContinueButton();
        }
        else if (pause.isLoaded && isPause)
        {
            GameObject[] glist = pause.GetRootGameObjects();

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
                            
                            butList[j].onClick.AddListener(OnContinueButton);
                        }
                        else if (butList[j].name == "Retry")
                        {
                            butList[j].onClick.AddListener(OnRetryButton);
                        }
                        else if (butList[j].name == "Levels")
                        {
                            butList[j].onClick.AddListener(OnLevelsButton);
                        }
                        else if (butList[j].name == "Menu")
                        {
                            butList[j].onClick.AddListener(OnMenuButton);
                        }

                    }
                }
            }
        }

        if (levelEnded && !EndMenuLoaded)
        {
            EndMenuLoaded = true;
            LoadSceneParameters p = new LoadSceneParameters
            {
                loadSceneMode = LoadSceneMode.Additive
            };

            Time.timeScale = 0;
            end = SceneManager.LoadScene("Scenes/End", p);
        }
        else if (EndMenuLoaded && end.isLoaded)
        {
            GameObject[] glist = end.GetRootGameObjects();

            for (int i = 0; i < glist.Length; i++)
            {
                GameObject obj = glist[i];
      
                if (obj.CompareTag("Canvas"))
                {
                    Button[] butList = obj.GetComponentsInChildren<Button>();
                    for (int j = 0; j < butList.Length; j++)
                    {
                        if (butList[j].name == "Replay")
                        {
                            butList[j].onClick.AddListener(OnRetryButton);
                        }
                        else if (butList[j].name == "Levels")
                        {
                            butList[j].onClick.AddListener(OnLevelsButton);
                        }
                        else if (butList[j].name == "Menu")
                        {
                            butList[j].onClick.AddListener(OnMenuButton);
                        }

                    }
                }
            }
        }
       
    }
    public void OnContinueButton()
    {
        if(!isPause)
            return;
        Debug.Log("vvvvvv");
        isPause = false;
        SceneManager.UnloadSceneAsync(pause);
        Time.timeScale = 1;
    }

    public void OnRetryButton()
    {
        Debug.Log	("replay");
        Scene sceneToUnload = pause	;
        if (levelEnded)
        {
            levelEnded = false;
            EndMenuLoaded = false;
            sceneToUnload = end;
        }
        Time.timeScale = 1;
        SceneManager.UnloadSceneAsync(sceneToUnload);
        isPause = false;
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

    public void OnLevelsButton()
    {
        
        if(type == LoadedType.TYPE_MAIN && mainAnim	!= null)
            mainAnim.SetBool("PressedLevels", true);
        else
        {
            string name = currentScene.name;
            if(op == null)
            {
                op = SceneManager.LoadSceneAsync(1);
                SceneManager.UnloadSceneAsync(name);
            }
            
            type = LoadedType.TYPE_MAIN	;
            Time.timeScale = 1;
            loadedMain = false;
            isPause = false;
            
            hasToGoToLevelMenu = true;
        }
    }

    public void OnMenuButton()
    {
        if (type == LoadedType.TYPE_MAIN)
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
            type = LoadedType.TYPE_MAIN	;
            hasToGoToLevelMenu = false;
        }
    }
    public void OnControlsButton()
    {
        mainAnim.SetBool("PressedControls", true);
    }
    public void OnPrevButton()
    {
        mainAnim.SetBool("Previous", true);
    }
    public void OnNextButton()
    {
        mainAnim.SetBool("Next", true);
    }
    public void OnCreditsButton()
    {
        mainAnim.SetBool("PressedCredits", true);
    }

    public void OnBackButton()
    {
        mainAnim.SetBool("Goback", true);
    }

    public void LaunchLevel1()
    {
        type = LoadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(2);
        loadedMain = false;
    }
    public void LaunchLevel2()
    {
        type = LoadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(3);
        loadedMain = false;
    }
    public void LaunchLevel3()
    {
        type = LoadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(4);
        loadedMain = false;
    }
    public void LaunchLevel4()
    {
        type = LoadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(5);
        loadedMain = false;
    }
    public void LaunchLevel5()
    {
        type = LoadedType.TYPE_LEVEL;
        mainAnim = null;
        SceneManager.LoadScene(6);
        loadedMain = false;
    }
    public void OnExitButton()
    {
        Application.Quit();
    }
}
