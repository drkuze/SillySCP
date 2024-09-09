using Exiled.API.Interfaces;

namespace SillySCP
{
    public sealed class Config : IConfig

    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        public string Token { get; set; } = "token";
        public ulong ChannelId { get; set; } = 1279544677334253610;
        public ulong MessageId { get; set; } = 1280910252325339311;
        public string EmbedTitle { get; set; } = "Silly SCP Member List";
        public string NoPlayersOnlineText { get; set; } = "No players online";
        public string EmbedColor { get; set; } = "#3498db";
        public string ImageUrl { get; set; } = "";
        public string ThumbnailUrl { get; set; } = "";
        public int PlayerLimit { get; set; } = 40;

    }
}