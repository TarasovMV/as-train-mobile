using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static ExperienceVrButtonComponent;
using static StateController;

public class ExperienceVrController : MonoBehaviour {
    public enum ScenePosition
    {
        Default,
        First,
        Middle,
        Last,
        Single
    }
    public GameObject MenuGo;
    public GameObject SceneContainer;
    public GameObject ScenePrefabContainer;

    public TestingVrHelmet currentSet;
    public TestingVrHelmetQuestion currentScene;
    public ScenePosition currentScenePosition = ScenePosition.Default;

    public GameObject PopupPanel;

    [Header("Переключатели сцены")]
    public GameObject NextButton;
    public GameObject PreviousButton;
    public GameObject FinishButton;

    [Header("Объекты VR сцены")]
    public Image TimerFill;
    public Text TimerValue;

    [Header("Префабы VR сцен")]
    public GameObject TankScene;
    public GameObject GasSeparatorScene;
    public GameObject RectificationSchemeScene;
    public GameObject CompressorSchemeScene;
    public GameObject PumpScene;

    private SignalrConnector signalrConnector;

    private static List<TestingVrHelmetSet> defaultScenes = new List<TestingVrHelmetSet>
    {
        new TestingVrHelmetSet
        {
            prepearedScene = PrepearedScenes.Set1,
            testingVrHelmet = new TestingVrHelmet
            {
                allTime = 1800,
                restTime = 1800,
                questions = new List<TestingVrHelmetQuestion>
                {
                    new TestingVrHelmetQuestion
                    {
                        id = 2,
                        title = ""
                    },
                    new TestingVrHelmetQuestion
                    {
                        id = 3,
                        title = ""
                    },
                }
            }
        },
        new TestingVrHelmetSet
        {
            prepearedScene = PrepearedScenes.Set2,
            testingVrHelmet = new TestingVrHelmet
            {
                allTime = 1800,
                restTime = 1800,
                questions = new List<TestingVrHelmetQuestion>
                {
                    new TestingVrHelmetQuestion
                    {
                        id = 1,
                        title = ""
                    },
                    new TestingVrHelmetQuestion
                    {
                        id = 5,
                        title = ""
                    },
                }
            }
        },
        new TestingVrHelmetSet
        {
            prepearedScene = PrepearedScenes.Set3,
            testingVrHelmet = new TestingVrHelmet
            {
                allTime = 1800,
                restTime = 1800,
                questions = new List<TestingVrHelmetQuestion>
                {
                    new TestingVrHelmetQuestion
                    {
                        id = 4,
                        title = ""
                    },
                    new TestingVrHelmetQuestion
                    {
                        id = 5,
                        title = ""
                    },
                }
            }
        },
        new TestingVrHelmetSet
        {
            prepearedScene = PrepearedScenes.Set4,
            testingVrHelmet = new TestingVrHelmet
            {
                allTime = 1800,
                restTime = 1800,
                questions = new List<TestingVrHelmetQuestion>
                {
                    new TestingVrHelmetQuestion
                    {
                        id = 2,
                        title = ""
                    },
                    new TestingVrHelmetQuestion
                    {
                        id = 5,
                        title = ""
                    },
                }
            }
        },
    };

    private Coroutine timerCoroutine = null;
    private Coroutine menuCoroutine = null;

    private void Start()
    {
        //BackMenu();
        enableMenu();
        signalrConnector = GameObject.Find("SocketService")?.GetComponent<SignalrConnector>();
    }

    public void SetPrepearedScene(PrepearedScenes prepearedScene)
    {
        var set = defaultScenes.FirstOrDefault(x => x.prepearedScene == prepearedScene)?.testingVrHelmet ?? null;
        if (set == null)
            return;
        SetScene(set);
    }

    public void SetScene(TestingVrHelmet set)
    {
        PopupPanel.SetActive(false);
        disableMenu();
        clearScenesContainer();
        currentSet = JsonUtility.FromJson<TestingVrHelmet>(JsonUtility.ToJson(set)); // copy
        enableVrScene();
        instantiateScenes(currentSet.questions);
        activateSceneByIdx(0);
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(TimerCoroutine());
        signalrConnector?.StartVrView();
    }

    public void BackMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void BackMenu()
    {
        PopupPanel.SetActive(false);
        disableVrScene();
        enableMenu();
    }

    public void ActivateSceneNext()
    {
        int idx = getCurrentSceneIdx(currentScene) + 1;
        if (idx >= currentSet.questions.Count)
        {
            return;
        }
        activateSceneByIdx(idx);
    }

    public void ActivateScenePrevious()
    {
        int idx = getCurrentSceneIdx(currentScene) - 1;
        if (idx < 0)
        {
            return;
        }
        activateSceneByIdx(idx);
    }

    public void ActivateSceneFinish()
    {
        saveCurrentSceneResult();
        SceneContainer.SetActive(false);
        PopupPanel.SetActive(true);
        PopupPanel.GetComponent<ExperienceVrPopupPanel>().Init(
            ExperienceVrPopupPanel.ExperienceVrPopupPanelType.User,
            () => finishScene(),
            () => {
                SceneContainer.SetActive(true);
                activateSceneByIdx(getCurrentSceneIdx(currentScene));
                PopupPanel.SetActive(false);
            },
            "Вы уверены что хотите завершить испытание?"
        );
    }

