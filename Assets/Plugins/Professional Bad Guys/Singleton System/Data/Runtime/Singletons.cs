
using System.Collections.Generic;

namespace SingletonSystem {

	/// <summary>
	/// Static methods for accessing Singletons
	/// </summary>
	public static class Singletons {

		/// <inheritdoc cref="SingletonManager.GetAll"/>
		public static IEnumerable<SingletonBase> GetAll() => SingletonManager.Instance.GetAll();


		/// <inheritdoc cref="SingletonManager.Get"/>
		public static T Get<T>() where T : SingletonBase => SingletonManager.Instance.Get<T>();
	}
}