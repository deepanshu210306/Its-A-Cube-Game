using UnityEngine;

public class MusicFader : MonoBehaviour
{
    public AudioSource musicSource;
    public float fadeTime = 3f;   // fade duration (3 seconds)

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }

    private System.Collections.IEnumerator FadeOutRoutine()
    {
        float startVolume = musicSource.volume;
        float t = 0;

        // use unscaled time so it works even when game is paused
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
    }
}
