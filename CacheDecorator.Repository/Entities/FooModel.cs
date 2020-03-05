using System;
using CacheDecorator.Common;
using CacheDecorator.Common.Interface;
using MessagePack;

namespace CacheDecorator.Repository.Entities
{
    /// <summary>
    /// Class FooModel.
    /// </summary>
    [MessagePackObject]
    public class FooModel : INullObject
    {
        /// <summary>
        /// Id.
        /// </summary>
        [Key(0)]
        public Guid FooId { get; set; }

        /// <summary>
        /// 平台產品名稱
        /// </summary>
        [Key(1)]
        public string Name { get; set; }

        /// <summary>
        /// 平台產品描述
        /// </summary>
        [Key(2)]
        public string Description { get; set; }

        /// <summary>
        /// 是否使用 (default: false)
        /// </summary>
        [Key(3)]
        public bool Enable { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        [Key(4)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        [Key(5)]
        public DateTime UpdateTime { get; set; }

        //-----------------------------------------------------------------------------------------

        public virtual bool IsNull()
        {
            return false;
        }

        private static FooModel _null;

        public static FooModel Null
        {
            get
            {
                if (_null.EqualNull())
                {
                    _null = new NullFooModel();
                }

                return _null;
            }
        }

        private class NullFooModel : FooModel
        {
            public NullFooModel()
            {
                this.FooId = Guid.Empty;
                this.Name = string.Empty;
                this.Description = string.Empty;
                this.Enable = false;
                this.CreateTime = DateTime.MinValue;
                this.UpdateTime = DateTime.MinValue;
            }

            public override bool IsNull()
            {
                return true;
            }
        }
    }
}