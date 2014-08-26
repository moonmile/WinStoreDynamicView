using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using Windows.Web.Http;

// 空白ページのアイテム テンプレートについては、http://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace WinStoreDynamicView
{
    /// <summary>
    /// それ自体で使用できる空白ページまたはフレーム内に移動できる空白ページ。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            originalGrid = this.Content as Grid;
        }

        Grid originalGrid;

        /// <summary>
        /// XAML文字列からロードして遷移する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClickString(object sender, RoutedEventArgs e)
        {
            // var xaml = "<TextBlock FontSize=\"50\" Text=\"From XAML\" \"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" />";
            string xaml = "<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"><Ellipse Name=\"backbtn\" Width=\"300.5\" Height=\"200\" Fill=\"Red\" /></Grid>";
            var doc = XamlReader.Load(xaml) as Grid;
            this.Content = doc ;
            // 短いXAML文字列だと FindName できる？
            var el = doc.FindName("backbtn") as UIElement;
            el.Tapped += (_,__) =>
            {
                this.Content = originalGrid;
            };
        }

        private async void OnClickHttp(object sender, RoutedEventArgs e)
        {
            var url = "http://moonmile.net/up/samplepage.xaml";
            var cl = new HttpClient();
            var xaml = await cl.GetStringAsync(new Uri(url));
            /*
            var xaml = "";
            xaml += "<Grid xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\">\n";
            xaml += "        <Grid.RowDefinitions>\n";
            xaml += "            <RowDefinition Height=\"100\"/>\n";
            xaml += "            <RowDefinition Height=\"*\"/>\n";
            xaml += "        </Grid.RowDefinitions>\n";
            xaml += "        <TextBlock \n";
            xaml += "            FontSize=\"50\"\n";
            xaml += "            HorizontalAlignment=\"Left\" Margin=\"26,29,0,0\" TextWrapping=\"Wrap\" Text=\"dynamic loading page.\" VerticalAlignment=\"Top\"/>\n";
            xaml += "        <Button \n";
            xaml += "            FontSize=\"50\"\n";
            xaml += "            Content=\"back\" \n";
            xaml += "			HorizontalAlignment=\"Left\" \n";
            xaml += "			Margin=\"23,30,0,0\" \n";
            xaml += "			Grid.Row=\"1\" \n";
            xaml += "			VerticalAlignment=\"Top\"/>\n";
            xaml += "        <Ellipse Fill=\"#FFF4F4F5\" HorizontalAlignment=\"Left\" Height=\"239\" Margin=\"185,158,0,0\" Grid.Row=\"1\" Stroke=\"Black\" VerticalAlignment=\"Top\" Width=\"239\"/>\n";
            xaml += "        <Ellipse Fill=\"#FF2828BC\" HorizontalAlignment=\"Left\" Height=\"239\" Margin=\"452,158,0,0\" Grid.Row=\"1\" Stroke=\"Black\" VerticalAlignment=\"Top\" Width=\"239\"/>\n";
            xaml += "        <Ellipse Fill=\"#FFD52B61\" HorizontalAlignment=\"Left\" Height=\"239\" Margin=\"723,158,0,0\" Grid.Row=\"1\" Stroke=\"Black\" VerticalAlignment=\"Top\" Width=\"239\"/>\n";
            xaml += "</Grid>\n";
             */
            var doc = XamlReader.Load(xaml) as Grid;
            this.Content = doc;
            // 戻るボタン
            // HTTP からロードしたときは x:Name がスルーされない？
            // var el = doc.FindName("backbtn") as UIElement;
            var el = doc.Children.First<UIElement>( t => { 
                var btn = t as Button;
                return (btn != null && ((string)btn.Content)=="back" )? true : false;
            });
            el.Tapped += (_, __) =>
            {
                this.Content = originalGrid;
            };
            // マウスオーバーで色を変える
            // var ell = doc.FindName("ellipse") as Ellipse;
            // var sb1 = doc.FindName("sb1") as Storyboard;
            // ell.PointerEntered += (_, __) => sb1.Begin();
            // ell.PointerExited += (_, __) => sb1.Stop();
        }
    }
}
