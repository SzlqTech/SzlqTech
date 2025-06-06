using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;
using SzlqTech.Core.Consts;

namespace SzlqTech.Core.Services.Datapage
{
    public class PagedInputDto : IPagedResultRequest
    {
        [Range(1, AppSharedConsts.MaxPageSize)]
        public int MaxResultCount { get; set; }

        [Range(0, int.MaxValue)]
        public int SkipCount { get; set; }

        public PagedInputDto()
        {
            MaxResultCount = AppSharedConsts.DefaultPageSize;
        }
    }
}
