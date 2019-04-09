using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour {

	private float fingerStartTime  = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;

	private bool isSwipe = false;
	private float minSwipeDist  = 50.0f;
	private float maxSwipeTime = 0.5f;


	// Update is called once per frame
	void Update () {
		return;
		if (GoogleAPI.gameMode != 1) {
			this.enabled = false;
			return;
		}

		if (Input.touchCount > 0) {

			foreach (Touch touch in Input.touches) {
				switch (touch.phase) {
				case TouchPhase.Began:
					/* this is a new touch */
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;

				case TouchPhase.Canceled:
					/* The touch is being canceled */
					isSwipe = false;
					break;

				case TouchPhase.Ended:
					

					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;

					if (touch.position.x > Screen.width / 6.6f) {
						if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
							Vector2 direction = touch.position - fingerStartPos;
							Vector2 swipeType = Vector2.zero;

							if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {
								// the swipe is horizontal:
								swipeType = Vector2.right * Mathf.Sign (direction.x);
							} else {
								// the swipe is vertical:
								swipeType = Vector2.up * Mathf.Sign (direction.y);
							}

							if (swipeType.x != 0.0f) {
								if (swipeType.x > 0.0f) {
									// MOVE RIGHT
									EnemyPlayerHandler.s.MoveWithDir (2);
									DataLogger.LogMessage ("MOVE RIGHT");
								} else {
									// MOVE LEFT
									EnemyPlayerHandler.s.MoveWithDir (0);
									DataLogger.LogMessage ("MOVE LEFT");
								}
							}

							if (swipeType.y != 0.0f) {
								if (swipeType.y > 0.0f) {
									// MOVE UP
									EnemyPlayerHandler.s.MoveWithDir (1);
									DataLogger.LogMessage ("MOVE UP");
								} else {
									// MOVE DOWN
									EnemyPlayerHandler.s.MoveWithDir (3);
									DataLogger.LogMessage ("MOVE DOWN");
								}
							}

						} else {
							//TOUCH
							EnemyPlayerHandler.s.SelectCard ();
							DataLogger.LogMessage ("TOUCH");
						}
					}

					break;
				}
			}
		}

	}
}
