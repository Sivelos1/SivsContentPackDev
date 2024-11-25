using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using RoR2;
using EntityStates;
using UnityEngine.Networking;
using SivsContentPack.Config;
using SivsContentPack.Items;
using System.Linq;


namespace SivsContentPack.CustomEntityStates
{
    public class ChargingLaserBaseState : EntityState
    {
        public float charge;
        public GameObject displayInstance;
        protected bool linkedToDisplay;
        public override void FixedUpdate()
        {
            base.FixedUpdate();

        }

        public void LinkToDisplay()
        {
            if (linkedToDisplay)
            {
                return;
            }
            Transform m = this.GetModelTransform();
            if (m != null)
            {
                CharacterModel cm = m.GetComponent<CharacterModel>();
                List<GameObject> displays = cm.GetEquipmentDisplayObjects(Content.Items.ChargingLaser.equipmentIndex);
                if(displays.Count > 0)
                {
                    displayInstance = displays.First();
                    linkedToDisplay = true;
                }
            }
        }
    }
}
