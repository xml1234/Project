using LTMCompanyName.YoyoCmsTemplate.Dtos;

namespace LTMCompanyName.YoyoCmsTemplate.Organizations.Dtos
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}