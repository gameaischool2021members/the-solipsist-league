using UnityEngine;
using Fungus;

[CommandInfo("Variable",
				 "Set If Found",
				 "Sets a Boolean if a substring is found in a variable.")]
[AddComponentMenu("")]
public class SetVariableIfSubstring : Command {
	[Tooltip("The bool to set to true if found.")]
	[VariableProperty(typeof(BooleanVariable))]
	[SerializeField] protected BooleanVariable resultBool;

	[Tooltip("The variable containing the string to test.")]
	[VariableProperty(typeof(StringVariable))]
	[SerializeField] protected StringVariable testString;

	[Tooltip("The strings to search for in the test string.")]
	[SerializeField] protected string[] searchStrings;

	public override void OnEnter() {
		if (resultBool == null || testString == null || searchStrings == null || searchStrings.Length <= 0) {
			Debug.Log("Not all variables are set!");
			Continue();
			return;
		}

		foreach (var searchString in searchStrings) {
			if (testString.Value.Contains(searchString)) resultBool.Value = true;
		}

		Continue();
	}
}