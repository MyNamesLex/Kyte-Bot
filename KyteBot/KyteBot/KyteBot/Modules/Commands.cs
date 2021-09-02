using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace KyteBot.Modules
{
    public class Commands : ModuleBase<SocketCommandContext>
    {
        [Command("Ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        [Command("Ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have the permission ``ban_member``!")]
        public async Task BanMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please Specify A User!");
                return;
            }
            if (reason == null)
            {
                reason = "Not Specified";
            }
            await Context.Guild.AddBanAsync(user, 1, reason);

            var EmbedBuilder = new EmbedBuilder()
                .WithDescription($":white_check_mark: {user.Mention} was banned\n**Reason** {reason}")
                .WithFooter(footer =>
                {
                    footer
                    .WithText("User Ban Log")
                    .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
                });
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(882790764751495170) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder()
               .WithDescription($"{user.Mention} was banned\n**Reason** {reason}\n**Moderator** {Context.User.Mention}")
               .WithFooter(footer =>
               {
                   footer
                   .WithText("User Ban Log")
                    .WithIconUrl("https://i.imgur.com/6Bi17B3.png");
               });
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);
            await ReplyAsync(embed: embed);

        }

        [Command("kick")]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "You don't have the permission **kick member**!")]
        public async Task KickMember(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Message.DeleteAsync();

            var EmbedBuilder = new EmbedBuilder
            {
                //Title = $"{user} was kicked",
                //Description = $"**Reason** {reason}"
            };
            EmbedBuilder.AddField($"{user} was kicked", $"**Reason:** {reason}", true)
            .WithDescription($"Nickname: {user.Mention}")
            .WithThumbnailUrl("//Icon URL here")
            .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(882790764751495170) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder
            {
                Title = $"{user} was kicked",
                Description = $"**Reason** {reason}"
            };
            EmbedBuilderLog.AddField("The kick was issued by", $"**Moderator:** {Context.User.Mention}", true)
            .WithDescription($"Nickname: {user.Mention}\n Reason: {reason}")
            .WithCurrentTimestamp();
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);

            await user.KickAsync(reason);
        }


        [Command("Unban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have the permission **Ban Members**!")]
        public async Task Unban(IGuildUser user = null, [Remainder] string reason = null)
        {
            if (user == null)
            {
                await ReplyAsync("Please specify a user!");
                return;
            }
            if (reason == null) reason = "Not specified";

            await Context.Message.DeleteAsync();

            var EmbedBuilder = new EmbedBuilder
            {
                //Title = $"{user} was kicked",
                //Description = $"**Reason** {reason}"
            };
            EmbedBuilder.AddField($"{user} was unbanned", $"**Reason:** {reason}", true)
            .WithDescription($"Nickname: {user.Mention}")
            .WithThumbnailUrl("//Icon URL here")
            .WithCurrentTimestamp();
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);

            ITextChannel logChannel = Context.Client.GetChannel(882790764751495170) as ITextChannel;
            var EmbedBuilderLog = new EmbedBuilder
            {
                Title = $"{user} was unbanned",
                Description = $"**Reason** {reason}"
            };
            EmbedBuilderLog.AddField("The unban was issued by", $"**Moderator:** {Context.User.Mention}", true)
            .WithDescription($"Nickname: {user.Mention}\n Reason: {reason}")
            .WithCurrentTimestamp();
            Embed embedLog = EmbedBuilderLog.Build();
            await logChannel.SendMessageAsync(embed: embedLog);

            await Context.Guild.RemoveBanAsync(user);
        }
    }
}

