using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _musicController;
    [SerializeField] private AudioSource _soundController;

    [SerializeField] private GameObject _musicOn;
    [SerializeField] private GameObject _musicOff;
    [SerializeField] private GameObject _soundOn;
    [SerializeField] private GameObject _soundOff;

    [SerializeField] private AudioClip _clickSound;
    [SerializeField] private AudioClip _throwSound;
    [SerializeField] private AudioClip _moveSound;
    [SerializeField] private AudioClip _winSound;

    private void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVol", 0.5f);
        _musicController.volume = musicVol;
        if (musicVol > 0.25f)
        {
            _musicOff.SetActive(false);
            _musicOn.SetActive(true);
        }
        else
        {
            _musicOn.SetActive(false);
            _musicOff.SetActive(true);
        }

        float soundVol = PlayerPrefs.GetFloat("SoundVol", 1);
        _soundController.volume = soundVol;
        if (soundVol == 1)
        {
            _soundOff.SetActive(false);
            _soundOn.SetActive(true);
        }
        else
        {
            _soundOn.SetActive(false);
            _soundOff.SetActive(true);
        }
    }

    public void TurnOffMusic()
    {
        _musicOn.SetActive(false);
        _musicOff.SetActive(true);
        _musicController.volume = 0;
        PlayerPrefs.SetFloat("MusicVol", 0);
        PlayClickSound();
    }

    public void TurnOnMusic()
    {
        _musicOff.SetActive(false);
        _musicOn.SetActive(true);
        _musicController.volume = 0.5f;
        PlayerPrefs.SetFloat("MusicVol", 0.5f);
        PlayClickSound();
    }

    public void TurnOffSounds()
    {
        _soundOn.SetActive(false);
        _soundOff.SetActive(true);
        _soundController.volume = 0;
        PlayerPrefs.SetFloat("SoundVol", 0);
        PlayClickSound();
    }

    public void TurnOnSounds()
    {
        _soundOff.SetActive(false);
        _soundOn.SetActive(true);
        _soundController.volume = 1;
        PlayerPrefs.SetFloat("SoundVol", 1);
        PlayClickSound();
    }

    public void PlayClickSound()
    {
        _soundController.PlayOneShot(_clickSound, 0.1f);
    }

    public void PlayThrowSound()
    {
        _soundController.PlayOneShot(_throwSound);
    }

    public void PlayMoveSound()
    {
        _soundController.PlayOneShot(_moveSound);
    }

    public void MuteMusic()
    {
        _musicController.Stop();
    }

    public void PlayWinSound()
    {
        _soundController.PlayOneShot(_winSound);
    }
}
