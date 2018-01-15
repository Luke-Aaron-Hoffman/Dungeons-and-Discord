using Discord.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDiscord.Modules
{
    public class HelperClass
    {
        Stream stream;
        BinaryFormatter formatter = new BinaryFormatter();
        public User deserialize(String s)
        {
            User u;
            if (File.Exists(s + ".bin"))
            {
                if (new FileInfo(s + ".bin").Length > 0)
                {
                    stream = new FileStream(s + ".bin",
                          FileMode.Open,
                          FileAccess.Read,
                          FileShare.Read);
                    u = (User)formatter.Deserialize(stream);
                    stream.Close();
                    return u;
                }
                else
                {
                    u = new User(s);
                    return u;
                }
            }
            else
            {
                stream = new FileStream(s + ".bin",
                    FileMode.Create);
                u = new User(s);
                stream.Close();
                return u;
            }
        }
        public void serialize(User u, String s)
        {
            stream = new FileStream(s + ".bin",
                FileMode.Open,
                FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, u);
            stream.Close();
        }
    }
}
