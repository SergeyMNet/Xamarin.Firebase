using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alice.Models;
using Xamarin.Forms;

namespace Alice.Controls
{
    public class SessionCell : ViewCell
    {
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (Device.OS == TargetPlatform.iOS)
            {
                var session = (ChatMessage) BindingContext;
                this.Height = 80;

                if (session.Text.Length > 0)
                {
                    var len = session.Text.Length * 1.2;

                    if (len > 80)
                        this.Height = len;
                }
            }
        }

    }
}
