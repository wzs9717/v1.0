using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class DAZCharacter : MonoBehaviour
//LoadCharacter(skin, skin root bone)，load scene if no scene when became enabled
{
	public delegate void OnLoaded();

	public Atom containingAtom;

	public string displayName;

	public Transform characterPrefab;

	public string characterSceneName;

	public DAZBones rootBonesForSkinning;

	public bool isMale;

	public OnLoaded onLoadedHandlers;

	[SerializeField]
	protected DAZSkinV2 _skin;

	protected AsyncOperation async;

	[SerializeField]
	protected Transform characterInstance;

	public DAZSkinV2 skin => _skin;

	protected void InitInstance()
	//initialize the charactor to 0 position
	{
		characterInstance.parent = base.transform;
		characterInstance.localPosition = Vector3.zero;
		characterInstance.localRotation = Quaternion.identity;
	}

	protected void RegisterInstanceStorables()
	//add js from child components to the JS list of Atom
	{
		if (containingAtom != null && characterInstance != null)
		{
			JSONStorable[] componentsInChildren = characterInstance.GetComponentsInChildren<JSONStorable>();
			JSONStorable[] array = componentsInChildren;
			foreach (JSONStorable js in array)
			{
				containingAtom.RegisterAdditionalStorable(js);
			}
		}
	}

	protected void UnregisterInstanceStorables()
	{
		if (containingAtom != null && characterInstance != null)
		{
			JSONStorable[] componentsInChildren = characterInstance.GetComponentsInChildren<JSONStorable>();
			JSONStorable[] array = componentsInChildren;
			foreach (JSONStorable js in array)
			{
				containingAtom.UnregisterAdditionalStorable(js);
			}
		}
	}

	protected void PostInitInstance()
	//RestoreFromLast
	{
		if (containingAtom != null)
		{
			JSONStorable[] componentsInChildren = characterInstance.GetComponentsInChildren<JSONStorable>();
			JSONStorable[] array = componentsInChildren;
			foreach (JSONStorable js in array)
			{
				containingAtom.RestoreFromLast(js);//???? restore what
			}
		}
	}

	private IEnumerator LoadCharacterAsync()
	{
		yield return null;
		Scene sc = SceneManager.GetSceneByName(characterSceneName);
		if (!sc.IsValid() || !sc.isLoaded)//if no scene
		{
			async = SceneManager.LoadSceneAsync(characterSceneName, LoadSceneMode.Additive);
			yield return async;
			sc = SceneManager.GetSceneByName(characterSceneName);
		}
		if (sc.IsValid())
		{
			GameObject[] rootGameObjects = sc.GetRootGameObjects();
			if (rootGameObjects.Length > 0)
			{
				GameObject gameObject = rootGameObjects[0];
				characterInstance = Object.Instantiate(gameObject.transform);
				characterInstance.gameObject.SetActive(value: true);//active instanced obj
				gameObject.SetActive(value: false);//disable root obj??
				InitInstance();
				RegisterInstanceStorables();
				OnLoadComplete();
				PostInitInstance();
			}
		}
		else
		{
			Debug.LogError("Could not open character scene " + characterSceneName);
		}
	}

	protected void OnLoadComplete()
	//set skin, set root bone for skin
	{
		DAZSkinV2 dAZSkinV = GetComponentInChildren<DAZMergedSkinV2>(includeInactive: true);
		DAZSkinV2 componentInChildren = GetComponentInChildren<DAZSkinV2>(includeInactive: true);
		if (dAZSkinV == null)
		{
			dAZSkinV = componentInChildren;
		}
		_skin = dAZSkinV;//get skin
		if (rootBonesForSkinning != null)
		{
			DAZSkinV2[] componentsInChildren = GetComponentsInChildren<DAZSkinV2>(includeInactive: true);
			DAZSkinV2[] array = componentsInChildren;
			foreach (DAZSkinV2 dAZSkinV2 in array)
			{
				dAZSkinV2.root = rootBonesForSkinning;//set skin root bone
			}
		}
		DAZCharacterMaterialOptions[] componentsInChildren2 = GetComponentsInChildren<DAZCharacterMaterialOptions>();
		DAZCharacterMaterialOptions[] array2 = componentsInChildren2;
		foreach (DAZCharacterMaterialOptions dAZCharacterMaterialOptions in array2)
		{
			dAZCharacterMaterialOptions.skin = _skin;
		}
		DAZSkinControl componentInChildren2 = GetComponentInChildren<DAZSkinControl>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.skin = _skin;//set skin
		}
		if (onLoadedHandlers != null)
		{
			onLoadedHandlers();//call this delegate function 
			onLoadedHandlers = null;
		}
	}

	private void OnEnable()
	{
		if (characterInstance == null)
		{
			if (characterSceneName != null && characterSceneName != string.Empty)
			{
				StartCoroutine(LoadCharacterAsync());
			}
			else if (characterPrefab != null)
			{
				characterInstance = Object.Instantiate(characterPrefab);
				InitInstance();
				RegisterInstanceStorables();
				OnLoadComplete();
				PostInitInstance();//why Restore From Last
			}
			else
			{
				OnLoadComplete();
			}
		}
		else
		{
			OnLoadComplete();
		}
	}

	private void OnDisable()
	{
		if (characterInstance != null)
		{
			UnregisterInstanceStorables();
			if (!Application.isPlaying)
			{
				Object.DestroyImmediate(characterInstance.gameObject);
				characterInstance = null;
			}
			if (onLoadedHandlers != null)
			{
				onLoadedHandlers = null;
			}
		}
	}
}
