using Discord;
using Discord.Commands;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DungeonsAndDiscord.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        EmbedBuilder e = new EmbedBuilder();

        [Command("help")]
        public async Task help()
        {
            e.WithTitle("Would you like some help?");
            var send = "For help with /char, click :bust_in_silhouette:.";
            send += "\nFor help with /roll, click :game_die:.";
            send += "\nTo get back to this page, click :information_source:.";
            send += "\nWhen you're finished, click :regional_indicator_x:.";
            e.AddField("Info", send);
            var message = await Context.User.SendMessageAsync("", embed: e.Build());
            await message.AddReactionAsync(new Emoji("ℹ"));
            await message.AddReactionAsync(new Emoji("👤"));
            await message.AddReactionAsync(new Emoji("🎲"));
            await message.AddReactionAsync(new Emoji("🇽"));
        }

    }
}