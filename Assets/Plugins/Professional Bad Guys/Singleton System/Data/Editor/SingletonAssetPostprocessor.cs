
using UnityEditor;

namespace SingletonSystem {

	/// <summary>
	/// Implements AssetPostprocessor.OnPostprocessAllAssets() in order to check for and
	/// create any Singleton objects that haven't been made yet.
	/// </summary>
	public class SingletonAssetPostprocessor : AssetPostprocessor {

		private static void OnPostprocessAllAssets(
				string[] importedAssets,
				string[] deletedAssets,
				string[] movedAssets,
				string[] movedFromAssetPaths) {

			EditorApplication.update += SingletonEditorUtilities.CheckAllSingletonTypesForSingletonObjects;

			//SingletonEditorUtilities.CheckAllSingletonTypesForSingletonObjects();
		}
	}
}