using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grid", menuName = "GameSettings/Grid", order = 1)]
public class GridSettings : ScriptableObject {

	public int gridSizeX = 8;
	public int gridSizeY = 4;

	public float gridScaleX = 2.1f;
	public float gridScaleY = 2.75f;
	public float scaleMultiplier = 1.65f;
	public float centerOffsetX = -0.5f;
	public float centerOffsetY = -0.5f;
}
