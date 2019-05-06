using ProjectAutomata;
using UnityEngine;
using System;
using System.Timers;

namespace CustomMod
{
    public class CustomMod : Mod
    {
        bool worldReadyEventDispatched = false;

        public override void OnModWasLoaded()
	{
            base.OnModWasLoaded();
            Timer aTimer = new Timer();
            aTimer.Elapsed += OnTimedEvent;
            aTimer.Interval = 1000;
            aTimer.Enabled = true;

            Debug.Log("Loading CustomMod");
           
        }

        private void CustomCommand(string arg1 = "", string arg2 = "")
        {
            LogInfo("custom command");
            LogInfo("custom argument 1: " + arg1);
            LogInfo("custom argument 2: " + arg2);
        }

        private void OnWorldBecameReady()
        {
            DevConsole.Console.AddCommand(new DevConsole.Command<string, string>("somecommand", CustomCommand));

            string msg = "Sanasol World Seed: " + World.seed;
            LogInfo(msg);
        }

        private void OnTimedEvent(System.Object source, ElapsedEventArgs e)
        {
            //Debug.Log("Dispatch OnTimedEvent");
            //DevConsole.Console.Log("Dispatch OnTimedEvent");
            if (World.instance.isWorldReady && !worldReadyEventDispatched)
            {
                string msg = "Dispatch World Ready Event";
                LogInfo(msg);
                worldReadyEventDispatched = true;
                OnWorldBecameReady();
            }
        }

        public static void Log(String message)
        {
            message = LogPrefix + message;

            Debug.Log(message);
            DevConsole.Console.Log(message);
        }

        public static void LogError(String message)
        {
            message = LogPrefix + message;

            Debug.LogError(message);
            DevConsole.Console.LogError(message);
        }

        public static void LogInfo(String message)
        {
            message = LogPrefix + message;

            Debug.Log(message);
            DevConsole.Console.LogInfo(message);
        }

        public static void LogWarning(String message)
        {
            message = LogPrefix + message;

            Debug.LogWarning(message);
            DevConsole.Console.LogWarning(message);
        }

        public static string LogPrefix => "[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "] ";
    }
}
