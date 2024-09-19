namespace SmallShop.Services.Roles.Contracts.Dtos
{
    public class AddRoleDto
    {
        public AddRoleDto()
        {
            RoleName = string.Empty;
        }
        public string RoleName { get; set; }
    }
}
