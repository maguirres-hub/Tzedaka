using Tzedaka.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tzedaka.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCursos : ContentPage
    {
        public ViewCursos()
        {
            InitializeComponent();
            BindingContext = new ViewModel_Curso();
            string htmlContent = "<html><body<iframe src=\"https://drive.google.com/file/d/1hTo0PnsiChnQTAzgKeiJPvCh1kzu1zp9/preview\"></iframe></body></html>";
            webView.Source = new HtmlWebViewSource { Html = htmlContent };
        }
    }
}