using UnityEngine;
using Fungus;

[CommandInfo("Variable",
				 "Set Variable From Input",
				 "Sets a Boolean, Integer, Float or String variable from Input Dialog")]
[AddComponentMenu("")]
public class SetVariableFromInput : Command {
	// [Tooltip("The variable whos value will be set")]
	// [VariableProperty(typeof(BooleanVariable),
	// 					  typeof(IntegerVariable),
	// 					  typeof(FloatVariable),
	// 					  typeof(StringVariable))]
	// public Variable Variable;

	[Tooltip("The string variable to write into.")]
	[VariableProperty(typeof(StringVariable))]
	[SerializeField] protected StringVariable resultString;

	public string Label = "";
	public string Placeholder = "";

	public override void OnEnter() {
		InputDialog inputDialog = InputDialog.GetInputDialog();

		if (inputDialog == null || resultString == null) {
			Debug.Log("Result string not set!");
			Continue();
			return;
		}

		inputDialog.gameObject.SetActive(true);
		inputDialog.InitializeInputField(Label, Placeholder, OnInputSubmit);


	}

	private void OnInputSubmit(string value) {
		// if (Variable.GetType() == typeof(BooleanVariable)) {
		// 	BooleanVariable boolVar = (Variable as BooleanVariable);

		// 	if (value.ToLower() == "true" || value == "1") {
		// 		boolVar.value = true;
		// 	} else {
		// 		boolVar.value = false;
		// 	}

		// } else if (Variable.GetType() == typeof(IntegerVariable)) {
		// 	IntegerVariable intVar = (Variable as IntegerVariable);

		// 	int intResult;
		// 	if (int.TryParse(value, out intResult)) {
		// 		intVar.value = intResult;
		// 	}
		// 	intVar.value = 0;
		// } else if (Variable.GetType() == typeof(FloatVariable)) {
		// 	FloatVariable floatVariable = (Variable as FloatVariable);

		// 	float floatResult;
		// 	if (float.TryParse(value, out floatResult)) {
		// 		floatVariable.value = floatResult;
		// 	}
		// 	floatVariable.value = 0f;
		// } else if (Variable.GetType() == typeof(StringVariable)) {
		// 	StringVariable stringVar = (Variable as StringVariable);
		// 	stringVar.value = value;
		// }

		resultString.Value = value;

		InputDialog inputDialog = InputDialog.GetInputDialog();
		inputDialog.gameObject.SetActive(false);

		Continue();
	}
}