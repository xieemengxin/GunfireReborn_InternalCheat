using DYPublic;
using DYPublic.Duonet;
using DYPublic.SafeModule;
using Game;
using GameWorkDllNS;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GunfireReborn_Cheat_BepInex
{
    /*
     @反反作弊
     */
    public class AntiCheat : MonoBehaviour
    {

        void Start()
        {
            Plugin.Log.LogWarning("-----反小黑屋加载成功,作弊记录已清除------");
            PCSafeModule.StopGuard();
            PCSafeWrap.StopGuard();
            SecurityManager.Instance.ClearImmortal();
            SecurityManager.Instance.ClearReportSource();
        }

        void Update() { 


        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("Init")]
        public class Anti_init
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("TeammateReport")]
        public class TeammateReport
        {
            public static bool Prefix(string _reportname)
            {
                UI.whoReportMe = UI.whoReportMe + "\n" + _reportname;
                Plugin.Log.LogWarning("[-]有人举报你喽:" + _reportname);
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("CurIsPunish")]
        public class Anti_CurIsPunish
        {
            public static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("InitScanner")]
        public class InitScanner
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("InitScaner")]
        public class InitScaner
        {
            public static bool Prefix(ref string pParameters,ref IntPtr __result)
            {
                __result = (IntPtr)0;
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("InitSafeModule")]
        public class InitSafeModule
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("InitDefaultRule")]
        public class InitDefaultRule
        {
            public static bool Prefix()
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("Log")]
        public class Log
        {
            public static bool Prefix(string msg)
            {
                if (Plugin.debugMode) {
                    Plugin.Log.LogWarning("[-]AntiCheatHook: " + msg);
                }
               
                return false;
            }
        }

        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("FSCheat")]
        public class Anti_FSCheat
        {
            public static bool Prefix()
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("ClientCheat")]
        public class Anti_ClientCheat
        {

            public static bool Prefix(string _courseName)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(s2cnetwar))]
        [HarmonyPatch("C2GSImmortal")]
        public class Anti_C2GSImmortal
        {

            public static bool Prefix(int iImmortal)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("CanPunishByRule")]
        public class Anti_CanPunishByRule
        {

            public static bool Prefix(string _rulename, ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("ClientRecordImmortal")]
        public class Anti_ClientRecordImmortal
        {

            public static bool Prefix(string _log)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("CurIsPunish")]
        public class Anti_CurIsPunish1
        {

            public static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(GameWorkCheat))]
        [HarmonyPatch("DllMatch")]
        public class Anti_DllMatch
        {
           
            public static bool Prefix(string _content)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("InImmortalTime")]
        public class Anti_InImmortalTime
        {
       
            public static bool Prefix(IniFile fileData, string SectionName, ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("IsImmortal")]
        public class Anti_IsImmortal
        {
           
            public static bool Prefix(ref bool __result)
            {
                __result = false;
                return false;
            }
        }
        [HarmonyPatch(typeof(SecurityManager))]
        [HarmonyPatch("LSCheat")]
        public class Anti_LSCheat
        {
      
            public static bool Prefix(int cheattime)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(lscheat))]
        [HarmonyPatch("LS2CAddScanRule")]
        internal class Anti_netcheat
        {

            public static bool Prefix(lscheat_LS2CAddScanRuleClass data)
            {
                return false;
            }
        }
        [HarmonyPatch(typeof(lscheat))]
        [HarmonyPatch("LS2CPlayerCheat")]
        internal class Anti_netcheat1
        {

            public static bool Prefix(lscheat_LS2CPlayerCheatClass data)
            {
                return false;
            }
        }

        [HarmonyPatch(typeof(s2cnetwar))]
        [HarmonyPatch("GS2CConfirmCheat")]
        public class Anti_s2cnetwar
        {
            public static bool Prefix(s2cnetwar_GS2CConfirmCheatClass data)
            {
                return false;
            }
        }
    }
}
