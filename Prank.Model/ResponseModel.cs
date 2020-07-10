using System;

namespace Prank.Model
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public Object ResponseObject { get; set; }
        public string Message { get; set; }
        
    }

    public class ReferralInfoInviteResponseModel:ResponseModel
    {
        public int Id { get; set; }
    }

    public class FilterModel
    {
        public string Token { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10000;
        public string Search { get; set; } = string.Empty;
        public string SortCol { get; set; } = string.Empty;
        public string SortDir { get; set; } = string.Empty;
    }
}
