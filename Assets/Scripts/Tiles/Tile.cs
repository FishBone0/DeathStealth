using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {


	[SerializeField]
	Transform _tileSprite;

	[SerializeField]
	AnimationCurve _moveUpCurve;

	public bool walkable;

	void Start()
	{
		_tileSprite.localPosition = Vector3.down * 100.0f;
	}

	public Coroutine MoveToPlace(Transform __player)
	{
		return StartCoroutine(_MoveToPlace(__player));
	}

	IEnumerator _MoveToPlace(Transform __player)
	{
		float __dist = (__player.position - transform.position).magnitude;

		Vector3 __startPos = _tileSprite.transform.localPosition;


		float __time = Mathf.Max(1 - __dist * 0.1f, 0);

		while (__time < 1)
		{
			_tileSprite.transform.localPosition = Vector3.Lerp(__startPos, Vector3.zero, _moveUpCurve.Evaluate(__time));
			yield return null;
		}
	}
}
