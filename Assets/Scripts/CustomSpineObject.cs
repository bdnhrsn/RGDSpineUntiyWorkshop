using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class CustomSpineObject : MonoBehaviour {

	private SkeletonAnimation skeletonAnim;
	private string idleAnim = "idle";

	void Awake() {
		skeletonAnim = GetComponent<SkeletonAnimation>();
	}

	void Start() {

		// skeletonAnim.AnimationState.SetAnimation(0, idleAnim, true);
	}
}
