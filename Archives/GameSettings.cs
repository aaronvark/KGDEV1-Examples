using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public delegate void GameSettingEvent( string setting );

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public static class GameSettings
{
    [System.Serializable]
    class GameSettingsData
    {
        public string[] keys;
        public object[] values;
    }

    const string SETTINGS_STRING = "gamesettings";
    //const string KEY_STRING = "gamesettingsKeys";
	//const string VALUE_STRING = "gamesettingsValues";

	public static GameSettingEvent settingChanged;

	static Dictionary<string, object> _previous;

	static Dictionary<string, object> _settings;
	public static Dictionary<string, object> Settings
    {
        get
        {
            if (_settings == null)
            {
                //load defaults for now
                SetPlatformDefaults();
                //Start background load
                if (Application.isPlaying)
                {
                    GameObject g = new GameObject("gsloader");
                    g.hideFlags = HideFlags.DontSave;
                    MonoBehaviour mb = g.AddComponent<MonoBehaviour>();
                    mb.StartCoroutine(LoadGS(g));
                }
            }

            return _settings;
        }
    }

    public static void Init()
    {
        if (_settings == null)
        {
            SetPlatformDefaults();
        }

        if (Application.isPlaying)
        {
            GameObject g = new GameObject("gsloader");
            g.hideFlags = HideFlags.DontSave;
            MonoBehaviour mb = g.AddComponent<MonoBehaviour>();
            mb.StartCoroutine(LoadGS(g));
        }
    }

	public static void Set( string key, object value )
	{
		//store current when we're making changes
		if ( _previous == null )
		{
			_previous = GetCopy();
		}

		if ( Settings.ContainsKey( key ) )
		{
            if (value is System.Enum)
            {
                int intVal = (int)value;
                Settings[key] = intVal;
            }
            else
            {
                Settings[key] = value;
            }
		}
		else
		{
            if (value is System.Enum)
            {
                int intVal = (int)value;
                Settings.Add(key, intVal);
            }
            else
            {
                Settings.Add(key, value);
            }
		}

		//Debug.Log ( "Setting changed: "+ key + " to " + value );

		if ( settingChanged != null )
		{
			settingChanged( key );
		}
	}

	public static bool Get<T>( string key, out T value )
	{
#if UNITY_XBOXONE || UNITY_PS4
        //will happen upon startup with certain scripts in startup scene
        if ( _settings == null && !DataUtils.storageReady )
        {
            value = default(T);
            return false;
        }
#endif

        if ( Settings.ContainsKey( key ) )
		{
			value = (T)Settings[key];
			return true;
		}

		value = default(T);
		return false;
	}
	
	public static bool HasChanges()
	{
		return _previous != null;
	}

    /// <summary>
    /// Don't call this too often. Preferably only when the user exits the settings menu (and after any necessary confirmations).
    /// </summary>
    public static void Apply(bool save = true)
    {
        _previous = null;
        if (save)
        {
            Save();
        }
	}

	public static void Revert()
	{
		if ( _previous != null )
		{
			Settings.Clear ();

			foreach( KeyValuePair<string,object> pair in _previous )
			{
				Set( pair.Key, pair.Value );
			}

			_previous = null;
		}
	}

	static Dictionary<string, object> GetCopy()
	{
		Dictionary<string, object> copy = new Dictionary<string, object>();
		foreach( KeyValuePair<string,object> pair in _settings )
		{
			copy.Add ( pair.Key, pair.Value );
		}
		return copy;
	}
	
	/// <summary>
	/// Sets platform-relevant defaults and saves them to persistent storage.
	/// </summary>
	static void SetPlatformDefaults()
	{
		//DISCUSS: Is this necessary?
		_settings = new Dictionary<string, object>();
		
		//TODO: Maybe do a locale check?
		Set ( "language", LANGUAGE.ENGLISH );

#if UNITY_EDITOR || DEMO_BUILD
        Set("r_width", Screen.width);
        Set("r_height", Screen.height);
        Set("post_fx", 0);
        Set("battery_save", 1);
        Set("rumble", 1);
        Set("lefty", false);
        Set("language", LANGUAGE.ENGLISH);
        Set(VolumeManager.MASTER_VOLUME, 10);
        Set(VolumeManager.MUSIC_VOLUME, 10);
        Set(VolumeManager.SFX_VOLUME, 10);
#elif UNITY_IPHONE
		Set ( "r_width", Screen.width );
		Set ( "r_height", Screen.height );
		Set ( "post_fx", 0 );
		Set ( "battery_save", 1 );
        Set("rumble", 0);
		Set ( "lefty", false );
        Set(VolumeManager.MASTER_VOLUME, 10);
        Set(VolumeManager.MUSIC_VOLUME, 10);
        Set(VolumeManager.SFX_VOLUME, 10);
#elif UNITY_ANDROID
		Set ( "r_width", Screen.width );
		Set ( "r_height", Screen.height );
		Set ( "post_fx", 0 );
		Set ( "battery_save", 1 );
        Set("rumble", 0);
		Set ( "lefty", false );
		Set ( "language", LANGUAGE.ENGLISH );
        Set(VolumeManager.MASTER_VOLUME, 10);
        Set(VolumeManager.MUSIC_VOLUME, 10);
        Set(VolumeManager.SFX_VOLUME, 10);
#elif UNITY_XBOXONE || UNITY_PS4
        Set( "r_width", 1920 );
		Set ( "r_height", 1080 );
		Set ( "post_fx", 1 );
		Set ( "battery_save", 0 );
        Set("rumble", 1);
		Set ( "lefty", false );
		Set ( "language", LANGUAGE.ENGLISH );
        Set(VolumeManager.MASTER_VOLUME, 10);
        Set(VolumeManager.MUSIC_VOLUME, 10);
        Set(VolumeManager.SFX_VOLUME, 10);
#elif UNITY_STANDALONE
		Set ( "r_width", Screen.width );
		Set ( "r_height", Screen.height );
		Set ( "post_fx", 1);
		Set ( "battery_save", 0 );
        Set("rumble", 1);
		Set ( "lefty", false );
		Set ( "language", LANGUAGE.ENGLISH );
        Set(VolumeManager.MASTER_VOLUME, 10);
        Set(VolumeManager.MUSIC_VOLUME, 10);
        Set(VolumeManager.SFX_VOLUME, 10);
#endif

        Apply(false);
	}

	static void Save()
	{
		_previous = null;
		
		List<string> keys = new List<string>();
		List<object> values = new List<object>();
		
		foreach (string key in Settings.Keys)
		{
			keys.Add(key);
		}
		foreach (object value in Settings.Values)
		{
			values.Add(value);
		}

        GameSettingsData data = new GameSettingsData();
        data.keys = keys.ToArray();
        data.values = values.ToArray();
        DataUtils.SaveToFile(data, Application.persistentDataPath, SETTINGS_STRING, true);
	}

    static IEnumerator LoadGS( GameObject loader )
    {
        while (!DataUtils.storageReady) yield return null;

        DataLoader<GameSettingsData> dataLoader = new DataLoader<GameSettingsData>(Application.persistentDataPath, SETTINGS_STRING);

        while (!dataLoader.isDone ) yield return null;

        if (dataLoader.Data != null )
        {
            _settings = new Dictionary<string, object>();

            for (int i = 0; i < dataLoader.Data.keys.Length; ++i)
            {
                _settings.Add(dataLoader.Data.keys[i], dataLoader.Data.values[i]);
                if ( settingChanged != null )
                {
                    settingChanged(dataLoader.Data.keys[i]);
                }
            }
        }
        else
        {
            SetPlatformDefaults();
        }

#if UNITY_EDITOR
        if (Application.isPlaying)
        {
            GameObject.Destroy(loader);
        }
        else
        {
            GameObject.DestroyImmediate(loader);
        }
#else
         GameObject.Destroy(loader);
#endif

        yield return null;
    }
}
