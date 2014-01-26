using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {


	[SerializeField]
	Transform _tileSprite;

	public bool walkable;


	public static Tile CreateTile(Color __pixelColor)
	{
		Tile __tilePrefab = TileData.GetTile(__pixelColor);

		if (__tilePrefab != null)
		{
			Tile __tile = Instantiate(__tilePrefab) as Tile;
			return __tile;
		}
		else
		{
			return null;
		}
	}

	void Start()
	{
		_tileSprite.localPosition = Vector3.down * 100.0f;
		MoveToPlace(null);
	}

	public void RotateTile(int __degrees)
	{
		_tileSprite.rotation = Quaternion.Euler(0, 0, __degrees);
	}

	public Coroutine MoveToPlace(Transform __player = null)
	{
		return StartCoroutine(_MoveToPlace(__player));
	}
	
	IEnumerator _MoveToPlace(Transform __player)
	{
		float __dist = 0;
		if (__player != null)
		{
			__dist = (__player.position - transform.position).magnitude;
		}
		else
		{
			__dist = transform.position.magnitude;
		}

		Vector3 __startPos = _tileSprite.transform.localPosition;


		float __time = -__dist * 0.2f - Random.value * 0.3f;

		while (__time < 1)
		{
			__time += Time.deltaTime / 2.0f;
			_tileSprite.transform.localPosition = __startPos * (1-TileData.GetCurve().Evaluate(__time));
			yield return null;
		}
	}
}
