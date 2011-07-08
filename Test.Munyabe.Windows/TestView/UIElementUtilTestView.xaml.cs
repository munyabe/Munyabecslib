using System.Windows.Controls;

namespace Test.Munyabe.Windows.TestView
{
    /// <summary>
    /// <see cref="UIElementUtilTest"/>でテストするビューです。
    /// </summary>
    public partial class UIElementUtilTestView : UserControl
    {
        public UIElementUtilTestView()
        {
            InitializeComponent();

            SubControl.Content = new UIElementUtilTestSubView();
        }
    }
}