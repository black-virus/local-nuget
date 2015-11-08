using System;
using System.Reflection;
using LocalNuget.Core.Results;
using Xunit;
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace LocalNuget.Tests
{

    [Trait("", "Formatter tests")]
    public class FormatterTest
    {

        [Fact(DisplayName = "Output formatter - single object format")]
        public void OutputFormatSingleTest()
        {
            var exl = new ExampleChild
            {
                BaseId = 1,
                BaseTitle = "Base element",
                Amount = 23.5439,
                ChildId = 11,
                Complex = new ExampleComplex { Title = "Antoni", Value = 7 }
            };
            IOutputFormat formatter = new TextOutputFormat();
            var result = formatter.FormatSingle(exl);
            var expected = "Base ID: 1" + Environment.NewLine +
                           "Base title: Base element" + Environment.NewLine +
                           "Inner ID: 11" + Environment.NewLine +
                           "Amount: 23,54 zł" + Environment.NewLine +
                           "Complex:" + Environment.NewLine + "Antoni: 7,00";
            Assert.Equal(expected, result);
        }

        [Fact(DisplayName = "Output formatter - collection format")]
        public void OutputFormatCollectionTest()
        {
            var exls = new[] {
                    new ExampleChild {
                        BaseId = 1,
                        BaseTitle = "Base element",
                        Amount = 23.5439,
                        ChildId = 11,
                        Complex = new ExampleComplex { Title = "Antoni", Value = 7 }
                    },
                    new ExampleChild {
                        BaseId = 2,
                        BaseTitle = "Base element 2",
                        Amount = 6543,
                        ChildId = 21,
                        Complex = new ExampleComplex { Title = "Jarek", Value = 2 }
                    }};
            IOutputFormat formatter = new TextOutputFormat();
            var result = formatter.FormatArray(exls);
            var expected = "---------- # 1 # ----------" + Environment.NewLine +
                           "Base ID: 1" + Environment.NewLine +
                           "Base title: Base element" + Environment.NewLine +
                           "Inner ID: 11" + Environment.NewLine +
                           "Amount: 23,54 zł" + Environment.NewLine +
                           "Complex:" + Environment.NewLine + "Antoni: 7,00" + Environment.NewLine +
                           "---------- # 2 # ----------" + Environment.NewLine +
                           "Base ID: 2" + Environment.NewLine +
                           "Base title: Base element 2" + Environment.NewLine +
                           "Inner ID: 21" + Environment.NewLine +
                           "Amount: 6 543,00 zł" + Environment.NewLine +
                           "Complex:" + Environment.NewLine + "Jarek: 2,00";
            Assert.Equal(expected, result);
        }

        [Fact(DisplayName = "Output formatter - single object format with template format class")]
        public void OutputFormatSingleWithTemplateFormatInfoTest()
        {
            var exl = new CleanExampleChild
            {
                BaseId = 1,
                BaseTitle = "Base element",
                Amount = 23.5439,
                ChildId = 11,
                Complex = new CleanExampleComplex { Title = "Antoni", Value = 7 }
            };
            IOutputFormat formatter = new TextOutputFormat(new ExampleFormatProvider());
            var result = formatter.FormatSingle(exl);
            var expected = "ROOT ID: 1" + Environment.NewLine +
                           "BaseTitle: Base element" + Environment.NewLine +
                           "Inner ID: 11" + Environment.NewLine +
                           "Amount: 23,54 zł" + Environment.NewLine +
                           "Employ receive: Antoni: 7,00";
            Assert.Equal(expected, result);
        }

        private class ExampleBase
        {
            [FormatInfo(Title = "Base ID", Order = 1)]
            public int BaseId { get; set; }
            [FormatInfo(Title = "Base title", Order = 2)]
            public string BaseTitle { get; set; }
        }

        private class ExampleChild : ExampleBase
        {
            [FormatInfo(Title = "Inner ID", Order = 3)]
            public int ChildId { get; set; }
            [FormatInfo(Title = "Amount", Format = "{0:C2}", Order = 4)]
            public double Amount { get; set; }
            [FormatInfo(Display = PropertyDisplay.Block)]
            public ExampleComplex Complex { get; set; }
        }

        private class ExampleComplex
        {
            public string Title { private get; set; }
            public float Value { private get; set; }

            public override string ToString()
            {
                return $"{Title}: {Value:F2}";
            }
        }

        private class CleanExampleBase
        {
            public int BaseId { get; set; }
            public string BaseTitle { get; set; }
        }

        private class CleanExampleChild : CleanExampleBase
        {
            [FormatInfo(Order = 4, Title = "Inner ID")]
            public int ChildId { get; set; }
            [FormatInfo(Order = 5)]
            public double Amount { get; set; }
            [FormatInfo(Order = 10)]
            public CleanExampleComplex Complex { get; set; }
        }

        private class CleanExampleComplex
        {
            public string Title { private get; set; }
            public float Value { private get; set; }

            public override string ToString()
            {
                return $"{Title}: {Value:F2}";
            }
        }

        private class ExampleFormatProvider : IPropertyFormatInfoProvider
        {
            public PropertyFormat GetFormat(PropertyInfo property)
            {
                switch (property.Name)
                {
                    case "BaseId":
                        return new PropertyFormat { Title = "ROOT ID", Order = 1 };
                    case "BaseTitle":
                        return new PropertyFormat { Order = 2 };
                    case "Complex":
                        return new PropertyFormat { Title = "Employ receive", Display = PropertyDisplay.Inline };
                    case "Amount":
                        return new PropertyFormat { Format = "{0:C2}" };
                    default:
                        return null;
                }
            }
        }

    }
}
