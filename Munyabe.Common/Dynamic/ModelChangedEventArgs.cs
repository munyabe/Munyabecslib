using System;

namespace Munyabe.Common.Dynamic
{
    /// <summary>
    /// <c>Model</c>の変更時に発生するイベントで渡すデータです。
    /// </summary>
    public class ModelChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 変更された<c>Model</c>を取得または設定します。
        /// </summary>
        public object Model { get; set; }
    }
}
