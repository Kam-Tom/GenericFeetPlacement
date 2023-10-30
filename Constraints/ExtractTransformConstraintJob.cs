using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public struct ExtractTransformConstraintJob : IWeightedAnimationJob
{
	public ReadWriteTransformHandle Bone;
	public Vector3Property Position;
	public Vector4Property Rotation;
	public FloatProperty jobWeight { get; set; }

	public void ProcessAnimation(AnimationStream stream)
	{
		AnimationRuntimeUtils.PassThrough(stream,Bone);

		Vector3 pos = Bone.GetPosition(stream);
		Quaternion rot = Bone.GetRotation(stream);

		Position.Set(stream,pos);
		Rotation.Set(stream,new Vector4(rot.x,rot.y,rot.z,rot.w));

	}

	public void ProcessRootMotion(AnimationStream stream){}
}
