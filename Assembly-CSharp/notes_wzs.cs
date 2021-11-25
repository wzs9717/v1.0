
//vamessentials
// Getting atom from the scene
Atom person = SuperController.singleton.GetAtomByUid("Person");

// Getting control object of an atom
FreeControllerV3 headControl = person.GetStorableByID("headControl") as FreeControllerV3;

// Getting player position
Transform player = CameraTarget.centerTarget.transform;

// Current scene directory
string loadDirectory = SuperController.singleton.currentLoadDir;

// Finding a GameObject spawned from prefab. You need to add "(Clone)" to the name your prefab had when putting it in the AssetBundle.
GameObject go = GameObject.Find("Fireball(Clone)");

// Getting a Unity component on a GameObject
ParticleSystem ps = go.GetComponent<ParticleSystem>();

//Getting atoms and storables
// Atom from name, its the same name as in VaM. Returns null if the atom is not found.
public static Atom GetAtom(string atomID);//should be getatombyUID() ??

// Storable by name. Returns null if the storable is not found.
public static JSONStorable GetStorable(Atom atom, string storableID);

// Storable by name. Returns null if the storable is not found.
public static JSONStorable GetStorable(string atomID, string storableID);

// Storable by name, casted to type. Returns null if the storable is not found or doesn't match type.
public static T GetStorable<T>(string atomID, string storableID) where T : JSONStorable;

// Storable by name, casted to type. Returns null if the storable is not found or doesn't match type.
public static T GetStorable<T>(Atom atom, string storableID) where T : JSONStorable;

// All Storable going by that name in the scene. 
public static List<JSONStorable> GetAllStorablesOfID(string storableID);

// All Storable going by that name and matching type in the scene.
public static List<T> GetAllStorablesOfID<T>(string storableID) where T : JSONStorable

//Selection
public static Atom GetSelectedAtom();
public static JSONStorable GetSelectedStorable();
public static T GetSelectedStorable<T>() where T : JSONStorable;
public static FreeControllerV3 GetSelectedControl();

//Player
// Transform of the player (center between the eyes)
public static Transform GetPlayerHead();

// Transform of the left hand, returns null in Desktop mode
public static Transform GetPlayerLeftHand();

// Transform of the right hand, returns null in Desktop mode
public static Transform GetPlayerRightHand();

// System
// Directory of the active scene.
public static string GetSceneDirectory();

// True in Desktop mode, False in VR mode.
public static bool IsDesktopMode();

// Time
// Get current timestamp
public static long GetTimestamp();

// Time passed in seconds since given timestamp
public static float TimeSince(long timestamp);

// Audio
// Get AudioClip from filename. Needs to be already loaded in Audio menu in VaM.
public static NamedAudioClip GetAudioClip(string audioID);
public static NamedAudioClip[] GetAudioClips(string[] audioIDs);

// Get AudioClip from name of embedded audio file. Use just the name, category is not needed.
public static NamedAudioClip GetAudioClipEmbedded(string audioID);
public static NamedAudioClip[] GetAudioClipsEmbedded(string[] audioIDs);


// Callbacks
// Called before scene loading. Register your trigger inputs here and do basic initialization.
public virtual void OnPreLoad() { }

// Called when done with scene loading. Register your trigger outputs here and do any initialization
// that requires knowledge of the scene. E.g. cache objects in the scene you want to reference.
public virtual void OnPostLoad() { }

// Called once per frame.
public virtual void OnUpdate() { }

// Called once per frame, during Unity's second round of Update calls. Use when making changes after everything else has updated.
public virtual void OnLateUpdate() { }

// Called once per physics frame. Use to apply forces, etc.
public virtual void OnFixedUpdate() { }

// Called when before loading a new scene or when quiting VaM. Cleanup any unmanaged resources (i.e. network and file stuff).
public virtual void OnUnLoad() { }

// Input Trigger connections
// InputTriggers can be used if you want your VaM scene to trigger things in your script. When creating a trigger connection in VaM they show up in VaM under CoreControl -> ScriptEngine. The RegisterIn*** methods need to be called in OnPreLoad(), so objects in your scene can find and validate the trigger connection.

public delegate void InTriggerAction();
public delegate void InTriggerBool(bool value);
public delegate void InTriggerFloat(float value);
public delegate void InTriggerString(string value);
public delegate void InTriggerColorHSV(HSVColor value);
public delegate void InTriggerColorRGB(Color value);
public delegate void InTriggerURL(string value);

public void RegisterInAction(string name, InTriggerAction callback);
public void RegisterInBool(string name, InTriggerBool callback);
public void RegisterInFloat(string name, InTriggerFloat callback, float min = -1.0f, float max = 1.0f);
public void RegisterInString(string name, InTriggerString callback);
public void RegisterInColorHSV(string name, InTriggerColorHSV callback);
public void RegisterInColorRGB(string name, InTriggerColorRGB callback);
public void RegisterInURL(string name, InTriggerURL callback, string filter = ""); // use filter for file extensions, e.g. *.json


// Output Trigger connections
// Via OutputTriggers your script can control things in your VaM scene. OutputTriggers need to be defined after the scene was loaded, so OnPostLoad() would be perfect. The result of the RegisterOut*** method is a OutTrigger*** object, which you should keep around. Call the Trigger() method to trigger things, some also have a ReadValue() method. The three parameters of the Register methods match those used in VaM for triggers.

public OutTriggerAction RegisterOutAction(string atomID, string receiverID, string actionID);
public OutTriggerBool RegisterOutBool(string atomID, string receiverID, string paramID);
public OutTriggerFloat RegisterOutFloat(string atomID, string receiverID, string paramID);
public OutTriggerString RegisterOutString(string atomID, string receiverID, string paramID);
public OutTriggerColor RegisterOutColor(string atomID, string receiverID, string paramID);
public OutTriggerAudio RegisterOutAudio(string atomID, string receiverID, string paramID);
public OutTriggerScene RegisterOutScene(string atomID, string receiverID, string paramID);
public OutTriggerURL RegisterOutURL(string atomID, string receiverID, string paramID);


// Hotkey connections
// You can hook up a hotkey to a custom callback function. You need to press CTRL plus
// the given key to trigger the callback. This is intended to be used in VaM Desktop.
public void RegisterHotkey(KeyCode key, InTriggerAction callback);

// Logging to VaM Error/Message windows
public void LogError(string message);
public void LogErrorFormat(string message, params object[] args);
public void LogMessage(string message);
public void LogMessageFormat(string message, params object[] args);