using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.Storage; 
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace At_Before.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class AboutMe : Page
    {
        public AboutMe()
        {
            this.InitializeComponent();
        }

        private void Page_Load(object sender, RoutedEventArgs e)
        {
        }

        private void myScroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

            if (ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 2))
            {
                // Get a referece to a "propertyset" that contains the following keys
                //  Translation (Vector3)
                //  CenterPoint (Vector3)
                //  Scale (Vector3)
                //  Matrix (Matrix4x4)
                // that represent the state of the scrollview at any moment (i.e. as the user manipulates the scrollviewer with mouse, touch, touchpad)

                CompositionPropertySet scrollerManipProps = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(myScroller);

                Compositor compositor = scrollerManipProps.Compositor;

                // Create the expression
                ExpressionAnimation expression = compositor.CreateExpressionAnimation("scroller.Translation.Y * parallaxFactor");

                // wire the ParallaxMultiplier constant into the expression
                expression.SetScalarParameter("parallaxFactor", 0.3f);

                // set "dynamic" reference parameter that will be used to evaluate the current position of the scrollbar every frame
                expression.SetReferenceParameter("scroller", scrollerManipProps);

                // Get the background image and start animating it's offset using the expression
                Visual backgroundVisual = ElementCompositionPreview.GetElementVisual(background);
                backgroundVisual.StartAnimation("Offset.Y", expression);
            }
        }
    }
}
