using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonsAndDiscord.Modules
{
    internal class InputClass
    {
        private User u;
        private HelperClass helper = new HelperClass();
        SocketCommandContext Context;
        String[] input;

        internal async Task runCommandAsync(User use, SocketCommandContext Context)
        {
            this.u = use; this.Context = Context;
            var message = (IUserMessage)Context.Channel.GetMessageAsync(u.getInput()[Context.Channel.ToString()]).Result;
            var embed = message.Embeds;
            EmbedBuilder e = new EmbedBuilder();
            int argPos = 0;
            if (Context.Message.ToString().ToLower().Equals("cancel"))
            {
                u.removeInput(Context.Channel.Id.ToString());
                helper.serialize(u, Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
                await Context.Channel.SendMessageAsync("Input cancelled.");
            }
            if (u.getCommand() == "add")
            {
                u.setCommand("");
                u.removeInput(Context.Channel.ToString());
                if (u.getCharacters().Contains(Context.Message.ToString()))
                {
                    await Context.Channel.SendMessageAsync("A character by that name already exists.");
                    helper.serialize(u, Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
                    return;
                }
                u.add5eChar(Context.Message.ToString());
                helper.serialize(u, Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
                e.WithTitle(embed.ElementAt(0).Title);
                String s = embed.ElementAt(0).Fields.ElementAt(0).Value;
                if (s == "You have no characters. Use :heavy_plus_sign: to make one.")
                    s = Context.Message.ToString() + "**<**";
                else s += "\n" + Context.Message.ToString();
                e.AddField("Characters", s);
                await message.ModifyAsync(m => m.Embed = e.Build());
            }
        }
    }
}