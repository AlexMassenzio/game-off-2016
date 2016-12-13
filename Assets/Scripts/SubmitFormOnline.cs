using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SubmitFormOnline : MonoBehaviour
{
	public InputField textReference;
	public InputField fName;
	public Button cancelButton;
	public string pendingDir;

	// FormID comes from the google drive ID, eg:
	// https://docs.google.com/forms/d/_YOURFORMID_/edit
	private string formID = "1wCFYymcxvkIIyS-sZm0dMtgG13o0jgL_mgNT0-uWnds";

	// fieldID comes from the published feedback form. You can pull the value from the HTML source of the form:
	private const string feedbackFieldID = "entry.2036359856";
	private const string senderFieldID = "entry.878489457";
	private const string ipID = "entry.1411911730";

	private string ip;
	private bool lastError = false;

	IEnumerator SendForm(string n, string text)
	{
		cancelButton.interactable = false;
		// Prevent spamming of the button when the field is empty:
		if (string.IsNullOrEmpty(text)) yield break;

		WWW w;
		if (string.IsNullOrEmpty(ip))
		{
			// This fetches our external IP:
			w = new WWW("https://api.ipify.org/?format=text");
			yield return w;
			ip = w.text;
		}

		WWWForm form = new WWWForm();
		form.AddField(senderFieldID, n); // "Your Name" if you were doing the HTML entry.
		form.AddField(feedbackFieldID, text);
		form.AddField(ipID, ip);

		string url = "https://docs.google.com/forms/d/" + formID + "/formResponse";
		w = new WWW(url, form.data);
		yield return w;
		if (string.IsNullOrEmpty(w.error))
		{
			Debug.Log("Sent feedback :)");
			lastError = false;
		}
		else
		{
			Debug.Log("Failed to send feedback :( Stowing for later.");
			// Let's stow this feedback in a pending folder so we can retry later:
			//GetComponent<SaveFormToDisk>().SaveToFile(text, pendingDir);
			lastError = true;
		}

		cancelButton.interactable = true;
		Time.timeScale = 1;
		this.gameObject.SetActive(false);
	}

	/*IEnumerator SendPending()
	{
		yield return 0;
		string path = FileManager.dataPath + "/" + pendingDir;
		var info = new DirectoryInfo(path);
		if (!info.Exists)
		{
			// Folder doesn't exist, nothing to send.
			yield break;
		}

		var fileInfo = info.GetFiles();
		foreach (FileInfo file in fileInfo)
		{
			yield return StartCoroutine(SendForm(File.ReadAllText(file.FullName)));
			if (lastError)
			{
				// Failed to send, let's back out of this.
				yield break;
			}
			else
			{
				// All was good! We can delete the file.
				file.Delete();
			}
		}
	}

	// On startup try sending previous failures.
	void Start()
	{
		StartCoroutine(SendPending());
	}*/

	// Button activation from UI:
	public void Activate()
	{
		StartCoroutine(SendForm(fName.text, textReference.text));
		textReference.text = "";
	}

	public void Cancel()
	{
		Time.timeScale = 1;
		this.gameObject.SetActive(false);
	}
}