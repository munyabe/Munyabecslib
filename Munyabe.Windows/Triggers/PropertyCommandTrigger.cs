using System.ComponentModel;
using System.Windows;

namespace Munyabe.Windows.Triggers
{
    /// <summary>
    /// プロパティが特定の値になったときにコマンドを実行するトリガーです。
    /// </summary>
    public class PropertyCommandTrigger : CommandTriggerBase
    {
        /// <summary>
        /// <see cref="Property"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty PropertyProperty = DependencyProperty.Register(
            "Property",
            typeof(DependencyProperty),
            typeof(PropertyCommandTrigger),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// <see cref="Value"/>依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(string),
            typeof(PropertyCommandTrigger),
            new FrameworkPropertyMetadata(null));

        /// <summary>
        /// コマンド実行のトリガーになるプロパティを取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public DependencyProperty Property
        {
            get { return (DependencyProperty)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        /// <summary>
        /// コマンド実行のトリガーになるプロパティの値を取得または設定します。これは、依存関係プロパティです。
        /// </summary>
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 新しいインスタンスを作成します。
        /// </summary>
        protected override Freezable CreateInstanceCore()
        {
            return new PropertyCommandTrigger();
        }

        /// <summary>
        /// トリガーを初期化する処理です。
        /// </summary>
        protected override void InitializeCore(FrameworkElement source)
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(Property, source.GetType());
            descriptor.AddValueChanged(source, (s, e) =>
            {
                object value = Value;

                var converter = descriptor.Converter;
                if (converter != null && converter.CanConvertFrom(typeof(string)))
                {
                    value = converter.ConvertFromString(Value);
                }

                if (Equals(source.GetValue(Property), value))
                {
                    ExecuteCommand();
                }
            });
        }
    }
}