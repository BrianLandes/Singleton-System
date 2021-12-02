
using UnityEditor;
using UnityEngine.UIElements;

namespace SingletonSystem {

	/// <summary>
	/// Wrapper for a Singleton object, allowing it to be displayed in the Project Settings.
	/// </summary>
	public class SingletonSettingsProvider : SettingsProvider {

		public SingletonBase singleton;

		private Editor editor;

		public SingletonSettingsProvider(string path, SingletonBase singleton, SettingsScope scope = SettingsScope.User) : base(path, scope) {
			this.singleton = singleton;
		}

		public override void OnActivate(string searchContext, VisualElement rootElement) {
			// This function is called when the user clicks on the MyCustom element in the Settings window.
			editor = Editor.CreateEditor(singleton);
		}

		public override void OnGUI(string searchContext) {
			editor.OnInspectorGUI();
		}

		public override void OnDeactivate() {
			AssetDatabase.SaveAssets();
			base.OnDeactivate();
		}
	}
}