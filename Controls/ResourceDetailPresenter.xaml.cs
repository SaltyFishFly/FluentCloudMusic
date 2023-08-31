using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class ResourceDetailPresenter : UserControl
    {
        public static DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(ResourceDetailPresenter), new PropertyMetadata(string.Empty));
        public static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ResourceDetailPresenter), new PropertyMetadata(string.Empty));
        public static DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ResourceDetailPresenter), new PropertyMetadata(string.Empty));

        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public UIElement CoverImage { get => CoverImageContainer; }

        public ResourceDetailPresenter()
        {
            InitializeComponent();
        }
    }
}
