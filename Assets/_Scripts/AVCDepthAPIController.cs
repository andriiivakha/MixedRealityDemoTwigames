using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.Depth;

public class AVCDepthAPIController : Singleton<AVCDepthAPIController>
{
	[SerializeField] private EnvironmentDepthOcclusionController depthOcclusionControllerPrefab;
	[SerializeField] private OcclusionType defaultOcclusionType;

	private EnvironmentDepthOcclusionController depthOcclusionController;
	private OcclusionType occlusionType = OcclusionType.NoOcclusion;

	public bool IsDepthAPIActive { get; private set; } = false;

	protected override void Init()
	{
		base.Init();

		OVRManager.SystemHeadsetType headsetType = OVRManager.systemHeadsetType;
		IsDepthAPIActive = headsetType == OVRManager.SystemHeadsetType.Meta_Link_Quest_3
			|| headsetType == OVRManager.SystemHeadsetType.Meta_Quest_3;
		Debug.Log($"AVCDepthAPIController.Init IsDepthAPIActive={IsDepthAPIActive} headsetType={headsetType}");

		if (IsDepthAPIActive)
		{
			depthOcclusionController = Instantiate(depthOcclusionControllerPrefab);
			depthOcclusionController.GetComponent<EnvironmentDepthTextureProvider>().enabled = false;
			Invoke(nameof(InitDepthOcclusionController), 1f);
		}
	}

	private void InitDepthOcclusionController()
	{
		if (IsDepthAPIActive)
		{
			if (depthOcclusionController == null)
				depthOcclusionController = FindObjectOfType<EnvironmentDepthOcclusionController>();
			OcclusionType = defaultOcclusionType;
		}
	}

	public OcclusionType OcclusionType
	{
		get => occlusionType;
		set
		{
			Debug.Log($"AVCDepthAPIController.SetOcclusionType IsDepthAPIActive={IsDepthAPIActive} type={value}");
			if (IsDepthAPIActive && !Application.isEditor && depthOcclusionController != null)
			{
				Debug.Log($"AVCDepthAPIController.SetOcclusionType - SET");
				depthOcclusionController.EnableOcclusionType(value);
				occlusionType = value;
			}
			if (depthOcclusionController != null)
				depthOcclusionController.GetComponent<EnvironmentDepthTextureProvider>().enabled = occlusionType != OcclusionType.NoOcclusion;
		}
	}
}
