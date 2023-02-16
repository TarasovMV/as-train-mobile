using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ExperienceVrButtonComponent;

public class SignalrConnector : MonoBehaviour {
    List<System.Action> actionList = new List<System.Action>();
    private static HubConnection connection;
    private readonly string defaultUri = "http://";
    private string configPath => $"{Application.persistentDataPath}/config.txt";
    private string deviceId;
    private string connectedPc;

    void Awake()
    {
        Application.runInBackground = true;
        deviceId = SystemInfo.deviceUniqueIdentifier;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        Application.runInBackground = true;
        Debug.Log(configPath);
        if (!checkFileOrCreate())
        {
            Debug.LogWarning("Config not found");
            return;
        }
        var url = getConfigUrl();
        if (url.Length < 10)
        {
            Debug.LogWarning("Config not valid");
            return;
        }
        Debug.Log($"Config url: {url}");

        //url = "http://192.168.1.18:11001";
        connection = new HubConnectionBuilder()
            .WithUrl($"{url}/signalr")
            .Build();
        connection.Closed += async (error) =>
        {
            actionList.Add(new System.Action(() =>
            {
                setStatusText("Not connected");
            }));
            await Task.Delay(1 * 1000);
            await Connect();
        };

        Connect();
    }

    private string getConfigUrl()
    {
        string json = File.ReadAllText(configPath);
        Debug.Log(json);
        return JsonUtility.FromJson<ConfigSettings>(json).wsUrl;
    }

    private bool checkFileOrCreate()
    {
        if(!File.Exists(configPath))
        {
            var json = JsonUtility.ToJson(new ConfigSettings { wsUrl = defaultUri });
            using (StreamWriter sw = File.CreateText(configPath))
            {
                sw.Write(json);
            }
            return false;
        }
        return true;
    }

    private void LateUpdate()
    {
        threadHandler();
    }

    private async Task Connect()
    {
        connection.On<string>("Send", message => actionList.Add(new System.Action(() =>
        {
            Debug.Log($"msg: {message}");
        })));

        connection.On<int>("PanoActiveHub", idx => actionList.Add(new System.Action(() =>
        {
            Debug.Log($"active pano: {idx}");
            activatePano(idx);
        })));

        connection.On<string>("SetStatusText", msg => actionList.Add(new System.Action(() =>
        {
            Debug.Log($"user: {msg}");
            setStatusText(msg);
        })));

        connection.On("PanoMenu", () => actionList.Add(new System.Action(() =>
        {
            goToMenu();
        })));

        connection.On<string>("CloseVr", (msg) => actionList.Add(new System.Action(() =>
        {
            closeVr();
        })));

        connection.On<string, string>("StartVrHeadset", (sourceId, message) => actionList.Add(new System.Action(() =>
        {
            connectedPc = sourceId;
            startVrHeadset(message);
        })));

        try
        {
            await connection.StartAsync();
            actionList.Add(new System.Action(() =>
            {
                setStatusText("Connected");
                Debug.Log("Connection started status");
            }));
        }
        catch (System.Exception ex)
        {
            actionList.Add(new System.Action(() =>
            {
                Debug.Log(ex.Message);
            }));
        }
        await AddDevice();
        //await VrConnect();
        await Send("hello world!");
    }

    private async Task AddDevice()
    {
        exceptionHandler(async () => await connection.InvokeAsync("SetDeviceId", deviceId));
    }

    private async Task VrConnect()
    {
        exceptionHandler(async () => await connection.InvokeAsync("VrDeviceConnect", deviceId));
    }

    private async Task Send(string msg)
    {
        exceptionHandler(async () => await connection.InvokeAsync("Send", msg));
    }

    private async Task SetExperienceVrState(string msg)
    {
        exceptionHandler(async () => await connection.InvokeAsync("SetExperienceVrState", connectedPc, msg));
    }

    private async Task StartVrViewAsync()
    {
        exceptionHandler(async () => await connection.InvokeAsync("StartVrView", connectedPc));
    }

    private async Task SendResultAsync(string code)
    {
        exceptionHandler(async () => await connection.InvokeAsync("SendResult", connectedPc, code));
    }

    private void activatePano(int idx)
    {
        Debug.Log("activate pano enter");
        var sceneName = "PanoVR";
        Action panoAction = () => GameObject.Find("System")?.GetComponent<PanoController>()?.SelectPano(idx);
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            Debug.Log("scene comparasion true");
            panoAction?.Invoke();
            return;
        }
        Debug.Log("scene comparasion false");
        StartCoroutine(StartDelayCoroutine(sceneName, panoAction));
    }

    private void goToMenu()
    {
        GameObject.Find("System").GetComponent<PanoController>().BackMenu();
    }

    private void closeVr()
    {
        // вернуться в меню сцены vr
        GameObject.Find("Scripts")?.GetComponent<ExperienceVrController>()?.BackMenu();
    }

    private void startVrHeadset(string msg)
    {
        // перейти на сцену VR и сделать sets
        var headset = JsonUtility.FromJson<TestingVrHelmet>(msg);
        Action headsetAction = () => GameObject.Find("Scripts")?.GetComponent<ExperienceVrController>()?.SetScene(headset);
        if (SceneManager.GetActiveScene().name == "ExperienceVR")
        {
            headsetAction?.Invoke();
            return;
        }
        StartCoroutine(StartDelayCoroutine("ExperienceVR", headsetAction));
    }

    private void threadHandler()
    {
        foreach (var action in actionList)
        {
            action();
        }
        actionList.Clear();
    }

    private void exceptionHandler(System.Action callback)
    {
        try
        {
            callback?.Invoke();
        }
        catch (System.Exception ex)
        {
            actionList.Add(new System.Action(() =>
            {
                Debug.Log(ex.Message);
            }));
        }
    }

    private void setStatusText(string msg)
    {
        //GameObject.Find("StatusText").GetComponent<Text>().text = msg;
        var textComponent = GameObject.Find("StatusText")?.GetComponent<Text>() ?? null;
        if (textComponent == null)
            return;
        textComponent.text = msg;
    }

    public void SetVrStates(ExperienceVrState vrState)
    {
        var msg = JsonUtility.ToJson(vrState);
        //Debug.Log(msg);
        SetExperienceVrState(msg);
    }

    public void StartVrView()
    {
        Debug.Log("StartVrView");
        StartVrViewAsync();
    }

    public void SendResult(string code)
    {
        Debug.Log($"SendResult: {code}");
        SendResultAsync(code);
    }

    private IEnumerator StartDelayCoroutine(string scene, Action action)
    {
        SceneManager.LoadScene(scene);
        yield return new WaitForSeconds(1);
        action?.Invoke();
    }
}

public class ConfigSettings
{
    public string wsUrl;
    public string restUrl;
}

public class TestingVrHelmetSet
{
    public TestingVrHelmet testingVrHelmet;
    public PrepearedScenes prepearedScene;
}

[Serializable]
public class TestingVrHelmet
{
    public List<TestingVrHelmetQuestion> questions;
    public int restTime;
    public int allTime;

    public TestingVrHelmet()
    {
        questions = new List<TestingVrHelmetQuestion>();
    }
}

[Serializable]
public class TestingVrHelmetQuestion
{
    public int id;
    public string title;
    public int score;
    public GameObject scene;
}