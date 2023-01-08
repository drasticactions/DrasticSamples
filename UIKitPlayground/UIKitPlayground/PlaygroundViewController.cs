#if !TVOS
using System;
using Drastic.PureLayout;

namespace UIKitPlayground
{
    public class PlaygroundViewController : UIViewController
    {
        UISwitch switchButton = new UISwitch() { Title = "Test" };

        public PlaygroundViewController()
        {
            this.SetupUI();
            this.SetupLayout();
        }

        public void SetupUI()
        {
            this.View.AddSubview(this.switchButton);
        }

        public void SetupLayout()
        {
            this.switchButton.AutoCenterInSuperview();
        }
    }
}
#endif