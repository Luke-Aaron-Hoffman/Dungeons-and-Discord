using Discord;
using System;

namespace DungeonsAndDiscord
{
    [Serializable]
    public class FakeEmbed
    {
        String title;
        Object content;

        public void AddField(String title, Object content)
        { this.title = title; this.content = content; }
        public EmbedBuilder ConvertToEmbed()
        {
            EmbedBuilder b = new EmbedBuilder();
            b.AddField(title, content);
            return b;
        }

    }
}