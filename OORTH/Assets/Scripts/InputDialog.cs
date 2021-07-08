using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using Fungus;

public class InputDialog : MonoBehaviour {
	// Currently active Input Dialog used to display Menu options
	public static InputDialog activeInputDialog;

	public InputField inputField;
	public Text Label;
	public Button SubmitButton;
	public Text progressText;
	public int limit = 70;

	public Color validColour = Color.white;
	private Color defaultColour;

	public bool mustBeQuestion = false;

	protected Action<string> Callback;

	public static InputDialog GetInputDialog() {
		if (activeInputDialog == null) {
			// Use first Input Dialog found in the scene (if any)
			InputDialog id = GameObject.FindObjectOfType<InputDialog>();
			if (id != null) {
				activeInputDialog = id;
			}

			if (activeInputDialog == null) {
				// Auto spawn a menu dialog object from the prefab
				GameObject prefab = Resources.Load<GameObject>("InputDialog");
				if (prefab != null) {
					GameObject go = Instantiate(prefab) as GameObject;
					go.SetActive(false);
					go.name = "InputDialog";
					activeInputDialog = go.GetComponent<InputDialog>();
				}
			}
		}

		return activeInputDialog;
	}

	public virtual void Awake() {
		defaultColour = inputField.textComponent.color;
		if (Application.isPlaying) {
			Clear();
		}
		Canvas.ForceUpdateCanvases();
	}

	public virtual void OnEnable() {
		// The canvas may fail to update if the input dialog is enabled in the first game frame.
		// To fix this we just need to force a canvas update when the object is enabled.
		Canvas.ForceUpdateCanvases();
	}

	public virtual void Clear() {
		StopAllCoroutines();

		if (inputField != null) {
			inputField.onEndEdit.RemoveAllListeners();
			inputField.onValueChanged.RemoveAllListeners();
			inputField.text = "";
			inputField.textComponent.color = defaultColour;
		}

        if (progressText != null) {
			progressText.text = "0/" + limit;
		}
	}

	public virtual void InitializeInputField(string label, string placeholder, Action<string> callback) {
		// If inputfield is not set send empty input and continue
		Callback = callback;

		if (inputField == null) {
			Callback("");
			return;
		} else {
			EventSystem.current.SetSelectedGameObject(inputField.gameObject);
            inputField.ActivateInputField();
			inputField.Select();

			Text placeholderText = inputField.placeholder.GetComponent<Text>();
			if (placeholderText != null) {
				placeholderText.text = placeholder;
			}

			inputField.onEndEdit.AddListener(OnEndEdit);
			inputField.onValueChanged.AddListener(OnValueChanged);
		}

		if (Label != null) {
			Label.text = label;
		}

		if (SubmitButton != null) {
			SubmitButton.onClick.AddListener(OnClickSubmit);
		}

		HideSayDialog();
	}

	public bool isValid(string text) {
        // true if it doesn't have to be a question or if it ends with a question mark. and if below limit.
		return text.Length > 0 && (!mustBeQuestion || text.LastIndexOf('?') == text.Length - 1) && text.Length <= limit;
	}

	public void OnValueChanged(string text) {
		string progress = text.Length + "/" + limit;
		if (progressText != null) {
			progressText.text = progress;
		}

		if (inputField != null) {
			inputField.textComponent.color = isValid(text) ? validColour : defaultColour;
		}
	}

	public virtual void OnEndEdit(string inputValue) {
		if (!Input.GetKey(KeyCode.Return) || !isValid(inputValue)) {
			inputField.ActivateInputField();
			inputField.Select();

			return;
		}

		if (inputValue != "") {
			Callback(inputValue);
			Clear();
		}

		SayDialog sayDialog = SayDialog.GetSayDialog();
		if (sayDialog != null) {
			sayDialog.SetActive(true);
		}
	}

	public virtual void OnClickSubmit() {
		OnEndEdit(inputField.text);
	}

	public virtual void HideSayDialog() {
		SayDialog sayDialog = SayDialog.GetSayDialog();
		if (sayDialog != null) {
			sayDialog.SetActive(false);
		}
	}

}
