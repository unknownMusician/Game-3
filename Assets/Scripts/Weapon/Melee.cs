﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : Weapon {

    const string TAG = "Melee: ";

    #region Public Variables

    [Space]
    [Space]
    [SerializeField]
    private float standardDamage;

    #endregion

    public override void Attack() {

    }
    //protected override void SetSprite() {

    //}
    protected override void InstallModCards() {

    }
    protected override void GetCardsFromChildren() {

    }
    private void OnTriggerEnter2D(Collider2D collision) {
        // collision.gameObject.GetComponent<Player>().DMG();
    }

    #region Service Methods

    public override List<Card> GetAllCardsList() {
        List<Card> cards = new List<Card>();
        if (CardGen)
            cards.Add(CardGen);
        if (CardFly)
            cards.Add(CardFly);
        if (CardEff)
            cards.Add(CardEff);
        return cards;
    }

    #endregion
}
