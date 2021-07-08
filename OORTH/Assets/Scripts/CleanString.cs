
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

public static class Extensions {
	public static string Filter(this string str, List<char> charsToRemove) {
		foreach (char c in charsToRemove) {
			str = str.Replace(c.ToString(), string.Empty);
		}

		return str;
	}
}

namespace Fungus {
	/// <summary>
	/// Clean a string from all kinds of stuff.
	/// </summary>
	[CommandInfo("Scripting",
				 "Clean String",
				 "Clean a string from all kinds of stuff.")]
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class CleanString : Command {

		[Tooltip("The prompt string variable to read from.")]
		[VariableProperty(typeof(StringVariable))]
		[SerializeField] protected StringVariable cleanString;

		[Tooltip("Restrict to a single sentence?")]
		[SerializeField] protected bool singleSentence = false;

		#region Public members

		public override void OnEnter() {

			if (cleanString == null || cleanString.Value.Length == 0) {
				Continue();
				return;
			}

			List<char> charsToRemove = new List<char>() { '@', '"' };

			string cleanedString = cleanString.Value;

			cleanedString.Trim();
			// cleanedString.Replace("\\\\\\\\", " ");
			cleanedString.Replace("  ", " ");
			cleanedString = cleanedString.Filter(charsToRemove);

			// cut everything past last period
			int lastIndex = cleanedString.LastIndexOf('.');

			if (lastIndex >= 0 && lastIndex < cleanedString.Length - 1) {
				cleanedString = cleanedString.Remove(lastIndex + 1);
			}

			// add missing spaces after punctuation
			List<int> punctuationIndices = new List<int>();
			int index = 0;
			foreach (char c in cleanedString) { 
				if (index < cleanedString.Length - 1 &&
					(c == '.' || c == '!' || c == '?' || c == ';' || c == ',' || c == ')')
					&& cleanedString[index+1] != ' ') punctuationIndices.Add(index);
				index = index + 1;
			}
			foreach (int i in punctuationIndices) {
				cleanedString.Insert(i + 1, " ");
			}

			// cut off multi-line returns
			if (cleanedString.Contains("\n\n")) {
				index = cleanedString.IndexOf("\n\n");
				cleanedString = cleanedString.Remove(index);
			}
			// cut off \\\\\\
			if (cleanedString.Contains("\\")) {
				index = cleanedString.IndexOf("\\");
				cleanedString = cleanedString.Remove(index);
			}
			if (singleSentence && cleanedString.Contains(".")) {
				index = cleanedString.IndexOf(".");
				cleanedString = cleanedString.Remove(index);
			}

			// append ellipsis if unfinished sentence
			if (!cleanedString.Contains(".")) cleanedString += "...";

			cleanString.Value = cleanedString;

			Continue();
		}

		public override string GetSummary() {
			if (cleanString == null) {
				return "Error: No string variable specified";
			}
			return cleanString.Key;
		}

        #endregion
	}
}