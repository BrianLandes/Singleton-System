
using UnityEngine;
using UnityEngine.UI;

namespace SingletonSystem.Examples {

	/// <summary>
	/// Simply displays the 'points' of one side or the other.
	/// Mainly used as an example of getting an Instance to a Singleton and accessing values from it.
	/// </summary>
	public class PongPointDisplay : MonoBehaviour {

		public enum Side {
			LeftSide,
			RightSide
		}

		[SerializeField]
		private Side side = Side.LeftSide;

		[SerializeField]
		private Text textComponent;

		private PongSingleton pongSingleton;

		private void Start() {

			pongSingleton = Singletons.Get<PongSingleton>();

			// The other way to access the instance for a Singleton is the following line:
			//pongSingleton = PongSingleton.Instance;
			// both ways work pretty much the same.

			if (textComponent == null) {
				textComponent = GetComponentInChildren<Text>();
			}
		}

		private void Update() {

			switch (side) {
				case Side.LeftSide:
					textComponent.text = "Left Side Points: " + pongSingleton.LeftSidePoints;
					break;
				case Side.RightSide:
					textComponent.text = "Right Side Points: " + pongSingleton.RightSidePoints;
					break;
			}
		}

	}
}