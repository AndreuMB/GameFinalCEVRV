using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f,1f)]
    public float volume;
    [Range(0f,3f)]
    public float pitch;
    public bool loop;
    public bool spatialBlend;
    [Header("3d options")]
    public float minDistance;
    public float maxDistance;
    [HideInInspector]
    public AudioSource source;
    public TagsEnum ownerTag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
