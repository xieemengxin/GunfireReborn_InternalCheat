

using BepInEx.IL2CPP.Utils.Collections;
using Game;

using HarmonyLib;
using HeroCameraName;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.Linq;
using KinematicCharacterController;
using Ludiq;
using ProBuilder2.Common;


using System.Linq;
using UnhollowerBaseLib;
using UnityEngine;
using static GunfireReborn_Cheat_BepInex.Slient;

using Object = Il2CppSystem.Object;

namespace GunfireReborn_Cheat_BepInex
{
    /*
     @杂项
     */
    public class Misc : MonoBehaviour
    {
        public static bool bighead;
        public static bool xiguai;
        public static int TelePortType;
        public static bool Pirece;
        public static bool AirWall;
        public static float xiguaiDis;//吸怪的距离
        public static bool removeLHand;
        public static bool largerWeakPoint;
        public static bool noCollider;//穿墙
        public static bool forcedHit;//强制命中

        private static LayerMask monster = 1 << LayerMask.NameToLayer("Monster") | 1 << Layer.CanDestroy | 1 << Layer.PenetrateNPC | 1 << Layer.Powerball;



        //测试

        public static float GetRandomNumber(float minimum, float maximum)
        {
            return UnityEngine.Random.value * (maximum - minimum) + minimum;
        }

        void Update() {
            //自动拾取
            foreach (KeyValuePair<int, NewPlayerObject> keyValuePair in NewPlayerManager.PlayerDict)
            {
                if ((keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_TRIGGER && keyValuePair.Value.Shape==5513) || keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_CASH ||
                    keyValuePair.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_BULLET )
                {
                    keyValuePair.Value.DropOPCom.AutoPickRange = 9999f;
                }

            }

            //Shift瞬移
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                if (TelePortType % 3 == 1)
                {
                    float height = CameraManager.MainCamera.position.y - HeroCameraManager.HeroTran.position.y;
                    Ray ray = new Ray(CameraManager.MainCamera.position, HeroMoveManager.HMMJS.GetDirection());
                    Vector3 point = ray.GetPoint(7f);
                    HeroCameraManager.HeroTran.position = new Vector3(point.x, point.y - height, point.z);

                }
                if (TelePortType % 3 == 2)
                {
                    float height = CameraManager.MainCamera.position.y - HeroCameraManager.HeroTran.position.y;
                    Ray ray2 = new Ray(CameraManager.MainCamera.position, CameraManager.MainCamera.forward);
                    Vector3 point2 = ray2.GetPoint(7f);
                    HeroCameraManager.HeroTran.position = new Vector3(point2.x, point2.y - height, point2.z);
                }
            }
            //吸怪
            if (xiguai)
            {
                Ray ray3 = new Ray(CameraManager.MainCamera.position, CameraManager.MainCamera.forward);
                Vector3 point3 = ray3.GetPoint(xiguaiDis);
                foreach (NewPlayerObject newPlayerObject in NewPlayerManager.MonsterLst)
                {

                    if ((newPlayerObject.playerProp.HP > 1f || !(newPlayerObject.BloodBarCom.BloodBar.HolaName == "不朽的")) && !newPlayerObject.BloodBarCom.BloodBar.isSpecialUndieChallenge && !newPlayerObject.BloodBarCom.BloodBar.isUndieChallenge && !newPlayerObject.BloodBarCom.BloodBar.isUndieStart)
                    {
                        newPlayerObject.gameTrans.position = new Vector3(point3.x, HeroCameraManager.HeroTran.position.y, point3.z);

                    }
                }
            }
           
