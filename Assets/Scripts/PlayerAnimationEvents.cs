using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals _visuals;

    private void Start()
    {
        _visuals = GetComponentInParent<PlayerWeaponVisuals>();
    }

    public void RealoadIsOver()
    {
        _visuals.MaximizeRigWeight();
    }

    public void WeaponGrabIsOver()
    {
        _visuals.NotBusyGrabbingWeapon();
    }

    public void ReturnRig()
    {
        _visuals.MaximizeRigWeight();
        _visuals.MaximizeLeftHandRigWeight();
    }
}
