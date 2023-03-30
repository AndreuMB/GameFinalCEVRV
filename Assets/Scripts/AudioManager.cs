using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake(){
        foreach (Sound sound in sounds)
        {
            // if (!sound.owner) sound.owner = gameObject;
            if (sound.ownerTag != TagsEnum.Untagged){
                foreach (GameObject owner in GameObject.FindGameObjectsWithTag(sound.ownerTag.ToString()))
                {
                    sound.source = owner.AddComponent<AudioSource>();
                    setSource(ref sound.source,sound);
                }
            }else{
                sound.source = gameObject.AddComponent<AudioSource>();
                setSource(ref sound.source,sound);

            }
            
        }
    }

    void Start(){
        Play("Theme");
    }

    public void Play(string name){
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null) return;
        sound.source.Play();
    }

    private void setSource(ref AudioSource source, Sound sound){
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.spatialBlend = sound.loop ? 1 : 0;
        source.minDistance = sound.minDistance;
        source.maxDistance = sound.maxDistance;
    }
}
