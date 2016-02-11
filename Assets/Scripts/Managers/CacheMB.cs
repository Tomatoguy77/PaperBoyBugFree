using UnityEngine;
using System.Collections;

public class CacheMB : MonoBehaviour 
{
	Transform _transform;
	public new Transform transform
	{
		get 
		{
			return _transform ?? (_transform = base.transform); 
		}
	}
}
