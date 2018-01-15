using System;
using System.Collections.Generic;

namespace DungeonsAndDiscord
{
    [Serializable]
    public class Character
    {
        String name;
        Dictionary<String, Stat> stats = new Dictionary<String, Stat>();
        String picture;
        Notes notes;

        public Character(String s)
        {
            name = s;
            notes = new Notes();
        }
        public String getName() { return name; }
        public void setName(String s) { name = s; }
        public Stat getStat(String s)
        {
            if(stats.ContainsKey(s))
                return stats[s];
            return null;
        }
        public String addStat(String s, int i, int m=-1000)
        {
            if(stats.ContainsKey(s))
            {
                stats[s].setName(s); stats[s].setStat(i);
                if (m == -1000)
                    stats[s].setMod(i);
                else stats[s].setMod(m);
                return "Stat " + s + " has been updated for "+name+".";
            }
            stats.Add(s, new Stat(s, i, m));
            return "Stat " + s + " has been added to " + name + ".";
        }
        public String removeStat(String s)
        {
            if(stats.ContainsKey(s))
            {
                stats.Remove(s);
                return "Stat " + s + " has been removed from " + name + ".";
            }
            return "Stat " + s + " does not exist for " + name + ".";
        }
        public Notes getNotes()
        {
            return notes;
        }
    }
}