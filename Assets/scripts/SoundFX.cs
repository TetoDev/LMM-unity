using UnityEngine;

public class SoundFX : MonoBehaviour
{
    public static SoundFX instance; // Singleton instance of SoundFX

    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Assign the singleton instance
        }
    }


    public void PlaySound(AudioClip clip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity); // Create a new AudioSource component
    
        audioSource.clip = clip; // Set the audio clip
        audioSource.volume = volume; // Set the volume
        audioSource.Play(); // Play the sound

        float clipLength = audioSource.clip.length; // Get the length of the audio clip
        Destroy(audioSource.gameObject, clipLength); // Destroy the AudioSource object after the clip has finished playing
    }
}
