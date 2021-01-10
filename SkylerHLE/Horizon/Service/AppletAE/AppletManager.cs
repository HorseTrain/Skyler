using SkylerHLE.Horizon.Service.Sessions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkylerHLE.Horizon.Service.AppletAE
{
    public enum AppletMessage
    {
        FocusStateChanged = 15
    }

    public class AppletManager
    {
        //TODO:

        public KEvent MessegeSendEvent      { get; set; }

        List<AppletMessage> Messages        { get; set; }

        public bool InFocus                 { get; set; } //Maybe use an enum instead.

        public AppletManager()
        {
            MessegeSendEvent = new KEvent();
            Messages = new List<AppletMessage>();
        }

        public void SetFocus(bool focused)
        {
            InFocus = focused;

            PushMessage(AppletMessage.FocusStateChanged);
        }

        void PushMessage(AppletMessage message)
        {
            Messages.Add(message);

            MessegeSendEvent.Send();
        }

        public AppletMessage PopMessage()
        {
            AppletMessage Out = Messages[Messages.Count - 1];

            Messages.RemoveAt(Messages.Count - 1);

            return Out;
        }
    }
}
