﻿using SV20T1080033.DomainModels;

namespace SV20T1080033.Web.Models
{
    public class PaginationSearchCategory : PaginationSearchBaseResult
    {
        public IList<Category> Data { get; set; }
    }
}
