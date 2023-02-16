using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public enum MainMenuItem
    {
        PanoVr,
        ExperienceVr
    }
    public void ChooseItem(MainMenuItem item)
    {
        switch (item)
        {
            case MainMenuItem.PanoVr:
                SceneManager.LoadScene("PanoVr");
                break;

            case MainMenuItem.ExperienceVr:
                SceneManager.LoadScene("ExperienceVr");
                break;
        }
    }
}
