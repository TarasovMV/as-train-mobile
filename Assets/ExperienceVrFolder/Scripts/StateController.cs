using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class StateController : MonoBehaviour {

    public enum SceneStack
    {
        Tank = 1,
        GasSeparator = 2,
        RectificationScheme = 3,
        CompressorScheme = 4,
        Pump = 5,
    }

    delegate bool Condition(GameObject item);

    public SceneStack CurrentStage;
    public Transform CameraTransform;
    public Transform ControllerTransform;

    private ExperienceVrController experienceVrController;
    private SignalrConnector signalrConnector;



    void Start () {
        Application.targetFrameRate = 60;
        experienceVrController = GameObject.Find("Scripts")?.GetComponent<ExperienceVrController>();
        signalrConnector = GameObject.Find("SocketService")?.GetComponent<SignalrConnector>();
    }

    void FixedUpdate()
    {
        sendStates();
    }

    private ExperienceVrState getState()
    {
        var vrState = new ExperienceVrState();
        vrState.CameraTransform = new StateTransorm(CameraTransform);
        vrState.ControllerTransform = new StateTransorm(ControllerTransform);
        vrState.MainGoTransform = getMainGoTransform();
        vrState.Scene = experienceVrController?.currentScene?.id ?? 0;
        vrState.UseMovableItems = getConditionMovableItems(isUse);
        vrState.ChooseMovableItems = getConditionMovableItems(isChoose);
        vrState.CursorMovableItems = getConditionMovableItems(isCursor);
        vrState.ZoneItems = getZoneObjects();
        vrState.CursorZonesIdx = getCursorZoneIdx();
        vrState.RestTimeVr = getRestTime();
        vrState.AllTimeVr = getAllTime();
        return vrState;
    }

    private StateTransorm getMainGoTransform()
    {
        var mainGo = GameObject.Find("System")?.GetComponent<commonData>()?.MainRotationObject ?? null;
        if (!mainGo || mainGo == null)
            return null;
        return new StateTransorm(mainGo.transform);
    }

    private void sendStates()
    {
        if (experienceVrController == null || !(signalrConnector?.isActiveAndEnabled ?? false) || GameObject.Find("System")?.GetComponent<commonData>() == null)
        {
            return;
        }
        var state = getState();
        signalrConnector.SetVrStates(state);
    }

    private List<int> getZoneObjects()
    {
        return GameObject.Find("System")?.GetComponent<commonData>()?.massPoints?.Select(x => x?.GetComponent<IGreenField>()?.chooseNum ?? -1)?.ToList() ?? new List<int>();
    }
    
    private List<int> getConditionMovableItems(Condition condition)
    {
        var conditionMovableItems = new List<int>();
        var movableItems = GameObject.Find("MovableItems");
        int idx = 0;
        foreach(Transform item in movableItems.transform)
        {
            if (condition(item.gameObject))
                conditionMovableItems.Add(idx);
            idx++;
        }
        return conditionMovableItems;
    }

    private int getCursorZoneIdx()
    {
        int idx = 0;
        var zoneCursorStates = GameObject.Find("System")?.GetComponent<commonData>()?.massPoints?.Select(x => x?.GetComponent<IGreenField>()?.isCursor ?? false) ?? new bool[0];
        foreach (var state in zoneCursorStates)
        {
            if (state)
                return idx;
            idx++;
        }
        return -1;
    }

    private int getRestTime()
    {
        return experienceVrController?.currentSet?.restTime ?? 0;
    }

    private int getAllTime()
    {
        return experienceVrController?.currentSet?.allTime ?? 0;
    }

    bool isUse(GameObject item)
    {
        return item.GetComponent<IChooseItem>().isUse;
    }

    bool isChoose(GameObject item)
    {
        return item.GetComponent<IChooseItem>().isChoose;
    }

    bool isCursor(GameObject item)
    {
        return item.GetComponent<IChooseItem>().isCursor;
    }
}

[Serializable]
public class ExperienceVrState
{
    public int Scene;
    public StateTransorm CameraTransform;
    public StateTransorm ControllerTransform;
    public StateTransorm MainGoTransform;
    public List<int> UseMovableItems;
    public List<int> ChooseMovableItems;
    public List<int> CursorMovableItems;
    public List<int> ZoneItems;
    public int CursorZonesIdx;
    public int RestTimeVr;
    public int AllTimeVr;
}

[Serializable]
public class StateTransorm
{
    public StateTransorm(Transform t)
    {
        position = new List<float> { t.position.x, t.position.y, t.position.z };
        rotation = new List<float> { t.rotation.x, t.rotation.y, t.rotation.z, t.rotation.w };
    }
    public List<float> position;
    public List<float> rotation;
}