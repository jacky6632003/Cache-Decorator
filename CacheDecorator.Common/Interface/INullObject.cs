using System;

namespace CacheDecorator.Common.Interface
{
    /// <summary>
    /// 定義 Null Object 模式方法。
    /// </summary>
    public interface INullObject
    {
        /// <summary>
        /// 確認此執行個體是否為 Null。
        /// </summary>
        /// <returns>
        /// 此執行個體是 Null 則為 <c>true</c>，否則為 <c>false</c>。
        /// </returns>
        bool IsNull();
    }
}