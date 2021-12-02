
using System.Collections.Generic;
using UnityEditor;

namespace SingletonSystem {

	/// <summary>
	/// Static class that builds and returns Unity.SettingsProvider that display the Singleton objects in the Project Settings.
	/// </summary>
	public static class SingletonSettingsProviderBuilder {


		[SettingsProviderGroup]
		public static SettingsProvider[] BuildSettingsProvidersForSingletons() {

			SingletonEditorUtilities.CheckAllSingletonTypesForSingletonObjects();

			var providers = new List<SettingsProvider>();

			foreach (var type in SingletonEditorUtilities.GetAllSingletonTypes()) {

				if (SingletonEditorUtilities.TryLoadSingletonOfType(type, out var singletonObject)) {

					string projectSettingsPath = singletonObject.GetProjectSettingsPath();

					var provider = new SingletonSettingsProvider(projectSettingsPath, singletonObject, SettingsScope.Project);

					providers.Add(provider);
				}
			}

			return providers.ToArray();
		}


	}
}