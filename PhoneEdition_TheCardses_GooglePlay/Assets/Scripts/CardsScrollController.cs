using UnityEngine;
using System.Collections;

public class CardsScrollController : MonoBehaviour {

	public static CardsScrollController s;

	Vector2?[] oldTouchPositions = {
		null,
		null
	};
	Vector2 oldTouchVector;
	float oldTouchDistance;

	LocalPlayerController pc;
	Transform myCam;

	public float minZoom = 6f;
	public float maxZoom = 15f;
	public float scrollSpeed = 1f;


	public Vector2 cameraMinLimits = new Vector2 (-10, -10);
	public Vector2 cameraMaxLimits = new Vector2 (10, 10);


	public float scrollTime = 0.2f;
	float timer = 0f;
	public bool isScrolling = false;
	public float minScrollDelta = 5f;
	Vector3 startPos = Vector3.zero;
	public float curScrollDelta = 0f;

	public bool isScrollEnabled = true;
	// Use this for initialization
	void Start () {
		s = this;
		pc = GetComponent<LocalPlayerController> ();
		myCam = Camera.main.gameObject.transform;

		if (GS.a.gridSettings.gridSizeX <= 9 && GS.a.gridSettings.gridSizeY <= 3) {
			isScrollEnabled = false;
		}
	}

	int defZero = 0;

	// Update is called once per frame
	void Update () {
		if (!isScrollEnabled)
			return;

		myCam.position = new Vector3 (Mathf.Clamp (myCam.position.x, cameraMinLimits.x, cameraMaxLimits.x),
			 Mathf.Clamp (myCam.position.y, cameraMinLimits.y, cameraMaxLimits.y),
			 -10);
		/*if (!Input.touchSupported)
			return;*/

		if (!pc.canSelect)
			return;
		/*if (pc.isPlacingItem) {
			defZero = 1;
		} else {*/
			defZero = 0;
		//}

		if (Input.touchCount > 0) {
			if (timer == 0) {
				startPos = Input.GetTouch (defZero).position;
			}

			timer += Time.deltaTime;
			curScrollDelta = Vector3.Distance(startPos, Input.GetTouch (defZero).position);

			if (timer < scrollTime)
				return;
			else if (curScrollDelta < minScrollDelta)
				return;
			else
				isScrolling = true;
		} else {
			if(timer == 0)
			isScrolling = false;
			timer = 0;
		}


		if (Input.touchCount == defZero) {
			oldTouchPositions[0] = null;
			oldTouchPositions[1] = null;
		}
		else if (Input.touchCount == defZero + 1) {
			if (oldTouchPositions[0] == null || oldTouchPositions[1] != null) {
				oldTouchPositions[0] = Input.GetTouch(defZero).position;
				oldTouchPositions[1] = null;
			}
			else {
				Vector2 newTouchPosition = Input.GetTouch(defZero).position;

				myCam.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition)* scrollSpeed * 
					myCam.GetComponent<Camera>().fieldOfView / myCam.GetComponent<Camera>().pixelHeight * 2f));

				oldTouchPositions[0] = newTouchPosition;
			}
		}
		else {
			if (oldTouchPositions[1] == null) {
				oldTouchPositions[0] = Input.GetTouch(defZero).position;
				oldTouchPositions[1] = Input.GetTouch(defZero + 1).position;
				oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
				oldTouchDistance = oldTouchVector.magnitude;
			}
			else {
				Vector2 screen = new Vector2(myCam.GetComponent<Camera>().pixelWidth, myCam.GetComponent<Camera>().pixelHeight);

				Vector2[] newTouchPositions = {
					Input.GetTouch(defZero).position,
					Input.GetTouch(defZero + 1).position
				};
				Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
				float newTouchDistance = newTouchVector.magnitude;

				myCam.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * scrollSpeed * myCam.GetComponent<Camera>().fieldOfView / screen.y));
				//myCam.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
				myCam.GetComponent<Camera>().fieldOfView *= oldTouchDistance / newTouchDistance;
				myCam.GetComponent<Camera> ().fieldOfView = Mathf.Clamp (myCam.GetComponent<Camera> ().fieldOfView, minZoom, maxZoom);
				foreach (Camera cam in myCam.GetComponentsInChildren<Camera> ())
					cam.fieldOfView = myCam.GetComponent<Camera> ().fieldOfView;

				myCam.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * scrollSpeed * myCam.GetComponent<Camera>().fieldOfView / screen.y);

				oldTouchPositions[0] = newTouchPositions[0];
				oldTouchPositions[1] = newTouchPositions[1];
				oldTouchVector = newTouchVector;
				oldTouchDistance = newTouchDistance;
			}
		}

		myCam.position = new Vector3( Mathf.Clamp (myCam.position.x, cameraMinLimits.x, cameraMaxLimits.x),
			 Mathf.Clamp (myCam.position.y, cameraMinLimits.y, cameraMaxLimits.y),
			 -10);
	}

	public void MoveObject (Vector3 moveDelta) {
		
	}

	public void ScaleObject (Vector3 scaleDelta) {

	}
}
