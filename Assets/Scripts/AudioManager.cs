using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake(){

        if (instance == null)
        {
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        
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
            if (sound.autoplay){
                Play(sound.name);
            }  
        }
    }

    void Start(){
        Scene scene = SceneManager.GetActiveScene();
        if (scene.rootCount == 0)
        {
            Play("ThemeMM");
        }
    }

    public void Play(string name){
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null ||sound.source == null) return;
        sound.source.Play();
    }

    private void setSource(ref AudioSource source, Sound sound){
        source.clip = sound.clip;
        source.volume = sound.volume;
        source.pitch = sound.pitch;
        source.loop = sound.loop;
        source.spatialBlend = sound.spatialBlend ? 1 : 0;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = sound.minDistance;
        source.maxDistance = sound.maxDistance;
    }
}
