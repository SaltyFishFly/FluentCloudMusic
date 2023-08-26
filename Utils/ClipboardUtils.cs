using Windows.ApplicationModel.DataTransfer;

namespace FluentCloudMusic.Utils
{
    public class ClipboardUtils
    {
        public static void SetText(string text)
        {
            var dataPackage = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Copy
            };
            dataPackage.SetText(text);
            Clipboard.SetContent(dataPackage);
        }
    }
}
