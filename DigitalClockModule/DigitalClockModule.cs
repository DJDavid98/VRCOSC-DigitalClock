using VRCOSC.Game.Modules;

namespace DigitalClockModule
{
    public partial class DigitalClockModule : Module
    {
        public override string Title => "DigitalClock";
        public override string Description => "DigitalClock description";
        public override string Author => "Author";
        public override ModuleType Type => ModuleType.General;
        protected override TimeSpan DeltaUpdate => TimeSpan.MaxValue;

        protected override void CreateAttributes()
        {
            CreateSetting(DigitalClockSetting.ExampleSetting, "Example Setting", "An example setting", string.Empty);
            CreateParameter<bool>(DigitalClockParameter.ExampleParameter, ParameterMode.ReadWrite, "ExampleParameterName", "Example Parameter Display Name", "This is an example parameter");
        }

        protected override void OnModuleStart()
        {
        }

        protected override void OnModuleUpdate()
        {
        }

        protected override void OnModuleStop()
        {
        }

        private enum DigitalClockSetting
        {
            ExampleSetting
        }

        private enum DigitalClockParameter
        {
            ExampleParameter
        }
    }
}
