using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems = null;

    public virtual void PlayStop(bool play = true)
    {
        for(int i = 0; i< _particleSystems.Length; i++)
        {
            if (play)
                _particleSystems[i]?.Play();
            else
                _particleSystems[i]?.Stop();
        }    
    }

    public virtual void StartStopLoop(bool isLoop = true) {

        for (int i = 0; i < _particleSystems.Length; i++)
        {
            ParticleSystem.MainModule psMain = _particleSystems[i].main;
            psMain.loop = isLoop;
        }
    }
}
