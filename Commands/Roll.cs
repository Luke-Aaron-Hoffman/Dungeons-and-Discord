using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DungeonsAndDiscord.Commands
{
    public class Roll : ModuleBase<SocketCommandContext>
    {
        Random rand = new Random();
        User u;
        String send;
        String[] die;
        String[] s;
        int temp;
        int tempTotal;
        Modules.HelperClass helper = new Modules.HelperClass();
        [Command("roll")]
        public async Task roll(params String[] original)
        {
            String tempString = String.Join("", original);
            s = Regex.Split(tempString, @"([+-])");
            u = helper.deserialize(Context.User.Username.ToString() + "#" + Context.User.Discriminator.ToString());
            for (int i = 0; i < s.Length; i++)
            {
                if (Regex.IsMatch(s[i], "[0-9]+d[0-9]+"))
                {
                    send += "( ";
                    die = Regex.Split(s[i], "(d)");
                    tempTotal = 0;
                    for (int d = 0; d < Convert.ToInt32(die[0]); d++)
                    {
                        temp = rand.Next(Convert.ToInt32(die[2]))+1;
                        tempTotal += temp;
                        send += temp + " ";
                    }
                    send += "= " + tempTotal + " ) ";
                    s[i] = tempTotal.ToString();
                }
                else if (Regex.IsMatch(s[i], "[0-9]+"))
                {
                    send += s[i] + " ";
                }
                else if (!(s[i].Equals("+") || s[i].Equals("-")))
                {
                    if (u.getDefault() == null)
                    {
                        send = "Please set a default character before trying to get stats.";
                        i = s.Length;
                    }
                    else if (u.getDefault().getStat(s[i]) == null)
                    {
                        send = "The stat " + s[i] + " does not exist.";
                        i = s.Length;
                    }
                    else
                        send += s[i] + "(" + u.getDefault().getStat(s[i]).getMod() + ") ";
                    s[i] = u.getDefault().getStat(s[i]).getMod().ToString();
                }
                else
                    send += s[i] + " ";
            }
            await Context.Channel.SendMessageAsync(send +" = " + new DataTable().Compute(String.Join("", s), ""));
        }
    }
}
