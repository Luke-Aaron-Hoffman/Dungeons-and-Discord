using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.IO;
using Discord.Rest;
using Discord.WebSocket;

namespace DungeonsAndDiscord.Commands
{
    public class CharBase : ModuleBase<SocketCommandContext>
    {
        User u;
        Modules.HelperClass helper = new Modules.HelperClass();
        Emoji up = new Emoji("🔼");
        Emoji down = new Emoji("🔽");
        Emoji check = new Emoji("☑");
        Emoji plus = new Emoji("➕");
        Emoji minus = new Emoji("➖");
        Emoji x = new Emoji("🇽");
        String send = "You have no characters. Use :heavy_plus_sign: to make one.";
        [Command("char")]
        public async Task chara()
        {
            u = helper.deserialize(Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
            EmbedBuilder sendEmbed = new EmbedBuilder();
            sendEmbed.WithTitle(Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
            foreach(String s in u.getCharacters())
            {
                if (s.Equals(u.getCharacters()[0]))
                    send = (s + "**<**\n");
                else
                    send += (s + "\n");
            }
            sendEmbed.AddField("Characters", send);
            var message = await Context.Channel.SendMessageAsync("", embed: sendEmbed.Build());
            await message.AddReactionAsync(up);
            await message.AddReactionAsync(down);
            await message.AddReactionAsync(check);
            await message.AddReactionAsync(plus);
            await message.AddReactionAsync(minus);
            await message.AddReactionAsync(x);
        }
        [Command("makeChars")]
        public async Task testing()
        {
            u = helper.deserialize(Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
            u.add5eChar("Valkyr");
            u.add5eChar("George");
            u.add5eChar("Rebecca");
            u.add5eChar("Ollie");
            u.add5eChar("End");
            helper.serialize(u, Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
        }
    }
}
