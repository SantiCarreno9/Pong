using UnityEngine;

public class AudioSourceToggle : MonoBehaviour
{
    [SerializeField]
    private AudioSource[] _audioSources;
    [SerializeField]
    private GameObject _onIcon;
    [SerializeField]
    private GameObject _offIcon;

    public void ToggleAudio(bool value)
    {
        for (int i = 0; i < _audioSources.Length; i++)
            _audioSources[i].mute = !value;

        _onIcon.SetActive(value);
        _offIcon.SetActive(!value);
    }
}
