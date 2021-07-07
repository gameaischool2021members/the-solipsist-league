using UnityEngine;
using UnityEngine.UI;
using OpenAi.Unity.V1;

namespace OpenAi.Sherlock {
	public class OpenAISherlock : MonoBehaviour {
		public InputField Input;
		public Text Output;

		public void DoApiCompletion() {
			string text = Input.text;

			if (string.IsNullOrEmpty(text)) {
				Debug.LogError("Example requires input in input field");
				return;
			}

			Debug.Log("Asking Sherlock...");

			Output.text = "Let me think about this ...";

			OpenAiCompleterV2.Instance.Complete(
				text,
				s => Output.text = s,
				e => Output.text = $"ERROR: StatusCode: {e.StatusCode}"
			);
		}

		public void QuitApp() {
			Application.Quit();
		}
	}
}