using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DungeonsAndDiscord
{
    [Serializable]
    public class User
    {
        String user;
        //this is to "simulate commands" for use with input without args
        //First string is the channel, second thing is the message
        Dictionary<String, ulong> input = new Dictionary<String, ulong>();
        String command = "";
        Dictionary<String, Character> characters = new Dictionary<string, Character>();
        Character defaultChar = null;

        public User(String user)
        {
            this.user = user;
        }
        public String setDefault(String s)
        {
            if (characters.ContainsKey(s))
            {
                defaultChar = characters[s];
                return "Your default character is set to " + s + ".";
            }
            return "You have not created a character by the name of " + s + ".";
        }
        public Character getChar(String s)
        {
            if (characters.ContainsKey(s))
                return characters[s];
            return null;
        }
        public List<String> getCharacters()
        {
            return new List<String>(characters.Keys);
        }
        public Character getDefault() { return defaultChar; }
        public String addChar(String s)
        {
            if (characters.ContainsKey(s))
                return "A character by that name already exists.";
            characters.Add(s, new Character(s));
            return "Created character \"" + s + "\".";
        }
        public void removeChar(String s)
        {
            if (!characters.ContainsKey(s))
                return;
            if (defaultChar == characters[s])
                defaultChar = null;
            characters.Remove(s);
        }
        public String addStat(String cha, String s, int i, int m = -1000)
        {
            return characters[cha].addStat(s, i, m);
        }
        public Dictionary<String, ulong> getInput()
        {
            return input;
        }
        public void addInput(String c, ulong m)
        {
            input.Add(c, m);
        }
        public void removeInput(String c)
        {
            input.Remove(c);
        }
        public void setCommand(String s)
        { command = s; }
        public String getCommand()
        { return command; }
        public void add5eChar(String s)
        {
            Character temp = new Character(s);
            temp.addStat("Str", 10);
            temp.addStat("Dex", 10);
            temp.addStat("Con", 10);
            temp.addStat("Int", 10);
            temp.addStat("Wis", 10);
            temp.addStat("Cha", 10);
            temp.addStat("Prof", 2);
            characters.Add(s, temp);

        }
    }
}
