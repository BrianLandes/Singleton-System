
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SingletonSystem {

	public static class SingletonEditorUtilities {

		const string ResourcesPath = "Assets/Resources/Singletons";

		public static IEnumerable<Type> GetAllSingletonTypes() {

			bool AssemblyIsNotBuiltInOrDynamic(Assembly assembly) {
				if (assembly.IsDynamic) {
					return false;
				}
				if (assembly.FullName.StartsWith("System") ||
						assembly.FullName.StartsWith("mscorlib") ||
						assembly.FullName.StartsWith("netstandard")) {
					return false;
				}
				if (assembly.FullName.StartsWith("UnityEditor") ||
						assembly.FullName.StartsWith("UnityEngine") ||
						assembly.FullName.StartsWith("Unity.")) {
					return false;
				}

				return true;
			}

			var allAssemblies = AppDomain.CurrentDomain.GetAssemblies()
				.Where(assembly => AssemblyIsNotBuiltInOrDynamic(assembly));

			return allAssemblies
				.SelectMany(assembly => assembly.GetTypes())
				.Where(x => !x.IsAbstract)
				.Where(x => !x.IsGenericTypeDefinition)
				.Where(x => typeof(SingletonBase).IsAssignableFrom(x));
		}

		public static bool TryLoadSingletonOfType(Type type, out SingletonBase singleton) {

			if (TryLoadSingletonOfTypeUsingAssetDatabase(type, out singleton)) {
				return true;
			}
			//Debug.LogError($"Failed to load a Singleton asset of type {type.Name} using AssetDatabase");

			AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);

			if (TryLoadSingletonOfTypeUsingAssetDatabase(type, out singleton)) {
				return true;
			}
			//Debug.LogError($"Failed to load a Singleton asset of type {type.Name} using AssetDatabase even after Refresh");


			if (TryLoadSingletonOfTypeUsingResources(type, out singleton)) {
				return true;
			}
			//Debug.LogError($"Failed to load a Singleton asset of type {type.Name} using Resources");

			return false;
		}

		private static bool TryLoadSingletonOfTypeUsingAssetDatabase(Type type, out SingletonBase singleton) {
			string[] guids = AssetDatabase.FindAssets("t:" + type.Name);
			if (guids.Length == 0) {
				singleton = null;
				return false;
			}
			string path = AssetDatabase.GUIDToAssetPath(guids[0]);

			singleton = AssetDatabase.LoadAssetAtPath(path, type) as SingletonBase;
			return singleton != null;
		}

		private static bool TryLoadSingletonOfTypeUsingResources(Type type, out SingletonBase singleton) {
			var singletons = Resources.LoadAll<SingletonBase>(string.Empty);
			singleton = singletons.Where(s => type.IsAssignableFrom(s.GetType()))
				.FirstOrDefault();
			return singleton != null;
		}

		public static SingletonBase CreateNewSingletonObject(Type singletonType) {
			var newObject = ScriptableObject.CreateInstance(singletonType);

			newObject.name = singletonType.Name;

			// ensure the folder exists
			{
				var directoryAbsolutePath = PathUtilities.AsAbsolutePath(ResourcesPath);
				Directory.CreateDirectory(directoryAbsolutePath);
			}

			var path = PathUtilities.CreateNewAssetPath(ResourcesPath, singletonType.Name);

			AssetDatabase.CreateAsset(newObject, path);
			AssetDatabase.SaveAssets();

			return newObject as SingletonBase;
		}

		public static bool DisplayCreateSingletonObjectPrompt( Type singletonType ) {

			string title = "Singleton Object not found";
			string message = $"Could not find the Singleton object for {singletonType.Name}. " +
				$"Would you like to create one? " +
				$"This should only be necessary once, when a new Singleton class is made and will overwrite the existing one if it DOES exist.";

			return EditorUtility.DisplayDialog(title, message, "Yes", "Cancel");
		}

		public static void CheckAllSingletonTypesForSingletonObjects() {
			//AssetDatabase.Refresh();

			var missingSingletons = new List<Type>();

			foreach (var type in GetAllSingletonTypes()) {

				// Attempt to load the actual Singleton ScriptableObject in Resources
				if (!TryLoadSingletonOfType(type, out var singletonObject)) {
					//Debug.LogError($"Failed to find a Singleton asset of type {type.Name}");

					missingSingletons.Add(type);
				}
			}

			if ( missingSingletons.Count > 0 ) {

				//string title = "Singleton Object(s) not found";
				//string message = $"Could not find the Singleton object(s) for some singleton types. " +
				//	$"Would you like to create them? " +
				//	$"This should only be necessary once, when a new Singleton class is made and will overwrite the existing one if it DOES exist.";

				//if ( EditorUtility.DisplayDialog(title, message, "Yes", "Cancel")) {
				CreateSingletonAssetsForAnyMissingSingletonTypes();
				//}

			}

			EditorApplication.update -= CheckAllSingletonTypesForSingletonObjects;
		}

		public static void CreateSingletonAssetsForAnyMissingSingletonTypes() {

			foreach (var type in GetAllSingletonTypes()) {

				// Attempt to load the actual Singleton ScriptableObject in Resources
				if (!TryLoadSingletonOfType(type, out var singletonObject)) {

					singletonObject = CreateNewSingletonObject(type);
				}
			}
		}
	}
}