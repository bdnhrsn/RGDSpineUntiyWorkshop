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
	private const string goggleSlotName = "goggles";
	private const string gogglesAttachmentName = "goggles";
	bool inputLeft, inputRight, inputJump, inputFire, inputGoggles, onGround;

	const int movementTrack = 0;
	const int actionTrack = 1;

	void Awake() {
		skeletonAnim = GetComponent<SkeletonAnimation> ();
		onGround = true;

		skeletonAnim.AnimationState.Complete += OnAnimationComplete;
	}

	void Start() {
		skeletonAnim.AnimationState.SetAnimation(movementTrack, idleAnim, true);

		skeletonAnim.AnimationState.Data.SetMix(idleAnim, walkAnim, 0.15f);
		skeletonAnim.AnimationState.Data.SetMix(walkAnim, idleAnim, 0.15f);
	}

	void OnDestroy() {
		skeletonAnim.AnimationState.Complete -= OnAnimationComplete;
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
		if (inputGoggles) {
			ToggleGoggles();
		}
	}

	void Idle() {
		if (skeletonAnim.AnimationName != idleAnim) {
			skeletonAnim.AnimationState.SetAnimation(movementTrack, idleAnim, true);
		}
	}

	void ToggleGoggles() {
		if (skeletonAnim.skeleton.FindSlot(goggleSlotName).attachment == null) {
			skeletonAnim.skeleton.SetAttachment(goggleSlotName, gogglesAttachmentName);
		} else {
			skeletonAnim.skeleton.SetAttachment(goggleSlotName, null);
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
		inputGoggles = Input.GetKeyDown("left alt");
	}

	enum InputDirection {
		Left,
		Right,
		Jump,
		Fire,
		Goggles,
	}
}
