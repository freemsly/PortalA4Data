using System.Collections.Generic;
using CamprayPortal.Web.Framework.Mvc;

namespace CamprayPortal.Web.Framework
{
    public class Pagination<T> where T : BaseNopEntityModel
    {
        private const int Splitindex = 2;

        public Pagination()
        {
            PageItemList = new List<int>();
        }

        public Pagination(IList<T> modelItem, int currentIndex, int totalPage) : this()
        {
            this.ModelItem = modelItem;
            this.CurrentIndex = currentIndex;
            this.TotalPage = totalPage;
            const int maxpagelist = Splitindex*2 + 1;

            PrevIndex = currentIndex - 1 >= 1 ? currentIndex - 1 : 1;
            NextIndex = totalPage - currentIndex >= 1 ? currentIndex + 1 : totalPage;

            if (totalPage <= maxpagelist)
            {
                for (var i = 0; i < totalPage; i++)
                {
                    PageItemList.Add(i + 1);
                }
                FirstIndex = 1;
                LastIndex = totalPage;
            }
            else
            {

                var vircurrentIndex = 0;

                if (currentIndex - Splitindex <= 0)
                    vircurrentIndex = Splitindex + 1;
                else if (currentIndex + Splitindex > totalPage)
                    vircurrentIndex = totalPage - Splitindex;
                else
                    vircurrentIndex = currentIndex;

                var prespitindex = vircurrentIndex - Splitindex;
                if (prespitindex <= 1)
                {
                    for (var i = 0; i < Splitindex; i++)
                    {
                        PageItemList.Add(i + 1);
                    }
                    FirstIndex = 1;
                }
                else
                {
                    for (var i = prespitindex; i < vircurrentIndex; i++)
                    {
                        PageItemList.Add(i);
                    }
                    FirstIndex = prespitindex - 1;
                }

                var nextspitindex = vircurrentIndex + Splitindex;
                if (nextspitindex >= totalPage)
                {
                    for (var i = vircurrentIndex; i <= totalPage; i++)
                    {
                        PageItemList.Add(i);
                    }
                    LastIndex = totalPage;
                }
                else
                {
                    for (var i = vircurrentIndex; i <= nextspitindex; i++)
                    {
                        PageItemList.Add(i);
                    }
                    LastIndex = nextspitindex + 1;
                }

            }
        }

        public IList<T> ModelItem { get; set; }

        public IList<int> PageItemList { get; set; }

        public int CurrentIndex { get; set; }

        public int TotalPage { get; set; }

        public int FirstIndex { get; set; }
        public int PrevIndex { get; set; }
        public int NextIndex { get; set; }
        public int LastIndex { get; set; }
    }
}