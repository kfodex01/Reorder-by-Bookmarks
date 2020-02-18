using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reorder_by_Bookmarks
{
    class BookmarkSorter
    {

        private readonly string path;
        private readonly bool prioritizeBookmarks;
        private readonly string[] priorityList;

        public BookmarkSorter(string path, bool prioritizeBookmarks, string[] priorityList)
        {
            this.path = path;
            this.prioritizeBookmarks = prioritizeBookmarks;
            this.priorityList = priorityList;
        }

        public string ReorderBookmarks()
        {
            string result = "";
            iTextSharp.text.pdf.PdfReader reader;
            string tempSavePath;
            System.IO.FileStream fs;
            iTextSharp.text.Document document;
            iTextSharp.text.pdf.PdfCopy copier;
            IList<Dictionary<string, object>> bookmarkList;
            Bookmark firstBookmark;
            SortedList<long, Bookmark> byStartPages;
            SortedList<long, Bookmark> byIndexNumbers;
            Bookmark thisBookmark;
            Bookmark previousBookmark;
            long totalPages;
            long currentPages;
            IList<Dictionary<string, object>> newBookmarkList;

            // check to see if file is open by another process
            string quickCheck = System.IO.Path.GetDirectoryName(path) + "\\~" + System.IO.Path.GetFileNameWithoutExtension(path) + System.IO.Path.GetExtension(path);
            long safeNumber = 1;
            while (System.IO.File.Exists(quickCheck))
            {
                quickCheck = System.IO.Path.GetDirectoryName(path) + "\\~" + System.IO.Path.GetFileNameWithoutExtension(path) + "(" + safeNumber + ")" + System.IO.Path.GetExtension(path);
                safeNumber += 1;
            }
            bool isSafe = false;
            while (!isSafe)
            {
                try
                {
                    System.IO.File.Move(path, quickCheck);
                    System.IO.File.Move(quickCheck, path);
                    isSafe = true;
                }
                catch
                {
                    System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.OKCancel;
                    System.Windows.Forms.DialogResult diagResult = System.Windows.Forms.MessageBox.Show(path + " is being used by another process. Please close that process and click Ok.", "File in Use", buttons);
                    if (diagResult == System.Windows.Forms.DialogResult.Cancel)
                    {
                        result = "File in use.";
                        return result;
                    }
                }
            }

            // initalizing reader
            reader = new iTextSharp.text.pdf.PdfReader(path);
            bookmarkList = iTextSharp.text.pdf.SimpleBookmark.GetBookmark(reader);
            totalPages = reader.NumberOfPages;

            // empty bookmark list
            if (bookmarkList == null)
            {
                reader.Close();
                System.Windows.Forms.MessageBox.Show("No bookmarks detected.");
                result = "Error";
                return result;
            }

            // get all bookmarks and add them to a list sorted by start pages and another list sorted by index numbers
            byStartPages = new SortedList<long, Bookmark>();
            byIndexNumbers = new SortedList<long, Bookmark>();
            long index = 0;
            AddBookmarkToSortedLists(bookmarkList, byStartPages, byIndexNumbers, index);

            // get end pages
            previousBookmark = null;
            foreach (KeyValuePair<long, Bookmark> element in byStartPages)
            {
                if (previousBookmark == null)
                {
                    previousBookmark = element.Value;
                }
                else
                {
                    previousBookmark.SetEndPage(element.Key - 1);
                    previousBookmark = element.Value;
                }
                element.Value.SetPriority(GetPriorityFromList(priorityList, element.Value.GetName()));
            }
            previousBookmark.SetEndPage(totalPages);

            // sort bookmarks by priority or sort by index
            if (prioritizeBookmarks)
            {
                firstBookmark = ReorderNodesByPriority(byIndexNumbers);
            }
            else
            {
                firstBookmark = ReorderNodesByIndex(byIndexNumbers);
            }

            // create a new pdf document
            newBookmarkList = new List<Dictionary<string, object>>();
            tempSavePath = System.IO.Path.GetDirectoryName(path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(path) + " - Sorted.pdf";
            long saveCount = 0;
            while (System.IO.File.Exists(tempSavePath))
            {
                saveCount++;
                tempSavePath = System.IO.Path.GetDirectoryName(path) + "\\" + System.IO.Path.GetFileNameWithoutExtension(path) + " - Sorted(" + saveCount + ").pdf";
            }
            fs = new System.IO.FileStream(tempSavePath, System.IO.FileMode.Create);
            document = new iTextSharp.text.Document();
            copier = new iTextSharp.text.pdf.PdfCopy(document, fs);
            document.Open();
            thisBookmark = firstBookmark;
            currentPages = 0;
            long firstBookmarkedPage = byStartPages.ElementAt(0).Key;
            if (firstBookmarkedPage > 1)
            {
                for (int i = 1; i < firstBookmarkedPage; i++)
                {
                    copier.AddPage(copier.GetImportedPage(reader, i));
                    currentPages++;
                }
            }
            while (thisBookmark != null)
            {
                int startPage = (int)thisBookmark.GetStartPage();
                int endPage = (int)thisBookmark.GetEndPage();
                for (int i = startPage; i <= endPage; i++)
                {
                    copier.AddPage(copier.GetImportedPage(reader, i));
                }
                if (startPage != currentPages + 1)
                {
                    thisBookmark.ChangePage(currentPages + 1);
                }
                currentPages += (endPage - startPage) + 1;
                newBookmarkList.Add(thisBookmark.GetDictionary());
                thisBookmark = thisBookmark.GetNextNode();
            }
            copier.Outlines = newBookmarkList;
            copier.Close();
            document.Close();
            fs.Close();
            reader.Close();

            reader = new iTextSharp.text.pdf.PdfReader(path);
            IList<Dictionary<string, object>> currentBookmarkList = iTextSharp.text.pdf.SimpleBookmark.GetBookmark(reader);
            reader.Close();

            // delete original and rename sorted file to original name
            if (totalPages != currentPages)
            {
                result = "The number of pages in the new document(s) does not match the number of pages in the old document. Aborting overwrite.";
                return result;
            }
            if (bookmarkList.Count != currentBookmarkList.Count)
            {
                result = "Bookmark count of original file does not match bookmark count of file being replaced. Aborting overwrite.";
                return result;
            }
            try
            {
                System.IO.File.Delete(path);
                System.IO.File.Move(tempSavePath, path);
            }
            catch
            {
                result = "Could not overwrite. Sorted bookmarks saved as " + tempSavePath;
            }

            return result;
        }

        private Bookmark ReorderNodesByIndex(SortedList<long, Bookmark> byIndexNumbers)
        {
            Bookmark firstBookmark = null;
            Bookmark lastBookmark = null;
            foreach (KeyValuePair<long, Bookmark> element in byIndexNumbers)
            {
                if (lastBookmark == null)
                {
                    firstBookmark = element.Value;
                    lastBookmark = firstBookmark;
                }
                else
                {
                    lastBookmark.SetNextNode(element.Value);
                    element.Value.SetPreviousNode(lastBookmark);
                    lastBookmark = element.Value;
                }
            }
            return firstBookmark;
        }

        private Bookmark ReorderNodesByPriority(SortedList<long, Bookmark> byIndexNumbers)
        {
            Bookmark firstBookmark = null;
            SortedList<long, Bookmark> byPriorities = new SortedList<long, Bookmark>();
            foreach (KeyValuePair<long, Bookmark> element in byIndexNumbers)
            {
                if (byPriorities.ContainsKey(element.Value.GetPriority()))
                {
                    byPriorities.TryGetValue(element.Value.GetPriority(), out Bookmark lastBookmark);
                    lastBookmark.SetNextNode(element.Value);
                    element.Value.SetPreviousNode(lastBookmark);
                    byPriorities.Remove(element.Value.GetPriority());
                    byPriorities.Add(element.Value.GetPriority(), element.Value);
                }
                else
                {
                    byPriorities.Add(element.Value.GetPriority(), element.Value);
                }
            }

            Bookmark lastPriorityBookmark = null;
            foreach (KeyValuePair<long, Bookmark> element in byPriorities)
            {
                Bookmark lastBookmark = element.Value;
                Bookmark firstInPriorityBookmark = lastBookmark;
                while (firstInPriorityBookmark.GetPreviousNode() != null)
                {
                    firstInPriorityBookmark = firstInPriorityBookmark.GetPreviousNode();
                }
                if (lastPriorityBookmark == null)
                {
                    lastPriorityBookmark = lastBookmark;
                    firstBookmark = firstInPriorityBookmark;
                }
                else
                {
                    lastPriorityBookmark.SetNextNode(firstInPriorityBookmark);
                    firstInPriorityBookmark.SetPreviousNode(lastPriorityBookmark);
                    lastPriorityBookmark = lastBookmark;
                }
            }
            return firstBookmark;
        }

        private void AddBookmarkToSortedLists(IList<Dictionary<string, object>> bookmarkList, SortedList<long, Bookmark> byStartPages, SortedList<long, Bookmark> byIndexNumbers, long index)
        {
            foreach (Dictionary<string, object> element in bookmarkList)
            {
                string name;
                long page;
                string tempString;
                IList<Dictionary<string, object>> tempKids = null;
                if (element.TryGetValue("Title", out object tempObject))
                {
                    try
                    {
                        name = (string)tempObject;
                    }
                    catch
                    {
                        name = "";
                    }
                }
                else
                {
                    name = "";
                }
                if (element.TryGetValue("Page", out tempObject))
                {
                    try
                    {
                        tempString = (string)tempObject;
                        string[] tempArray = tempString.Split(' ');
                        if (!long.TryParse(tempArray[0], out page))
                        {
                            page = -1;
                        }

                    }
                    catch
                    {
                        page = -1;
                    }
                }
                else
                {
                    page = -1;
                }

                if (element.TryGetValue("Kids", out tempObject))
                {
                    try
                    {
                        element.Remove("Kids");
                        tempKids = (IList<Dictionary<string, object>>)tempObject;
                    }
                    catch
                    {
                        tempKids = null;
                    }
                }
                if (!name.Equals("") && page > 0)
                {
                    if (byStartPages.ContainsKey(page))
                    {
                        byStartPages.TryGetValue(page, out Bookmark tempBookmark);
                        tempBookmark.AddKid(element);
                    }
                    else
                    {
                        index++;
                        Bookmark thisBookmark = new Bookmark(name, page, null, index, element);
                        byStartPages.Add(page, thisBookmark);
                        byIndexNumbers.Add(index, thisBookmark);
                    }
                }
                if (tempKids != null)
                {
                    AddBookmarkToSortedLists(tempKids, byStartPages, byIndexNumbers, index);
                }
            }
        }
        private long GetPriorityFromList(string[] targets, string name)
        {
            long result = -1;
            bool foundPriority = false;
            long i = 0;
            long j = 0;
            name = name.ToLower();
            string[] testArray = name.Split(' ');
            while (!foundPriority && i < testArray.Length)
            {
                while (!foundPriority && j < targets.Length)
                {
                    string secondTarget = targets[j] + "s";
                    string thirdTarget = targets[j] + "es";
                    if (testArray[i].Equals(targets[j]) || secondTarget.Equals(targets[j]) || thirdTarget.Equals(targets[j]))
                    {
                        result = j;
                        foundPriority = true;
                    }
                    j++;
                }
                i++;
                j = 0;
            }
            if (!foundPriority)
            {
                result = targets.Length;
            }
            return result;
        }
    }
}

