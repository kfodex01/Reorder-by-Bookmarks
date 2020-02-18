using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reorder_by_Bookmarks
{
    class Bookmark
    {
        private readonly string name;
        private long startPage;
        private long endPage;
        private Bookmark previousNode;
        private Bookmark nextNode;
        private long priority;
        private readonly long index;
        private Dictionary<String, object> myDictionary;

        public Bookmark(string name, long startPage, Bookmark previousNode, long index, Dictionary<string, object> myDictionary)
        {
            this.name = name;
            this.startPage = startPage;
            this.previousNode = previousNode;
            this.index = index;
            SetDictionary(myDictionary);
            endPage = -1;
            nextNode = null;
            priority = -1;
        }

        public void SetDictionary(Dictionary<string, object> myDictionary)
        {
            this.myDictionary = CopyDictionary(myDictionary);
        }

        private Dictionary<string, object> CopyDictionary(Dictionary<string, object> thisDictionary)
        {
            Dictionary<string, object> newDictionary = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> element in thisDictionary)
            {
                if (element.Value.Equals(typeof(IList<Dictionary<string, object>>)))
                {
                    newDictionary.Add(element.Key, CopyIList((IList<Dictionary<string, object>>)element.Value));
                }
                else
                {
                    newDictionary.Add(element.Key, element.Value);
                }
            }
            return newDictionary;
        }

        private IList<Dictionary<string, object>> CopyIList(IList<Dictionary<string, object>> thisIList)
        {
            IList<Dictionary<string, object>> newIList = new List<Dictionary<string, object>>();

            foreach (Dictionary<string, object> element in thisIList)
            {
                newIList.Add(CopyDictionary(element));
            }

            return newIList;
        }

        public string GetName()
        {
            return name;
        }
        public long GetStartPage()
        {
            return startPage;
        }
        public long GetEndPage()
        {
            return endPage;
        }
        public Bookmark GetPreviousNode()
        {
            return previousNode;
        }
        public Bookmark GetNextNode()
        {
            return nextNode;
        }
        public long GetPriority()
        {
            return priority;
        }
        public long GetIndex()
        {
            return index;
        }
        public Dictionary<string, object> GetDictionary()
        {
            return myDictionary;
        }
        public void SetStartPage(long startPage)
        {
            this.startPage = startPage;
        }
        public void SetEndPage(long endPage)
        {
            this.endPage = endPage;
        }
        public void SetPreviousNode(Bookmark previousNode)
        {
            this.previousNode = previousNode;
        }
        public void SetNextNode(Bookmark nextNode)
        {
            this.nextNode = nextNode;
        }
        public void SetPriority(long priority)
        {
            this.priority = priority;
        }
        public void ChangePage(long page)
        {
            if (!myDictionary.TryGetValue("Page", out object thisItem))
            {
                return;
            }
            try
            {
                string thisString = (string)thisItem;
                string[] pageString = thisString.Split(' ');
                string newPageString = "" + page;
                for (int i = 1; i < pageString.Length; i++)
                {
                    newPageString += " " + pageString[i];
                }
                myDictionary.Remove("Page");
                myDictionary.Add("Page", newPageString);
            }
            catch
            {
                return;
            }
            if (myDictionary.TryGetValue("Kids", out object theseKidsObject))
            {
                try
                {
                    IList<Dictionary<string, object>> theseKids = (IList<Dictionary<string, object>>)theseKidsObject;
                    foreach (Dictionary<string, object> element in theseKids)
                    {
                        if (element.TryGetValue("Page", out object thisElementItem))
                        {
                            try
                            {
                                string thisString = (string)thisElementItem;
                                string[] pageString = thisString.Split(' ');
                                string newPageString = "" + page;
                                for (int i = 1; i < pageString.Length; i++)
                                {
                                    newPageString += " " + pageString[i];
                                }
                                element.Remove("Page");
                                element.Add("Page", newPageString);
                            }
                            catch
                            {

                            }
                        }
                    }
                }
                catch
                {
                    return;
                }
            }

        }

        public bool AddKid(Dictionary<string, object> thisKid)
        {
            if (myDictionary.ContainsKey("Kids"))
            {
                myDictionary.TryGetValue("Kids", out object myKidsObject);
                try
                {
                    IList<Dictionary<string, object>> myKids = (IList<Dictionary<string, object>>)myKidsObject;
                    myKids.Add(thisKid);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                IList<Dictionary<string, object>> myKids = new List<Dictionary<string, object>>
                {
                    thisKid
                };
                myDictionary.Add("Kids", myKids);
                return true;
            }
        }
    }
}
