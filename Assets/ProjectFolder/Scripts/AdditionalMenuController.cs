using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdditionalMenuController : MonoBehaviour {

	public Text CodeInput;
	public string code
    {
		get
        {
			return _code;
        }
		set
        {
			_code = value;
			CodeInput.text = _code;
			Debug.Log(_code);
		}
    }
	private string _code = "";
	private bool isBlock = false;
	private WebService webService => GameObject.Find("WebService").GetComponent<WebService>();

	void Start()
    {
		// code = "814";
        // GetUser();
    }

	public void ChooseItem(int number)
	{
		if (isBlock) return;
		if (number != -1) {
			code += number.ToString();
		}
		else if (code.Length > 0) {
			code = code.Remove(code.Length - 1, 1);
		}

		if (code.Length >= 3)
		{
			isBlock = true;
			GetUser();
		}
	}

	public void GetUser()
    {
		webService.Get<Participant>(
			$"additional/participant/{code}", 
			(res) => { 
				Debug.Log(res.id);
				PlayerPrefs.SetInt("participant-id", res.id);
				SceneManager.LoadScene("AdditionalVr");

            },
			() => { code = ""; isBlock = false; }
		);
    }
}
