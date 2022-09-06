﻿global using CitizenFX.Core;
global using CitizenFX.Core.Native;
using FxEvents.EventSystem;
using FxEvents.Shared;
using Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FxEvents
{
    public class EventDispatcher : BaseScript
    {
        internal static Log Logger { get; set; }
        internal static EventDispatcher Instance { get; set; }
        internal ExportDictionary GetExports => Exports;
        internal PlayerList GetPlayers => Players;
        internal static ServerGateway Events { get; set; }
        public static bool Debug { get; set; } = false;


        public EventDispatcher()
        {
            Instance = this;
            Logger = new Log();
            Events = new ServerGateway();
        }

        /// <summary>
        /// registra un evento (TriggerEvent)
        /// </summary>
        /// <param name="name">Nome evento</param>
        /// <param name="action">Azione legata all'evento</param>
        internal void AddEventHandler(string eventName, Delegate action)
        {
            EventHandlers[eventName] += action;
        }

        public static void Send(Player player, string endpoint, params object[] args) => Events.Send(Convert.ToInt32(player.Handle), endpoint, args);
        public static void Send(ClientId client, string endpoint, params object[] args) => Events.Send(client.Handle, endpoint, args);
        public static void Send(List<Player> players, string endpoint, params object[] args) => Events.Send(players.Select(x => Convert.ToInt32(x.Handle)).ToList(), endpoint, args);
        public static void Send(List<ClientId> clients, string endpoint, params object[] args) => Events.Send(clients.Select(x => x.Handle).ToList(), endpoint, args);
        public static Task<T> Get<T>(Player player, string endpoint, params object[] args) where T : class => Events.Get<T>(Convert.ToInt32(player.Handle), endpoint, args);
        public static Task<T> Get<T>(ClientId client, string endpoint, params object[] args) where T : class => Events.Get<T>(client.Handle, endpoint, args);
        public static void Mount(string endpoint, Delegate @delegate) => Events.Mount(endpoint, @delegate);
        public static void Unmount(string endpoint) => Events.Unmount(endpoint);
    }
}