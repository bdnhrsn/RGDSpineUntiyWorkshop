using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class CustomSpineObject : MonoBehaviour {

	private SkeletonAnimation skeletonAnim;
	private const string idleAnim = "idle";
	private const string walkAnim = "walk";
	private const string jumpAnim = "jump";
	bool inputLeft, inputRight, inputJump, onGround;

	void Awake() {
		skeletonAnim = GetComponent<SkeletonAnimation> ();
		onGround = true;
	}

	void Start() {
		skeletonAnim.AnimationState.Complete += OnAnimationComplete;
		// skeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
	}

	void Update() {
		UpdateCurrentInputValues();
		if (inputJump) {
			onGround = false;
			skeletonAnim.AnimationState.SetAnimation(0, jumpAnim, false);
		} 
		if (inputLeft || inputRight) {
			if (inputLeft && inputRight && onGround) {
				Idle();
			} else {
				if (inputLeft) {
					FlipCharacter(InputDirection.Left);
				} else if (inputRight) {
					FlipCharacter(InputDirection.Right);
				}
				if (onGround && skeletonAnim.AnimationName != walkAnim) {
					skeletonAnim.AnimationState.SetAnimation(0, walkAnim, true);
				}
			}
		} else if (onGround) {
			Idle();
		}
	}

	void Idle() {
		if (skeletonAnim.AnimationName != idleAnim) {
			skeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
		}
	}

	void OnAnimationComplete(TrackEntry pTrackEntry) {
		switch (pTrackEntry.animation.name) {
			case jumpAnim:
				Idle();
				onGround = true;
				break;
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
