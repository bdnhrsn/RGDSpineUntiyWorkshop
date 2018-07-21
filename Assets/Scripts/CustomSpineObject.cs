using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class CustomSpineObject : MonoBehaviour {

	private SkeletonAnimation skeletonAnim;
	private string idleAnim = "idle";
	private string walkAnim = "walk";
	bool inputLeft, inputRight, inputJump;

	void Awake() {
		skeletonAnim = GetComponent<SkeletonAnimation> ();
	}

	void Start() {

		skeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
	}

	void Update() {
		UpdateCurrentInputValues();

		if (inputLeft || inputRight) {
			if (inputLeft && inputRight) {
				skeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
			} else {
				if (inputLeft) {
					FlipCharacter(InputDirection.Left);
				} else if (inputRight) {
					FlipCharacter(InputDirection.Right);
				}
				if (skeletonAnim.AnimationName != walkAnim) {
					skeletonAnim.AnimationState.SetAnimation(0, walkAnim, true);
				}
			}
		} else {
			skeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
		}
	}

	void FlipCharacter(InputDirection pDirection) {
		Vector3 tScale = transform.localScale;
		switch (pDirection) {
			case InputDirection.Left:
				tScale.x = -1;
				break;
			case InputDirection.Right:
				tScale.x = 1;
				break;
			default:
				break;
		}
		if (tScale != transform.localScale) {
			transform.localScale = tScale;
		}
	}

	void UpdateCurrentInputValues() {
		
		inputLeft = Input.GetKey("left");
		inputRight = Input.GetKey("right");
		inputJump = Input.GetKeyDown("space");
	}

	enum InputDirection {
		Left,
		Right,
		Jump,
	}
}
