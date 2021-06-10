using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SoundManagerNamespace;

public class CSSoundManager : MonoBehaviour
{
	public AudioSource[] SoundAudioSources;
	public AudioSource[] MusicAudioSources;

	public static CSSoundManager instance;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	private void Start()
	{
		if (MainRoot.Instance.userConfig.isMusic) SetVolumeMusic(1);        
        else SetVolumeMusic(0);
		if (MainRoot.Instance.userConfig.isSound) SetVolumeSound(1);
		else SetVolumeSound(0);
	}

	#region SOUND
	public void PlaySound(int index)
	{			
		SoundAudioSources[index].PlayOneShotSoundManaged(SoundAudioSources[index].clip);
	}

	public void PlayLoopingSound(int index)
	{		
		SoundAudioSources[index].PlayLoopingSoundManaged();
	}

	public void SetVolumeSound(float value)
	{
		SoundManager.SoundVolume = value;
	}    

    public void StopSound(int index)
	{
		SoundAudioSources[index].Stop();
		SoundAudioSources[index].StopLoopingSoundManaged();
	}
    
	#endregion

	#region MUSIC
	public void PlayMusic(int index)
    {				
		MusicAudioSources[index].PlayOneShotMusicManaged(MusicAudioSources[index].clip);
    }

    public void PlayLoopingMusic(int index)
    {
		MusicAudioSources[index].PlayLoopingMusicManaged();
    }

    public void SetVolumeMusic(float value)
    {
		SoundManager.MusicVolume = value;
    }

    public void StopMusic(int index)
	{
		MusicAudioSources[index].Stop();
		MusicAudioSources[index].StopLoopingMusicManaged();
	}

	#endregion

	public void StopAll()
    {
        SoundManager.StopAll();
    }
}
