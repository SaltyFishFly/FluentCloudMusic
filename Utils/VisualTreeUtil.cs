using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace FluentCloudMusic.Utils
{
    public static class VisualTreeUtil
    {
        public static DependencyObject FindChildByName(DependencyObject parent, string name)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is FrameworkElement elem && elem.Name == name) 
                    return child;

                var result = FindChildByName(child, name);
                if (result != null) 
                    return result;
            }
            return null;
        }
    }
}
