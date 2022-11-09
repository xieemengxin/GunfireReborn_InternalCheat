using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GunfireReborn_Cheat_BepInex
{
    public class UI : MonoBehaviour
    {
        private static Rect windowRect = new Rect(Screen.width-460, 10, 400, 600);

        public static int toolBarIndex = 0;
        public static bool showWindow = true;
        public static string whoReportMe = "";

        void OnGUI() {
            if (showWindow) {
                windowRect = GUI.Window(1, windowRect, (GUI.WindowFunction)windowUpdate, "[HOME]隐藏");
            }
        }
        void Update() {
            if (Input.GetKeyUp(KeyCode.Home)) {
                showWindow = !showWindow;
            }
        }
     
        void windowUpdate(int windowId) {
          
            GUILayout.Label("本修改器免费使用，Q群：854546689，谨防上当受骗");
            GUILayout.Label("上排9吸物品，~键瞬移到队友或npc");
            GUILayout.Label("--------↓↓↓↓↓---有人举报你↓↓↓↓↓------------");
            GUILayout.Label(whoReportMe);
            GUILayout.Label("--------↑↑↑↑↑---有人举报你↑↑↑↑↑------------");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("属性")) {
                toolBarIndex = 0;
            }
            if (GUILayout.Button("追踪"))
            {
                toolBarIndex = 1;
            }
            if (GUILayout.Button("杂项"))
            {
                toolBarIndex = 2;
            }
           
            GUILayout.EndHorizontal();
            if (toolBarIndex == 0)
            { //属性
                GUILayout.BeginHorizontal();
                Attribute.infinityAmo = GUILayout.Toggle(Attribute.infinityAmo, Attribute.infinityAmo ? "无限子弹 <color=lime>[开]</color>" : "无限子弹 [关]");
                Attribute.damInteval =  GUILayout.Toggle(Attribute.damInteval, Attribute.damInteval ? "秒伤 <color=lime>[开]</color>" : "秒伤 [关]");
                Attribute.immediateCharge = GUILayout.Toggle(Attribute.immediateCharge, Attribute.immediateCharge ? "秒充能 <color=lime>[开]</color>" : "秒充能 [关]");
                Attribute.weaopnAttr = GUILayout.Toggle(Attribute.weaopnAttr, Attribute.weaopnAttr ? "武器属性 <color=lime>[开]</color>" : "武器属性 [关]");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("扩散（飞剑数）：");
                Attribute.radius = float.Parse(GUILayout.TextField(Attribute.radius.ToString()));
                GUILayout.Label("子弹飞行速度:");
                Attribute.Bulletspeed = float.Parse(GUILayout.TextField(Attribute.Bulletspeed.ToString()));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("子弹数量:");
                Attribute.attQauntity = int.Parse(GUILayout.TextField(Attribute.attQauntity.ToString()));
                Attribute.attSpeed = GUILayout.Toggle(Attribute.attSpeed, "攻速值:");
                Attribute.attSpeedval = int.Parse(GUILayout.TextField(Attribute.attSpeedval.ToString()));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Attribute.movSpeed = GUILayout.Toggle(Attribute.movSpeed, Attribute.movSpeed ? "移速 <color=lime>[开]</color> 倍率:" : "移速 [关]");
                Attribute.movSpeedval = float.Parse(GUILayout.TextField(Attribute.movSpeedval.ToString()));
                Attribute.highJump = GUILayout.Toggle(Attribute.highJump, Attribute.highJump ? "高跳 <color=lime>[开]</color> 倍率:" : "高跳 [关]");
                Attribute.highJumpval = float.Parse(GUILayout.TextField(Attribute.highJumpval.ToString()));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Attribute.speedAsdie = GUILayout.Toggle(Attribute.speedAsdie, Attribute.speedAsdie ? "倒地移速 <color=lime>[开]</color> " : "倒地移速 [关]");
                Attribute.airJump = GUILayout.Toggle(Attribute.airJump, Attribute.airJump ? "踏空跳 <color=lime>[开]</color>" : "踏空跳[关]");
                Attribute.singleFov = GUILayout.Toggle(Attribute.singleFov, Attribute.singleFov ? "单倍镜 <color=lime>[开]</color>" : "单倍镜[关]");
                GUILayout.EndHorizontal();
            }


            if (toolBarIndex == 1)
            { //追踪
                GUILayout.BeginHorizontal();
                Slient.magicGun = GUILayout.Toggle(Slient.magicGun, Slient.magicGun ? "追踪 <color=lime>[开]</color>" : "追踪 [关]");
                Slient.lufei = GUILayout.Toggle(Slient.lufei, Slient.lufei ? "路飞 <color=lime>[开]</color>" : "路飞 [关]");
                Slient.magicCaihong = GUILayout.Toggle(Slient.magicCaihong, Slient.magicCaihong ? "彩虹追踪 <color=lime>[开]</color>" : "彩虹追踪 [关]");
                Slient.aimbot = GUILayout.Toggle(Slient.aimbot, Slient.aimbot ? "自瞄 <color=lime>[开]</color>" : "自瞄 [关]");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Slient.magicLaser = GUILayout.Toggle(Slient.magicLaser, Slient.magicLaser ? "手套追踪 <color=lime>[开]</color>" : "手套追踪 [关]");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label( "fov(适用于强制命中)：");
                Slient.magFov = float.Parse(GUILayout.TextField(Slient.magFov.ToString()));
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                Slient.AimBotMagneticState = GUILayout.Toggle(Slient.AimBotMagneticState, Slient.AimBotMagneticState ? " <color=lime>磁性自瞄</color>" : "<color=lime>暴力自瞄</color>");
                GUILayout.Label( "自瞄键代码(整数)：");
                Slient.keyCode = int.Parse(GUILayout.TextField(Slient.keyCode.ToString()));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Visual.esp= GUILayout.Toggle(Visual.esp, Visual.esp ? "人物透视 <color=lime>[开]</color>" : "人物透视 [关]");
                GUILayout.EndHorizontal();
            }

                if (toolBarIndex == 2)
                { //杂项
                GUILayout.BeginHorizontal();
                Misc.bighead = GUILayout.Toggle(Misc.bighead, Misc.bighead ? "大头 <color=lime>[开]</color>" : "大头 [关]");
                Misc.xiguai = GUILayout.Toggle(Misc.xiguai, Misc.xiguai ? "吸怪 <color=lime>[开]</color> 距离" : "吸怪 [关]");
                Misc.xiguaiDis = float.Parse(GUILayout.TextField(Misc.xiguaiDis.ToString()));
                Misc.AirWall = GUILayout.Toggle( Misc.AirWall, Misc.AirWall ? "去除空气墙 <color=lime>[开]</color>" : "去除空气墙 [关]");
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Misc.removeLHand = GUILayout.Toggle(Misc.removeLHand, Misc.removeLHand ? "穿盾 <color=lime>[开]</color>" : "穿盾 [关]");
                Misc.largerWeakPoint = GUILayout.Toggle(Misc.largerWeakPoint, Misc.largerWeakPoint ? "弱点放大(百爆) <color=lime>[开]</color>" : "弱点放大(百爆) [关]");
             
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Misc.noCollider = GUILayout.Toggle(Misc.noCollider, Misc.noCollider ? "人物穿墙(按住LCtrl) <color=lime>[开]</color>" : "人物穿墙 [关]");
                Misc.forcedHit = GUILayout.Toggle(Misc.forcedHit, Misc.forcedHit ? "(强制命中)穿墙 <color=lime>[开]</color>" : "(强制命中)穿墙");
                
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                Misc.Pirece= GUILayout.Toggle( Misc.Pirece, Misc.Pirece ? "穿透 <color=lime>[开]</color>" : "穿透 [关]");
                GUILayout.Label( "瞬移(0,1,2)：");
                Misc.TelePortType = int.Parse(GUILayout.TextField( Misc.TelePortType.ToString()));
                GUILayout.EndHorizontal();
             }
                if (GUILayout.Button( "保存配置"))
                {
                    Plugin.SaveConfig();
                }
               Plugin.debugMode = GUILayout.Toggle(Plugin.debugMode, Plugin.debugMode ? "debugMode <color=lime>[开]</color>" :"debugMode 不要开[关]");
            
        }
    }
}
