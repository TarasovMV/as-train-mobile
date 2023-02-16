using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class interface4 : MonoBehaviour {

    public GameObject timerObj;
    public Text timerText;
    float time;

    public GameObject btnContinue;
    public GameObject btnEnd;
    public GameObject btnBack;

    public GameObject scene1;
    public GameObject scene2;

    public GameObject tableEtapTime;
    public GameObject tableEtapEnd;

    public GameObject mainCamera;

    // Use this for initialization
    void Start ()
    {
        time = 5100;
        timerObj.GetComponent<Animation>().Play("timerGo");
	}
	
	// Update is called once per frame
	void Update ()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            int a = scene1.GetComponentInChildren<commonData>().countScore();
            int b = scene2.GetComponentInChildren<commonData>().countScore();

            int a1 = Random.Range(0, 9);
            int b1 = Random.Range(0, 9);

            string score = $"{ a1 }{ a }{ b1 }{ b }";
            PlayerPrefs.SetString("score", score);
            //завершение (табличка - ваше время вышло)
            tableEtapTime.SetActive(true);
        }
        else
        {
            if((time % 60) < 10)
            {
                timerText.text = ((int)(time / 60)).ToString() + ":0" + ((int)(time % 60)).ToString();
            }
            else
            {
                timerText.text = ((int)(time / 60)).ToString() + ":" + ((int)(time % 60)).ToString();
            }
            
            if(time >= 0 && time <= 180)
            {
                timerObj.GetComponent<Animation>().Blend("timerAlarm");
            }
        }


    }

    public void forward()
    {
        btnBack.SetActive(true);
        btnEnd.SetActive(true);
        btnContinue.SetActive(false);
        scene2.SetActive(true);
        scene1.SetActive(false);

        Vector3 oldPos = mainCamera.transform.rotation.eulerAngles;
        mainCamera.transform.rotation = Quaternion.Euler(oldPos.x, -120, oldPos.z);
    }

    public void back()
    {
        btnContinue.SetActive(true);
        btnBack.SetActive(false);
        btnEnd.SetActive(false);
        scene1.SetActive(true);
        scene2.SetActive(false);

        Vector3 oldPos = mainCamera.transform.rotation.eulerAngles;
        mainCamera.transform.rotation = Quaternion.Euler(oldPos.x, -120, oldPos.z);
    }

    public void end()
    {
        //завершение (табличка - желаете закончить 4 этап?)
        tableEtapEnd.SetActive(true);

        //составление кода
        int a = scene1.GetComponentInChildren<commonData>().countScore();
        int b = scene2.GetComponentInChildren<commonData>().countScore();

        int a1 = Random.Range(0, 9);
        int b1 = Random.Range(0, 9);

        string score = a1.ToString() + a.ToString() + b1.ToString() + b.ToString();
        PlayerPrefs.SetString("score", score);
    }
}
