using Cpp2IL.Core.Analysis.Actions.x86.Important;
using Game;
using HarmonyLib;
using HeroCameraName;
using Il2CppSystem.Collections.Generic;
using SkillBolt;
using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using UnhollowerBaseLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GunfireReborn_Cheat_BepInex
{
    /*
     @追踪
     */
    public class Slient : MonoBehaviour
    {
        public static bool magicGun;
        public static bool lufei;
        public static float magFov;
        public static bool magicCaihong;
        public static bool aimbot;
        public static bool magicLaser;
        public static int keyCode;
        public static bool AimBotMagneticState;

 
 


        public struct lufeiInfo
        {
            public NewPlayerObject lufeiObject;
            public Vector3 point;
            public Vector3 position;
            public Vector3 forward;
        }

        public struct magicInfo
        {
            public NewPlayerObject magicObject;
            public Vector3 point;
            public Vector3 position;
            public Vector3 foot;

        }
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        public static magicInfo myMagicInfo = new magicInfo();
        public static lufeiInfo mylufeiInfo = new lufeiInfo();



        public static Vector3 localScale = new Vector3(0.01f, 0.01f, 0.01f);

      

        void Update() {
            //追踪
            List<NewPlayerObject> monsters = NewPlayerManager.MonsterLst;
            if (monsters == null) { return; }
            Vector3 HeroPos = CameraManager.MainCamera.position;
            float CenterRange = 1;  //筛选准心最近

            foreach (var monster in monsters)
            {
                if (monster.playerProp.HP == 1 && (monster.BloodBarCom.BloodBar.isSpecialUndieChallenge || monster.BloodBarCom.BloodBar.isUndieChallenge || monster.BloodBarCom.BloodBar.isUndieStart)) { continue; }    // 无敌怪
                try
                {
                    Transform weakTrans = monster.BodyPartCom.GetWeakTrans(true);
                    if (weakTrans != null)
                    {
                        Vector3 vector = CameraManager.MainCameraCom.WorldToViewportPoint(weakTrans.position);  // 转二维坐标

                        if (vector.z <= 0) { continue; } // 忽略身后目标
                        float CenterDistance = Vector3.Distance(new Vector3(0.5f, 0.5f, 0), new Vector3(vector.x, vector.y, 0));  // 计算目标与准星距离
                        vector = weakTrans.position - HeroPos;
                        float Distance = vector.magnitude;
                        if (CenterDistance > magFov)
                        {
                            continue;
                        }
                        if (!Misc.forcedHit) {//关闭强制命中时
                            //发射射线查看是否有阻挡
                            Ray ray = new Ray(HeroPos, vector);
                            var hits = Physics.RaycastAll(ray, Distance);

                            //查询是否存在阻挡
                            bool query = hits.Any(hit => hit.collider.gameObject.layer == 0 || hit.collider.gameObject.layer == 30 || hit.collider.gameObject.layer == 31 || hit.collider.gameObject.tag == "Monster_Shield");
                            if (query) { continue; }
                        }
                       


                        if (CenterDistance < CenterRange)
                        {
                            CenterRange = CenterDistance;
                            myMagicInfo.magicObject = monster;
                            mylufeiInfo.lufeiObject = monster;
                        }
                    }
                }
                catch
                {
                    continue;
                }

            }


            if (myMagicInfo.magicObject != null && myMagicInfo.magicObject.playerProp.HP != 0f && myMagicInfo.magicObject.IsSafe())
                {
                    Vector3 position2 = myMagicInfo.magicObject.BodyPartCom.GetWeakTrans(true).position;
                    Vector3 forward2 = myMagicInfo.magicObject.BodyPartCom.gameTrans.forward;
                    Vector3 vector = myMagicInfo.magicObject.BodyPartCom.GetWeakTrans(true).localScale;
                    Vector3 point5 = new Vector3(position2.x + 3f * forward2.x, position2.y + vector.y / 4f, position2.z + 3f * forward2.z);
                    myMagicInfo.point = point5;
                    myMagicInfo.position = new Vector3(position2.x, position2.y + vector.y / 4f, position2.z);
                    myMagicInfo.foot = myMagicInfo.magicObject.BodyPartCom.gameTrans.position;
                    mylufeiInfo.point = point5;
                    mylufeiInfo.position = position2;
                    mylufeiInfo.forward = forward2;
                }
   
        
            if (myMagicInfo.magicObject != null && myMagicInfo.magicObject.playerProp.HP != 0f && myMagicInfo.magicObject.IsSafe() && Input.GetKey((KeyCode)keyCode) && aimbot) {
                Transform ResTran = myMagicInfo.magicObject.BodyPartCom.GetWeakTrans();
                if (AimBotMagneticState)
                {

                    Vector3 position = ResTran.position + new Vector3(0, 0.2f);
                    Vector3 screenAim = CameraManager.MainCameraCom.WorldToScreenPoint(position);
                    Vector2 aimTarget = new Vector2(screenAim.x, Screen.height - screenAim.y);
                    if (aimTarget != Vector2.zero)
                    {
                        float x = (aimTarget.x - Screen.width / 2.0f) / 2.5f;
                        float y = (aimTarget.y - Screen.height / 2.0f) / 2.5f;
                        mouse_event(0x0001, (int)x, (int)y, 0, 0);
                    }
                }
                else
                {

                    Vector3 objpos = new Vector3
                    {
                        x = HeroCameraManager.HeroObj.gameTrans.position.x,
                        y = ResTran.position.y + 0.2f,
                        z = HeroCameraManager.HeroObj.gameTrans.position.z
                    };
                    Vector3 forward = ResTran.position - objpos;
                    forward.y += 0.14f;
                    HeroCameraManager.HeroObj.gameTrans.rotation = Quaternion.LookRotation(forward);

                    forward = ResTran.position - HeroPos;
                    forward.y += 0.14f;
                    CameraManager.MainCamera.rotation = Quaternion.LookRotation(forward);
                }
            }
           

        }

        //Patch
        //-----------------------落点型追踪----------------------
        [HarmonyPatch(typeof(CArgBase), "CrtArgSightAccPos")]
        public class Bullte4
        {
  
            public static bool Prefix(CSkillBase skill, CCartoonBase cartoon, ref Vector3 __result)
            {

                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null &&
                    (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的"))
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {


                    __result = myMagicInfo.position;

                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(CArgBase), "CrtArgSightCentrePosByDis")]
        public class CrtArgSightAccPosByDisHack
        {
            public static bool Prefix(CSkillBase skill, float inner, float outside, float dis, ref Vector3 __result)
            {

                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null &&
                    (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的"))
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {


                    __result = myMagicInfo.position;

                    return false;
                }
                return true;
            }
        }
       
        
        //-----------------------直线型追踪----------------------
        [HarmonyPatch(typeof(CArgBase), "GetBulletCurPos")]
        public class MagicStright
        {

            public static bool Prefix(CSkillBase skill, int sid, ref Vector3 __result)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null && (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {


                    __result = myMagicInfo.point;

                    return false;
                }
                return true;
            }
        }
        //-----------------------弓箭追踪----------------------
        [HarmonyPatch(typeof(CArgBase), "CrtArgSightCentrePos")]
        public class MagicBow
        {

            public static bool Prefix(CSkillBase skill, CCartoonBase cartoon, ref Vector3 __result)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null && (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {
                    if (skill.SkillID == 1410)
                    {
                        __result = new Vector3(myMagicInfo.foot.x, myMagicInfo.foot.y, myMagicInfo.foot.z);
                    }
                    else
                    {
                        Vector3 position = myMagicInfo.magicObject.BodyPartCom.GetWeakTrans(true).position;
                        Vector3 localScale = myMagicInfo.magicObject.BodyPartCom.GetWeakTrans(true).localScale;
                        __result = new Vector3(position.x, position.y + localScale.y / 4f, position.z);
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(CArgBase), "CrtArgCameraCenterPos")]
        public class BullteCaiHong
        {

            public static bool Prefix(CSkillBase skill, CCartoonBase cartoon, ref Vector3 __result)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null && (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {


                    __result = myMagicInfo.point;

                    return false;
                }
                return true;
            }
        }
        //-----------------------LightningChain----------------------
        [HarmonyPatch(typeof(CArgBase), "GetCartoonStart")]
        public class CartoonStart
        {

            public static bool Prefix(CSkillBase skill, int sid, ref Vector3 __result)
            {

                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null &&
                    (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的"))
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge
                    && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {


                    __result = myMagicInfo.position;

                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(CArgBase), "GetCartoonEnd")]
        public class magic_Sowrd
        {

            public static bool Prefix(CSkillBase skill, int sid, ref Vector3 __result)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && lufei && mylufeiInfo.lufeiObject != null && (mylufeiInfo.lufeiObject.playerProp.HP > 1f || !(mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isUndieChallenge && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isUndieStart)
                {
                    __result = myMagicInfo.position;
                    return false;
                }
                return true;
            }
        }
        //飞镖追踪
        [HarmonyPatch(typeof(CServerArg), "GetCurBulletFinalPos")]
        public class magic_handSword1
        {

            public static bool Prefix(CSkillBase skill, int sid, ref Vector3 __result)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicGun && myMagicInfo.magicObject != null && (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {

                    __result = myMagicInfo.position;

                    return false;
                }
                return true;

            }
        }

        //-----------------------路飞----------------------
        [HarmonyPatch(typeof(CArgBase), "CrtArgMuzzlePos")]
        public class flufei
        {

            public static bool Prefix(CSkillBase skill, CCartoonBase cartoon, ref Vector3 __result)
            {

                if (NewPlayerManager.MonsterLst.Count > 0 && lufei && mylufeiInfo.lufeiObject != null && (mylufeiInfo.lufeiObject.playerProp.HP > 1f || !(mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isUndieChallenge && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isUndieStart)
                {
                    if (HeroCameraManager.HeroObj.BulletPreFormCom.playerProp.CurWeapon.Contains(1509))
                    {
                        __result = new Vector3
                        {
                            x = mylufeiInfo.position.x,
                            y = mylufeiInfo.position.y + mylufeiInfo.forward.y + 2f,
                            z = mylufeiInfo.position.z
                        };
                    }
                    else
                    {
                        __result = mylufeiInfo.point;
                    }
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(CArgBase), "GetCustomPos")]
        public class flufei_Sowrd
        {
            public static bool Prefix(CSkillBase skill, ref Vector3 __result)
            {

                if (NewPlayerManager.MonsterLst.Count > 0 && lufei && mylufeiInfo.lufeiObject != null && (mylufeiInfo.lufeiObject.playerProp.HP > 1f || !(mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isUndieChallenge && !mylufeiInfo.lufeiObject.BloodBarCom.BloodBar.isUndieStart)
                {

                    __result = mylufeiInfo.point;


                    return false;
                }

                return true;
            }
        }

       
        //手套追踪
        [HarmonyPatch(typeof(CUnityUtility), "GetRayByScreenCenterPos")]
        class LaserTrace
        {
            static bool Prefix(Camera camera, ref Ray __result)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicLaser && myMagicInfo.magicObject != null &&
                                   (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的"))
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {

                    Vector3 heroPos = CameraManager.MainCamera.position;
                    __result = new Ray(heroPos, myMagicInfo.position - heroPos);
                    return false;

                }
                return true;
            }
        }
        //彩虹追踪
        [HarmonyPatch(typeof(TraceLineCartoon), "GetEndPos")]
        class LineTracer
        {
            static void Postfix(ref Vector3 __result,ref TraceLineCartoon __instance)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicCaihong && myMagicInfo.magicObject != null &&
                                   (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的"))
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {
                    
                    __result = myMagicInfo.position;
                  
                }
            
            }
        }
        [HarmonyPatch(typeof(WarGetTargetManager), "GetRangeMonster")]
        class LineTracer1
        {
            static bool Prefix(ref List<int> __result, ref TraceLineCartoon __instance, float searchAngle, float searchDis, bool lockHideDoor, Vector3 startPos = default(Vector3), Vector3 forward = default(Vector3), bool lockSumm = false, bool isBlocked = false, bool lockPetrochemical = true, bool isCheckMonsterCenter = false, bool isCheckHeight = false, float heightThreshold = 0f)
            {
                if (NewPlayerManager.MonsterLst.Count > 0 && magicCaihong && myMagicInfo.magicObject != null &&
                                   (myMagicInfo.magicObject.playerProp.HP > 1f || !(myMagicInfo.magicObject.BloodBarCom.BloodBar.HolaName == "不朽的"))
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isSpecialUndieChallenge
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieChallenge
                                   && !myMagicInfo.magicObject.BloodBarCom.BloodBar.isUndieStart)
                {
                    //获取目标 修改为瞄准目标
                    List<int> attList = new List<int>();
                    attList.Add(myMagicInfo.magicObject.ObjectID);
                    __result = attList;
                    return false;
                }
                return true;
            }
        }



    }
}
