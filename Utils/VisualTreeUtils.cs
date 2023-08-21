using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FluentCloudMusic.Utils
{
    public static class VisualTreeUtils
    {
        public static DependencyObject FindChildByName(DependencyObject parent, string name)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement element && element.Name == name) return child;

                var result = FindChildByName(child, name);
                if (result != null) return result;
            }
            return null;
        }
    }
}
