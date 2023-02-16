using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanoController : MonoBehaviour
{
    public List<Material> PanoTextureList;
    public List<GameObject> PanoItemsList;
    public GameObject PanoUI;
    public GameObject PanoContainer;
    public GameObject MenuGo;
    public GameObject Background;
    public GameObject UiInfoGo;
    public Text UiInfoText;

    public int? currentIndex = null;

    private bool isOnline;

    private void Start()
    {
        BackMenu();
    }

    public void SelectPano(int idx)
    {
        if (currentIndex == idx)
        {
            return;
        }
        disablePanoItems();
        disableMenu();
        PanoUI.SetActive(true);
        PanoContainer.SetActive(true);
        PanoContainer.GetComponent<MeshRenderer>().material = PanoTextureList[idx];
        PanoItemsList[idx].SetActive(true);
    }

    public void BackMenu()
    {
        currentIndex = null;
        disablePanoItems();
        PanoUI.SetActive(false);
        PanoContainer.SetActive(false);
        PanoContainer.GetComponent<MeshRenderer>().material = null;
        Background.SetActive(true);
        MenuGo.SetActive(true);
        isOnline = false;
    }

    public void SetUiInfo(string msg)
    {
        UiInfoGo.SetActive(true);
        UiInfoText.GetComponent<Text>().text = msg;
    }

    public void DisableUiInfo()
    {
        UiInfoGo.SetActive(false);
    }

    private void disableMenu()
    {
        MenuGo.SetActive(false);
        Background.SetActive(false);
    }

    private void disablePanoItems()
    {
        PanoItemsList.ForEach(x => x.SetActive(false));
    }
}
