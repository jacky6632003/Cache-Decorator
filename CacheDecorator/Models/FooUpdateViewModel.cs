using System;
using System.ComponentModel.DataAnnotations;

namespace CacheDecorator.Models
{
    /// <summary>
    /// Class FooUpdateViewModel.
    /// </summary>
    public class FooUpdateViewModel
    {
        /// <summary>
        /// the Foo id.
        /// </summary>
        [Required(ErrorMessage = "FooId 為必要資料")]
        public Guid FooId { get; set; }

        /// <summary>
        /// the name.
        /// </summary>
        [Required(ErrorMessage = "名稱為必填資料")]
        [StringLength(maximumLength: 50, ErrorMessage = "字串長度不可超過 50")]
        public string Name { get; set; }

        /// <summary>
        /// the description.
        /// </summary>
        [Required(ErrorMessage = "描述為必填資料")]
        [StringLength(maximumLength: 100, ErrorMessage = "字串長度不可超過 100")]
        public string Description { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool Enable { get; set; }
    }
}