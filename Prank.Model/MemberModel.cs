using System;
using System.Collections.Generic;
using System.Text;

namespace Prank.Model
{
    public class MemberModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string MemberGuid { get; set; }
        public string MiddleName { get; set; }

        public string Password { get; set; }
        public string UserRole { get; set; }
    }
    public class PrankMemberModel
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
