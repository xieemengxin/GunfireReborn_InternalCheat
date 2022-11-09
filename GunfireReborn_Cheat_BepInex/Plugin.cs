using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Ludiq;
using System.IO;
using UnhollowerRuntimeLib;
using UnityEngine;


namespace GunfireReborn_Cheat_BepInex
{


    [BepInPlugin("org.cgp.bep.GunfireReborn", "GunfireReborn", "1.0.1.10")]
    public class Plugin : BasePlugin
    {
        public static new BepInEx.Logging.ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource("GunfireReborn");
        public static readonly Harmony harmony = new Harmony("org.cgp.bep.GunfireReborn");
        public static bool debugMode;
       
        //配置项加载
        public static new ConfigFile modConfig = new ConfigFile(Path.Combine(Paths.ConfigPath, "GunfireReborn.cfg"), true);
        //Attribute
        public static ConfigEntry<bool> infinityAmo = modConfig.Bind("Attribute", "无限子弹", Attribute.infinityAmo);
        public static ConfigEntry<bool> damInteval = modConfig.Bind("Attribute", "秒伤", Attribute.damInteval);
        public static ConfigEntry<bool> immediateCharge = modConfig.Bind("Attribute", "秒充能(部分枪)", Attribute.immediateCharge);
        public static ConfigEntry<bool> weaopnAttr = modConfig.Bind("Attribute", "武器属性", Attribute.weaopnAttr);
        public static ConfigEntry<bool> attSpeed = modConfig.Bind("Attribute", "攻速", Attribute.attSpeed);
        public static ConfigEntry<bool> airJump = modConfig.Bind("Attribute", "踏空跳", Attribute.airJump);
        public static ConfigEntry<bool> highJump = modConfig.Bind("Attribute", "高跳", Attribute.highJump);
        public static ConfigEntry<bool> movSpeed = modConfig.Bind("Attribute", "移速", Attribute.movSpeed);
        public static ConfigEntry<bool> speedAsdie = modConfig.Bind("Attribute", "死亡移速", Attribute.speedAsdie);
        public static ConfigEntry<int> attQauntity = modConfig.Bind("Attribute", "子弹数量", Attribute.attQauntity);
        public static ConfigEntry<int> attSpeedval = modConfig.Bind("Attribute", "攻速值(面板)", Attribute.attSpeedval);
        public static ConfigEntry<float> radius = modConfig.Bind("Attribute", "扩散(飞剑数)", Attribute.radius);
        public static ConfigEntry<float> Bulletspeed = modConfig.Bind("Attribute", "子弹飞行速度", Attribute.Bulletspeed);
        public static ConfigEntry<float> movSpeedval = modConfig.Bind("Attribute", "移速倍率", Attribute.movSpeedval);
        public static ConfigEntry<float> highJumpval = modConfig.Bind("Attribute", "高跳倍率", Attribute.highJumpval);
        public static ConfigEntry<bool> singleFov = modConfig.Bind("Attribute", "单倍镜", Attribute.singleFov);

        //Slient
        public static ConfigEntry<bool> magicGun = modConfig.Bind("Slient", "追踪", Slient.magicGun);
        public static ConfigEntry<bool> aimbot = modConfig.Bind("Slient", "自瞄", Slient.aimbot);
        public static ConfigEntry<bool> aimMagnite = modConfig.Bind("Slient", "磁吸", Slient.AimBotMagneticState);
        public static ConfigEntry<bool> magicCaihong = modConfig.Bind("Slient", "彩虹追踪", Slient.magicCaihong);
        public static ConfigEntry<bool> magicLaser = modConfig.Bind("Slient", "手套追踪", Slient.magicLaser);
        public static ConfigEntry<bool> lufei = modConfig.Bind("Slient", "路飞", Slient.lufei);
        public static ConfigEntry<float> magFov = modConfig.Bind("Slient", "追踪范围", Slient.magFov);
        public static ConfigEntry<int> keycode = modConfig.Bind("Slient", "自瞄键代码", Slient.keyCode);

        //Misc
        public static ConfigEntry<bool> bighead = modConfig.Bind("Misc", "大头", Misc.bighead);
        public static ConfigEntry<bool> xiguai = modConfig.Bind("Misc", "吸怪", Misc.xiguai);
        public static ConfigEntry<float> xiguaiDis = modConfig.Bind("Misc", "吸怪距离", Misc.xiguaiDis);
        public static ConfigEntry<int> TelePortType = modConfig.Bind("Misc", "瞬移", Misc.TelePortType);
        public static ConfigEntry<bool> AirWall = modConfig.Bind("Misc", "去除空气墙", Misc.AirWall);
        public static ConfigEntry<bool> Pirece = modConfig.Bind("Misc", "穿透(子弹穿过怪不会消失)", Misc.Pirece);
        public static ConfigEntry<bool> removelHand = modConfig.Bind("Misc", "去掉左手(一般会有盾)", Misc.removeLHand);
        public static ConfigEntry<bool> removerHand = modConfig.Bind("Misc", "去掉右手", Misc.largerWeakPoint);
        public static ConfigEntry<bool> noCollider = modConfig.Bind("Misc", "穿墙", Misc.noCollider);
        public static ConfigEntry<bool> forcedHit = modConfig.Bind("Misc", "强制命中", Misc.forcedHit);
       
        //Visual
        public static ConfigEntry<bool> esp = modConfig.Bind("Visual", "人物透视", Visual.esp);
        //Other
        public static ConfigEntry<bool> isDebug = modConfig.Bind("Dev", "不要动", debugMode);

