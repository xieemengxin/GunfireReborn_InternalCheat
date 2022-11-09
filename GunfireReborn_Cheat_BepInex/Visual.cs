using DataHelper;
using Il2CppSystem.Collections.Generic;
using System;
using System.Text;
using UnityEngine;
using static Cinemachine.CinemachineBlendDefinition;

namespace GunfireReborn_Cheat_BepInex
{
    /*
     @视觉
     */
    public class Visual : MonoBehaviour
    {
        public static bool esp;

        public bool ShowObject(NewPlayerObject obj)
        {
            return obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_EQUIP || obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_RELIC || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SMITH || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SHOP || (obj.FightType == ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL && (obj.Shape == 4406 || obj.Shape == 4419 || obj.Shape == 4427)) || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_EVENT || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX || (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_TRANSFER && (obj.Shape == 4016 || obj.Shape == 4009 || obj.Shape == 4019)) || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_BENEDICTION || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GOLDENCUP || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ROOMCHALLENGE || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_TRANSFER || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_LOCKEDBOX || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_PASSBOX || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_INITBOX || obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_MAGICBOX;
        }

        public string FightTypeToString(NewPlayerObject obj)
        {
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_EQUIP)
            {
                return DataMgr.GetWeaponData(obj.Shape).Name + " " + obj.DropOPCom.WeaponInfo.SIProp.Grade.ToString();
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_DROP_RELIC)
            {
                return DataMgr.GetRelicData(obj.DropOPCom.RelicSid).Name;
            }
            if (obj.FightType == ServerDefine.FightType.WARRIOR_OBSTACLE_NORMAL)
            {
                return "未开秘境";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_TRANSFER)
            {
                return "秘境或者传送门";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ITEMBOX)
            {
                return "奖励宝箱";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_EVENT)
            {
                return "橙色宝箱";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SMITH)
            {
                return "工匠";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_SHOP)
            {
                return "商人";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_BENEDICTION)
            {
                return "灵猫庇佑";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GOLDENCUP)
            {
                return "大圣杯";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_LOCKEDBOX)
            {
                return "上锁的箱子";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_PASSBOX)
            {
                return "传送门箱子";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_INITBOX)
            {
                return "初始箱子";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_GSCASHSHOP)
            {
                return "奇遇商人";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_MAGICBOX)
            {
                return "技能箱子";
            }
            if (obj.FightType == ServerDefine.FightType.NWARRIOR_NPC_ROOMCHALLENGE)
            {
                return "房间挑战";
            }
            return "";
        }

        void Update() {
            if (esp) {
                WeaponInscriptionMgr.Instance.RealShowTar();
            }
          
          
        }

 
        void OnGUI()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
            {
                GUIStyle Style = new GUIStyle();
                Style.normal.textColor = Color.yellow;
                foreach (var keyValuePair in NewPlayerManager.PlayerDict)
                {
                    if (!ShowObject(keyValuePair.value)) { continue; }
                    var screenPos = CameraManager.MainCameraCom.WorldToScreenPoint(keyValuePair.value.centerPointTrans.transform.position);
                    if (screenPos.z > 0)
                    {
                        var dist = Vector3.Distance(HeroMoveManager.HeroObj.centerPointTrans.position, keyValuePair.value.centerPointTrans.position);
                        float Clamp = Mathf.Clamp(dist - 10, 10, 30);
                        Style.fontSize = 40 - (int)Clamp;

                        Clamp = (int)Mathf.Clamp(dist + 150, 150, 240);
                        Style.normal.textColor = new Color32(255, 128, 64, (byte)Clamp);

                        GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 150, 100), FightTypeToString(keyValuePair.value) + "(" + dist.ToString("0") + "m)", Style);
                    }
                }
            }
            //测试  开启
            if (Plugin.debugMode) {
                GUIStyle Style = new GUIStyle();
                Style.normal.textColor = Color.yellow;
                List<NewPlayerObject> monsters = NewPlayerManager.MonsterLst;

                foreach (var monster in monsters) {
                    var screenPos = CameraManager.MainCameraCom.WorldToScreenPoint(monster.centerPointTrans.transform.position);
                    if (screenPos.z > 0)
                    {
                        String show= "name:" + monster.gameTrans.gameObject.name + "\n"
                                     +"SID:" + monster.SID + "\n"
                                     + "MonsterTag:"+monster.gameTrans.tag+"\n"
                                     +"BodyPartComTag:"+monster.BodyPartCom.gameTrans.tag+"\n"
                                     + "monster.layer" + monster.gameTrans.gameObject.layer;
                        GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 300, 500), show);
                       // Plugin.Log.LogWarning("[+]All_Root:" + monster.gameTrans.FindChild("All_Root/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand").name);
                        //Plugin.Log.LogWarning(monster.gameTrans.FindChild("All_Root/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Spine1/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand").childCount);
                    
                    }
                }
            }
        }
    }
}
