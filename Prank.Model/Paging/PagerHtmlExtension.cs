//using Microsoft.AspNetCore.Mvc.ViewFeatures;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Mvc;

//namespace Prank.Model.Paging
//{
//    public static class PagerHtmlExtension
//    {
//        public static Pager Pager(this HtmlHelper helper, IPageableModel pagination,string pagerFunctionName)
//        {
//            return new Pager(pagination, helper.ViewContext, pagerFunctionName);
//        }
//        public static Pager Pager(this HtmlHelper helper, string viewDataKey,string pagerFunctionName)
//        {
//            var dataSource = helper.ViewContext.ViewData.Eval(viewDataKey) as IPageableModel;

//            if (dataSource == null)
//            {
//                throw new InvalidOperationException(string.Format("Item in ViewData with key '{0}' is not an IPagination.",
//                                                                  viewDataKey));
//            }
//            //For dynamic Pass  pagerFunctionName 
//            return helper.Pager(dataSource, pagerFunctionName);
//        }
//    }
//}
