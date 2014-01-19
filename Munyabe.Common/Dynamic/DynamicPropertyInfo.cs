using System;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Munyabe.Common.Dynamic
{
    /// <summary>
    /// 動的に生成するプロパティの情報です。
    /// </summary>
    public class DynamicPropertyInfo
    {
        /// <summary>
        /// プロパティ名を取得または設定します。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// プロパティの型を取得または設定します。
        /// </summary>
        public Type PropertyType { get; set; }

        /// <summary>
        /// プロパティに付与する属性の<c>Builder</c>を取得または設定します。
        /// </summary>
        public IEnumerable<CustomAttributeBuilder> AttributeBuilders { get; set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="name">プロパティ名</param>
        /// <param name="propertyType">プロパティの型</param>
        /// <param name="attributeBuilders">プロパティに付与する属性の<c>Builder</c></param>
        public DynamicPropertyInfo(string name, Type propertyType, params CustomAttributeBuilder[] attributeBuilders)
        {
            Name = name;
            PropertyType = propertyType;
            AttributeBuilders = attributeBuilders;
        }
    }
}
