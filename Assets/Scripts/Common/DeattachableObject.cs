using UnityEngine;

public class DeattachableObject
{    
    private Transform _objectTransform, _parent = null;
    private Vector3 _startPosition, _startRotation;

    public DeattachableObject(Transform objectTrasnform)
    {
        _objectTransform = objectTrasnform;
        _parent = objectTrasnform.parent;
        _startPosition = objectTrasnform.localPosition;
        _startRotation = objectTrasnform.localEulerAngles;
    }

    public virtual void Deattach()
    {
        _objectTransform.parent = null;
    }

    public virtual void Attach()
    {
        _objectTransform.parent = _parent;
        _objectTransform.localPosition = _startPosition;
        _objectTransform.localEulerAngles = _startRotation;
    }
}
