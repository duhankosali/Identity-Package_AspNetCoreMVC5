using System.Collections.Generic;

namespace AspNetCore.Identity.Models
{
    public class RoleAssignListModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public bool Exist { get; set; } // İlgili rolün kullanıcıda olup olmadığını tutmak için kullandığımız property
    }

    public class RoleAssignSendModel
    {
        public List<RoleAssignListModel> Roles { get; set; }
        public int UserId { get; set; }
    }
}
