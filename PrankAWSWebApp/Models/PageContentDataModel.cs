using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrankAWSWebApp.Models
{
    public class PageContentModelViewModel
    {
        public int PageContentId { get; set; }
        public string PageContentKey { get; set; }

       
        public string PageContent { get; set; }
        public bool IsActive { get; set; }

        public string AddedDate { get; set; }

        public string ModifiedDate { get; set; }
    }

    public class PageContentDataModel
    {
        public string statusCode { get; set; }

        public List<PageContentModelViewModel> responseObject { get; set; }
        public string message { get; set; }
    }

    //public class PageContentModel
    //{
    //    public int PageContentId { get; set; }
    //    public string PageContentKey { get; set; }
    //    public string PageContent { get; set; }
    //    public bool IsActive { get; set; }
    //}
}
