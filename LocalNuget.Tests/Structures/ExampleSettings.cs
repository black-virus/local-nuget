namespace LocalNuget.Tests.Structures
{
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class ExampleSettings
    {
        public int Setting1 { get; set; }
        public string Setting2 { get; set; }
        public int[] Settings3 { get; set; }
        public ExampleSettingsInner Settings4 { get; set; }
        public ExampleSettingsInner[] Settings5 { get; set; }

        public class ExampleSettingsInner
        {
            public int InnerSetting1 { get; set; }
            public string InnerSetting2 { get; set; }
            public int[] InnerSettings3 { get; set; }
        }

    }
    // ReSharper restore UnusedAutoPropertyAccessor.Global
}