        //加载设置
        public static void LoadConfig() {
            modConfig.Reload();
            Attribute.infinityAmo = infinityAmo.Value;
            Attribute.damInteval = damInteval.Value;
            Attribute.immediateCharge = immediateCharge.Value;
            Attribute.weaopnAttr = weaopnAttr.Value;
            Attribute.attSpeed = attSpeed.Value;
            Attribute.airJump = airJump.Value;
            Attribute.highJump = highJump.Value;
            Attribute.movSpeed = movSpeed.Value;
            Attribute.speedAsdie = speedAsdie.Value;
            Attribute.attQauntity = attQauntity.Value;
            Attribute.attSpeedval = attSpeedval.Value;
            Attribute.radius = radius.Value;
            Attribute.Bulletspeed = Bulletspeed.Value;
            Attribute.movSpeedval = movSpeedval.Value;
            Attribute.highJumpval = highJumpval.Value;
            Attribute.singleFov = singleFov.Value;

            Slient.magicGun = magicGun.Value;
            Slient.lufei = lufei.Value;
            Slient.magFov = magFov.Value;
            Slient.magicCaihong = magicCaihong.Value;
            Slient.aimbot = aimbot.Value;
            Slient.keyCode = keycode.Value;
            Slient.AimBotMagneticState = aimMagnite.Value;
            Slient.magicLaser = magicLaser.Value;

            Misc.bighead = bighead.Value;
            Misc.xiguai = xiguai.Value;
            Misc.xiguaiDis = xiguaiDis.Value;
            Misc.TelePortType = TelePortType.Value;
            Misc.AirWall = AirWall.Value;
            Misc.Pirece = Pirece.Value;
            Misc.removeLHand = removelHand.Value;
            Misc.largerWeakPoint = removerHand.Value;
            Misc.noCollider = noCollider.Value;
            Misc.forcedHit = forcedHit.Value;
        

            Visual.esp = esp.Value;
            debugMode = isDebug.Value;

            Log.LogWarning("-----更新配置成功-----");

        }

        //保存配置
        public static void SaveConfig() {
            infinityAmo.Value = Attribute.infinityAmo;
            damInteval.Value = Attribute.damInteval;
            immediateCharge.Value = Attribute.immediateCharge;
            weaopnAttr.Value = Attribute.weaopnAttr;
            attSpeed.Value = Attribute.attSpeed;
            airJump.Value = Attribute.airJump;
            highJump.Value = Attribute.highJump;
            movSpeed.Value = Attribute.movSpeed;
            speedAsdie.Value = Attribute.speedAsdie;
            attQauntity.Value = Attribute.attQauntity;
            attSpeedval.Value = Attribute.attSpeedval;
            radius.Value = Attribute.radius;
            Bulletspeed.Value = Attribute.Bulletspeed;
            movSpeedval.Value = Attribute.movSpeedval;
            highJumpval.Value = Attribute.highJumpval;
            singleFov.Value = Attribute.singleFov;

            magicGun.Value = Slient.magicGun;
            lufei.Value = Slient.lufei;
            magFov.Value = Slient.magFov;
            magicCaihong.Value = Slient.magicCaihong;
            keycode.Value = Slient.keyCode;
            aimbot.Value = Slient.aimbot;
            aimMagnite.Value = Slient.AimBotMagneticState;
            magicLaser.Value = Slient.magicLaser;

            bighead.Value = Misc.bighead;
            xiguai.Value = Misc.xiguai;
            xiguaiDis.Value = Misc.xiguaiDis;
            TelePortType.Value = Misc.TelePortType;
            Pirece.Value = Misc.Pirece;
            AirWall.Value = Misc.AirWall;
            noCollider.Value = Misc.noCollider;
            forcedHit.Value = Misc.forcedHit;
      

            removerHand.Value = Misc.largerWeakPoint;
            removelHand.Value = Misc.removeLHand;


            esp.Value = Visual.esp;
            isDebug.Value = debugMode;
            modConfig.Save();

        }
        public override void Load()
        {
            
            Log.LogWarning("------插件已成功注入-----");
            //属性
            ClassInjector.RegisterTypeInIl2Cpp<Attribute>();
            //追踪
            ClassInjector.RegisterTypeInIl2Cpp<Slient>();
            //杂项
            ClassInjector.RegisterTypeInIl2Cpp<Misc>();
            //反反作弊
            ClassInjector.RegisterTypeInIl2Cpp<AntiCheat>();
            //视觉
            ClassInjector.RegisterTypeInIl2Cpp<Visual>();

            //UI
            ClassInjector.RegisterTypeInIl2Cpp<UI>();

            //加载配置
            LoadConfig();

          

            harmony.PatchAll();
        }
        //注入脚本
        [HarmonyPatch(typeof(MainManager), "Start")]
        class Inject
        {
            static void Prefix()
            {
                GameObject AttrbuteObj = new GameObject("Attribute");
                GameObject SilentObj = new GameObject("Silent");
                GameObject MiscObj = new GameObject("Misc");
                GameObject AntiCheatObj = new GameObject("AntiCheat");
                GameObject VisualObj = new GameObject("Visual");
                GameObject UIObj = new GameObject("UI");

                AttrbuteObj.AddComponent<Attribute>();
                SilentObj.AddComponent<Slient>();
                MiscObj.AddComponent<Misc>();
                AntiCheatObj.AddComponent<AntiCheat>();
                VisualObj.AddComponent<Visual>();
                UIObj.AddComponent<UI>();

                Object.DontDestroyOnLoad(AttrbuteObj);
                Object.DontDestroyOnLoad(SilentObj);
                Object.DontDestroyOnLoad(MiscObj);
                Object.DontDestroyOnLoad(AntiCheatObj);
                Object.DontDestroyOnLoad(VisualObj);
                Object.DontDestroyOnLoad(UIObj);

            }
        }
    }
}
