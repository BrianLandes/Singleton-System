
using UnityEditor;
using UnityEngine;

namespace SingletonSystem {
	public static class CreateSingletonAssetMenu {

		const string InitialNewSingletonName = "NewSingleton.cs";

		[MenuItem("Assets/Create/Singleton Class", false, 120)]
		private static void CreateNewSingletonScript() {

			if ( TryFindNewSingletonTemplatePath(out var templatePath)) {

				ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, InitialNewSingletonName);

				// A line of code right here will run even before the user has finished naming the script
				// We'll create the Singleton asset in resources when assets get reimported, see SingletonAssetPostprocessor.cs

			}

		}

		private static bool TryFindNewSingletonTemplatePath(out string templatePath ) {
			string[] guids = AssetDatabase.FindAssets("SingletonTemplate.cs");
			if (guids.Length == 0) {
				Debug.LogWarning("SingletonTemplate.cs.txt not found in asset database");
				templatePath = null;
				return false;
			}
			templatePath = AssetDatabase.GUIDToAssetPath(guids[0]);
			return true;
		}
	}
}