using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;
using Cinemachine;


public class Player : NetworkBehaviour
{

    Rigidbody2D rb;
    float inputX;
    float inputY;
    bool inputAttack;
    public float speed;

    public bool isWeaponEquipped;
    public float attack;

    //eventos que ser�o disparados quando o jogador mover o Player e quiser atacar
    public InputEvent OnDirectionChanged;
    public BoolEvent OnAttack;

    public GameObject myCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject.FindGameObjectWithTag("HUD").
          GetComponent<InventoryHUD>().OnChangeEquipment.AddListener(EquipWeapon);

        PolygonCollider2D collider = GameObject.FindGameObjectWithTag("Confiner").GetComponent<PolygonCollider2D>();
        myCamera.GetComponent<CinemachineConfiner2D>().m_BoundingShape2D = collider;

        if (isLocalPlayer == false)
        {
            myCamera.GetComponent<CinemachineVirtualCamera>().enabled = false;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            inputX = Input.GetAxisRaw("Horizontal");
            inputY = Input.GetAxisRaw("Vertical");
            OnDirectionChanged?.Invoke(inputX, inputY);

            inputAttack = Input.GetKeyDown(KeyCode.Space);
            OnAttack?.Invoke(inputAttack);

            rb.velocity = new Vector2(inputX, inputY) * speed;
        }
    }

    void EquipWeapon(SO_Weapons weapon)
    {
        isWeaponEquipped = true;
        attack = weapon.attackBonus;
    }

}
