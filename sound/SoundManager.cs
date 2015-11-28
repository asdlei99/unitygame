using System;
using System.Collections.Generic;
using UnityEngine;
class SoundManager: BaseObject
{
    public static SoundManager getInstance()
    {
        return Singleton<SoundManager>.getInstance();
    }

    AudioSource mBackgroundAS;

    public SoundManager()
    {
        mBackgroundAS = loadClipOnObj(GlobalObject.getGlobalObject(), "sounds/background_kunlundao");
    }

    public void playBackground()
    {
        if (!mBackgroundAS.isPlaying)
        {
            mBackgroundAS.Play();
        }
    }

    public void stopBackground()
    {
        if (mBackgroundAS.isPlaying)
        {
            mBackgroundAS.Stop();
        }
    }

    public void setBackground(string path)
    {
        bool isPlaying = mBackgroundAS.isPlaying;
        if (isPlaying)
        {
            mBackgroundAS.Pause();
        }
        mBackgroundAS.clip = ResourceManager<AudioClip>.load(path);
        if (isPlaying)
        {
            mBackgroundAS.UnPause();
        }
    }

    AudioSource loadClipOnObj(GameObject obj, string clipPath, bool isLoop = true)
    {
        AudioSource aSrc = obj.GetComponent<AudioSource>();
        if (aSrc == null)
        {
            aSrc = obj.AddComponent<AudioSource>();
            aSrc.clip = ResourceManager<AudioClip>.load(clipPath);
        }
        else
        {
            if (aSrc.isPlaying)
            {
                aSrc.Stop();
            }
        }
        aSrc.playOnAwake = isLoop;
        aSrc.loop = isLoop;
        return aSrc;
    }

    public void playEffect(GameObject obj, string clipPath)
    {
        AudioSource.PlayClipAtPoint(ResourceManager<AudioClip>.load(clipPath), obj.transform.position);
    }

    public void playClip(GameObject obj, string clipPath, bool isLoop = false)
    {
        //播放新音乐
        AudioSource aSrc = loadClipOnObj(obj, clipPath, isLoop);
        aSrc.Play();
    }

    public void pauseClip(GameObject obj)
    {
        AudioSource aSrc = obj.GetComponent<AudioSource>();
        if (aSrc != null)
        {
            aSrc.Pause();
        }
    }

    public void resumeClip(GameObject obj)
    {
        AudioSource aSrc = obj.GetComponent<AudioSource>();
        if (aSrc != null)
        {
            aSrc.UnPause();
        }
    }

    public void stopClip(GameObject obj)
    {
        AudioSource aSrc = obj.GetComponent<AudioSource>();
        if(aSrc != null)
        {
            aSrc.Stop();
        }
    }
}