﻿using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {


	[SerializeField]
	Transform _tileSprite;

	public bool walkable;

	[SerializeField]
	Sprite[] _spriteSheet;

	SpriteRenderer _sprite;

	Vector2 __origPos;


	public void ChangeSprite(int __spriteIndex)
	{
		if (_sprite == null)
		{
			_sprite = _tileSprite.GetComponent<SpriteRenderer>();
		}

		if (_spriteSheet.Length > __spriteIndex)
		{
			if (_sprite != null)
			{
				if (__spriteIndex == 8)
				{
					_sprite.sortingOrder = 0;
				}
				else
				{
					_sprite.sortingOrder = 5;
				}

				_sprite.sprite = _spriteSheet[__spriteIndex];
			}
		}
	}

	void Awake()
	{
		__origPos = _tileSprite.localPosition;
		_tileSprite.localPosition = __origPos + _startOffset;
	}

	Vector2 _startOffset = -Vector2.up * 100.0f;

	public void RotateTile(int __degrees)
	{
		_tileSprite.rotation = Quaternion.Euler(0, 0, __degrees);
	}

	public Coroutine MoveToPlace(Transform __player)
	{
		return StartCoroutine(_MoveToPlace(__player));
	}
	
	IEnumerator _MoveToPlace(Transform __player)
	{
		float __origDist = 0;

		if (__player != null)
		{
			__origDist = (__player.position - transform.position).magnitude;
		}
		else
		{
			__origDist = transform.position.magnitude;
		}

		float __time = -__origDist * 0.05f - Random.value * 0.1f;

		while (__time < 1)
		{
			if (__player != null)
			{
				float __newDist = (__player.position - transform.position).magnitude;
				__time -= (__newDist - __origDist) * 0.02f;
				__origDist = __newDist;
			}

			__time += Time.deltaTime;
			_tileSprite.transform.localPosition = __origPos + _startOffset * (1-TileData.GetCurve().Evaluate(__time));
			yield return null;


		}

		_tileSprite.transform.localPosition = __origPos;
	}
	
	public Coroutine MoveOut(Transform __player)
	{
		if (!_moveOut)
		{
			_moveOut = true;
			return StartCoroutine(_MoveToPlace(__player));
		}
		return null;
	}
	
	IEnumerator _MoveOut(Transform __player)
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

		float __time = -__dist * 0.2f - Random.value * 0.3f;

		while (__time < 1)
		{

			__time += Time.deltaTime;
			_tileSprite.transform.localPosition = __origPos + _startOffset * (TileData.GetCurve().Evaluate(__time));
			yield return null;
		}

		_tileSprite.transform.localPosition = __origPos + _startOffset;

		_destroyObject = true;
	}

	bool _moveOut;
	float _moveOutTime;
	bool _destroyObject;

	void Update()
	{
		if (_moveOut)
		{
			Vector3 __startPos = Vector3.down * 100.0f;
			float __dist = (__startPos - PlayerControl.Instance.transform.position).magnitude;

			if (_moveOutTime == 0)
			{
				_moveOutTime = Mathf.Max(-__dist * 0.2f, 1.0f) - Random.value * 0.3f;
			}
			
			if (_moveOutTime < 1)
			{
				_moveOutTime += Time.deltaTime;
				_tileSprite.transform.localPosition = __startPos * (TileData.GetCurve().Evaluate(_moveOutTime));
			}
			else
			{
				_tileSprite.transform.localPosition = __startPos;
				_destroyObject = true;
			}
		}

		if (_destroyObject)
		{
			Destroy(gameObject);
		}
	}
}
