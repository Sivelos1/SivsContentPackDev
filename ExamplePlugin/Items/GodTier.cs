using R2API;
using RoR2;
using SivsContentPack.Config;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using RoR2.Navigation;
using HarmonyLib;
using RoR2.CharacterSpeech;
using UnityEngine.Networking;
using HG;

namespace SivsContentPack.Items
{
    internal class GodMode : ItemFactory
    {
        private static CharacterSpeechController.SpeechInfo[] brotherGodModeReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_GODMODE_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_GODMODE_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_GODMODE_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
        };
        private static CharacterSpeechController.SpeechInfo[] brotherHurtGodModeReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHERHURT_STEALGODMODE_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHERHURT_STEALGODMODE_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHERHURT_STEALGODMODE_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
        };

        private static CharacterSpeechController.SpeechInfo[] falseSonGodModeReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_GODMODE_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_GODMODE_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_GODMODE_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
        };
        private static CharacterSpeechController.SpeechInfo[] falseSonFinalGodModeReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESONFINAL_GODMODE_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESONFINAL_GODMODE_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESONFINAL_GODMODE_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
        };


        private static List<string> lunarAllyAddressables = new List<string>()
        {
            "RoR2/Base/LunarGolem/cscLunarGolem.asset",
            "RoR2/Base/LunarWisp/cscLunarWisp.asset",
            "RoR2/Base/LunarExploder/cscLunarExploder.asset",

        };
        private static List<CharacterSpawnCard> lunarAllyFamily;
        private static GameObject brotherDisplay;
        protected override void LoadAssets(ref ItemDef itemDef)
        {
            itemDef = Assets.AssetBundles.Items.LoadAsset<ItemDef>("GodMode");
            this.displayPrefab = Assets.AssetBundles.Items.LoadAsset<GameObject>("DisplayGodMode");
            brotherDisplay = Assets.AssetBundles.Items.LoadAsset<GameObject>("DisplayBrotherGodmode");
            Content.Misc.GodModeShockwave = Assets.AssetBundles.Items.LoadAsset<GameObject>("LunarShockwave");
            Content.contentPack.projectilePrefabs.AddItem<GameObject>(Content.Misc.GodModeShockwave);
            LoadAllyFamilies();
            Content.DeployableSlots.GodModeLunarAlly = Content.CreateDeployableSlot();
        }

        private void LoadAllyFamilies()
        {
            lunarAllyFamily = new List<CharacterSpawnCard>();
            foreach (var item in lunarAllyAddressables)
            {
                lunarAllyFamily.Add(Addressables.LoadAssetAsync<CharacterSpawnCard>(item).WaitForCompletion());
            }
        }
        protected override void HandleMaterials()
        {
            Material m = Assets.AssetBundles.Items.LoadAsset<Material>("matBrotherArmor 1");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matStoneDollProv");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matStoneDollMithrix");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLunarShockwave");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLunarSpike");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLunarWispFlames");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matTrimsheetMoon");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Snow Topped");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGodModeHead");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Intersection Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matGodModeEye");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matBrotherEyeTrail");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matBrotherDisplayEye");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matBrotherDisplaySigil");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matBrotherDisplayHalo");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
        }
        protected override bool CheckIfEnabled()
        {
            return Configuration.Items.GodMode.enabled.Value;
        }
        protected override void RegisterItemDisplayRules(ref ItemDisplayRuleDict itemDisplayRules)
        {
            itemDisplayRules.Add("CommandoBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(-0.0013F, 0.36551F, -0.04919F),
localAngles = new Vector3(340.4259F, 0F, 0F),
localScale = new Vector3(1.21264F, 1.21264F, 1.21264F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("HuntressBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.24957F, -0.05704F),
localAngles = new Vector3(339.1605F, 0F, 0F),
localScale = new Vector3(0.94249F, 0.94249F, 0.94249F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("Bandit2Body", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(-0.00003F, 0.10369F, 0.00003F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.74448F, 0.74448F, 0.74448F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("ToolbotBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 3.92196F, 1.1803F),
localAngles = new Vector3(286.2522F, 180F, 0F),
localScale = new Vector3(4.2414F, 4.2414F, 4.2414F)
                }
            ]);
            itemDisplayRules.Add("MageBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "HeadCenter",
localPos = new Vector3(0F, 0.09049F, 0.03048F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.90757F, 0.90757F, 0.90757F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("TreebotBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Eye",
localPos = new Vector3(0F, 0.7333F, -0.16892F),
localAngles = new Vector3(281.4878F, 180F, 180F),
localScale = new Vector3(1.38338F, 1.38338F, 1.38338F)
                }
            ]);
            itemDisplayRules.Add("LoaderBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.20015F, 0.00808F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.91329F, 0.91329F, 0.91329F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("MercBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "HeadCenter",
localPos = new Vector3(0F, 0.0921F, 0.06022F),
localAngles = new Vector3(9.41473F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("CaptainBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.18751F, 0.01418F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.95481F, 0.95481F, 0.95481F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("CrocoBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "HeadCenter",
localPos = new Vector3(0.00403F, -0.75599F, 1.23417F),
localAngles = new Vector3(286.2868F, 359.0399F, 181.2771F),
localScale = new Vector3(14.22257F, 14.22257F, 14.22261F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("EngiBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "HeadCenter",
localPos = new Vector3(0F, 0.17094F, 0.0272F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1.11683F, 1.11683F, 1.11683F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("RailgunnerBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.15701F, 0.01434F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.8236F, 0.8236F, 0.8236F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("VoidSurvivorBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.15241F, -0.15441F),
localAngles = new Vector3(314.6253F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("SeekerBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.28309F, -0.04214F),
localAngles = new Vector3(352.7456F, 0F, 0F),
localScale = new Vector3(1.23192F, 1.23192F, 1.23192F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("FalseSonBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.40705F, 0.00206F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1.65F, 1.65F, 1.65F)
                },
                new ItemDisplayRule(){
                    ruleType = ItemDisplayRuleType.LimbMask,
                    limbMask = LimbFlags.Head
                }
            ]);
            itemDisplayRules.Add("ChefBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(-0.83531F, -0.14482F, 0.00148F),
localAngles = new Vector3(287.9571F, 268.4866F, 181.6201F),
localScale = new Vector3(2.49599F, 2.49599F, 2.49599F)
                }
            ]);
            itemDisplayRules.Add("EngiTurretBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Root",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("EngiWalkerTurretBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Root",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("EquipmentDroneBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Base",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("ScavBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Base",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("BrotherBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = brotherDisplay,
                    childName = "Head",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
        }
        protected override void RegisterLanguageTokens()
        {
            base.RegisterLanguageTokens();
        }
        protected override void Hooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.ItemStealController.StolenInventoryInfo.RegisterItemAsAcquired += StolenInventoryInfo_RegisterItemAsAcquired;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            On.RoR2.CharacterSpeech.BrotherSpeechDriver.DoInitialSightResponse += BrotherSpeechDriver_DoInitialSightResponse;
            On.RoR2.CharacterSpeech.FalseSonBossSpeechDriver.DoInitialSightResponse += FalseSonBossSpeechDriver_DoInitialSightResponse;
            if (Configuration.General.godModeMithrixBonusesEnabled.Value)
            {
                On.EntityStates.BrotherMonster.ExitSkyLeap.FireRingAuthority += ExitSkyLeap_FireRingAuthority;
                On.EntityStates.BrotherMonster.Weapon.FireLunarShards.OnEnter += FireLunarShards_OnEnter;
                On.EntityStates.BrotherMonster.UltChannelState.FixedUpdate += UltChannelState_FixedUpdate;
            }
        }

        private void UltChannelState_FixedUpdate(On.EntityStates.BrotherMonster.UltChannelState.orig_FixedUpdate orig, EntityStates.BrotherMonster.UltChannelState self)
        {
            orig.Invoke(self);
            if (self.isAuthority)
            {
                if(self.fixedAge % Configuration.Items.GodMode.brotherUltShockwaveInterval == 0)
                {
                    Ray aimRay = self.GetAimRay();
                    aimRay.origin = self.characterBody.corePosition;
                    int count = Math.Max(self.characterBody.inventory.GetItemCount(Content.Items.Godmode), 1);
                    float damage = self.characterBody.damage * Util.GetStackingBehavior(Configuration.Items.GodMode.shockwaveDamage, Configuration.Items.GodMode.shockwaveDamageStack, count);
                    aimRay.direction = RoR2.Util.ApplySpread(aimRay.direction, 0f, 0f, 0f, 0f, 0f, 0f);
                    FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                    fireProjectileInfo.position = aimRay.origin;
                    fireProjectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                    fireProjectileInfo.crit = self.characterBody.RollCrit();
                    fireProjectileInfo.damage = damage;
                    fireProjectileInfo.damageColorIndex = DamageColorIndex.Item;
                    fireProjectileInfo.owner = self.gameObject;
                    fireProjectileInfo.procChainMask = default(ProcChainMask);
                    fireProjectileInfo.force = 0f;
                    fireProjectileInfo.useFuseOverride = false;
                    fireProjectileInfo.useSpeedOverride = false;
                    fireProjectileInfo.target = null;
                    fireProjectileInfo.projectilePrefab = Content.Misc.GodModeShockwave;
                    ProjectileManager.instance.FireProjectile(fireProjectileInfo);
                }
            }
        }

        private void FireLunarShards_OnEnter(On.EntityStates.BrotherMonster.Weapon.FireLunarShards.orig_OnEnter orig, EntityStates.BrotherMonster.Weapon.FireLunarShards self)
        {
            orig.Invoke(self);
            if (self.isAuthority && self.characterBody.inventory.GetItemCount(Content.Items.Godmode) > 0)
            {
                if(self.skillLocator.primary.stock < self.skillLocator.primary.maxStock - 1)
                {
                    return;
                }
                Ray aimRay = self.GetAimRay();
                Transform transform = self.FindModelChild(EntityStates.BrotherMonster.Weapon.FireLunarShards.muzzleString);
                if (transform)
                {
                    aimRay.origin = transform.position;
                }
                int count = Math.Max(self.characterBody.inventory.GetItemCount(Content.Items.Godmode), 1);
                float damage = self.characterBody.damage * Util.GetStackingBehavior(Configuration.Items.GodMode.shockwaveDamage, Configuration.Items.GodMode.shockwaveDamageStack, count);
                aimRay.direction = RoR2.Util.ApplySpread(aimRay.direction, 0f, 0f, 0f, 0f, 0f, 0f);
                FireProjectileInfo fireProjectileInfo = default(FireProjectileInfo);
                fireProjectileInfo.position = aimRay.origin;
                fireProjectileInfo.rotation = Quaternion.LookRotation(aimRay.direction);
                fireProjectileInfo.crit = self.characterBody.RollCrit();
                fireProjectileInfo.damage = damage;
                fireProjectileInfo.damageColorIndex = DamageColorIndex.Item;
                fireProjectileInfo.owner = self.gameObject;
                fireProjectileInfo.procChainMask = default(ProcChainMask);
                fireProjectileInfo.force = 0f;
                fireProjectileInfo.useFuseOverride = false;
                fireProjectileInfo.useSpeedOverride = false;
                fireProjectileInfo.target = null;
                fireProjectileInfo.projectilePrefab = Content.Misc.GodModeShockwave;
                ProjectileManager.instance.FireProjectile(fireProjectileInfo);
            }
        }
        private void ExitSkyLeap_FireRingAuthority(On.EntityStates.BrotherMonster.ExitSkyLeap.orig_FireRingAuthority orig, EntityStates.BrotherMonster.ExitSkyLeap self)
        {
            orig.Invoke(self);
            if(self.characterBody.inventory.GetItemCount(Content.Items.Godmode) > 0)
            {
                float num = 360f / Configuration.Items.GodMode.brotherSlamShockwaveCount;
                Vector3 point = Vector3.ProjectOnPlane(self.inputBank.aimDirection, Vector3.up);
                Vector3 footPosition = self.characterBody.footPosition;
                for (int i = 0; i < Configuration.Items.GodMode.brotherSlamShockwaveCount; i++)
                {
                    Vector3 forward = Quaternion.AngleAxis(num * (float)i, Vector3.up) * point;
                    if (self.isAuthority)
                    {
                        int count = Math.Max(self.characterBody.inventory.GetItemCount(Content.Items.Godmode), 1);
                        float damage = self.characterBody.damage * Util.GetStackingBehavior(Configuration.Items.GodMode.shockwaveDamage, Configuration.Items.GodMode.shockwaveDamageStack, count);
                        ProjectileManager.instance.FireProjectile(Content.Misc.GodModeShockwave, footPosition, RoR2.Util.QuaternionSafeLookRotation(forward), self.gameObject, damage, 500f, RoR2.Util.CheckRoll(self.characterBody.crit, self.characterBody.master), DamageColorIndex.Item, null, -1f);
                    }
                }
            }
            
        }
        private void FalseSonBossSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.FalseSonBossSpeechDriver.orig_DoInitialSightResponse orig, FalseSonBossSpeechDriver self)
        {
            ReadOnlyCollection<CharacterMaster> readOnlyInstancesList = CharacterMaster.readOnlyInstancesList;
            bool flag = false;
            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                if (readOnlyInstancesList[i].teamIndex == TeamIndex.Player)
                {
                    Inventory inv = readOnlyInstancesList[i].inventory;
                    if (inv != null)
                    {
                        flag = inv.GetItemCount(Content.Items.Godmode) > 0;
                    }
                }
            }
            if (flag)
            {
                string s = self.gameObject.name.Replace("(Clone)", "");
                if(s == "FalseSonBossFinalPhaseSpeechController")
                {
                    self.SendReponseFromPool(falseSonFinalGodModeReactions);
                }
                else
                {
                    self.SendReponseFromPool(falseSonGodModeReactions);
                }
                return;
            }
            orig.Invoke(self);
        }
        private void StolenInventoryInfo_RegisterItemAsAcquired(On.RoR2.ItemStealController.StolenInventoryInfo.orig_RegisterItemAsAcquired orig, object self, ItemIndex itemIndex)
        {
            if (itemIndex == Content.Items.Godmode.itemIndex)
            {
                BrotherSpeechDriver brotherSpeechDriver = BrotherSpeechDriver.FindFirstObjectByType<BrotherSpeechDriver>();
                if (brotherSpeechDriver != null)
                {
                    brotherSpeechDriver.SendReponseFromPool(brotherHurtGodModeReactions);
                }
            }
            orig.Invoke(self, itemIndex);
        }
        private void BrotherSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_DoInitialSightResponse orig, BrotherSpeechDriver self)
        {

            ReadOnlyCollection<CharacterMaster> readOnlyInstancesList = CharacterMaster.readOnlyInstancesList;
            bool flag = false;
            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                if(readOnlyInstancesList[i].teamIndex == TeamIndex.Player)
                {
                    Inventory inv = readOnlyInstancesList[i].inventory;
                    if (inv != null)
                    {
                        flag = inv.GetItemCount(Content.Items.Godmode) > 0;
                    }
                }
            }
            if (flag)
            {
                self.SendReponseFromPool(brotherGodModeReactions);
                return;
            }
            orig.Invoke(self);
        }
        private int CharacterMaster_GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            if (slot == Content.DeployableSlots.GodModeLunarAlly)
            {
                GameObject bodyObject = self.bodyInstanceObject;
                if (bodyObject != null)
                {
                    GodModeController gmc = bodyObject.GetComponent<GodModeController>();
                    if (gmc != null)
                    {
                        return (int)Util.GetStackingBehavior(Configuration.Items.GodMode.summonLimit, Configuration.Items.GodMode.summonLimitStack, gmc.stack);
                    }
                }
            }
            return orig.Invoke(self, slot);
        }
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig.Invoke(self);
            if (self != null)
            {
                Inventory i = self.inventory;
                if (i != null)
                {
                    self.AddItemBehavior<GodModeController>(i.GetItemCount(Content.Items.Godmode));
                }
            }
        }
        protected override void SubmitItemAsAddressablePair(ref ItemDef itemDef)
        {
            Content.SubmitAddressablePickupPair(new Content.AddressablePickupPair
            {
                objectAddress = "RoR2/Base/Brother/BrotherHurtBody.prefab",
                pickupDef = itemDef
            });
        }
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig.Invoke(self, damageInfo, victim);
            GameObject attacker = damageInfo.attacker;
            if (attacker != null)
            {
                CharacterBody cb = attacker.GetComponent<CharacterBody>();
                if (cb != null)
                {
                    GodModeController gmc = attacker.gameObject.GetComponent<GodModeController>();
                    if (gmc != null)
                    {
                        int stack = gmc.stack;
                        float duration = Util.GetStackingBehavior(Configuration.Items.GodMode.crippleDuration, Configuration.Items.GodMode.crippleDurationStack, stack) * damageInfo.procCoefficient;
                        CharacterBody victimBody = victim.GetComponent<CharacterBody>();
                        if (victimBody != null)
                        {
                            victimBody.AddTimedBuff(RoR2Content.Buffs.Cripple, duration);
                        }
                        gmc.FireShockwaves(damageInfo);
                    }
                }
            }
        }
        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            Inventory i = sender.inventory;
            if (i != null)
            {
                int itemCount = i.GetItemCount(Content.Items.Godmode);
                if (itemCount > 0)
                {
                    args.moveSpeedMultAdd += Configuration.Items.GodMode.moveSpeedBonus;
                    args.baseShieldAdd += sender.maxHealth * Configuration.Items.GodMode.shieldPercentage;
                }
            }
        }

        private class GodModeController : CharacterBody.ItemBehavior
        {
            private float shockwaveTimer;

            private float allySummonTimer;

            private List<GameObject> summonedAllies;

            private GameObject displayInstance;

            private const int baseMaxAllies = 1;

            private const int maxAlliesPerStack = 1;

            private const float minSpawnDist = 3f;

            private const float maxSpawnDist = 40f;


            private Xoroshiro128Plus rng;

            private WeightedSelection<CharacterSpawnCard> spawnSelection;

            private DirectorPlacementRule placementRule;
            private int allyLimit
            {
                get
                {
                    return (int)Util.GetStackingBehavior(Configuration.Items.GodMode.summonLimit, Configuration.Items.GodMode.summonLimitStack, stack);
                }
            }
            private void Awake()
            {
                ulong seed = Run.instance.seed ^ (ulong)((long)Run.instance.stageClearCount);
                this.rng = new Xoroshiro128Plus(seed);
                this.spawnSelection = new WeightedSelection<CharacterSpawnCard>(8);
                foreach (var item in lunarAllyFamily)
                {
                    this.spawnSelection.AddChoice(item, 15f);
                }
                this.placementRule = new DirectorPlacementRule
                {
                    placementMode = DirectorPlacementRule.PlacementMode.Approximate,
                    minDistance = 3f,
                    maxDistance = 40f,
                    spawnOnTarget = base.transform
                };
            }

            private void Start()
            {
                summonedAllies = new List<GameObject>();
                this.shockwaveTimer = 0f;
                this.allySummonTimer = Configuration.Items.GodMode.summonDuration;
            }

            private void FixedUpdate()
            {

                if (shockwaveTimer > 0)
                {
                    shockwaveTimer -= Time.deltaTime;
                    if(shockwaveTimer <= 0)
                    {
                        UpdateFX(true);
                    }
                }
                if (allySummonTimer > 0)
                {
                    allySummonTimer -= Time.deltaTime;
                }
                else
                {
                    SummonAlly();
                }
            }
            private void OnMasterSpawned(SpawnCard.SpawnResult spawnResult)
            {
                GameObject spawnedInstance = spawnResult.spawnedInstance;
                if (!spawnedInstance)
                {
                    return;
                }
                CharacterMaster component = spawnedInstance.GetComponent<CharacterMaster>();
                if (component)
                {
                    Deployable component2 = component.gameObject.AddComponent<Deployable>();
                    if (component2)
                    {
                        this.body.master.AddDeployable(component2, Content.DeployableSlots.GodModeLunarAlly);
                    }
                }
            }

            private void UpdateFX(bool var)
            {
                ModelLocator ml = this.body.modelLocator;
                if (ml != null)
                {
                    CharacterModel cm = ml.modelTransform.GetComponent<CharacterModel>();
                    if (cm != null)
                    {
                        List<GameObject> list = cm.GetItemDisplayObjects(Content.Items.Godmode.itemIndex);
                        if (list != null)
                        {
                            foreach (GameObject go in list)
                            {
                                ChildLocator cl = go.GetComponent<ChildLocator>();
                                if(cl != null)
                                {
                                    Transform vfx = cl.FindChild("VFX");
                                    if(vfx != null)
                                    {
                                        vfx.gameObject.SetActive(var);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            private void SummonAlly()
            {
                if(this.body.master.IsDeployableLimited(Content.DeployableSlots.GodModeLunarAlly))
                {
                    return;
                }
                DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(this.spawnSelection.Evaluate(this.rng.nextNormalizedFloat), this.placementRule, this.rng);
                directorSpawnRequest.summonerBodyObject = base.gameObject;
                directorSpawnRequest.onSpawnedServer = new Action<SpawnCard.SpawnResult>(this.OnMasterSpawned);
                directorSpawnRequest.summonerBodyObject = base.gameObject;
                DirectorCore.instance.TrySpawnObject(directorSpawnRequest);
                float cooldown = Configuration.Items.GodMode.summonDuration;
                allySummonTimer += cooldown * (Mathf.Pow(0.5f, stack-1));
            }

            public void FireShockwaves(DamageInfo damageInfo)
            {
                if(shockwaveTimer > 0)
                {
                    return;
                }
                if(damageInfo.procCoefficient <= 0 || damageInfo.damage <= 0)
                {
                    return;
                }
                Vector3 v1 = this.body.aimOrigin;
                Vector3 v2 = damageInfo.position;
                Vector3 v3 = (v2 - v1).normalized;
                float damageCoefficient = Util.GetStackingBehavior(Configuration.Items.GodMode.shockwaveDamage, Configuration.Items.GodMode.shockwaveDamageStack, stack);
                FireProjectileInfo fireProjectileInfo = new FireProjectileInfo()
                {
                    owner = this.body.gameObject,
                    crit = this.body.RollCrit(),
                    damage = damageInfo.damage * damageCoefficient,
                    position = v1,
                    procChainMask = default(ProcChainMask),
                    damageColorIndex = DamageColorIndex.Item,
                    projectilePrefab = Content.Misc.GodModeShockwave,
                    rotation = RoR2.Util.QuaternionSafeLookRotation(v3)
                };
                RoR2.Util.PlayAttackSpeedSound(EntityStates.LunarGolem.FireTwinShots.attackSoundString, base.gameObject, this.body.attackSpeed);
                float initialAngle = -Configuration.Items.GodMode.shockwaveAngle * ((float)Configuration.Items.GodMode.shockwaveCount / 2f);
                for (int i = 0; i < Configuration.Items.GodMode.shockwaveCount; i++)
                {
                    FireProjectileInfo fpi = fireProjectileInfo;
                    fpi.rotation *= Quaternion.Euler(new Vector3(0f, initialAngle + (Configuration.Items.GodMode.shockwaveAngle * (i)), 0f));
                    ProjectileManager.instance.FireProjectile(fpi);
                }
                UpdateFX(false);
                shockwaveTimer += (Configuration.Items.GodMode.shockwaveCooldown / this.body.attackSpeed);
            }
        }
    }
    internal class VoidEye : ItemFactory
    {
        private static CharacterSpeechController.SpeechInfo[] brotherVoidEyeReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_VOIDEYE_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 5f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_VOIDEYE_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 5f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_VOIDEYE_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 5f,
            },
        };
        private static CharacterSpeechController.SpeechInfo[] falseSonVoidEyeReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_VOIDEYE_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 5f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_VOIDEYE_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 5f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_VOIDEYE_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 5f,
            },
        };
        
        public static GameObject fogWard;
        public static GameObject voidlingDisplay;
        protected override void LoadAssets(ref ItemDef itemDef)
        {
            itemDef = Assets.AssetBundles.Items.LoadAsset<ItemDef>("VoidEye");
            displayPrefab = Assets.AssetBundles.Items.LoadAsset<GameObject>("DisplayVoidEye");
            voidlingDisplay = Assets.AssetBundles.Items.LoadAsset<GameObject>("DisplayVoidEyeRaidCrab");
            fogWard = Assets.AssetBundles.Items.LoadAsset<GameObject>("VoidEyeBodyAttachment");
            Content.ProcTypes.singularity = Content.CreateProcType();
            Content.DeployableSlots.Singularity = Content.CreateDeployableSlot();
            Content.Misc.VoidEyeSingularity = Assets.AssetBundles.Items.LoadAsset<GameObject>("SingularityProj");
            ProjectileDeployToOwner pdto = Content.Misc.VoidEyeSingularity.GetComponent<ProjectileDeployToOwner>();
            if(pdto != null)
            {
                pdto.deployableSlot = Content.DeployableSlots.Singularity;
            }
            ContentAddition.AddProjectile(Content.Misc.VoidEyeSingularity);
        }
        protected override void SubmitItemAsAddressablePair(ref ItemDef itemDef)
        {
            Content.SubmitAddressablePickupPair(new Content.AddressablePickupPair
            {
                objectAddress = "RoR2/DLC1/VoidRaidCrab/MiniVoidRaidCrabBodyPhase3.prefab",
                pickupDef = itemDef
            });
        }
        protected override void HandleMaterials()
        {
            Material m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidCrab");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidMetal");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidCrabEyes");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidEyeEye");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Opaque Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidEyeGlow");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidEyeIris");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidEyeBillboard");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityOrb");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Intersection Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityBlast");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Intersection Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityBillboard");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityTracer");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityDust");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidEyeFog");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityTether");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matSingularityDistortion");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Distortion");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matVoidRangeIndicator");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Intersection Remap");
        }
        protected override bool CheckIfEnabled()
        {
            return Configuration.Items.VoidEye.enabled.Value;
        }
        protected override void RegisterItemDisplayRules(ref ItemDisplayRuleDict itemDisplayRules)
        {
            itemDisplayRules.Add("CommandoBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.19439F, 0.10457F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.228F, 0.228F, 0.228F)
                }
            ]);
            itemDisplayRules.Add("HuntressBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.18075F, 0.0446F),
localAngles = new Vector3(347.4589F, 0F, 0F),
localScale = new Vector3(0.2073F, 0.2073F, 0.2073F)
                }
            ]);
            itemDisplayRules.Add("Bandit2Body", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(-0.00002F, 0.03547F, 0.1017F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.13285F, 0.13285F, 0.13285F)
                }
            ]);
            itemDisplayRules.Add("ToolbotBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 2.88184F, -0.26451F),
localAngles = new Vector3(305.3866F, 180F, 0F),
localScale = new Vector3(3.23397F, 3.23397F, 3.23397F)
                }
            ]);
            itemDisplayRules.Add("MageBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "HeadCenter",
localPos = new Vector3(0F, -0.03239F, 0.07385F),
localAngles = new Vector3(13.10351F, 0F, 0F),
localScale = new Vector3(0.15462F, 0.15462F, 0.15462F)
                }
            ]);
            itemDisplayRules.Add("TreebotBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Eye",
localPos = new Vector3(0F, 0.61997F, 0.01559F),
localAngles = new Vector3(271.459F, 0F, 0F),
localScale = new Vector3(0.75721F, 0.75721F, 0.75721F)
                }
            ]);
            itemDisplayRules.Add("LoaderBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.09576F, 0.10152F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.20784F, 0.20784F, 0.20784F)
                }
            ]);
            itemDisplayRules.Add("MercBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.08461F, 0.11357F),
localAngles = new Vector3(3.43563F, 0F, 0F),
localScale = new Vector3(0.19801F, 0.19801F, 0.19801F)
                }
            ]);
            itemDisplayRules.Add("CaptainBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.06742F, 0.09909F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.19443F, 0.19443F, 0.19443F)
                }
            ]);
            itemDisplayRules.Add("CrocoBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 1.57627F, 0.81144F),
localAngles = new Vector3(356.582F, 0F, 180F),
localScale = new Vector3(2.7355F, 2.7355F, 2.7355F)
                }
            ]);
            itemDisplayRules.Add("EngiBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "HeadCenter",
localPos = new Vector3(0F, 0.00013F, 0.08137F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.2404F, 0.2404F, 0.2404F)
                }
            ]);
            itemDisplayRules.Add("RailgunnerBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.04934F, 0.05546F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.17361F, 0.17361F, 0.17361F)
                }
            ]);
            itemDisplayRules.Add("VoidSurvivorBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "ForeArmR",
localPos = new Vector3(0.18858F, 0.16127F, -0.04173F),
localAngles = new Vector3(4.26991F, 97.14848F, 189.4752F),
localScale = new Vector3(0.31548F, 0.31548F, 0.31548F)
                }
            ]);
            itemDisplayRules.Add("SeekerBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.09982F, 0.09006F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.20954F, 0.20954F, 0.20954F)
                }
            ]);
            itemDisplayRules.Add("FalseSonBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(0F, 0.18557F, 0.12626F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(0.33481F, 0.33481F, 0.33481F)
                }
            ]);
            itemDisplayRules.Add("ChefBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Head",
localPos = new Vector3(-0.31937F, 0.10677F, 0F),
localAngles = new Vector3(270F, 90F, 0F),
localScale = new Vector3(0.27914F, 0.27914F, 0.27914F)
                }
            ]);
            itemDisplayRules.Add("EngiTurretBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Root",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("EngiWalkerTurretBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Root",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("EquipmentDroneBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Base",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("ScavBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Base",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("MiniVoidRaidCrabBodyBase", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = voidlingDisplay,childName = "Head",
localPos = new Vector3(0.10425F, 21.00407F, 7.0139F),
localAngles = new Vector3(327.0072F, 359.897F, 0.03388F),
localScale = new Vector3(10F, 10F, 10F)
                }
            ]);
        }
        protected override void RegisterLanguageTokens()
        {
            base.RegisterLanguageTokens();
        }
        protected override void Hooks()
        {
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            On.RoR2.GenericSkill.OnExecute += GenericSkill_OnExecute;
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.Inventory.GiveItem_ItemIndex_int += Inventory_GiveItem_ItemIndex_int;
            On.RoR2.VoidSurvivorController.OnInventoryChanged += VoidSurvivorController_OnInventoryChanged;
            On.RoR2.PickupDisplay.RebuildModel += PickupDisplay_RebuildModel;

            On.RoR2.CharacterSpeech.BrotherSpeechDriver.DoInitialSightResponse += BrotherSpeechDriver_DoInitialSightResponse;
            On.RoR2.CharacterSpeech.FalseSonBossSpeechDriver.DoInitialSightResponse += FalseSonBossSpeechDriver_DoInitialSightResponse;

        }

        private void PickupDisplay_RebuildModel(On.RoR2.PickupDisplay.orig_RebuildModel orig, PickupDisplay self, GameObject modelObjectOverride)
        {
            orig.Invoke(self, modelObjectOverride);
            PickupDef pickupDef = PickupCatalog.GetPickupDef(self.pickupIndex);
            ItemIndex itemIndex = (pickupDef != null) ? pickupDef.itemIndex : ItemIndex.None;
            if(itemIndex == Content.Items.VoidEye.itemIndex)
            {
                if (self.voidParticleEffect)
                {
                    self.voidParticleEffect.SetActive(true);
                }
            }
        }
        private void VoidSurvivorController_OnInventoryChanged(On.RoR2.VoidSurvivorController.orig_OnInventoryChanged orig, VoidSurvivorController self)
        {
            orig.Invoke(self);
            CharacterBody cb = self.characterBody;
            if (cb != null)
            {
                Inventory i = cb.inventory;
                if(i != null)
                {
                    self.voidItemCount += i.GetItemCount(Content.Items.VoidEye);
                }
            }
        }
        private void BrotherSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_DoInitialSightResponse orig, BrotherSpeechDriver self)
        {

            ReadOnlyCollection<CharacterMaster> readOnlyInstancesList = CharacterMaster.readOnlyInstancesList;
            bool flag = false;
            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                if (readOnlyInstancesList[i].teamIndex == TeamIndex.Player)
                {
                    Inventory inv = readOnlyInstancesList[i].inventory;
                    if (inv != null)
                    {
                        flag = inv.GetItemCount(Content.Items.VoidEye) > 0;
                    }
                }
            }
            if (flag)
            {
                self.SendReponseFromPool(brotherVoidEyeReactions);
                return;
            }
            orig.Invoke(self);
        }
        private void FalseSonBossSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.FalseSonBossSpeechDriver.orig_DoInitialSightResponse orig, FalseSonBossSpeechDriver self)
        {
            ReadOnlyCollection<CharacterMaster> readOnlyInstancesList = CharacterMaster.readOnlyInstancesList;
            bool flag = false;
            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                if (readOnlyInstancesList[i].teamIndex == TeamIndex.Player)
                {
                    Inventory inv = readOnlyInstancesList[i].inventory;
                    if (inv != null)
                    {
                        flag = inv.GetItemCount(Content.Items.VoidEye) > 0;
                    }
                }
            }
            if (flag)
            {
                self.SendReponseFromPool(falseSonVoidEyeReactions);
                return;
            }
            orig.Invoke(self);
        }
        private void Inventory_GiveItem_ItemIndex_int(On.RoR2.Inventory.orig_GiveItem_ItemIndex_int orig, Inventory self, ItemIndex itemIndex, int count)
        {
            int itemCount = self.GetItemCount(Content.Items.VoidEye);
            if (itemCount > 0)
            {
                CharacterMaster cm = self.GetComponent<CharacterMaster>();
                if (cm != null)
                {
                    float chance = Util.GetStackingBehavior(Configuration.Items.VoidEye.corruptionChance, Configuration.Items.VoidEye.corruptionChanceStack, itemCount);
                    bool flag = RoR2.Util.CheckRoll(chance, cm);
                    if (flag)
                    {
                        ItemDef corrupted = null;
                        ReadOnlyArray<ItemDef.Pair> pairs = ItemCatalog.GetItemPairsForRelationship(DLC1Content.ItemRelationshipTypes.ContagiousItem);
                        if (pairs.Length > 0)
                        {
                            foreach (var pair in pairs)
                            {
                                if(pair.itemDef1.itemIndex == itemIndex)
                                {
                                    corrupted = pair.itemDef2;
                                }
                            }
                        }
                        if(corrupted != null)
                        {
                            CharacterMasterNotificationQueue.SendTransformNotification(cm, itemIndex, corrupted.itemIndex, CharacterMasterNotificationQueue.TransformationType.ContagiousVoid);
                            itemIndex = corrupted.itemIndex;
                        }
                    }
                }
            }
            orig.Invoke(self, itemIndex, count);
        }
        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            orig.Invoke(self, damageInfo, victim);
            GameObject attacker = damageInfo.attacker;
            if (attacker != null)
            {
                CharacterBody cb = attacker.GetComponent<CharacterBody>();
                if (cb != null)
                {
                    VoidEyeController vec = cb.GetComponent<VoidEyeController>();
                    if (vec != null)
                    {
                        vec.FireSingularity(damageInfo);
                    }
                }
            }
        }
        private void GenericSkill_OnExecute(On.RoR2.GenericSkill.orig_OnExecute orig, GenericSkill self)
        {
            orig.Invoke(self);
            CharacterBody cb = self.characterBody;
            if (cb != null)
            {
                Inventory i = cb.inventory;
                if(i.GetItemCount(Content.Items.VoidEye) <= 0)
                {
                    return;
                }
                SkillLocator skl = cb.skillLocator;
                if (skl != null)
                {
                    bool flag = (skl.special == self);
                    if (flag)
                    {
                        CharacterMaster cm = cb.master;
                        if (cm != null)
                        {
                            if(cm.deployablesList.Count > 0)
                            {
                                foreach (var deployable in cm.deployablesList)
                                {
                                    if (deployable.slot == Content.DeployableSlots.Singularity)
                                    {
                                        GameObject obj = deployable.deployable.gameObject;
                                        if (obj != null)
                                        {
                                            ProjectileExplosion pe = obj.GetComponent<ProjectileExplosion>();
                                            pe.Detonate();
                                        }
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
        }
        private int CharacterMaster_GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            if (slot == Content.DeployableSlots.Singularity)
            {
                return 12;
            }
            return orig.Invoke(self, slot);
        }
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig.Invoke(self);
            Inventory i = self.inventory;
            if (i != null)
            {
                self.AddItemBehavior<VoidEyeController>(i.GetItemCount(Content.Items.VoidEye));
            }
        }

        private class VoidEyeController : CharacterBody.ItemBehavior
        {
            bool originallyImmuneToObliteration;

            bool originallyIsVoid;

            private float singularityTimer;

            private GameObject bodyAttachment;

            private void OnEnable()
            {
                if (!body)
                {
                    this.body = gameObject.GetComponent<CharacterBody>();
                }
                originallyIsVoid = (body.bodyFlags & CharacterBody.BodyFlags.Void) > CharacterBody.BodyFlags.None;
                originallyImmuneToObliteration = (body.bodyFlags & CharacterBody.BodyFlags.ImmuneToVoidDeath) > CharacterBody.BodyFlags.None;
                if (!originallyImmuneToObliteration)
                {
                    body.bodyFlags |= CharacterBody.BodyFlags.ImmuneToVoidDeath;
                }
                if (!originallyIsVoid)
                {

                    body.bodyFlags |= CharacterBody.BodyFlags.Void;
                }
            }
            private void OnDisable()
            {
                if (bodyAttachment)
                {
                    GameObject.Destroy(bodyAttachment);
                }
                if (!originallyImmuneToObliteration)
                {
                    body.bodyFlags &= ~CharacterBody.BodyFlags.ImmuneToVoidDeath;
                }
                if (!originallyIsVoid)
                {
                    body.bodyFlags &= ~CharacterBody.BodyFlags.Void;
                }
            }
        

            private void Start()
            {
                singularityTimer = 0f;
            }
            private void FixedUpdate()
            {
                if (!bodyAttachment)
                {
                    bodyAttachment = GameObject.Instantiate(fogWard, this.body.gameObject.transform);
                    TeamFilter tf = bodyAttachment.GetComponent<TeamFilter>();
                    if(tf != null)
                    {
                        tf.teamIndex = body.teamComponent.teamIndex;
                    }
                    NetworkedBodyAttachment nbo = bodyAttachment.GetComponent<NetworkedBodyAttachment>();
                    if (nbo != null)
                    {
                        nbo.AttachToGameObjectAndSpawn(this.body.gameObject);
                    }
                }
                if (bodyAttachment)
                {
                    SphereZone sphereZone = bodyAttachment.GetComponent<SphereZone>();
                    if(sphereZone != null)
                    {
                        float range = Util.GetStackingBehavior(Configuration.Items.VoidEye.voidFogRadius, Configuration.Items.VoidEye.voidFogRadiusStack, stack);
                        if(sphereZone.radius != range)
                        {
                            sphereZone.radius = range;
                        }
                    }
                }
                if(singularityTimer > 0)
                {
                    singularityTimer -= Time.deltaTime;
                    if(singularityTimer <= 0f)
                    {
                        UpdateFX(true);
                    }
                }
            }
            public void FireSingularity(DamageInfo damageInfo)
            {
                if (singularityTimer > 0 || damageInfo.procChainMask.HasProc(Content.ProcTypes.singularity) || damageInfo.procCoefficient <= 0)
                {
                    return;
                }
                ProcChainMask pci = new ProcChainMask();
                pci.AddProc(Content.ProcTypes.singularity);
                FireProjectileInfo fpi = new FireProjectileInfo()
                {
                    projectilePrefab = Content.Misc.VoidEyeSingularity,
                    crit = body.RollCrit(),
                    owner = body.gameObject,
                    damage = body.damage * Configuration.Items.VoidEye.singularityDamageCoeff * damageInfo.procCoefficient,
                    damageColorIndex = DamageColorIndex.Void,
                    position = damageInfo.position,
                    procChainMask = pci,
                    rotation = Quaternion.identity,
                };
                ProjectileManager.instance.FireProjectile(fpi);
                UpdateFX(false);
                singularityTimer = Configuration.Items.VoidEye.singularityInterval / body.attackSpeed;
            }

            private void UpdateFX(bool var)
            {
                ModelLocator ml = this.body.modelLocator;
                if (ml != null)
                {
                    CharacterModel cm = ml.modelTransform.GetComponent<CharacterModel>();
                    if (cm != null)
                    {
                        List<GameObject> list = cm.GetItemDisplayObjects(Content.Items.VoidEye.itemIndex);
                        if (list != null)
                        {
                            foreach (GameObject go in list)
                            {
                                ChildLocator cl = go.GetComponent<ChildLocator>();
                                if (cl != null)
                                {
                                    Transform vfx = cl.FindChild("VFX");
                                    if (vfx != null)
                                    {
                                        vfx.gameObject.SetActive(var);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    internal class ThunderAura : ItemFactory
    {
        private static CharacterSpeechController.SpeechInfo[] brotherThunderAuraReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_THUNDERAURA_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_THUNDERAURA_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "BROTHER_THUNDERAURA_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 10f,
            },
        };
        private static CharacterSpeechController.SpeechInfo[] falseSonThunderAuraReactions = new CharacterSpeechController.SpeechInfo[]
        {
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_THUNDERAURA_REACTION_1",
                duration = 1f,
                maxWait = 0.1f,
                priority = 50f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_THUNDERAURA_REACTION_2",
                duration = 1f,
                maxWait = 0.1f,
                priority = 50f,
            },
            new CharacterSpeechController.SpeechInfo()
            {
                token = "FALSESON_THUNDERAURA_REACTION_3",
                duration = 1f,
                maxWait = 0.1f,
                priority = 50f,
            },
        };
        private GameObject thunderStormDisplay;
        protected override void LoadAssets(ref ItemDef itemDef)
        {
            itemDef = Assets.AssetBundles.Items.LoadAsset<ItemDef>("ThunderAura");
            this.displayPrefab = Assets.AssetBundles.Items.LoadAsset<GameObject>("DisplayThunderAura");
            thunderStormDisplay = Assets.AssetBundles.Items.LoadAsset<GameObject>("DisplayThunderStorm");
            Content.Misc.ThunderAuraLightning = Assets.AssetBundles.Items.LoadAsset<GameObject>("ThunderAuraStrike");
            ContentAddition.AddProjectile(Content.Misc.ThunderAuraLightning);
            Content.Misc.ThunderAuraOrbiter = Assets.AssetBundles.Items.LoadAsset<GameObject>("ThunderAuraProjectile");
            Content.DeployableSlots.ThunderAuraOrbiter = Content.CreateDeployableSlot();
            Content.Misc.ThunderAuraOrbiter.AddComponent<ThunderAuraOrbiterController>();
            ProjectileDeployToOwner pdto = Content.Misc.ThunderAuraOrbiter.GetComponent<ProjectileDeployToOwner>();
            if(pdto != null)
            {
                pdto.deployableSlot = Content.DeployableSlots.ThunderAuraOrbiter;
            }
            ContentAddition.AddProjectile(Content.Misc.ThunderAuraOrbiter);
        }

        protected override void HandleMaterials()
        {
            Material m = Assets.AssetBundles.Items.LoadAsset<Material>("matFalseSonCore");
            Materials.SubmitMaterialFix(m, "Hopoo Games/Deferred/Standard");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matThunderStorm");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Opaque Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLightning");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLightningIndicatorWall");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLightningInicator");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Intersection Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matShockAttack");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
            m = Assets.AssetBundles.Items.LoadAsset<Material>("matLightningSmall");
            Materials.SubmitMaterialFix(m, "Hopoo Games/FX/Cloud Remap");
        }
        protected override bool CheckIfEnabled()
        {
            return Configuration.Items.ThunderAura.enabled.Value;
        }
        protected override void RegisterItemDisplayRules(ref ItemDisplayRuleDict itemDisplayRules)
        {
            itemDisplayRules.Add("CommandoBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(0.9093F, 0F, -0.95171F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0.00001F, -2.40679F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("HuntressBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(0.80345F, -0.44527F, -0.82662F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0.00001F, -1.9166F),
localAngles = new Vector3(270F, -0.00007F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("Bandit2Body", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(0.52456F, 0.69623F, -0.75745F),
localAngles = new Vector3(16.14824F, 89.99987F, 269.9999F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -1.6506F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("ToolbotBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(-5.59195F, -4.35387F, 5.03663F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, 11.80516F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(5F, 5F, 5F)
                }
            ]);
            itemDisplayRules.Add("MageBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(0.58678F, -0.67138F, -0.60916F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -1.81564F),
localAngles = new Vector3(270F, 0.00001F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("TreebotBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1.91345F, 0F, -1.20924F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -3.56506F),
localAngles = new Vector3(270F, 0.00001F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("LoaderBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1F, 0F, -1F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -1.66341F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("MercBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1F, 0F, -1F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -2.02582F),
localAngles = new Vector3(270F, 0.00001F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("CaptainBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1F, 0F, -1F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -2.0744F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("CrocoBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(6.47746F, 0F, 4.99994F),
localAngles = new Vector3(270F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0.80793F, 11.5744F),
localAngles = new Vector3(90F, 0F, 0F),
localScale = new Vector3(5F, 5F, 5F)
                }
            ]);
            itemDisplayRules.Add("EngiBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1F, 0F, -1F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -1.80672F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("RailgunnerBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1F, 0F, -1F),
localAngles = new Vector3(90F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0F, -1.49524F),
localAngles = new Vector3(270F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("VoidSurvivorBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(1F, 0.87154F, 0.68769F),
localAngles = new Vector3(270F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 0.69132F, 2.03746F),
localAngles = new Vector3(71.25774F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("SeekerBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(0.72143F, 2.08137F, 0F),
localAngles = new Vector3(0F, 180F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(0F, 3.36458F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("FalseSonBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Root",
localPos = new Vector3(0F, 3.73564F, 0F),
localAngles = new Vector3(0F, 0F, 0F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("ChefBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,childName = "Base",
localPos = new Vector3(-1.32172F, 0.00016F, -1.00251F),
localAngles = new Vector3(87.46673F, 190.3822F, 280.7914F),
localScale = new Vector3(1F, 1F, 1F)
                },
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = thunderStormDisplay,childName = "Base",
localPos = new Vector3(-2.49884F, 0F, 0F),
localAngles = new Vector3(0F, 0F, 90F),
localScale = new Vector3(1F, 1F, 1F)
                }
            ]);
            itemDisplayRules.Add("EngiTurretBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Root",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("EngiWalkerTurretBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Root",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("EquipmentDroneBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Base",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
            itemDisplayRules.Add("ScavBody", [
                new ItemDisplayRule()
                {
                    ruleType = ItemDisplayRuleType.ParentedPrefab,
                    followerPrefab = displayPrefab,
                    childName = "Base",
                    localAngles = new UnityEngine.Vector3(0f, 0f, 0f),
                    localPos = new UnityEngine.Vector3(0f, 0f, 0f),
                    localScale = new UnityEngine.Vector3(1f, 1f, 1f),
                }
            ]);
        }
        protected override void RegisterLanguageTokens()
        {
            base.RegisterLanguageTokens();
        }
        protected override void Hooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
            On.RoR2.CharacterMaster.GetDeployableSameSlotLimit += CharacterMaster_GetDeployableSameSlotLimit;
            On.RoR2.CharacterBody.OnInventoryChanged += CharacterBody_OnInventoryChanged;
            On.RoR2.CharacterSpeech.BrotherSpeechDriver.DoInitialSightResponse += BrotherSpeechDriver_DoInitialSightResponse;
            On.RoR2.CharacterSpeech.FalseSonBossSpeechDriver.DoInitialSightResponse += FalseSonBossSpeechDriver_DoInitialSightResponse;
        }

        private int CharacterMaster_GetDeployableSameSlotLimit(On.RoR2.CharacterMaster.orig_GetDeployableSameSlotLimit orig, CharacterMaster self, DeployableSlot slot)
        {
            if (slot == Content.DeployableSlots.ThunderAuraOrbiter)
            {
                GameObject bodyObject = self.bodyInstanceObject;
                if (bodyObject != null)
                {
                    ThunderAuraController tac = bodyObject.GetComponent<ThunderAuraController>();
                    if (tac != null)
                    {
                        return (int)Util.GetStackingBehavior(Configuration.Items.ThunderAura.orbiterMax, Configuration.Items.ThunderAura.orbiterMaxStack, tac.stack);
                    }
                }
            }
            return orig.Invoke(self, slot);
        }
        private void BrotherSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.BrotherSpeechDriver.orig_DoInitialSightResponse orig, BrotherSpeechDriver self)
        {

            ReadOnlyCollection<CharacterMaster> readOnlyInstancesList = CharacterMaster.readOnlyInstancesList;
            bool flag = false;
            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                if (readOnlyInstancesList[i].teamIndex == TeamIndex.Player)
                {
                    Inventory inv = readOnlyInstancesList[i].inventory;
                    if (inv != null)
                    {
                        flag = inv.GetItemCount(Content.Items.ThunderAura) > 0;
                    }
                }
            }
            if (flag)
            {
                self.SendReponseFromPool(brotherThunderAuraReactions);
                return;
            }
            orig.Invoke(self);
        }
        private void FalseSonBossSpeechDriver_DoInitialSightResponse(On.RoR2.CharacterSpeech.FalseSonBossSpeechDriver.orig_DoInitialSightResponse orig, FalseSonBossSpeechDriver self)
        {
            ReadOnlyCollection<CharacterMaster> readOnlyInstancesList = CharacterMaster.readOnlyInstancesList;
            bool flag = false;
            for (int i = 0; i < readOnlyInstancesList.Count; i++)
            {
                if (readOnlyInstancesList[i].teamIndex == TeamIndex.Player)
                {
                    Inventory inv = readOnlyInstancesList[i].inventory;
                    if (inv != null)
                    {
                        flag = inv.GetItemCount(Content.Items.ThunderAura) > 0;
                    }
                }
            }
            if (flag)
            {
                self.SendReponseFromPool(falseSonThunderAuraReactions);
                return;
            }
            orig.Invoke(self);
        }
        private void CharacterBody_OnInventoryChanged(On.RoR2.CharacterBody.orig_OnInventoryChanged orig, CharacterBody self)
        {
            orig.Invoke(self);
            if (self != null)
            {
                Inventory i = self.inventory;
                if (i != null)
                {
                    self.AddItemBehavior<ThunderAuraController>(i.GetItemCount(Content.Items.ThunderAura));
                }
            }
        }
        protected override void SubmitItemAsAddressablePair(ref ItemDef itemDef)
        {
            Content.SubmitAddressablePickupPair(new Content.AddressablePickupPair
            {
                objectAddress = "RoR2/DLC2/FalseSonBoss/FalseSonBossBodyBrokenLunarShard.prefab",
                pickupDef = itemDef
            });
        }
        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, RecalculateStatsAPI.StatHookEventArgs args)
        {
            Inventory i = sender.inventory;
            if (i != null)
            {
                int itemCount = i.GetItemCount(Content.Items.ThunderAura);
                if (itemCount > 0)
                {
                    args.healthMultAdd += itemCount;
                }
            }
        }

        [DisallowMultipleComponent]
        [RequireComponent(typeof(ProjectileOwnerOrbiter))]
        [RequireComponent(typeof(ProjectileController))]
        [RequireComponent(typeof(ProjectileImpactExplosion))]
        public class ThunderAuraOrbiterController : MonoBehaviour
        {
            public void OnEnable()
            {
                this.explosion = base.GetComponent<ProjectileImpactExplosion>();
                if (NetworkServer.active)
                {
                    ProjectileController component = base.GetComponent<ProjectileController>();
                    if (component.owner)
                    {
                        this.AcquireOwner(component);
                        return;
                    }
                    component.onInitialized += this.AcquireOwner;
                }
            }

            private void AcquireOwner(ProjectileController controller)
            {
                controller.onInitialized -= this.AcquireOwner;
                CharacterBody component = controller.owner.GetComponent<CharacterBody>();
                if (component)
                {
                    ProjectileOwnerOrbiter component2 = base.GetComponent<ProjectileOwnerOrbiter>();
                    component.GetComponent<ThunderAuraController>().InitializeOrbiter(component2, this);
                }
            }

            public void Detonate()
            {
                if (this.explosion)
                {
                    this.explosion.Detonate();
                }
            }
            private ProjectileImpactExplosion explosion;
        }
        private class ThunderAuraController : CharacterBody.ItemBehavior
        {
            private float lightningTimer;

            private float orbiterTimer;

            private SphereSearch sphereSearch;

            private Xoroshiro128Plus rng;

            private void Awake()
            {
                sphereSearch = new SphereSearch();
            }

            private void Start()
            {
                lightningTimer = Configuration.Items.ThunderAura.lightningDuration;
            }
            public void InitializeOrbiter(ProjectileOwnerOrbiter orbiter, ThunderAuraOrbiterController tac)
            {
                float orbiterCount = 0f;
                CharacterMaster cm = this.body.master;
                if(cm != null)
                {
                    orbiterCount = cm.GetDeployableCount(Content.DeployableSlots.ThunderAuraOrbiter);
                }
                float extra = Mathf.Floor((orbiterCount / Configuration.Items.ThunderAura.orbiterMax));
                float num = this.body.radius + 4f + (3f * extra);
                float num2 = num / 2f;
                num2 *= num2;
                float degreesPerSecond = 180f * Mathf.Pow(0.9f, num2);
                Quaternion lhs = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 360f), Vector3.up);
                Quaternion rhs = Quaternion.AngleAxis(UnityEngine.Random.Range(0f, 0f), Vector3.forward);
                Vector3 planeNormal = lhs * rhs * Vector3.up;
                float initialDegreesFromOwnerForward = UnityEngine.Random.Range(0f, 360f);
                orbiter.Initialize(planeNormal, num, degreesPerSecond, initialDegreesFromOwnerForward);
            }
            private void LightningStrike()
            {
                List<HurtBox> targets = FindTarget(stack);
                if(targets.Count > 0)
                {
                    for (int i = 0; i < stack; i++)
                    {
                        if(i < targets.Count)
                        {

                            HurtBox target = targets[i];
                            if (target != null)
                            {
                                float fuse = Configuration.Items.ThunderAura.lightningFuse / body.attackSpeed;
                                NodeGraph groundNodes = SceneInfo.instance.groundNodes;
                                Vector3 position = Vector3.zeroVector;
                                Vector3 targetPos = target.transform.position;
                                if (target.healthComponent != null)
                                {
                                    if (target.healthComponent.body != null)
                                    {
                                        CharacterMotor cm = target.healthComponent.body.characterMotor;
                                        if (cm != null)
                                        {
                                            targetPos += cm.velocity * fuse;
                                        }
                                    }
                                }
                                groundNodes.GetNodePosition(groundNodes.FindClosestNode(targetPos, HullClassification.Human), out position);
                                float damageCoefficient = Util.GetStackingBehavior(Configuration.Items.ThunderAura.lightningDamage, Configuration.Items.ThunderAura.lightningDamageStack, stack);
                                FireProjectileInfo fpi = new FireProjectileInfo()
                                {
                                    crit = this.body.RollCrit(),
                                    damage = this.body.damage * damageCoefficient,
                                    owner = this.gameObject,
                                    position = position,
                                    projectilePrefab = Content.Misc.ThunderAuraLightning,
                                    procChainMask = default,
                                    useFuseOverride = true,
                                    fuseOverride = fuse,
                                    rotation = Quaternion.Euler(Vector3.one)
                                };
                                ProjectileManager.instance.FireProjectile(fpi);
                            }
                        }
                    }
                }
                lightningTimer += Configuration.Items.ThunderAura.lightningDuration / body.attackSpeed;
            }
            public List<HurtBox> FindTarget(int count)
            {
                List<HurtBox> validTargets = new List<HurtBox>();
                this.sphereSearch.mask = LayerIndex.entityPrecise.mask;
                this.sphereSearch.origin = this.transform.position;
                this.sphereSearch.radius = Configuration.Items.ThunderAura.lightningMaxDistance + body.radius;
                this.sphereSearch.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                this.sphereSearch.RefreshCandidates();
                TeamMask t = TeamMask.GetEnemyTeams(this.body.teamComponent.teamIndex);
                t.RemoveTeam(TeamIndex.Neutral);
                this.sphereSearch.FilterCandidatesByHurtBoxTeam(t);
                this.sphereSearch.OrderCandidatesByDistance();
                this.sphereSearch.FilterCandidatesByDistinctHurtBoxEntities();
                this.sphereSearch.GetHurtBoxes(validTargets);
                this.sphereSearch.ClearCandidates();
                return validTargets;
            }
            private void OnDisable()
            {
                CharacterMaster cm = this.body.master;
                if (cm != null)
                {
                    foreach (var deployable in cm.deployablesList)
                    {
                        if(deployable.slot == Content.DeployableSlots.ThunderAuraOrbiter)
                        {
                            ProjectileImpactExplosion pie = deployable.deployable.GetComponent<ProjectileImpactExplosion>();
                            if(pie != null)
                            {
                                pie.Detonate();
                            }
                        }
                    }
                }
            }
            private void UpdateOrbiter()
            {
                if (this.body.master.IsDeployableLimited(Content.DeployableSlots.ThunderAuraOrbiter))
                {
                    return;
                }
                FireProjectileInfo fpi = new FireProjectileInfo()
                {
                    crit = this.body.RollCrit(),
                    damage = this.body.damage * Configuration.Items.ThunderAura.orbiterDamageCoefficient,
                    damageColorIndex = DamageColorIndex.Item,
                    owner = this.body.gameObject,
                    position = this.body.corePosition,
                    procChainMask = default,
                    projectilePrefab = Content.Misc.ThunderAuraOrbiter,
                    rotation = Quaternion.identity,
                };
                ProjectileManager.instance.FireProjectile(fpi);
                orbiterTimer += Configuration.Items.ThunderAura.orbiterRechargeInterval * (Mathf.Pow(0.5f, stack - 1));
            }

            private void FixedUpdate()
            {
                if(lightningTimer > 0)
                {
                    lightningTimer -= Time.deltaTime;
                }
                else
                {
                    LightningStrike();
                }
                if(orbiterTimer > 0)
                {
                    orbiterTimer -= Time.deltaTime;
                }
                else
                {
                    UpdateOrbiter();
                }
            }

        }
    }
}