            //瞬移至队友
            if (Input.GetKeyUp(KeyCode.BackQuote))
            {
                List<NewPlayerObject> list3 = new List<NewPlayerObject>();
                foreach (int pid in NewPlayerManager.TeamPlayerLst)
                {
                    list3.Add(NewPlayerManager.GetPlayer(pid));
                }
                foreach (NewPlayerObject pid in NewPlayerManager.NpcLst)
                {
                    list3.Add(pid);
                }
                
                float num = -1f;
                NewPlayerObject newPlayerObject2 = null;
                Vector3 position = CameraManager.MainCamera.position;
                Vector2 b = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                foreach (NewPlayerObject newPlayerObject3 in list3)
                {
                    if (CameraManager.MainCameraCom.WorldToViewportPoint(newPlayerObject3.gameTrans.position).z > 0f)
                    {
                        Vector2 a = new Vector2
                        {
                            x = CameraManager.MainCameraCom.WorldToScreenPoint(newPlayerObject3.gameTrans.position).x,
                            y = CameraManager.MainCameraCom.WorldToScreenPoint(newPlayerObject3.gameTrans.position).y
                        };
                        if (newPlayerObject2 == null)
                        {
                            newPlayerObject2 = newPlayerObject3;
                        }
                        else if (num == -1f)
                        {
                            num = Vector2.Distance(a, b);
                            Vector2 a2 = new Vector2
                            {
                                x = CameraManager.MainCameraCom.WorldToScreenPoint(newPlayerObject2.gameTrans.position).x,
                                y = CameraManager.MainCameraCom.WorldToScreenPoint(newPlayerObject2.gameTrans.position).y
                            };
                            if (num < Vector2.Distance(a2, b))
                            {
                                newPlayerObject2 = newPlayerObject3;
                            }
                        }
                        else if (Vector2.Distance(a, b) < num)
                        {
                            newPlayerObject2 = newPlayerObject3;
                        }
                    }
                }

                Transform gameTrans = newPlayerObject2.gameTrans;
                if (gameTrans != null)
                {
                    Vector3 b2 = new Vector3
                    {
                        x = HeroCameraManager.HeroObj.gameTrans.position.x,
                        y = gameTrans.position.y + 0.2f,
                        z = HeroCameraManager.HeroObj.gameTrans.position.z
                    };
                    Vector3 forward = gameTrans.position - b2;
                    forward.y += 0.00f;
                    HeroCameraManager.HeroObj.gameTrans.rotation = Quaternion.LookRotation(forward);
                    forward = gameTrans.position - position;
                    forward.y += 0.00f;
                    CameraManager.MainCamera.rotation = Quaternion.LookRotation(forward);
                    HeroMoveManager.HeroTran.position = new Vector3(newPlayerObject2.gameTrans.position.x, newPlayerObject2.gameTrans.position.y, newPlayerObject2.gameTrans.position.z);
                }
            }
            //吸取周围物品
            if (Input.GetKeyUp(KeyCode.Alpha9))
            {
                foreach (KeyValuePair<int, NewPlayerObject> keyValuePair4 in NewPlayerManager.PlayerDict)
                {
                    if (keyValuePair4.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_EQUIP || keyValuePair4.Value.FightType == ServerDefine.FightType.NWARRIOR_DROP_RELIC || keyValuePair4.Value.FightType == ServerDefine.FightType.NWARRIOR_NPC_GOLDENCUP)
                    {
                        Ray ray4 = new Ray(CameraManager.MainCamera.position, CameraManager.MainCamera.forward);
                        Vector3 point4 = ray4.GetPoint(3f);
                        keyValuePair4.Value.gameTrans.position = new Vector3(point4.x + GetRandomNumber(-2f, 2f), HeroCameraManager.HeroTran.position.y+3f, point4.z + GetRandomNumber(-2f, 2f));
                    }
                }
            }
        

           
           

        }

        void FixedUpdate() {
            
            //去除空气墙
            if (AirWall)
            {
                var airWall = GameObject.Find("AirWall");
                Destroy(airWall);
            }
            //去除滚石
            var rollingStone2 = GameObject.Find("1621");
            var rollingStone1 = GameObject.Find("1610_1");
            Destroy(rollingStone2);
            Destroy(rollingStone1);
        }
        //-----------------------------穿透--------------------
        
        //无后座
        [HarmonyPatch(typeof(CameraCtrl), "Recoil")]
        class NoRecoil
        {
            public static bool Prefix() {
                return false; 
            }
        }

        // ColliderBody
        [HarmonyPatch(typeof(KinematicCharacterMotor), "CheckIfColliderValidForCollisions")]
        class ColliderBody
        {
           public static bool Prefix(Collider coll,ref bool __result) {
               

                if (noCollider && Input.GetKey(KeyCode.LeftControl) &&
                   !coll.name.Contains(GameSceneManager.curSceneID.ToString()) &&
                   !coll.name.Contains("ground") &&
                   !coll.name.Contains("Terrain") &&
                   !coll.name.Contains("boxcolloder") &&
                   !coll.name.Contains("stairs") &&
                   !coll.name.Contains("shelf") &&
                   !coll.name.Contains("floor") &&
                   !coll.name.Equals("terrain") &&
                   !coll.tag.Equals("desert"))
                {
                    __result = false;
                }
                else if (coll.tag.Equals("Controller")) {
                    __result = false;
                }
                else
                {
                    __result = true;
                }

                return false;
            }
        }

