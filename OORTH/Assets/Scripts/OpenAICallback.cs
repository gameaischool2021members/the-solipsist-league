// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

using UnityEngine;
using OpenAi.Unity.V1;

namespace Fungus {
	/// <summary>
	/// Sends a prompt and waits for 1 completion.
	/// </summary>
	[CommandInfo("AI",
				 "Query OpenAI",
				 "Sends a prompt and waits for 1 completion.")]
	[AddComponentMenu("")]
	public class OpenAICallback : Command {

		[Tooltip("The prompt string variable to read from.")]
		[VariableProperty(typeof(StringVariable))]
		[SerializeField] protected StringVariable promptString;

		[Tooltip("The result string variable to write into.")]
		[VariableProperty(typeof(StringVariable))]
		[SerializeField] protected StringVariable resultString;

		[Tooltip("A prefix for the prompt.")]
		[SerializeField] protected string prefix;

		[Tooltip("A postfix for the prompt.")]
		[SerializeField] protected string postfix;

		#region Public members

		public override void OnEnter() {
			if (promptString == null || resultString == null ||
                promptString.Value.Length == 0) {
				Continue();
				return;
			}

			string prompt = prefix + promptString.Value + postfix;

			OpenAiCompleterV2.Instance.Complete(
				prompt,
				s => { resultString.Value = s; Continue(); },
				e => resultString.Value = $"ERROR: StatusCode: {e.StatusCode}"
			);
		}

		public override string GetSummary() {
			if (promptString == null) {
				return "Error: No prompt variable specified";
			}
			if (resultString == null) {
				return "Error: No result variable specified";
			}
			if (OpenAiCompleterV2.Instance == null) {
				return "Error: Create an OpenAiCompleterV2 component!";
			}

			return promptString.Key;
		}

		public override Color GetButtonColor() {
			return new Color32(235, 191, 217, 255);
		}

		#endregion
	}
}