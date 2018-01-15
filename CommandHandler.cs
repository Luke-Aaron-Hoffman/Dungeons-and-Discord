using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Discord.Commands;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DungeonsAndDiscord
{
    public class CommandHandler
    {
        private DiscordSocketClient _discord;

        private CommandService _commands;

        private IServiceProvider _services;

        private Modules.HelperClass helper = new Modules.HelperClass();

        private Modules.InputClass input = new Modules.InputClass();

        private User u;

        Emoji up = new Emoji("🔼");
        Emoji down = new Emoji("🔽");
        Emoji check = new Emoji("☑");
        Emoji plus = new Emoji("➕");
        Emoji minus = new Emoji("➖");
        Emoji x = new Emoji("🇽");
        Emoji str = new Emoji("🇸");
        Emoji dex = new Emoji("🇩");
        Emoji con = new Emoji("🇨");
        Emoji intel = new Emoji("🇮");
        Emoji wis = new Emoji("🇼");
        Emoji cha = new Emoji("↪");
        Emoji prof = new Emoji("🇵");
        Emoji charList = new Emoji("👥");

        String[] values;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _discord = client;

            _commands = new CommandService();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

            _discord.MessageReceived += HandleCommandAsync;
            _discord.ReactionAdded += OnReactionAdded;
            _discord.ReactionRemoved += OnReactionAdded;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(_discord, msg);
            u = helper.deserialize(context.User.Username.ToString() + "#" + context.User.Discriminator.ToString());
            int argPos = 0;
            
            if(u.getInput().ContainsKey(context.Channel.ToString()))
            {
                await input.runCommandAsync(u, context);
            }
            else if (msg.HasCharPrefix('/', ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos); 
                //pass in the dependencyMap, which lets you call the logic code in the commandHandler
                
                if(!result.IsSuccess  && result.Error != CommandError.UnknownCommand)
                {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }

        async Task OnReactionAdded(Cacheable<IUserMessage, ulong> messageParam, ISocketMessageChannel channel, SocketReaction reaction)
        {
            var message = await messageParam.GetOrDownloadAsync();
            EmbedBuilder e = new EmbedBuilder();
            int selected = 0;
            String send = "";
            User u = helper.deserialize(reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
            if (!(message.Author.Id.ToString() == "387292286196776962"))
                return;
            if (reaction.UserId.ToString() == "387292286196776962")
                return;
            var embed = message.Embeds;
            if(reaction.Emote.Name == x.Name)
            {
                await message.DeleteAsync();
                return;
            }
            if(embed.ElementAt(0).Title== "Would you like some help?")
            {
                #region Help Command
                if (reaction.Emote.Name == "ℹ")
                {
                    e.WithTitle("Would you like some help?");
                    send = "For help with /char, click :bust_in_silhouette:.";
                    send += "\nFor help with /roll, click :game_die:.";
                    send += "\nTo get back to this page, click :information_source:.";
                    send += "\nWhen you're finished, click :regional_indicator_x:.";
                    e.AddField("Info", send);
                    await message.ModifyAsync(m => m.Embed = e.Build());

                }
                if(reaction.Emote.Name == "👤")
                {
                    e.WithTitle("Would you like some help?");
                    send = "To bring up the character manager, type /char.";
                    send += "\nThe 'Characters' section lets you view, manage, and select your characters.";
                    send += "\nUse :arrow_up_small: and :arrow_down_small: to select a character.";
                    send += "\nUse :heavy_plus_sign: to create a new character. Use the :heavy_minus_sign: to delete a selected character.";
                    send += "\nUse :ballot_box_with_check: to select a character (this sets it as your default).";
                    send += "\nWhen a character is selected, select a stat from the list below [the letters correspond to the stats].";
                    send += "\nUse :heavy_plus_sign: and :heavy_minus_sign: to change a selected stat.";
                    send += "\nUse :busts_in_silhouette: to return to the 'Characters' section.";
                    send += "\nWhen you're finished, click :regional_indicator_x:.";
                    e.AddField("Character", send);
                    await message.ModifyAsync(m => m.Embed = e.Build());
                }
                if(reaction.Emote.Name == "🎲")
                {
                    e.WithTitle("Would you like some help?");
                    send = "/roll dice [+num] [+stat] [+dice] - Rolls dice determined by 'dice.' Any combination of dice, numbers, and stats (from your default character) can be added.";
                    send += "\nExample 1: /roll 1d20+Str-4 will roll a 20 sided die, add your character's strength, and then subtract 4.";
                    send += "\nExample 2: /roll 5 + 3d6 - 6d4 will roll 3d6 and add that to 5, and then roll 6d4 and subtract that from the result.";
                    e.AddField("Roll", send);
                    await message.ModifyAsync(m => m.Embed = e.Build());
                }
                #endregion
            }
            if(embed.ElementAt(0).Title==reaction.User.Value.Username.ToString()+ "#" +reaction.User.Value.Discriminator.ToString())
            {
                if (embed.ElementAt(0).Fields.ElementAt(0).Name == "Characters")
                {
                    values = embed.ElementAt(0).Fields.ElementAt(0).Value.Split('\n');
                    send = "";
                    #region Character Up
                    if (reaction.Emote.Name == up.Name)
                    {
                        for(int i = 0;i<values.Length;i++)
                        {
                            if (values[i].Contains("**<**"))
                            {
                                values[i] = values[i].Replace("**<**","");
                                if (i == 0)
                                    i = values.Length - 1;
                                else
                                    i = i - 1;
                                values[i] = values[i] + "**<**";
                                i = values.Length;
                            }
                        }
                        e.WithTitle(reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                        e.AddField("Characters", String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    #endregion
                    #region Character Down
                    if (reaction.Emote.Name == down.Name)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i].Contains("**<**"))
                            {
                                values[i] = values[i].Replace("**<**", "");
                                if (i == values.Length-1)
                                    i = 0;
                                else
                                    i = i + 1;
                                values[i] = values[i] + "**<**";
                                i = values.Length;
                            }
                        }
                        e.WithTitle(reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                        e.AddField("Characters", String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    #endregion
                    #region Character Check
                    if (reaction.Emote.Name == check.Name)
                    {
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i].Contains("**<**"))
                            {
                                String character = values[i].Replace("**<**", "");
                                u.setDefault(character);
                                helper.serialize(u, reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                                e.WithTitle(reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                                Character c = u.getChar(character);
                                send += "Str "+ c.getStat("Str").getStat() + "\n";
                                send += "Dex " + c.getStat("Dex").getStat() + "\n";
                                send += "Con " + c.getStat("Con").getStat() + "\n";
                                send += "Int " + c.getStat("Int").getStat() + "\n";
                                send += "Wis " + c.getStat("Wis").getStat() + "\n";
                                send += "Cha " + c.getStat("Cha").getStat() + "\n";
                                send += "Prof " + c.getStat("Prof").getStat()+"\n";
                                e.AddField(character, send);
                                await message.ModifyAsync(m => m.Embed = e.Build());
                                //Add old reactions to "get rid of them.
                                await message.RemoveAllReactionsAsync();
                                //Add new reactions
                                await message.AddReactionAsync(str);
                                await message.AddReactionAsync(dex);
                                await message.AddReactionAsync(con);
                                await message.AddReactionAsync(intel);
                                await message.AddReactionAsync(wis);
                                await message.AddReactionAsync(cha);
                                await message.AddReactionAsync(prof);
                                await message.AddReactionAsync(plus);
                                await message.AddReactionAsync(minus);
                                await message.AddReactionAsync(charList);
                                await message.AddReactionAsync(x);
                                return;
                            }
                        }
                    }
                    #endregion
                    #region Character Plus
                    if (reaction.Emote.Name == plus.Name)
                    {
                        u.addInput(message.Channel.ToString(), message.Id);
                        u.setCommand("add");
                        helper.serialize(u, reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                        await message.Channel.SendMessageAsync("Please input a name for your new character.");
                        return;
                    }
                    #endregion
                    #region Character Minus
                    if (reaction.Emote.Name == minus.Name)
                    {
                        if (u.getCharacters().Count <= 0)
                            return;
                        send = "";
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        if (values.Count == 1)
                        {
                            values[0] = values[0].Replace("**<**", "");
                            send = "You have no characters. Use :heavy_plus_sign: to make one.";
                            u.removeChar(values[0]);
                        }
                        else
                        {
                            for (int i = 0; i < values.Count; i++)
                            {
                                if (values[i].Contains("**<**"))
                                {
                                    if(i==values.Count-1)
                                    {
                                        values[0] = values[0] + "**<**";
                                    }
                                    else
                                    {
                                        values[i + 1] = values[i + 1] + "**<**";
                                    }
                                    values[i] = values[i].Replace("**<**", "");
                                    u.removeChar(values[i]);
                                    values.RemoveAt(i);
                                }
                            }
                            send = String.Join("\n", values);
                        }
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField("Characters", send);
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        helper.serialize(u, reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                        return;
                    }
                    #endregion
                }
                if (u.getCharacters().Contains(embed.ElementAt(0).Fields.ElementAt(0).Name))
                {
                    #region Character CharList
                    if(reaction.Emote.Name == charList.Name)
                    {
                        e.WithTitle(reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                        foreach (String s in u.getCharacters())
                        {
                            if (s.Equals(u.getCharacters()[0]))
                                send = (s + "**<**\n");
                            else
                                send += (s + "\n");
                        }
                        e.AddField("Characters", send);
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        await message.RemoveAllReactionsAsync();
                        await message.AddReactionAsync(up);
                        await message.AddReactionAsync(down);
                        await message.AddReactionAsync(check);
                        await message.AddReactionAsync(plus);
                        await message.AddReactionAsync(minus);
                        await message.AddReactionAsync(x);
                    }
                    #endregion
                    #region Character Stats
                    if (reaction.Emote.Name == str.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for(int i = 0;i<values.Count;i++)
                        {
                            values[i] = values[i].Replace("**<**","");
                        }
                        values[0] = values[0] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    if (reaction.Emote.Name == dex.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for (int i = 0; i < values.Count; i++)
                        {
                            values[i] = values[i].Replace("**<**", "");
                        }
                        values[1] = values[1] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    if (reaction.Emote.Name == con.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for (int i = 0; i < values.Count; i++)
                        {
                            values[i] = values[i].Replace("**<**", "");
                        }
                        values[2] = values[2] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    if (reaction.Emote.Name == intel.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for (int i = 0; i < values.Count; i++)
                        {
                            values[i] = values[i].Replace("**<**", "");
                        }
                        values[3] = values[3] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    if (reaction.Emote.Name == wis.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for (int i = 0; i < values.Count; i++)
                        {
                            values[i] = values[i].Replace("**<**", "");
                        }
                        values[4] = values[4] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    if (reaction.Emote.Name == cha.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for (int i = 0; i < values.Count; i++)
                        {
                            values[i] = values[i].Replace("**<**", "");
                        }
                        values[5] = values[5] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    if (reaction.Emote.Name == prof.Name)
                    {
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        for (int i = 0; i < values.Count; i++)
                        {
                            values[i] = values[i].Replace("**<**", "");
                        }
                        values[6] = values[6] + "**<**";
                        e.WithTitle(message.Embeds.ElementAt(0).Title);
                        e.AddField(message.Embeds.ElementAt(0).Fields.ElementAt(0).Name, String.Join("\n", values));
                        await message.ModifyAsync(m => m.Embed = e.Build());
                        return;
                    }
                    #endregion
                    #region Stat Plus
                    if(reaction.Emote.Name == plus.Name || reaction.Emote.Name == minus.Name)
                    {
                        int add = 1;
                        if (reaction.Emote.Name == minus.Name)
                            add = -1;
                        List<String> values = message.Embeds.ElementAt(0).Fields.ElementAt(0).Value.Split('\n').ToList();
                        String character = message.Embeds.ElementAt(0).Fields.ElementAt(0).Name;
                        for(int i = 0;i<values.Count;i++)
                        {
                            if (values[i].Contains("**<**"))
                            {
                                if (i == 0)
                                {
                                    u.addStat(character, "Str", u.getChar(character).getStat("Str").getStat() + add);
                                    values[i] = "Str " + u.getChar(character).getStat("Str") + "**<**";
                                }
                                else if (i == 1)
                                {
                                    u.addStat(character, "Dex", u.getChar(character).getStat("Dex").getStat() + add);
                                    values[i] = "Dex " + u.getChar(character).getStat("Dex") + "**<**";
                                }
                                else if (i == 2)
                                {
                                    u.addStat(character, "Con", u.getChar(character).getStat("Con").getStat() + add);
                                    values[i] = "Con " + u.getChar(character).getStat("Con") + "**<**";
                                }
                                else if (i == 3)
                                {
                                    u.addStat(character, "Int", u.getChar(character).getStat("Int").getStat() + add);
                                    values[i] = "Int " + u.getChar(character).getStat("Int") + "**<**";
                                }
                                else if (i == 4)
                                {
                                    u.addStat(character, "Wis", u.getChar(character).getStat("Wis").getStat() + add);
                                    values[i] = "Wis " + u.getChar(character).getStat("Wis") + "**<**";
                                }
                                else if (i == 5)
                                {
                                    u.addStat(character, "Cha", u.getChar(character).getStat("Cha").getStat() + add);
                                    values[i] = "Cha " + u.getChar(character).getStat("Cha") + "**<**";
                                }
                                else if (i == 6)
                                {
                                    u.addStat(character, "Prof", u.getChar(character).getStat("Prof").getStat() + add);
                                    values[i] = "Prof " + u.getChar(character).getStat("Prof") + "**<**";
                                }
                                e.WithTitle(message.Embeds.ElementAt(0).Title);
                                e.AddField(character, String.Join("\n", values));
                                await message.ModifyAsync(m => m.Embed = e.Build());
                                helper.serialize(u, reaction.User.Value.Username.ToString() + "#" + reaction.User.Value.Discriminator.ToString());
                            }
                        }
                    }
                    #endregion
                }
            }
        }
    }
}
