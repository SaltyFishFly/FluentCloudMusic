using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“内容对话框”项模板

namespace FluentCloudMusic.Controls
{
    public sealed partial class CommonDialog : ContentDialog
    {
        public new string Title { get; set; }
        public string Message { get; set; }
        public string ButtonText { get; set; }

        public CommonDialog(string title, string message, string buttonText)
        {
            InitializeComponent();
            Title = title;
            Message = message;
            ButtonText = buttonText;
        }
    }
}