    private void timeComplete()
    {
        saveCurrentSceneResult();
        SceneContainer.SetActive(false);
        PopupPanel.SetActive(true);
        PopupPanel.GetComponent<ExperienceVrPopupPanel>().Init(
            ExperienceVrPopupPanel.ExperienceVrPopupPanelType.Time,
            () => finishScene(),
            null,
            "К сожалению, Ваше время истекло."
        );
    }

    private void finishScene()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        string result = getResultCode();
        PopupPanel.SetActive(true);
        PopupPanel.GetComponent<ExperienceVrPopupPanel>().Init(
            ExperienceVrPopupPanel.ExperienceVrPopupPanelType.Time,
            () => BackMenu(),
            null,
            $"Введите проверочный код:\n{result}.",
            5
        );
        signalrConnector.SendResult(result);
    }

    private void clearScenesContainer()
    {
        foreach (Transform child in ScenePrefabContainer.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void enableScene(GameObject activeScene)
    {
        foreach (Transform child in ScenePrefabContainer.transform)
        {
            child.gameObject.SetActive(false);
        }
        activeScene.SetActive(true);
    }

    private GameObject instantiateScene(SceneStack scene)
    {
        GameObject go = null;
        switch (scene)
        {
            case SceneStack.Tank:
                go = Instantiate(TankScene);
                break;
            case SceneStack.GasSeparator:
                go = Instantiate(GasSeparatorScene);
                break;
            case SceneStack.RectificationScheme:
                go = Instantiate(RectificationSchemeScene);
                break;
            case SceneStack.CompressorScheme:
                go = Instantiate(CompressorSchemeScene);
                break;
            case SceneStack.Pump:
                go = Instantiate(PumpScene);
                break;
        }
        go.transform.SetParent(ScenePrefabContainer.transform);
        return go;
    }

    private void activateSceneByIdx(int idx)
    {
        if (idx < 0 || !(currentSet?.questions?.Count > 0) || idx >= currentSet.questions.Count)
        {
            return;
        }
        saveCurrentSceneResult();
        currentScene = currentSet.questions[idx];
        enableScene(currentScene.scene);
        if (currentSet.questions.Count == 1) {
            currentScenePosition = ScenePosition.Single;
        }
        else if (idx == 0 && idx != currentSet.questions.Count - 1)
        {
            currentScenePosition = ScenePosition.First;
        }
        else if (idx == currentSet.questions.Count - 1)
        {
            currentScenePosition = ScenePosition.Last;
        }
        else
        {
            currentScenePosition = ScenePosition.Middle;
        }
        setSceneControllerButtons(currentScenePosition);
    }

    private void setSceneControllerButtons(ScenePosition scenePosition)
    {
        switch (scenePosition)
        {
            case ScenePosition.First:
                NextButton.SetActive(true);
                PreviousButton.SetActive(false);
                FinishButton.SetActive(false);
                break;
            case ScenePosition.Middle:
                NextButton.SetActive(true);
                PreviousButton.SetActive(true);
                FinishButton.SetActive(false);
                break;
            case ScenePosition.Last:
                NextButton.SetActive(false);
                PreviousButton.SetActive(true);
                FinishButton.SetActive(true);
                break;
            case ScenePosition.Single:
                NextButton.SetActive(false);
                PreviousButton.SetActive(false);
                FinishButton.SetActive(true);
                break;
            default:
                NextButton.SetActive(false);
                PreviousButton.SetActive(false);
                FinishButton.SetActive(false);
                break;
        }
    }

    private void instantiateScenes(List<TestingVrHelmetQuestion> questions)
    {
        questions.ForEach(q =>
        {
            q.scene = instantiateScene((SceneStack)q.id);
            q.scene.SetActive(false);
        });
    } 

    private int getCurrentSceneIdx(TestingVrHelmetQuestion scene)
    {
        return currentSet.questions.FindIndex(x => x == scene);
    }

    private void saveCurrentSceneResult()
    {
        if (currentScene == null)
            return; 
        currentScene.score = GameObject.Find("System")?.GetComponent<commonData>()?.countScore() ?? 0;
    }

    private void enableMenu()
    {
        menuCoroutine = StartCoroutine(DelayCoroutine(() => MenuGo.SetActive(true)));
    }

    private void disableMenu()
    {
        if (menuCoroutine != null)
        {
            StopCoroutine(menuCoroutine);
        }
        MenuGo.SetActive(false);
    }

    private void enableVrScene()
    {
        SceneContainer.SetActive(true);
    }

    private void disableVrScene()
    {
        clearScenesContainer();
        SceneContainer.SetActive(false);
    }

    private string getResultCode()
    {
        // code = concat(9 - (true answer)) * 5
        int allScore = 0;
        for(int i = 0; i < currentSet.questions.Count; i++)
        {
            allScore += (int)Mathf.Pow(10, currentSet.questions.Count - i - 1) * (9 - currentSet.questions[i].score);
        }
        allScore *= 5;
        return allScore.ToString();
    }

    IEnumerator DelayCoroutine(System.Action action)
    {
        yield return new WaitForSeconds(1);
        action?.Invoke();
    }

    IEnumerator TimerCoroutine()
    {
        while (true)
        {
            if (--currentSet.restTime <= 0)
            {
                timeComplete();
                break;
            }
            else
            {
                TimerValue.text = $"{ currentSet.restTime / 60 }:{ (currentSet.restTime % 60 < 10 ? "0" : "") }{ currentSet.restTime % 60 }";
                TimerFill.fillAmount = 1 - (float)currentSet.restTime / currentSet.allTime;
            }
            yield return new WaitForSecondsRealtime(1);
        }
    }
}
