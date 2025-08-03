using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerWeaponVisuals : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransforms;

    private Animator anim;
    private bool isGrabbingWeapon;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revolver;
    [SerializeField] private Transform autoRiffle;
    [SerializeField] private Transform shotgun;
    [SerializeField] private Transform rifle;

    private Transform currentGun;

    [Header("Rig")] 
    [SerializeField] private float rigWeightIncreaseRate;
    private bool ShouldIncrease_RigWeight;
    private Rig rig;

    [Header("Left hand IK")]
    [SerializeField] private Transform leftHandIK_Target;
    [SerializeField] private TwoBoneIKConstraint leftHandIK;
    [SerializeField] private float leftHandIKIncreaseRate;
    private bool shouldIncrease_LeftHandIKWeight;



    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rig = GetComponentInChildren<Rig>();
        SwitchOn(pistol);
    }
    
    private void Update()
    {
        CheckWeaponSwitch();

        if (Input.GetKeyDown(KeyCode.R) && !isGrabbingWeapon)
        {
            anim.SetTrigger("Reload");
            ReduceRigWeight();
        }
        
        UpdateRigWeight();
        UpdateLeftHandRigWeight();
    }

    private void UpdateLeftHandRigWeight()
    {
        if (shouldIncrease_LeftHandIKWeight)
        {
            leftHandIK.weight += leftHandIKIncreaseRate * Time.deltaTime;
            if (leftHandIK.weight >= 1)
                shouldIncrease_LeftHandIKWeight = false;
        }
    }

    private void UpdateRigWeight()
    {
        if (ShouldIncrease_RigWeight)
        {
            rig.weight += rigWeightIncreaseRate * Time.deltaTime;
            if (rig.weight >= 1)
                ShouldIncrease_RigWeight = false;
        }
    }

    private void ReduceRigWeight()
    {
        rig.weight = 0.15f;
    }
    
    private void PlayWeaponGrabAnimation(GrabType grabType)
    {
        leftHandIK.weight = 0;
        anim.SetFloat("WeaponGrabType", (float)grabType);
        anim.SetTrigger("WeaponGrab");
        ReduceRigWeight();
        
        isGrabbingWeapon = true;
        anim.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }


    public void MaximizeRigWeight() => ShouldIncrease_RigWeight = true;
    public void MaximizeLeftHandRigWeight() => shouldIncrease_LeftHandIKWeight = true;
    public void NotBusyGrabbingWeapon() {
        isGrabbingWeapon = false;
        anim.SetBool("BusyGrabbingWeapon", isGrabbingWeapon);
    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGuns();
        gunTransform.gameObject.SetActive(true);

        currentGun = gunTransform;
        AttachLeftHand();
    }

    private void SwitchOffGuns()
    {
        foreach (var gun in gunTransforms)
        {
            gun.gameObject.SetActive(false);
        }
    }
    
    private void AttachLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTargetTransform>().transform;

        leftHandIK_Target.localPosition = targetTransform.localPosition;
        leftHandIK_Target.localRotation = targetTransform.localRotation;
    }

    private void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < anim.layerCount; i++)
        {
            anim.SetLayerWeight(i, 0);
        }
        
        anim.SetLayerWeight(layerIndex, 1);
    }
    
    private void CheckWeaponSwitch()
    {
        if (Input.inputString.Length > 0)
        {
            foreach (char c in Input.inputString)
            {
                switch (c)
                {
                    case '1':
                        SwitchOn(pistol);
                        SwitchAnimationLayer(1);
                        PlayWeaponGrabAnimation(GrabType.SideGrab);
                        break;
                    case '2':
                        SwitchOn(revolver);
                        SwitchAnimationLayer(1);
                        PlayWeaponGrabAnimation(GrabType.SideGrab);
                        break;
                    case '3':
                        SwitchOn(autoRiffle);
                        SwitchAnimationLayer(1);
                        PlayWeaponGrabAnimation(GrabType.BackGrab);
                        break;
                    case '4':
                        SwitchOn(shotgun);
                        SwitchAnimationLayer(2);
                        PlayWeaponGrabAnimation(GrabType.BackGrab);
                        break;
                    case '5':
                        SwitchOn(rifle);
                        SwitchAnimationLayer(3);
                        PlayWeaponGrabAnimation(GrabType.BackGrab);
                        break;
                    default:
                        // 其他按键不做处理
                        break;
                }
            }
        }
    }
}

public enum GrabType
{
    SideGrab,
    BackGrab
}
