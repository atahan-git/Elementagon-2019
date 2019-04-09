using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Objective", menuName = "GameSettings/Objective", order = 6)]
public class ObjectiveSettings : ScriptableObject {

	public bool objs_isEnabled = true;
	public Vector2 objs_betweenTime = new Vector2(25f,55f);
	//[Space]
	//public ObjectivesSystem.BasicObjectives objs_basic = new ObjectivesSystem.BasicObjectives();
	//[Space]
	//public ObjectivesSystem.AdvancedObjective[] objs_advanced = new ObjectivesSystem.AdvancedObjective[0];

}
