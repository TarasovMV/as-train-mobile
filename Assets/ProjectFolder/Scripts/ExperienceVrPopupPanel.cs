using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceVrPopupPanel : MonoBehaviour {
    public enum ExperienceVrPopupPanelType
    {
        Time,
        User
    }

    public GameObject EndButton;
    public GameObject ReturnButton;
    public Text TextMessage;

    private System.Action nextAction;
    private System.Action returnAction;

    public void Init(ExperienceVrPopupPanelType type, System.Action next, System.Action ret, string msg, int latencyTime = 0)
    {
        EndButton.SetActive(false);
        ReturnButton.SetActive(false);
        StartCoroutine(ActiveButtonsCoroutine(type, latencyTime));
        TextMessage.text = msg;
        nextAction = next;
        returnAction = ret;
    }

    IEnumerator ActiveButtonsCoroutine(ExperienceVrPopupPanelType type, int latensyTime)
    {
        yield return new WaitForSeconds(latensyTime);
        EndButton.GetComponent<dopMenuBtns>().action = nextAction;
        ReturnButton.GetComponent<dopMenuBtns>().action = returnAction;
        switch (type)
        {
            case ExperienceVrPopupPanelType.Time:
                EndButton.SetActive(true);
                break;
            case ExperienceVrPopupPanelType.User:
                EndButton.SetActive(true);
                ReturnButton.SetActive(true);
                break;
        }
    }
}
