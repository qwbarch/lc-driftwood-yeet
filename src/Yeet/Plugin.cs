using BepInEx;
using HarmonyLib;
using GameNetcodeStuff;
using GiantSpecimens.Enemy;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
using System.Collections;
using System;
using BepInEx.Logging;
using System.Linq.Expressions;

namespace Yeet
{
    [BepInPlugin(PluginId, PluginName, PluginVersion)]
    [BepInDependency(GiantSpecimens.PluginInfo.PLUGIN_GUID)]
    class Plugin : BaseUnityPlugin
    {
        public const string PluginId = "qwbarch.DriftwoodYeet";
        public const string PluginName = "DriftwoodYeet";
        public const string PluginVersion = "1.0.0";

        internal static Plugin Instance;

        void Awake()
        {
            Plugin.Instance = this;
            new Harmony(PluginId).PatchAll(typeof(PlayYeetSound));
        }
    }

    class PlayYeetSound
    {
        internal static AudioClip YeetSFX = null;

        static IEnumerator LoadYeetSFX()
        {
            var directory = Path.Join(Path.GetDirectoryName(Plugin.Instance.Info.Location), "DriftwoodYeet");
            var sfxFile = Path.Join(directory, "yeet.wav");
            using var request = UnityWebRequestMultimedia.GetAudioClip("file://" + sfxFile, AudioType.WAV);
            yield return request.SendWebRequest();
            YeetSFX = DownloadHandlerAudioClip.GetContent(request);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PreInitSceneScript), nameof(PreInitSceneScript.Start))]
        static void OnPreInitScene(PreInitSceneScript __instance)
        {
            __instance.StartCoroutine(LoadYeetSFX());
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(DriftwoodGiantAI), nameof(DriftwoodGiantAI.ThrowingPlayer))]
        static void Yeet(DriftwoodGiantAI __instance)
        {
            Console.WriteLine("yeet.");
            Console.WriteLine($"null: {YeetSFX == null}");
            __instance.creatureSFX.PlayOneShot(YeetSFX);
        }
    }
}