        [HarmonyPatch(typeof(PhysicsUtility), "GetRaycastHitList")]
        class physicHack1
        {
            static RaycastHit[] raycastHits = new RaycastHit[50];
            static Il2CppStructArray<RaycastHit> m_Results = new Il2CppStructArray<RaycastHit>(raycastHits);
            static List<RaycastHit> myList = new List<RaycastHit>();
            public static bool Prefix(Vector3 origin, Vector3 direction, float maxDis, int layerMask, ref List<RaycastHit> __result)
            {
                try
                {
                    if (forcedHit)
                    {
                        myList.Clear();
                        int hits = Physics.SphereCastNonAlloc(origin, magFov, direction, m_Results, 999, monster.value);
                        for (int i = 0; i < hits; i++)
                        {

                            myList.Add(m_Results[i]);
                        }
                        __result = myList;
                        return false;
                    }
                    return true;
                }
                catch {
                    return true;
                }

               
            }

        }
        [HarmonyPatch(typeof(PhysicsUtility), "GetSpherecastAndRaycastHitList")]
        class physicHack2
        {
            static RaycastHit[] raycastHits = new RaycastHit[50];
            static Il2CppStructArray<RaycastHit> m_Results = new Il2CppStructArray<RaycastHit>(raycastHits);
            static List<RaycastHit> myList = new List<RaycastHit>();
            public static bool Prefix(Vector3 origin, float radius, Vector3 direction, float maxDis, int layerMask0, int layerMask1, ref List<RaycastHit> __result)
            {
                try {
                    if (forcedHit)
                    {
                        myList.Clear();
                        int hits = Physics.SphereCastNonAlloc(origin, magFov, direction, m_Results, 999, monster.value);
                        for (int i = 0; i < hits; i++)
                        {

                            myList.Add(m_Results[i]);
                        }
                        __result = myList;
                        return false;

                    }
                    return true;
                }
                catch {
                    return true;
                }
             
                
            }
        }
        [HarmonyPatch(typeof(PhysicsUtility), "GetRaycastHitListByRay")]
        class physicHack3
        {
            static RaycastHit[] raycastHits = new RaycastHit[50];
            static  Il2CppStructArray<RaycastHit> m_Results = new Il2CppStructArray<RaycastHit>(raycastHits);
            static List<RaycastHit> myList = new List<RaycastHit>();
            public static bool Prefix(Ray ray, float maxDis, int layerMask, ref List<RaycastHit> __result)
            {
                try {
                    if (forcedHit)
                    {
                        myList.Clear();
                        int hits = Physics.SphereCastNonAlloc(ray, magFov, m_Results, 999, monster.value);
                        for (int i = 0; i < hits; i++)
                        {

                            myList.Add(m_Results[i]);
                        }
                        __result = myList;
                        return false;
                    }
                    return true;
                }
                catch {
                    return true;
                }
               
            }

        }


        //[HarmonyPatch(typeof(PhysicsUtility), "GetRaycastHitList")]
        //class physicHack1
        //{
        //    static RaycastHit[] raycastHits;
        //    public static void Postfix(Vector3 origin, Vector3 direction, float maxDis, int layerMask, List<RaycastHit> __result)
        //    {


        //        if (forcedHit)
        //        {
        //            raycastHits = Physics.SphereCastAll(origin, magFov, direction, 9999, monster.value);
        //            __result.Clear();
        //            foreach (RaycastHit hit in raycastHits)
        //            {

        //                __result.Add(hit);

        //            }
        //        }

        //    }

        //}
        //[HarmonyPatch(typeof(PhysicsUtility), "GetSpherecastAndRaycastHitList")]
        //class physicHack2
        //{
        //    static RaycastHit[] raycastHits = new RaycastHit[15];
        //    Il2CppStructArray<RaycastHit> m_Results = new Il2CppStructArray<RaycastHit>(raycastHits);
        //    public static void Postfix(Vector3 origin, float radius, Vector3 direction, float maxDis, int layerMask0, int layerMask1, List<RaycastHit> __result)
        //    {

        //        if (forcedHit)
        //        {

        //            raycastHits = Physics.SphereCastAll(origin, magFov, direction, 999, monster.value);
        //            __result.Clear();
        //            foreach (RaycastHit hit in raycastHits)
        //            {

        //                __result.Add(hit);

        //            }
        //        }
        //        Il2CppStructArray<RaycastHit> m_Results = new Il2CppStructArray<RaycastHit>(raycastHits);
        //        Plugin.Log.LogWarning("[+]GetSpherecastAndRaycastHitList");
        //        Plugin.Log.LogWarning("     [-]origin:" + origin);
        //        int hits = Physics.SphereCastNonAlloc(origin, 2, direction, m_Results, Mathf.Infinity, monster.value);
        //        for (int i = 0; i < hits; i++)
        //        {
        //            Plugin.Log.LogWarning("fuck");
        //            Plugin.Log.LogWarning("Hit " + m_Results[i].collider.gameObject.name);
        //        }

