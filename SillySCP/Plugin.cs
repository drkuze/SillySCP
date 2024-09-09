using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Exiled.API.Features;
using PluginAPI.Events;

namespace SillySCP
{
    public class Plugin : Plugin<Config>
    {
        public static Plugin Instance;
        public List<PlayerStat> PlayerStats;
        private EventHandler handler;
        public static DiscordSocketClient Client;
        private bool statusUpdating;

        public override void OnEnabled()
        {
            Instance = this;
            handler = new EventHandler();
            EventManager.RegisterEvents(this, handler);
            Task.Run(StartClient);
            base.OnEnabled();
        }
        
        public override void OnDisabled()
        {
            EventManager.UnregisterEvents(this, handler);
            handler = null;
            Task.Run(StopClient);
            base.OnDisabled();
        }
        
        private static Task Log(LogMessage msg)
        {
            PluginAPI.Core.Log.Info(msg.ToString());
            return Task.CompletedTask;
        }

        private static async Task StartClient()
        {
            var config = new DiscordSocketConfig()
            {
                GatewayIntents = GatewayIntents.Guilds | GatewayIntents.GuildMessages
            };
            Client = new DiscordSocketClient(config);
            Client.Log += Log;
            Client.Ready += Instance.Ready;
            await Client.LoginAsync(TokenType.Bot, Instance.Config.Token);
            await Client.StartAsync();
        }
        
        private static async Task StopClient()
        {
            await Client.StopAsync();
            await Client.LogoutAsync();
        }

        public Color ConvertHexToColor(string hex)
        {
            hex = hex.Replace("#", string.Empty);

            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);

            return new Color(r, g, b);
        }

        public void SetStatus()
        {
            string players = "";
            var textChannel = GetChannel(Instance.Config.ChannelId);
            var playerList = Player.List;

            foreach (var player in playerList)
            {
                players += "- " + player.Nickname + "\n";
            }

            var embedBuilder = new EmbedBuilder()
                .WithTitle(Instance.Config.EmbedTitle)
                .WithColor(ConvertHexToColor(Instance.Config.EmbedColor))
                .WithDescription(players == "" ? Instance.Config.NoPlayersOnlineText : players);

            if (!string.IsNullOrEmpty(Instance.Config.ImageUrl))
            {
                embedBuilder.WithImageUrl(Instance.Config.ImageUrl);
            }

            if (!string.IsNullOrEmpty(Instance.Config.ThumbnailUrl))
            {
                embedBuilder.WithThumbnailUrl(Instance.Config.ThumbnailUrl);
            }

            SetMessage(textChannel, Instance.Config.MessageId, embedBuilder.Build());
            SetCustomStatus();
        }


        private SocketTextChannel GetChannel(ulong id)
        {
            var channel = Client.GetChannel(id);
            if (channel.GetChannelType() != ChannelType.Text) return null;
            var textChannel = (SocketTextChannel) channel;
            return textChannel;
        }

        private IMessage GetMessage(SocketTextChannel channel, ulong id)
        {
            var message = channel.GetMessageAsync(id).GetAwaiter().GetResult();
            if (message.Author.Id != Client.CurrentUser.Id) return null;
            return message;
        }
        
        private void SetMessage(SocketTextChannel channel, ulong id, Embed embed)
        {
            try
            {
                var message = GetMessage(channel, id);
                if (message.Author.Id != Client.CurrentUser.Id) return;
                channel.ModifyMessageAsync(id, msg =>
                {
                    msg.Embed = embed;
                    msg.Content = "";
                }).GetAwaiter().GetResult();
            }
            catch (Exception error)
            {
                PluginAPI.Core.Log.Error(error.ToString());
            }
        }

        private void SetCustomStatus()
        {
            var playerLimit = Instance.Config.PlayerLimit;
            var status = Server.PlayerCount + "/" + playerLimit;

            try
            {
                Client.SetCustomStatusAsync(status).GetAwaiter().GetResult();
                statusUpdating = false;
            }
            catch (Exception error)
            {
                PluginAPI.Core.Log.Error(error.ToString());
                if (!statusUpdating)
                {
                    statusUpdating = true;
                    Task.Delay(5).ContinueWith(_ => SetCustomStatus());
                }
            }
        }

        private async Task Ready()
        {
            SetStatus();
        }
    }
}