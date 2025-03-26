using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Areas.Admin.Models
{
	public class UserRoleMapping
	{
		public string UserID { get; set; }
		public string RoleID { get; set; }
		public string UserName { get; set; }
		public string RoleName { get; set; }
	}
}
