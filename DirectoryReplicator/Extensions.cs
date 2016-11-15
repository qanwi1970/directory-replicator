using System;
using System.Windows;
using System.Windows.Threading;

namespace DirectoryReplicator
{
    public static class Extensions
    {
        private static Action EmptyDelegate = delegate { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Background, EmptyDelegate);
        }
    }
}
