using System.Collections.Generic;
using UnityEngine;

public class DAZBones : MonoBehaviour
//bones: get bone, init bones
{
	private Dictionary<string, DAZBone> boneNameToDAZBone;

	private Dictionary<string, DAZBone> boneIdToDAZBone;

	private DAZBone[] dazBones;

	public bool useScale;

	private bool _isMale;

	[SerializeField]
	private Dictionary<string, float> _morphGeneralScales;

	private float _currentGeneralScale;

	private bool _wasInit;

	public bool isMale
	{
		get
		{
			return _isMale;
		}
		set
		{
			if (_isMale != value)
			{
				_isMale = value;
				SetMorphedTransform();
			}
		}
	}

	public Dictionary<string, float> morphGeneralScales => _morphGeneralScales;

	public float currentGeneralScale => _currentGeneralScale;

	public bool wasInit => _wasInit;

	public void SetGeneralScale(string morphName, float scale)
	//seems no use
	{
		if (_morphGeneralScales == null)
		{
			_morphGeneralScales = new Dictionary<string, float>();
		}
		if (_morphGeneralScales.TryGetValue(morphName, out var _))
		{
			_morphGeneralScales.Remove(morphName);
		}
		if (scale != 0f)
		{
			_morphGeneralScales.Add(morphName, scale);
		}
		_currentGeneralScale = 0f;
		foreach (float value2 in _morphGeneralScales.Values)
		{
			float num = value2;
			_currentGeneralScale += num;//this var seems no use
		}
		SetMorphedTransform();
	}

	public DAZBone GetDAZBone(string boneName)
	//GetDAZBoneByNmae
	{
		Init();
		if (boneNameToDAZBone != null)
		{
			if (boneNameToDAZBone.TryGetValue(boneName, out var value))
			{
				return value;
			}
			return null;
		}
		return null;
	}

	public DAZBone GetDAZBoneById(string boneId)
	//GetDAZBoneById
	{
		Init();
		if (boneIdToDAZBone != null)
		{
			if (boneIdToDAZBone.TryGetValue(boneId, out var value))
			{
				return value;
			}
			return null;
		}
		return null;
	}

	public void Reset()
	//init
	{
		_wasInit = false;
		boneNameToDAZBone = null;
		boneIdToDAZBone = null;
		Init();
	}

	private void InitBonesRecursive(Transform t)
	//do DAZBone.init() Recursively to genrate boneNameToDAZBone and boneIdToDAZBone
	{
		foreach (Transform item in t)
		{
			DAZBones component = item.GetComponent<DAZBones>();
			if (!(component == null))
			{
				continue;
			}
			DAZBone component2 = item.GetComponent<DAZBone>();
			if (component2 != null)
			{
				component2.Init();//DAZBone init
				if (boneNameToDAZBone.ContainsKey(component2.name))
				{
					Debug.LogError("Found duplicate bone " + component2.name);
				}
				else
				{
					boneNameToDAZBone.Add(component2.name, component2);
				}
				if (boneIdToDAZBone.ContainsKey(component2.id))
				{
					Debug.LogError("Found duplicate bone id " + component2.id);
				}
				else
				{
					boneIdToDAZBone.Add(component2.id, component2);//add to bones dic
				}
				InitBonesRecursive(component2.transform);//Recursive
			}
		}
	}

	public void Init()
	//InitBonesRecursive and SetMorphedTransform to init dazBones
	{
		if (!_wasInit || boneNameToDAZBone == null)
		{
			_wasInit = true;
			boneNameToDAZBone = new Dictionary<string, DAZBone>();
			boneIdToDAZBone = new Dictionary<string, DAZBone>();
			InitBonesRecursive(base.transform);
			dazBones = new DAZBone[boneNameToDAZBone.Count];
			boneNameToDAZBone.Values.CopyTo(dazBones, 0);
			SetMorphedTransform();
		}
	}

	public void SetTransformsToImportValues()
	//seems no use
	{
		if (dazBones != null)
		{
			DAZBone[] array = dazBones;
			foreach (DAZBone dAZBone in array)
			{
				dAZBone.SetTransformToImportValues();
			}
		}
	}

	public void SetMorphedTransform()
	//detach bones, apply morphed transform and attach them again
	{
		float x = base.transform.lossyScale.x;
		if (dazBones != null)
		{
			if (Application.isPlaying)
			{
				DAZBone[] array = dazBones;
				foreach (DAZBone dAZBone in array)
				{
					dAZBone.SaveTransform();//backup
				}
			}
			DAZBone[] array2 = dazBones;
			foreach (DAZBone dAZBone2 in array2)
			{
				dAZBone2.dazBones = this;
				dAZBone2.DetachJoint();
				dAZBone2.SaveAndDetachParent();
			}
			DAZBone[] array3 = dazBones;
			foreach (DAZBone dAZBone3 in array3)
			{
				dAZBone3.ResetScale();
			}
			DAZBone[] array4 = dazBones;
			foreach (DAZBone dAZBone4 in array4)
			{
				dAZBone4.SetMorphedTransform(useScale, x);
			}
			if (!Application.isPlaying)
			{
				DAZBone[] array5 = dazBones;
				foreach (DAZBone dAZBone5 in array5)
				{
					dAZBone5.ApplyOffsetTransform();
				}
			}
			DAZBone[] array6 = dazBones;
			foreach (DAZBone dAZBone6 in array6)
			{
				dAZBone6.RestoreParent();
				dAZBone6.AttachJoint();
			}
			if (Application.isPlaying)
			{
				DAZBone[] array7 = dazBones;
				foreach (DAZBone dAZBone7 in array7)
				{
					dAZBone7.RestoreTransform();
				}
			}
			else
			{
				DAZBone[] array8 = dazBones;
				foreach (DAZBone dAZBone8 in array8)
				{
					dAZBone8.ApplyPresetLocalTransforms();
				}
			}
		}
		else
		{
			Debug.LogWarning("SetMorphedTransform called when bones were not init");
		}
	}

	private void Start()
	{
		Init();
	}
}
