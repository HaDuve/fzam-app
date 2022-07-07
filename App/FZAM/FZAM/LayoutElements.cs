using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FZAM
{
    public static class LayoutElements
    {

        public static Dictionary<string, double> fontSizes = new Dictionary<string, double>
        {
            {"small", 10},
            {"medium", 15},
            {"large", 20},
            {"default", 13}
        };

        public static Dictionary<string, double> minSizes = new Dictionary<string, double>
        {
            {"spacer", 1}
        };

        public static Dictionary<string, Dictionary<string, object>> elementSettings =
            new Dictionary<string, Dictionary<string, object>>
            {
                {
                    "defaultButton",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.Transparent},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["default"]}
                    }
                },
                {
                    "startButton",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.Transparent},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["large"]},
                        {"verticalOptions", LayoutOptions.Center}
                    }
                },
                {
                    "choiceOptionButton",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.FromHex("#ac8596")},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["medium"]}
                    }
                },
                {
                    "choiceSelectedOptionButton",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.FromHex("#d61726")},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["medium"]}
                    }
                },
                {
                    "navButton",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.Transparent},
                        {"textColor", Color.FromHex("#e8816f")},
                        {"fontSize", fontSizes["medium"]},
                        {"verticalOptions", LayoutOptions.EndAndExpand}
                    }
                },
                {
                    "questionActionsWrapper",
                    new Dictionary<string, object>
                    {
                        {"verticalOptions", LayoutOptions.CenterAndExpand}
                    }
                },
                {
                    "textQuestionWrapper",
                    new Dictionary<string, object>
                    {
                        {"verticalOptions", LayoutOptions.StartAndExpand}
                    }
                },
                {
                    "pageTitle",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.Transparent},
                        {"textColor", Color.Black},
                        {"fontSize", fontSizes["large"]}
                    }
                },
                {
                    "pageTitleSmall",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.Transparent},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["medium"]}
                    }
                },
                {
                    "HorizontalWrapper",
                    new Dictionary<string, object>
                    {
                        {"HorizontalOptions", LayoutOptions.CenterAndExpand},
                        {"Orientation", StackOrientation.Horizontal}
                    }
                },
                {
                    "ResultDataSectionTitle",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.FromHex("#e8816f")},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["medium"]},
                        {"FontAttributes", FontAttributes.Bold}
                    }
                },
                {
                    "ResultDataSectionSubtitle",
                    new Dictionary<string, object>
                    {
                        {"backgroundColor", Color.FromHex("#e8816f")},
                        {"textColor", Color.FromHex("#764a6b")},
                        {"fontSize", fontSizes["medium"]},
                        {"FontAttributes", FontAttributes.Bold}
                    }
                },
                {
                    "ResultDataSection",
                    new Dictionary<string, object>
                    {
                        {"Margin", new Thickness(15, 5, 5, 5)}
                    }
                },
                {
                    "VerticalSpacer",
                    new Dictionary<string, object>
                    {
                        {"MinWidth", minSizes["spacer"]}
                    }
                },
                {
                    "HorizontalSpacer",
                    new Dictionary<string, object>
                    {
                        {"MinHeight", minSizes["spacer"]}
                    }
                },
                {
                    "Hyperlink",
                    new Dictionary<string, object>
                    {
                        {"TextColor", Color.Blue}
                    }
                }
            };

        public static Color BarTextColor()
        {
            return Color.FromHex("#d61726");
        }

        public static Color BarBackgroundColor()
        {
            return Color.FromHex("#764a6b");
        }


        public static Label DefaultLabel(string text)
        {
            return new Label
            {
                Text = text
            };
        }

        public static StackLayout HorizontalWrapper()
        {
            return new StackLayout
            {
                HorizontalOptions = (LayoutOptions) elementSettings["HorizontalWrapper"]["HorizontalOptions"],
                Orientation = (StackOrientation) elementSettings["HorizontalWrapper"]["Orientation"]
            };
        }

        public static BoxView HorizontalSpacer(double width = 100)
        {
            return new BoxView
            {
                WidthRequest = width,
                HeightRequest = (double) elementSettings["HorizontalSpacer"]["MinHeight"]
            };
        }
    }
}