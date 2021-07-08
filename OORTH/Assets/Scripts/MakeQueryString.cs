using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;
using Unitilities;

namespace Fungus {
	/// <summary>
	/// Clean a string from all kinds of stuff.
	/// </summary>
	[CommandInfo("AI",
				 "Make a Query String",
				 "Clean a string from all kinds of stuff.")]
	[AddComponentMenu("")]
	[ExecuteInEditMode]
	public class MakeQueryString : Command {

		[Tooltip("The result string with the prepended dialog.")]
		[VariableProperty(typeof(StringVariable))]
		[SerializeField] protected StringVariable queryString;

		[Tooltip("Number of entries to include.")]
		[SerializeField] protected int numEntries;

		[Tooltip("The prefix to append at the start.")]
		[SerializeField] protected string prefix;

		[Tooltip("The postfix to append at the end.")]
		[SerializeField] protected string postfix;

		[Tooltip("The stop sequence.")]
		[SerializeField] protected string stopSequence;

		[Tooltip("The stop sequence trigger (a name).")]
		[SerializeField] protected string stopSequenceTrigger;

		#region Public members

		public override void OnEnter() {

			if (queryString == null) {
				Continue();
				return;
			}

			string cleanedString = "";

			List<NarrativeLogEntry> entries = FungusManager.Instance.NarrativeLog.GetHistory();

			int count = entries.Count;
			int skip = count - Mathi.Min(entries.Count, numEntries);

			for (int i = skip; i < count; ++i) {
				if (entries[i].name != null && entries[i].name.Length > 0) cleanedString += entries[i].name + ": ";
				cleanedString += entries[i].text + "\n";

                if (entries[i].name == stopSequenceTrigger) cleanedString += stopSequence + "\n";
			}

			queryString.Value = "";

			if (prefix.Length > 0) queryString.Value += prefix + "\n\n";
            if (stopSequence.Length > 0) queryString.Value += stopSequence + "\n";

			queryString.Value += cleanedString + postfix;

			Continue();
		}

		public override string GetSummary() {
			if (queryString == null) {
				return "Error: No string variable specified";
			}
			return queryString.Key;
		}

		#endregion
	}
}