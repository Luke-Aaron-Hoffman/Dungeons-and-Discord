using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDiscord
{
    [Serializable]
    public class Stat
    {
        private string name;
        private int stat;
        private int modifier;

        public Stat(string n, int s, int m = -1000)
        {
            name = n;
            stat = s;
            if (m == 1000)
                modifier = s;
            else modifier = m;
        }

        public int getMod()
        {
            if (name == "prof")
                return stat;
            if (stat >= 10)
                return (int)(stat - 10) / 2;
            else return (int)(stat - 11) / 2;
        } //currently the formula is for 5e
        public int getStat(){ return stat; }
        public string getName(){return name;}
        public void setMod(int m) { modifier = m; }
        public void setStat(int s) { stat = s; }
        public void setName(string s) { name = s; }
        public override String ToString()
        {
            return stat.ToString();
        }
    }
}
