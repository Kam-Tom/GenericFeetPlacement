using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GenericFootIK : MonoBehaviour
{
	[SerializeField] Transform root;
	[SerializeField] Transform[] targets;
	[SerializeField] LayerMask groundMask;
	[SerializeField] float maxHitDistance = 1.5f;
	[SerializeField] float addedHeight = 0.5f;
	[SerializeField] Vector2 maxBodyRotation = new Vector2(10, 5);
	[SerializeField] float bodyRotationSpeed = 25f;
	ExtractTransformConstraint[] bones;
	Vector3[] hitNormals;

	private void Start()
	{
		hitNormals = new Vector3[targets.Length + 1];
		bones = new ExtractTransformConstraint[targets.Length];

		for (int i = 0; i < targets.Length; i++)
		{
			bones[i] = targets[i].parent.GetComponent<ExtractTransformConstraint>();
			if (bones[i] == null)
				Debug.LogError("Target should be a child of a GameObject that have an ExtractTransformConstraint component.");
			if (targets[i].parent.GetComponent<TwoBoneIKConstraint>() == null)
				Debug.LogError("Target should be a child of a GameObject that have an TwoBoneIKConstraint component.");
		}
		if (root.GetComponent<Animator>() == null)
			Debug.LogError("Root should be on the GameObject with our model/animator, so it should be child of the GameObject with movement script");
	}
	private void Update()
	{
		AdjustFeet();
		AdjustBody();
	}
	private void AdjustFeet()
	{
		for (int i = 0; i < targets.Length; i++)
		{
			//Get data from animation
			Vector3 bonePos = bones[i].data.position;
			Quaternion boneRot = bones[i].data.rotation;

			Vector3 origin = bonePos + root.up * addedHeight;

			if (Physics.Raycast(origin, -root.up, out RaycastHit hit, maxHitDistance, groundMask))
			{
				hitNormals[i] = hit.normal;

				//Get foot up position in animation
				Plane plane = new Plane(root.up, root.position);
				float dist = plane.GetDistanceToPoint(bonePos);

				//Move targets to hit.point
				targets[i].position = hit.point + dist * root.up;

				//Add target rotation
				Vector2 angles = ProjectAxisAngles(root.forward, root.right, hit.normal);
				targets[i].localEulerAngles = new Vector3(angles.x, 0, angles.y);
				targets[i].rotation = targets[i].rotation * Quaternion.Inverse(root.rotation) * boneRot;
			}
			else
			{
				hitNormals[i] = Vector3.up;
				//Move target to foot position
				targets[i].position = bonePos;
				targets[i].rotation = boneRot;
			}


		}
	}
	private Vector3 ProjectOnContactPlane(Vector3 vector, Vector3 hitNormal)
	{
		return vector - hitNormal * Vector3.Dot(vector, hitNormal);
	}
	private Vector2 ProjectAxisAngles(Vector3 foward, Vector3 right, Vector3 hitNormal)
	{
		Vector2 angles;

		Vector3 xAnglesProjected = ProjectOnContactPlane(foward, hitNormal);
		Vector3 yAnglesProjected = ProjectOnContactPlane(right, hitNormal);

		angles.x = Vector3.SignedAngle(foward,xAnglesProjected,right);
		angles.y = Vector3.SignedAngle(right,yAnglesProjected,foward);

		return angles;
	}
	private void AdjustBody()
	{
		//Raycast from body
		Vector3 origin = root.position + new Vector3(0, addedHeight, 0);
		if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, maxHitDistance, groundMask))
		{
			hitNormals[^1] = hit.normal;
		}
		else
		{
			hitNormals[^1] = Vector3.up;
		}

		//Avarage normals
		Vector3 hitNormalsSum = Vector3.zero;
		foreach (var normal in hitNormals)
			hitNormalsSum += normal;

		Vector3 hitNormalsAvg = (hitNormalsSum / hitNormals.Length).normalized;

		Vector2 angles = ProjectAxisAngles(root.forward, root.right, hitNormalsAvg);

		Vector2 currentRotation = new Vector2(root.localEulerAngles.x, root.localEulerAngles.z);

		//Map rotation from [0;360] to [-180;180]
		if (currentRotation.x > 180)
			currentRotation.x -= 360;
		if (currentRotation.y > 180)
			currentRotation.y -= 360;
		//Clamp body rotation
		if (currentRotation.x + angles.x > maxBodyRotation.x)
			angles.x = maxBodyRotation.x - currentRotation.x;
		if (currentRotation.x + angles.x < -maxBodyRotation.x)
			angles.x = -maxBodyRotation.x - currentRotation.x;

		if (currentRotation.y + angles.y > maxBodyRotation.y)
			angles.y = maxBodyRotation.y - currentRotation.y;
		if (currentRotation.y + angles.y < -maxBodyRotation.y)
			angles.y = -maxBodyRotation.y - currentRotation.y;

		angles.x = Mathf.Lerp(0, angles.x, Time.deltaTime * bodyRotationSpeed);
		angles.y = Mathf.Lerp(0, angles.y, Time.deltaTime * bodyRotationSpeed);

		root.localEulerAngles += new Vector3(angles.x, 0, angles.y);
	}

}
