using System;
using System.Collections.Generic;


namespace SV20T1080033.DomainModels
{
	/// <summary>
	/// 
	/// </summary>
	public class Category
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
	}
}
