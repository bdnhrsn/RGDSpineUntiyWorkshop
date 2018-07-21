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
	private const string fireAnim = "shoot";
	bool inputLeft, inputRight, inputJump, inputFire, onGround;

	const int movementTrack = 0;
	const int actionTrack = 1;

	void Awake() {
		skeletonAnim = GetComponent<SkeletonAnimation> ();
		onGround = true;
	}

	void Start() {
		skeletonAnim.AnimationState.Complete += OnAnimationComplete;
		skeletonAnim.AnimationState.SetAnimation(movementTrack, idleAnim, true);
	}

	void Update() {
		UpdateCurrentInputValues();
		if (inputJump) {
			onGround = false;
			skeletonAnim.AnimationState.SetAnimation(movementTrack, jumpAnim, false);
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
					skeletonAnim.AnimationState.SetAnimation(movementTrack, walkAnim, true);
				}
			}
		} else if (onGround) {
			Idle();
		}
		if (inputFire && skeletonAnim.AnimationName != fireAnim) {
			skeletonAnim.AnimationState.SetAnimation(actionTrack, fireAnim, false);
		}
	}

	void Idle() {
		if (skeletonAnim.AnimationName != idleAnim) {
			skeletonAnim.AnimationState.SetAnimation(movementTrack, idleAnim, true);
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
		inputFire = Input.GetKeyDown("left shift");
	}

	enum InputDirection {
		Left,
		Right,
		Jump,
		Fire,
	}
}
