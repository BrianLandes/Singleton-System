

namespace SingletonSystem {

	public abstract class Singleton<T> : SingletonBase where T : SingletonBase {

		public static T Instance {
			get {
				if (_instance == null) {
					_instance = Singletons.Get<T>();
				}
				return _instance;
			}
		}
		private static T _instance = null;

	}
}