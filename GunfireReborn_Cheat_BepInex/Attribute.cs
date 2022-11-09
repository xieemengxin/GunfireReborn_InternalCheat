using HarmonyLib;
using HeroCameraName;
using Il2CppSystem.Collections.Generic;
using Item;
using SkillBolt;
using System;
using System.Text;
using UnityEngine;
using Object = System.Object;

namespace GunfireReborn_Cheat_BepInex
{
    /*
     @属性
     */
    public class Attribute : MonoBehaviour
    {
        //开关
        public static bool infinityAmo;
        public static bool weaopnAttr;
        public static bool attSpeed;
        public static bool highJump;
        public static bool movSpeed;
        public static bool speedAsdie;
        public static int attQauntity;
        public static float radius;
        public static int attSpeedval;
        public static float Bulletspeed;
        public static float movSpeedval;
        public static float highJumpval;
        public static bool damInteval;//秒伤
        public static bool immediateCharge;//秒蓄力
        public static bool airJump;//踏空跳
        public static bool singleFov;//单倍镜

        //局部变量
        public static float originSpeed = 0f;
        public static float originHigh = 0f;
        public static List<int> charge = new List<int>();
        
      

        void Update() {
            if (HeroCameraManager.HeroObj != null && HeroCameraManager.HeroObj.BulletPreFormCom != null && HeroCameraManager.HeroObj.BulletPreFormCom.weapondict != null)
            {

                foreach (KeyValuePair<int, WeaponPerformanceObj> keyValuePair in HeroCameraManager.HeroObj.BulletPreFormCom.weapondict)
                {
                    //无限子弹
                    if (infinityAmo) {
                        List<int> Accuracy = new List<int>(4);
                        Accuracy.Add(100);
                        Accuracy.Add(100);
                        Accuracy.Add(100);
                        Accuracy.Add(10000);
                        keyValuePair.Value.WeaponAttr.Accuracy = Accuracy;
                        keyValuePair.Value.WeaponAttr.Stability = Accuracy;
                        keyValuePair.Value.ModifyBulletInMagzine(99999, 99999);
                        keyValuePair.Value.WeaponAttr.MaxBullet = attQauntity;
                        keyValuePair.Value.WeaponAttr.CurBullet = attQauntity;
                    }
                    //武器属性
                    if (weaopnAttr) {
                        keyValuePair.value.WeaponAttr.BulletSpeed = Bulletspeed;
                        keyValuePair.value.WeaponAttr.Radius = radius;
                        keyValuePair.value.WeaponAttr.AttDistance = 999f;
                        keyValuePair.Value.WeaponAttr.SnipeTime = 0;
                        keyValuePair.Value.WeaponAttr.ThrowInterval = 0;
                    }
                    //攻速
                    if (attSpeed) {
                        List<int> list2 = new List<int>(4);
                        list2.Add(attSpeedval);
                        list2.Add(0);
                        list2.Add(0);
                        list2.Add(attSpeedval);
                        keyValuePair.value.WeaponAttr.AttSpeed = list2;
                    }
                    //充能  charge
                    if (immediateCharge && charge.Count == 0) {
                        charge.Add(10);
                        charge.Add(0);
                        charge.Add(0);
                        charge.Add(10000);
                    }
                    if (singleFov) {
                        keyValuePair.Value.WeaponAttr.SnipeFov = 100;
                    }
                    
                }
            }

            //高跳
            if (originHigh != 0f)
            {
                if (highJump)
                {
                    HeroMoveManager.HMMJS.jumping.baseHeight = highJumpval * originHigh;
                }
                else
                {
                    HeroMoveManager.HMMJS.jumping.baseHeight = originHigh;
                }
            }
            else
            {
                originHigh = HeroMoveManager.HMMJS.jumping.baseHeight;
            }
            //移速
            if (movSpeed)
            {
                if (HeroCameraManager.HeroObj.playerProp.HP == 0f && !speedAsdie)
                {
                    HeroMoveManager.HMMJS.maxForwardSpeed = (float)(0.5 * (double)originSpeed);
                    HeroMoveManager.HMMJS.maxBackwardsSpeed = (float)(0.5 * (double)originSpeed);
                    HeroMoveManager.HMMJS.maxSidewaysSpeed = (float)(0.5 * (double)originSpeed);
                }
                else if (originSpeed != 0f)
                {
                    HeroMoveManager.HMMJS.maxForwardSpeed = originSpeed * movSpeedval;
                    HeroMoveManager.HMMJS.maxBackwardsSpeed = originSpeed * movSpeedval;
                    HeroMoveManager.HMMJS.maxSidewaysSpeed = originSpeed * movSpeedval;
                }
                else
                {
                    originSpeed = HeroMoveManager.HMMJS.maxForwardSpeed;
                }
            }

        }

