using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using System;
using System.IO;
using System.Threading.Tasks;

namespace KyteBot.Modules
{
    public class Commands : InteractiveBase
    {
        public string prefix = File.ReadAllText("prefix.txt");

        [Command("Ping")]
        public async Task Ping()
        {
            await ReplyAsync("Pong");
        }

        [Command("Ban")]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have the permission **Ban Members**!")]
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
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "You don't have the permission **Kick Member**!")]
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
                Title = $"{user} was kicked",
                Description = $"**Reason** {reason}"
            };

            EmbedBuilder.AddField($"{user} was kicked", $"**Reason:** {reason}", true)
            .WithDescription($"Nickname: {user.Mention}")
            .WithThumbnailUrl("https://i.imgur.com/6Bi17B3.png")
            .WithCurrentTimestamp();

            ITextChannel logChannel = Context.Client.GetChannel(882790764751495170) as ITextChannel;

            var EmbedBuilderLog = new EmbedBuilder
            {
                Title = $"{user} was kicked",
                Description = $"**Reason** {reason}"
            };

            EmbedBuilderLog.AddField("The kick was issued by", $"**Moderator:** {Context.User.Mention}", true)
            .WithDescription($"Nickname: {user.Mention}\n Reason: {reason}")
            .WithThumbnailUrl("https://i.imgur.com/6Bi17B3.png")
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
                Title = $"{user} was unbanned",
                Description = $"**Reason** {reason}"
            };

            EmbedBuilder.AddField($"{user} was unbanned", $"**Reason:** {reason}", true)
            .WithDescription($"Nickname: {user.Mention}")
            .WithThumbnailUrl("https://i.imgur.com/6Bi17B3.png")
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

        [Command("Search")]
        public async Task search([Remainder] string search = null)
        {
            if (search == null) search = "Not specified search query";

            await Context.Message.DeleteAsync();

            string cleansearch = search; // for embed
            search = search.Replace(" ", "+");

            var EmbedBuilder = new EmbedBuilder
            {

            };

            string endsearch = $"https://duckduckgo.com/?t=ffab&q=" + search;

            EmbedBuilder.AddField($"Searched for:", $"{cleansearch}", true)
            .WithDescription(endsearch)
            .WithCurrentTimestamp();

            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("help")]
        public async Task help()
        {
            var EmbedBuilder = new EmbedBuilder
            {
                Title = $"Help",
                Description = $"!RNG, !Search, !Unban, !Kick, !Ban, !Ping, !ChangePrefix, !XO, !8ball, !GameDevAdvice",
            };

            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
        }

        [Command("rng", RunMode = RunMode.Async)]
        public async Task RNGFunction()
        {
            int counter = 0;
            Random r = new Random();
            int GenerateRandom = r.Next(1, 10);
            await ReplyAsync("I have come up with a number between 1 and 10, try guess it! :partying_face:");
            await RNGLoop(GenerateRandom, counter);
        }

        public async Task RNGLoop(int GenerateRandom, int counter)
        {
            var response = await NextMessageAsync();
            Console.WriteLine(response);

            if (response.ToString() == GenerateRandom.ToString())
            {
                counter++;
                await ReplyAsync("Correct! The number was " + GenerateRandom + " You guessed it right in " + counter + " guesses!");
                Console.WriteLine("Completed");
                return;
            }
            else
            {
                counter++;
                await ReplyAsync("Incorrect!");
                await RNGLoop(GenerateRandom, counter);
            }
        }

        //Change Prefix

        [Command("ChangePrefix")]
        [RequireUserPermission(GuildPermission.Administrator, ErrorMessage = "You don't have the permission **Admin**!")]
        public async Task ChangePrefixFunction(string newprefix)
        {
            if (newprefix.Length == 1)
            {
                Console.WriteLine(File.ReadAllText("prefix.txt"));

                string path = "prefix.txt";

                File.Create(path).Close();

                File.WriteAllText(path, newprefix);
                await ReplyAsync(newprefix + " Is the new prefix");

                Console.WriteLine(File.ReadAllText("prefix.txt"));
            }
            else
            {
                await ReplyAsync("Prefix set is too long! Disabling new prefix");
                await ReplyAsync("Prefix is: " + File.ReadAllText("prefix.txt"));
            }
        }

        [Command("XO", RunMode = RunMode.Async)]
        public async Task NaughtsCrosses()
        {
            bool xturn = false;
            string pos1 = "-";
            string pos2 = "-";
            string pos3 = "-";
            string pos4 = "-";
            string pos5 = "-";
            string pos6 = "-";
            string pos7 = "-";
            string pos8 = "-";
            string pos9 = "-";

            var EmbedBuilder = new EmbedBuilder
            {
                Title = "Naughts and Crosses!",
            }
            .WithDescription("-|-|-\n-|-|-\n-|-|-\n\nPos1|Pos2|Pos3\nPos4|Pos5|Pos6\nPos7|Pos8|Pos9\n\nState the position you want to start in!\nreply with Pos1 - Pos9 !Stop to stop game");

            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
            await XOStart(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
        }

        //XO Board

        public async Task XOStart(string pos1, string pos2, string pos3, string pos4, string pos5, string pos6, string pos7, string pos8, string pos9, bool xturn)
        {
            if (xturn == true)
            {
                await ReplyAsync("``\n Current Turn: X``");
            }
            else
            {
                await ReplyAsync("``\n Current Turn: O``");
            }
            var message = await NextMessageAsync();
            string mes = message.ToString().ToLower();

            if (mes == "!stop")
            {
                Console.WriteLine("Stopped Game");
                await ReplyAsync("Stopped Game");
                return;
            }

            else
            {

                if (xturn == false)
                {
                    xturn = true;
                }
                else
                {
                    xturn = false;
                }
                Console.WriteLine(xturn);
                switch (mes)
                {
                    case "pos1":
                        if (pos1 == "-")
                        {
                            if (xturn == false)
                            {
                                pos1 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos1 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;
                    case "pos2":
                        if (pos2 == "-")
                        {
                            if (xturn == false)
                            {
                                pos2 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos2 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }

                        break;
                    case "pos3":
                        if (pos3 == "-")
                        {
                            if (xturn == false)
                            {
                                pos3 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos3 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;
                    case "pos4":
                        if (pos4 == "-")
                        {
                            if (xturn == false)
                            {
                                pos4 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos4 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;
                    case "pos5":
                        if (pos5 == "-")
                        {
                            if (xturn == false)
                            {
                                pos5 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos5 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;
                    case "pos6":
                        if (pos6 == "-")
                        {
                            if (xturn == false)
                            {
                                pos6 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos6 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;
                    case "pos7":
                        if (pos7 == "-")
                        {
                            if (xturn == false)
                            {
                                pos7 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos7 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;
                    case "pos8":
                        if (pos8 == "-")
                        {
                            if (xturn == false)
                            {
                                pos8 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos8 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }

                        break;
                    case "pos9":
                        if (pos9 == "-")
                        {
                            if (xturn == false)
                            {
                                pos9 = "x";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                            else
                            {
                                pos9 = "o";
                                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                                break;
                            }
                        }
                        else
                        {
                            await ReplyAsync("Position taken\n Turn wasted :laughing:");
                            await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
                        }
                        break;

                }
                await DrawBoard(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
            }
        }

        public async Task DrawBoard(string pos1, string pos2, string pos3, string pos4, string pos5, string pos6, string pos7, string pos8, string pos9, bool xturn)
        {
            var EmbedBuilder = new EmbedBuilder
            {
                Title = "Naughts and Crosses!",
            }
            .WithDescription($"{pos1}" + "|" + $"{pos2}" + "|" + $"{pos3}" + "\n" + $"{pos4}" + "|" + $"{pos5}" + "|" + $"{pos6}" + "\n" + $"{pos7}" + "|" + $"{pos8}" + "|" + $"{pos9}");
            Embed embed = EmbedBuilder.Build();
            await ReplyAsync(embed: embed);
            await XOStart(pos1, pos2, pos3, pos4, pos5, pos6, pos7, pos8, pos9, xturn);
        }

        // 8 ball
        [Command("8Ball", RunMode = RunMode.Async)]
        public async Task EightBallFunction()
        {
            await ReplyAsync("What's it you want to ask?");
            var response = await NextMessageAsync();
            Console.WriteLine(response);
            Random r = new Random();
            int rng = r.Next(1, 10);
            switch (rng)
            {
                case 1:
                    await ReplyAsync("Of Courseee!");
                    break;
                case 2:
                    await ReplyAsync("Definitely Not");
                    break;
                case 3:
                    await ReplyAsync("Yep");
                    break;
                case 4:
                    await ReplyAsync("Sure thing!");
                    break;
                case 5:
                    await ReplyAsync("Your right");
                    break;
                case 6:
                    await ReplyAsync("Maybe");
                    break;
                case 7:
                    await ReplyAsync("Unsure");
                    break;
                case 8:
                    await ReplyAsync("It's inconclusive");
                    break;
                default:
                    await ReplyAsync("It's inconclusive");
                    break;
            }
        }

        // Game Dev Advice

        [Command("GameDevAdvice")]
        public async Task CodingAdviceFunction()
        {
            Random r = new Random();
            int rng = r.Next(1, 10);
            switch (rng)
            {
                case 1:
                    await ReplyAsync("Long if statements are terrible in readability and efficiency, use case switches!");
                    break;
                case 2:
                    await ReplyAsync("Making long comments is a quick way to get people to not like you");
                    break;
                case 3:
                    await ReplyAsync("Good graphics go a long way in making a good first impressions");
                    break;
                case 4:
                    await ReplyAsync("Don't make UI in a rush, good UI can make or break an experience");
                    break;
                case 5:
                    await ReplyAsync("Play-testers are invaluable in spotting bugs and down sides to a game");
                    break;
                case 6:
                    await ReplyAsync("Think of the player when making a game, how will a person who has never seen your game play?");
                    break;
                case 7:
                    await ReplyAsync("If your stuck on a problem, take a break and come back to it with a fresh mind, a walk outside helps a lot!");
                    break;
                case 8:
                    await ReplyAsync("Make games you will like to play");
                    break;
                case 9:
                    await ReplyAsync("If your burnt out and not enjoying developing the game, leave the project for a while and come back when your excited to make it");
                    break;
            }
        }
    }

}

