using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
#if UNITY_EDITOR
[ExecuteInEditMode]
public class CSRunSpine : MonoBehaviour
{
	[SpineAnimation]
	public string nameAnimation;
	[SHOW_IN_HIER]
    public void PlayAnim()
	{
		GetComponent<SkeletonGraphic>().AnimationState.SetAnimation(0, nameAnimation, true);
	}

}
#endif