using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControls controls;

    [Header("Aim Info")]
    [SerializeField] private LayerMask aimLayer;
    [SerializeField] private Transform aim;
    private Vector3 lookingDirection;
    private Vector2 aimInput;
    void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }

    private void Update()
    {
        aim.position = new Vector3(GetMousePosition().x , transform.position.y + 1, GetMousePosition().z);
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayer))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
    
    private void AssignInputEvents()
    {
        controls = player.controls;
        
        controls.Charactor.Aim.performed += ctx=>aimInput = ctx.ReadValue<Vector2>();
        controls.Charactor.Aim.canceled += ctx=>aimInput = Vector2.zero;
    }
}
