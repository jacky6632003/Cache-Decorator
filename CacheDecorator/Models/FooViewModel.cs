using System;
using System.ComponentModel.DataAnnotations;

namespace CacheDecorator.Models
{
    /// <summary>
    /// Class FooViewModel.
    /// </summary>
    public class FooViewModel
    {
        /// <summary>
        /// FooId.
        /// </summary>
        public Guid FooId { get; set; }

        /// <summary>
        /// the name.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// the description.
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}