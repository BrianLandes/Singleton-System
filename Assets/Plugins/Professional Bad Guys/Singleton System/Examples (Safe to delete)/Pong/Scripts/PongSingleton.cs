
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SingletonSystem.Examples {

	/// <summary>
	/// A Trivial example that sort-of resembles 'Pong'. <br/>
	/// A ball object is moved around the screen, bouncing off of the edges. <br/>
	/// When the ball bounces off of the left side, the left team gets 1 point, and 
	/// when the ball bounces off of the right side, the right team gets 1 point. <br/>
	/// There are no inputs or controls other than the values that can be modified on the PongSingleton.
	/// </summary>
	public class PongSingleton : Singleton<PongSingleton> {

		#region Settings

		[SerializeField]
		private float ballSpeed = 1000;

		[SerializeField]
		private Vector2 startingBallDirection = Vector2.right + Vector2.up;

		[SerializeField]
		private float screenMargin = 20;

		[SerializeField]
		[Tooltip("Provided so that you can turn the ball's movement on or off while playing.")]
		private bool updateBallPosition = true;

		[SerializeField]
		[Tooltip("Whether or not to add random variety to the balls trajectory when it bounces off of the walls.")]
		private bool randomizeBallsDirectionOnReflect = true;

		#endregion

		#region Runtime Values

		private Vector3 ballDirection;

		private int leftSidePoints = 0;

		public int LeftSidePoints => leftSidePoints;


		private int rightSidePoints = 0;

		public int RightSidePoints => rightSidePoints;

		#endregion

		#region In-Scene Components/GameObject

		/// <summary>
		/// The In-scene reference to the ball. <br/>
		/// It will be found if/when the scene is loaded.<br/>
		/// We can determine whether we are in the 'Pong' scene by whether we have a ball or not.
		/// </summary>
		private PongBall ball;

		#endregion

		#region Singleton Event Overrides

		public override void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
			ball = GameObject.FindObjectOfType<PongBall>();

			if (ball != null) {
				ResetForNewGame();
			}
		}

		public override void OnSceneUnloaded(Scene scene) {
			ball = null;
		}

		public override void OnUpdate() {
			if (ball == null) {
				return;
			}

			if (updateBallPosition) {
				UpdateBallPosition();
			}

			ReflectBallIfTouchingBounds();
		}

		#endregion

		#region Game Logic

		/// <summary>
		/// This method made public so that the in-scene button for 'New Game' can access it using UnityEvents.
		/// </summary>
		public void ResetForNewGame() {
			ballDirection = startingBallDirection;
			leftSidePoints = 0;
			rightSidePoints = 0;

			var rectTransform = ball.GetComponent<RectTransform>();
			rectTransform.position = new Vector2(Screen.width *0.5f, Screen.height * 0.5f);
		}

		private void UpdateBallPosition() {
			var rectTransform = ball.GetComponent<RectTransform>();
			rectTransform.localPosition += ballDirection.normalized * ballSpeed * Time.deltaTime;
		}

		private void ReflectBallIfTouchingBounds() {
			var rectTransform = ball.GetComponent<RectTransform>();

			var nextPosition = rectTransform.position + ballDirection.normalized * ballSpeed * Time.deltaTime;

			if ( nextPosition.y > Screen.height - screenMargin) {
				ballDirection.y *= -1;

				if (randomizeBallsDirectionOnReflect) {
					RandomlyAdjustBallsDirection();
				}
			}
			else
			if (nextPosition.y < 0 + screenMargin) {
				ballDirection.y *= -1;

				if (randomizeBallsDirectionOnReflect) {
					RandomlyAdjustBallsDirection();
				}
			}

			if (nextPosition.x > Screen.width - screenMargin) {
				ballDirection.x *= -1;

				rightSidePoints++;

				if (randomizeBallsDirectionOnReflect) {
					RandomlyAdjustBallsDirection();
				}
			}
			else
			if (nextPosition.x < 0 + screenMargin) {
				ballDirection.x *= -1;

				leftSidePoints++;

				if (randomizeBallsDirectionOnReflect) {
					RandomlyAdjustBallsDirection();
				}
			}
		}

		private void RandomlyAdjustBallsDirection() {
			ballDirection.x += UnityEngine.Random.Range(-0.1f, 0.1f);
			ballDirection.y += UnityEngine.Random.Range(-0.1f, 0.1f);
			ballDirection.Normalize();
		}

		#endregion
	}
}