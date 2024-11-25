using R2API;
using RoR2;
using SivsContentPack.Config;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using SivsContentPack;
using static SivsContentPack.Config.Configuration.Items;
using System.Diagnostics;
using RoR2.Projectile;
using UnityEngine.AddressableAssets;
using System.Runtime.CompilerServices;
using EntityStates;
using Mono.Security.X509.Extensions;
using SivsContentPack.CustomEntityStates.MiniConstructs;
using System.Linq;
using HarmonyLib;

namespace SivsContentPack.Items
{
    internal class GodTier : ItemTierFactory
    {
        protected override void LoadAssets(ref ItemTierDef itemTierDef)
        {
            itemTierDef = Assets.AssetBundles.Items.LoadAsset<ItemTierDef>("itdGodTier");
            systemPrefab = Assets.AssetBundles.Items.LoadAsset<GameObject>("GodTierSystem");
        }

        protected override void HandleMaterials()
        {
            Material m = Assets.AssetBundles.Items.LoadAsset<Material>("matTracer");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLightningLongBlue");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLightningLongYellow");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGlowItemPickup");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGlow2Soft");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matTracerBright");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGenericFlash");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGenericSwingTrail 1");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGodTierSparkles");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
        }

        protected override void Hooks()
        {
            On.RoR2.PickupDisplay.RebuildModel += PickupDisplay_RebuildModel;
        }

        private void PickupDisplay_RebuildModel(On.RoR2.PickupDisplay.orig_RebuildModel orig, PickupDisplay self, GameObject modelObjectOverride)
        {
            
            PickupDef pickupDef = PickupCatalog.GetPickupDef(self.pickupIndex);
            ItemIndex itemIndex = (pickupDef != null) ? pickupDef.itemIndex : ItemIndex.None;
            if (itemIndex != ItemIndex.None)
            {
                if (ItemCatalog.GetItemDef(itemIndex).tier == Content.ItemTiers.GodTier.tier)
                {
                    Transform t = self.gameObject.transform.parent.Find(systemPrefab.name);
                    if(t != null)
                    {
                        RoR2.Util.PlaySound("Play_UI_item_land_tier3", t.gameObject);
                        t.gameObject.SetActive(true);
                    }
                }
            }

            orig.Invoke(self, modelObjectOverride);
        }
    }
}
