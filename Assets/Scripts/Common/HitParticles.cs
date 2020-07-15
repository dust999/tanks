using UnityEngine;

public class HitParticles : ParticlesController
{
    [SerializeField] private float _resetDelay = 2f;
    
    private DeattachableObject _deattachableFX = null;

    private void Awake()
    {
        _deattachableFX = new DeattachableObject(transform);
    }

    public override void PlayStop(bool play = true)
    {
        ResetParticles();

        _deattachableFX.Deattach();

        base.PlayStop();

        CancelInvoke();
        Invoke(nameof(ResetParticles), _resetDelay);
    }

    private void ResetParticles()
    {
        _deattachableFX.Attach();
    }
}
