using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioSource musicSourceOne;
    [SerializeField] private AudioSource musicSourceTwo;

    private bool _didPlayerAlreadyWin;

    void Start()
    {
        musicSourceOne.Play();
        _didPlayerAlreadyWin = false;
    }

    private void Update()
    {
        if(SpawnWaves.Instance.didPlayerWin == true && _didPlayerAlreadyWin == false)
        {
            _didPlayerAlreadyWin = true;
            musicSourceOne.Stop();
            musicSourceTwo.Play();
        }
    }
}
