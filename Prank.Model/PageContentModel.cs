using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.Model
{
    public class PageContentModel
    {
        public int PageContentId { get; set; }
        public string PageContentKey { get; set; }
        public string PageContent { get; set; }
        public bool IsActive { get; set; }
    }
}
