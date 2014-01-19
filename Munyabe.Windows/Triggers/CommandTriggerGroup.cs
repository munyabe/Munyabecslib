using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Munyabe.Windows.Triggers
{
    /// <summary>
    /// 複数のコマンド<see cref="ICommandTrigger"/>を保持するトリガーです。
    /// </summary>
    public sealed class CommandTriggerGroup : FreezableCollection<CommandTriggerBase>, ICommandTrigger
    {
        private readonly HashSet<ICommandTrigger> _initList = new HashSet<ICommandTrigger>();

        /// <summary>
        /// トリガーを初期化します。
        /// </summary>
        void ICommandTrigger.Initialize(FrameworkElement source)
        {
            foreach (ICommandTrigger child in this.Where(x => _initList.Contains(x) == false))
            {
                child.Initialize(source);
                _initList.Add(child);
            }
        }
    }
}