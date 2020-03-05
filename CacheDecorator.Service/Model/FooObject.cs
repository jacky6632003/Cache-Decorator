using System;

namespace CacheDecorator.Service.Model
{
    /// <summary>
    /// Class FooObject.
    /// </summary>
    public class FooObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FooObject"/> class.
        /// </summary>
        public FooObject()
        {
            this.FooId = Guid.Empty;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Enable = false;
            this.CreateTime = DateTime.UtcNow;
            this.UpdateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Foo Id.
        /// </summary>
        public Guid FooId { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否使用 (default: false)
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