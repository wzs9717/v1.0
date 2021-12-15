using UnityEngine;

public class JointPositionHardLimit : MonoBehaviour
//HardLimit move from startAnchor+_zeroTargetPosition to startAnchor+TargetPosition
{
	public bool lateUpdateOn;

	public bool updateOn = true;

	public bool fixedUpdateOn = true;

	public bool useOffsetPosition;

	private ConfigurableJoint cj;

	public Vector3 _currentAnchor;

	public Vector3 _currentTargetPosition;

	[SerializeField]
	private float _percent;

	[SerializeField]
	private Vector3 _lowTargetPosition;

	[SerializeField]
	private Vector3 _zeroTargetPosition;

	[SerializeField]
	private Vector3 _highTargetPosition;

	public Vector3 startAnchor;

	public Quaternion startRotation;

	public float percent
	{
		get
		{
			return _percent;
		}
		set
		{
			if (_percent != value)
			{
				_percent = value;
				SetTargetPositionFromPercent();
			}
		}
	}

	public Vector3 lowTargetPosition
	{
		get
		{
			return _lowTargetPosition;
		}
		set
		{
			if (_lowTargetPosition != value)
			{
				_lowTargetPosition = value;
				SetTargetPositionFromPercent();
			}
		}
	}

	public Vector3 zeroTargetPosition
	{
		get
		{
			return _zeroTargetPosition;
		}
		set
		{
			if (_zeroTargetPosition != value)
			{
				_zeroTargetPosition = value;
				SetTargetPositionFromPercent();
			}
		}
	}

	public Vector3 highTargetPosition
	{
		get
		{
			return _highTargetPosition;
		}
		set
		{
			if (_highTargetPosition != value)
			{
				_highTargetPosition = value;
				SetTargetPositionFromPercent();
			}
		}
	}

	public void SetTargetPositionFromPercent()
	//set target position for cj.connectedAnchor to move towards _highTargetPosition
	{
		if (_percent < 0f)
		{
			_currentTargetPosition = Vector3.Lerp(_zeroTargetPosition, _lowTargetPosition, 0f - _percent);//interpolate
		}
		else
		{
			_currentTargetPosition = Vector3.Lerp(_zeroTargetPosition, _highTargetPosition, _percent);
		}
		if (cj != null && useOffsetPosition)
		{
			Quaternion localRotation = base.transform.localRotation;
			base.transform.localRotation = startRotation;
			cj.connectedAnchor = startAnchor + _currentTargetPosition;
			_currentAnchor = cj.connectedAnchor;
			base.transform.localRotation = localRotation;//seems restore base.transform.localRotation
		}
	}

	private void Start()
	//init ConfigurableJoint, set target posion and then update localPosition
	{
		cj = GetComponent<ConfigurableJoint>();
		if (cj != null)
		{
			startAnchor = cj.connectedAnchor;
			cj.autoConfigureConnectedAnchor = false;//manully config connected anchor
			startRotation = base.transform.localRotation;
		}
		SetTargetPositionFromPercent();
		DoUpdate();
	}

	private void DoUpdate()
	//set localPosition to cj.connectedAnchor
	{
		if (cj != null)
		{
			base.transform.localPosition = cj.connectedAnchor;
		}
	}

	private void Update()
	{
		if (updateOn)
		{
			DoUpdate();
		}
	}

	private void LateUpdate()
	{
		if (lateUpdateOn)
		{
			DoUpdate();
		}
	}

	private void FixedUpdate()
	{
		if (fixedUpdateOn)
		{
			DoUpdate();
		}
	}
}