        //Patch
        [HarmonyPatch(typeof(WeaponPerformanceObj), "ConsumeBulletFromMag")]
        public class ConsumeBulletFromMag
        {
 
            public static bool Prefix(int eatbullet, int PreMag)
            {
                if (infinityAmo)
                    eatbullet = 0;
                return true;
            }
        }


        [HarmonyPatch(typeof(WeaponPerformanceObj), "ConsumeBulletFromPack")]
        public class ConsumeBulletFromPack
        {

            public static bool Prefix(int eatbullet, int PreMag)
            {
                if (infinityAmo)
                    eatbullet = 0;
                return true;
            }
        }


        [HarmonyPatch(typeof(WeaponPerformanceObj), "WeaponConsumeBullet")]
        public class WeaponConsumeBullet
        {

            public static bool Prefix(int amount, SkillCollectData final)
            {
                return !infinityAmo;
            }
        }


        [HarmonyPatch(typeof(CSkillBase), "sendCostBullet")]
        public class sendCostBullet
        {
 
            public static bool Prefix()
            {
                return !infinityAmo;
            }
        }

        [HarmonyPatch(typeof(CSkillBase), "ConsumeBullet")]
        public class ConsumeBullet
        {

            public static bool Prefix()
            {
                return !infinityAmo;
            }
        }

        [HarmonyPatch(typeof(CSkillBase), "SelfSkillConsumeBullet")]
        public class SelfSkillConsumeBullet
        {

            public static bool Prefix(int skillSid)
            {
                return !infinityAmo;
            }
        }
        [HarmonyPatch(typeof(ReloadBulletCom), "CanReloadBullet")]
        public class CanReloadBullet
        {

            public static bool Prefix(out ReloadBulletType msg, ref bool __result)
            {
              
                if (infinityAmo)
                {
                    msg = ReloadBulletType.NoReloadSkill;
                    __result = false;
                }
                else
                {
                    msg = ReloadBulletType.FreeBulletTwo;
                    __result = true;
                }
                return !infinityAmo;
            }
        }

        //-----------------------扩散范围----------------------
        //GetSkillScaleByRadius   显示
        [HarmonyPatch(typeof(CArgBase), "GetSkillScaleByRadius")]
        public class ScatterHackSkill
        {

            public static bool Prefix(CSkillBase skill, ref Vector3 __result)
            {
                __result = new Vector3(1f, 1f, 1f);
                return false;
            }
        }
        [HarmonyPatch(typeof(CArgBase), "GetScatterRadius")]
        public class ScatterHack
        {
            public static bool Prefix(CSkillBase skill, ref float __result)
            {
                if (weaopnAttr)
                {
                    __result = radius;
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
        [HarmonyPatch(typeof(NewItemCache), "GetItemProp")]
        public class ItemHack
        {
            public static void Postfix(int itemid, ref NewItemProp __result)
            {
                
                if (weaopnAttr || attSpeed || damInteval)
                {
                    
                    __result.Radius = radius;
                    if (damInteval)
                    {
                        __result.DamInterval = 10;
                        __result.KeepTime = 300;
                    }
                    if (immediateCharge)
                    {
                        __result.ChargeTime = charge;

                    }
                    if (attSpeed)
                    {
                        List<int> list = new List<int>(4);
                        list.Add(attSpeedval);
                        list.Add(0);
                        list.Add(0);
                        list.Add(attSpeedval);
                        __result.AttSpeed = list;
                    }
                }
            }
        }


        //-----------------------攻击距离----------------------

        [HarmonyPatch(typeof(CArgBase), "GetWeaponAttackDis")]
        public class DisHack
        {
            public static void Postfix(CSkillBase skill, ref float __result)
            {
                if (weaopnAttr)
                {
                    __result = 9999f;
                }


            }
        }
      
        //-----------------------踏空跳----------------------
        [HarmonyPatch(typeof(HeroMoveManager), "OnStartJump")]
        public class JumpHack
        {
            public static bool Prefix(bool pass, float speed)
            {

                if (airJump)
                {
                    HeroMoveManager.Jump(true, speed);
                    HeroMoveManager.HMMJS.OnJump(true);
                    return false;
                }
                else return true;

            }
        }



    }
}
