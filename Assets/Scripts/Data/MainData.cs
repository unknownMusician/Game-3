﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainData : MonoBehaviour
{
    #region Main GameObjects

    private static GameObject _player;

    public static GameObject PlayerObject
    {
        get => _player;
        set
        {
            _player = value;
            ActionPlayerChange?.Invoke();
        }
    }
    /// <summary>
    /// Calls only on the start of each scene;
    /// </summary>
    public static Action ActionPlayerChange = ActionPlayerCoinsChange + ActionPlayerPositionChange + ActionHPChange + ActionSPChange + ActionOPChange;

    public static GameObject RoomSpawnerObject { get; set; }

    #endregion

    #region Player

    public static PlayerBehaviour PlayerBehaviour => PlayerObject?.GetComponent<PlayerBehaviour>();

    //

    public static Action ActionPlayerPositionChange;
    public static Vector3 PlayerPosition { get => PlayerObject.transform.position; }

    private static int coins = 5;
    public static Action ActionPlayerCoinsChange;
    public static int PlayerCoins {
        get => coins;
        set {
            coins = value;
            ActionPlayerCoinsChange?.Invoke();
        }
    }

    public static float PlayerHP => PlayerBehaviour.HP;
    public static float PlayerMaxHP => PlayerBehaviour.MaxHP;
    public static Action ActionHPChange;
    public static float PlayerSP => PlayerBehaviour.SP;
    public static float PlayerMaxSP => PlayerBehaviour.MaxSP;
    public static Action ActionSPChange;
    public static float PlayerOP => PlayerBehaviour.OP;
    public static float PlayerMaxOP => PlayerBehaviour.MaxOP;
    public static Action ActionOPChange;

    #endregion

    #region Inventory & Guns

    public static Inventory Inventory => PlayerBehaviour.Inventory;
    public static Weapon ActiveWeapon => Inventory?.Weapon;

    public static Action ActionInventoryCardsChange;
    public static Action ActionInventoryWeaponsChange;
    public static Action ActionInventoryActiveSlotChange;

    public static Action ActionGunBulletsChange;

    #endregion

    #region RoomSpawner

    public static RoomSpawner RoomSpawner => RoomSpawnerObject.GetComponent<RoomSpawner>();

    #endregion

    #region Level

    private static int level = 1;
    // To-Do: add level to MainData;
    public static Action ActionLevelChange;
    public static int Level {
        get => level;
        set {
            level = value;
            ActionLevelChange?.Invoke();
        }
    }

    #endregion

    #region UI

    public static GameUI GameUI { get; set; }

    #endregion

    #region Input

    public static InputMaster Controls { get; private set; }

    private void SetControlsActions() {

        #region Weapon

        Controls.Weapon.AttackPress.performed += ctx => { if (!Pause.GameIsPaused) Inventory.AttackWithWeaponOrFistPress(); };
        Controls.Weapon.AttackRelease.performed += ctx => { if (!Pause.GameIsPaused) Inventory.AttackWithWeaponOrFistRelease(); };
        Controls.Weapon.ChangeWeaponState.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ChangeWeaponState(); };
        Controls.Weapon.Reload.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ReloadGun(); };
        Controls.Weapon.ThrowPress.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ThrowPress(); };
        Controls.Weapon.ThrowRelease.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ThrowRelease(); };
        Controls.Weapon.Slot1.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ActiveSlot = Inventory.Slots.FIRST; };
        Controls.Weapon.Slot2.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ActiveSlot = Inventory.Slots.SECOND; };
        Controls.Weapon.Slot3.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ActiveSlot = Inventory.Slots.THIRD; };
        Controls.Weapon.Slot4.performed += ctx => { if (!Pause.GameIsPaused) Inventory.ActiveSlot = Inventory.Slots.FOURTH; };
        Controls.Weapon.ChangeSlot.performed += ctx => {
            if (!Pause.GameIsPaused)
                _ = Mouse.current.scroll.ReadValue().y < 0 ? Inventory.ActiveSlot-- : Inventory.ActiveSlot++;
        };
        Controls.Weapon.AimMouse.performed += ctx => {
            if (!Pause.GameIsPaused) {
                Vector3 worldCursor = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Inventory.Aim(worldCursor - PlayerObject.transform.position); // Weapon
                PlayerBehaviour.Side = PlayerBehaviour.CheckSideLR(worldCursor); // Player
            }
        };
        Controls.Weapon.AimStick.performed += ctx => {
            if (!Pause.GameIsPaused) {
                Vector3 localCursor = ctx.ReadValue<Vector2>();
                Inventory.Aim(localCursor); // Weapon
                PlayerBehaviour.Side = PlayerBehaviour.CheckSideLR(PlayerObject.transform.position + localCursor); // Player
            }
        };

        #endregion

        #region Player

        Controls.Player.Jump.performed += ctx => { if (!Pause.GameIsPaused) PlayerBehaviour.Jump(); };

        Controls.Player.Sneak.performed += ctx => { if (!Pause.GameIsPaused) PlayerBehaviour.IsSneaking = true; };
        Controls.Player.NoSneak.performed += ctx => { if (!Pause.GameIsPaused) PlayerBehaviour.IsSneaking = false; };

        Controls.Player.Run.performed += ctx => { if (!Pause.GameIsPaused) PlayerBehaviour.IsRunning = true; };
        Controls.Player.NoRun.performed += ctx => { if (!Pause.GameIsPaused) PlayerBehaviour.IsRunning = false; };

        Controls.Player.Interact.performed += ctx => { if (!Pause.GameIsPaused) PlayerBehaviour.TryInteract(); };

        Controls.Player.Move.performed += ctx => { PlayerBehaviour.IsMoving = true; };
        Controls.Player.NoMove.performed += ctx => { PlayerBehaviour.IsMoving = false; };

        #endregion

        #region UI

        Controls.UI.Menu.performed += ctx => {
            Pause.GameIsPaused = !Pause.GameIsPaused;
            GameUI.menu.SetActive(Pause.GameIsPaused);
        };

        Controls.UI.WeaponSettings.performed += ctx => {
            Pause.GameIsPaused = !Pause.GameIsPaused;
            GameUI.weaponSettings.SetActive(Pause.GameIsPaused);
        };

        #endregion

    }

    #endregion

    #region Mono

    private void Awake() {
        Controls = new InputMaster();
        SetControlsActions();
    }
    private void OnEnable() {
        Controls.Enable();
    }
    private void OnDisable() {
        Controls.Disable();
    }

    #endregion
}
