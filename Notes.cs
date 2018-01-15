using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonsAndDiscord
{
    [Serializable]
    public class Notes
    {
        Dictionary<String, List<FakeEmbed>> notebooks = new Dictionary<String, List<FakeEmbed>>();
        FakeEmbed add;
        public void addPage(String book, String page)
        {
            if (!notebooks.ContainsKey(book))
            {
                notebooks.Add(book, new List<FakeEmbed>());
            }
            add = new FakeEmbed();
            add.AddField(book + " Page " + (notebooks[book].Count + 1), page);
            notebooks[book].Add(add);
        }
        public String editPage(String book, int pageNum, String page)
        {
            if(!notebooks.ContainsKey(book))
            {
                return "There are no " + book + " notes for this character.";
            }
            if(notebooks[book].Count<pageNum)
            {
                return "There are only " + notebooks[book].Count + " pages for " + book + " notes.";
            }
            add = new FakeEmbed();
            add.AddField(book+" Page "+ pageNum, page);
            notebooks[book][pageNum] = add;
            return "Page " + pageNum + " for " + book + " notes has been changed.";
        }
        public FakeEmbed getPage(String book, int pageNum)
        {
            if (!notebooks.ContainsKey(book))
                return null;
            if (notebooks[book].Count < pageNum)
                return null;
                return notebooks[book][pageNum-1];
        }
        public List<FakeEmbed> getBook(String book)
        {
            if (!notebooks.ContainsKey(book))
                return null;
            return notebooks[book];
        }
    }
}