        //    }

        //}
        //[HarmonyPatch(typeof(PhysicsUtility), "GetRaycastHitListByRay")]
        //class physicHack3
        //{
        //    static RaycastHit[] raycastHits;
        //    public static void Postfix(Ray ray, float maxDis, int layerMask, List<RaycastHit> __result)
        //    {
        //        if (forcedHit)
        //        {

        //            raycastHits = Physics.SphereCastAll(ray, magFov, 9999, monster.value);
        //            __result.Clear();
        //            foreach (RaycastHit hit in raycastHits)
        //            {
        //                __result.Add(hit);
        //            }
        //        }


        //    }

        //}

        [HarmonyPatch(typeof(s2cnetaddobject), "CreatePlayerObject")]
        class transConnected {
            public static void Postfix(int pid, ServerDefine.FightType gt, Dictionary<string, Object> dPropInfo, NewPlayerObject __result) {
                if (NewPlayerManager.IsMonster(__result.ObjectID)) {
                    if (largerWeakPoint)
                    {
                    

                        UnhollowerBaseLib.Il2CppArrayBase<CapsuleCollider> monsterCapsuleColliders = __result.gameTrans.GetComponentsInChildren<CapsuleCollider>();//weakTran
                         UnhollowerBaseLib.Il2CppArrayBase<BoxCollider> weaKCapsuleColliders = __result.gameTrans.GetComponentsInChildren<BoxCollider>();//weakTran
                         UnhollowerBaseLib.Il2CppArrayBase<SphereCollider> weaKSphereColliders = __result.gameTrans.GetComponentsInChildren<SphereCollider>();//weakTran

                        foreach (CapsuleCollider capsuleCollider in monsterCapsuleColliders)
                         {
                                 capsuleCollider.tag = "Monster_Weakness";
                                 capsuleCollider.height = 3;
                                 capsuleCollider.radius = 3;
                            
                                // capsuleCollider.center = ;

                         }
                         foreach (BoxCollider boxcollider in weaKCapsuleColliders)
                         {

                            //放大弱点
                               boxcollider.tag = "Monster_Weakness";
                               boxcollider.size = new Vector3(3, 3, 3);
                          
                               //boxcollider.center = ;
                         }

                         foreach (SphereCollider sphereCollider in weaKSphereColliders)
                         {
                            sphereCollider.tag = "Monster_Weakness";
                                 sphereCollider.radius = 3;
                           
                            //sphereCollider.center = ;
                        }
                        /*Plugin.Log.LogWarning("[+]weakTranId:" + __result.ObjectID);
                        Plugin.Log.LogWarning("    [-]weakTranname:" + __result.BodyPartCom.GetWeakTrans(true).name);
                        Plugin.Log.LogWarning("    [-]weakTrantag:" + __result.BodyPartCom.GetWeakTrans(true).tag);*/
                    }

                    if (removeLHand)
                    { //穿盾
                        UnhollowerBaseLib.Il2CppArrayBase<BoxCollider> monsterShieldColliders = __result.gameTrans.GetComponentsInChildren<BoxCollider>();//shield
                        foreach (BoxCollider boxcollider in monsterShieldColliders)
                        {
                            //去盾
                            if (boxcollider.tag.Contains("shield") || boxcollider.tag.Contains("Shield"))
                            {
                                boxcollider.enabled = false;
                            }
                        }
                    }

                    //大头
                    if (bighead)
                    {

                        if (__result.SID == 20011)
                        {
                            Transform spine1 = __result.gameTrans.FindChild("All_Root/Bip001/Bip001 Pelvis/Bip001 Spine");
                            spine1.transform.localScale = new Vector3(3f, 3f, 3f);
                            Transform spine2 = __result.BodyPartCom.gameTrans.FindChild("All_Root/Bip001/Bip001 Pelvis/Bip001 Spine");
                            spine2.transform.localScale = new Vector3(3f, 3f, 3f);
                        }
                        else
                        {
                            Transform neck1 = __result.gameTrans.FindChild("All_Root/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck");
                            neck1.transform.localScale = new Vector3(3f, 3f, 3f);
                            Transform neck2 = __result.BodyPartCom.gameTrans.FindChild("All_Root/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 Neck");
                            neck2.transform.localScale = new Vector3(3f, 3f, 3f);
                            //monster.BodyPartCom.GetWeakTrans(true).localScale = new Vector3(3f, 3f, 3f);
                        }


                    }
                }
            }
        }
    }

}
