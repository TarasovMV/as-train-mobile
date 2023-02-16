using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebService : MonoBehaviour {

	private readonly string defaultUrl = "http://192.168.1.6:44950/api";
	private string configPath => $"{Application.persistentDataPath}/config.txt";
	private string restUrl
	{
		get
		{
			var url = JsonUtility.FromJson<ConfigSettings>(File.ReadAllText(configPath)).restUrl;
			if (url?.Length > 5)
			{
				return url;
			}
			else
			{
				return defaultUrl;
			}
		}
	}

	void Awake () {
		DontDestroyOnLoad(this.gameObject);
	}

	public void Post<T>(string url, string body, System.Action<T> thenCallback, System.Action errorCallback)
	{
		StartCoroutine(PostCoroutine(url, body, thenCallback, errorCallback));
	}

	public void Get<T>(string url, System.Action<T> thenCallback, System.Action errorCallback)
	{
		StartCoroutine(GetCoroutine(url, thenCallback, errorCallback));
	}

	private IEnumerator GetCoroutine<T>(string url, System.Action<T> thenCallback, System.Action errorCallback)
	{
		Debug.Log($"get");
		Debug.Log($"{restUrl}/{url}");
		UnityWebRequest www = UnityWebRequest.Get($"{restUrl}/{url}");
		yield return www.SendWebRequest();
		if (www.isNetworkError || www.isHttpError)
		{
			Debug.Log(www.error);
			errorCallback?.Invoke();
		}
		else
		{
			Debug.Log(www.downloadHandler.text);
			thenCallback?.Invoke(JsonUtility.FromJson<T>(www.downloadHandler.text));
		}
	}

	private IEnumerator PostCoroutine<T>(string url, string body, System.Action<T> thenCallback, System.Action errorCallback)
	{
		var request = new UnityWebRequest($"{restUrl}/{url}", "POST");
		byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
		request.uploadHandler = new UploadHandlerRaw(bodyRaw);
		request.downloadHandler = new DownloadHandlerBuffer();
		request.SetRequestHeader("Content-Type", "application/json");

		yield return request.SendWebRequest();

		if (request.isNetworkError || request.isHttpError)
		{
			Debug.LogError(request.error);
			errorCallback?.Invoke();
		}
		else
		{
			Debug.Log(request.downloadHandler.text);
			thenCallback?.Invoke(JsonUtility.FromJson<T>(request.downloadHandler.text));
		}
	}
}
