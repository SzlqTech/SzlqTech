using Abp.Application.Services.Dto;
using SzlqTech.Core.Consts;


namespace SzlqTech.Core.Services.Datapage
{
    public class PagedAndSortedInputDto : PagedInputDto, ISortedResultRequest
    {
        public string Sorting { get; set; }

        public PagedAndSortedInputDto()
        {
            MaxResultCount = AppSharedConsts.DefaultPageSize;
        }
    }
}
