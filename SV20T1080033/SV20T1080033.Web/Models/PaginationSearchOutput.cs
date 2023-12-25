using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SV20T1080033.Web.Models
{
    public abstract class PaginationSearchOutput
    {
        /// <summary>
        /// Số trang cần hiển thị
        /// </summary>
        public int Page { get; set; }
        /// <summary>
        /// Số dòng trên mỗi trang
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// Giá trị cần tìm kiếm
        /// </summary>
        public string SearchValue { get; set; }
        /// <summary>
        /// Đếm số dòng hiển thị
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// Tổng số trang
        /// </summary>
        public int PageCount
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }
                int page = RowCount / PageSize;
                if (RowCount % PageSize > 0)
                {
                    page += 1;
                }
                //return (int)Math.Ceiling((double)RowCount / PageSize);
                return page;
            }
        }


    }
}