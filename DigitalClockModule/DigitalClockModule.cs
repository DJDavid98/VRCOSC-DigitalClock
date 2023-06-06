using VRCOSC.Game.Modules;

namespace DigitalClockModule
{
    public partial class DigitalClockModule : Module
    {
        public override string Title => "Digital Clock";
        public override string Description => "Maps your local time to 60-based floats for use in a digital clock";
        public override string Author => "DJDavid98";
        public override string Prefab => "VRCOSC-DigitalClock";
        public override ModuleType Type => ModuleType.General;
        protected override TimeSpan DeltaUpdate => TimeSpan.FromSeconds(1);

        protected override void CreateAttributes()
        {
            CreateSetting(DigitalClockSetting.Mode, "Mode", "Whether the clock should be in 12-hour or 24-hour mode", DigitalClockMode.Twelve);

            CreateParameter<float>(DigitalClockParameter.Hours, ParameterMode.Write, "VRCOSC/DigitalClock/Hours", "Hours", "The current hour 0-59 mapped to a float");
            CreateParameter<float>(DigitalClockParameter.Minutes, ParameterMode.Write, "VRCOSC/DigitalClock/Minutes", "Minutes", "The current minute 0-59 mapped to a float");
            CreateParameter<float>(DigitalClockParameter.Seconds, ParameterMode.Write, "VRCOSC/DigitalClock/Seconds", "Seconds", "The current second 0-59 mapped to a float");
        }

        protected override void OnModuleStart()
        {
            base.OnModuleStart();
        }

        protected override void OnModuleUpdate()
        {
            var time = DateTime.Now;

            var hours = time.Hour;
            var minutes = time.Minute;
            var seconds = time.Second;
            var isTwelveHour = GetSetting<DigitalClockMode>(DigitalClockSetting.Mode) == DigitalClockMode.Twelve;

            float hourNormalised = (isTwelveHour ? hours % 12f : hours) / 59f;
            var minuteNormalised = minutes / 59f;
            var secondNormalised = seconds / 59f;


            SendParameter(DigitalClockParameter.Hours, hourNormalised);
            SendParameter(DigitalClockParameter.Minutes, minuteNormalised);
            SendParameter(DigitalClockParameter.Seconds, secondNormalised);
        }

        protected override void OnModuleStop()
        {
            base.OnModuleStop();
        }

        private enum DigitalClockParameter
        {
            Hours,
            Minutes,
            Seconds
        }

        private enum DigitalClockSetting
        {
            Mode
        }

        private enum DigitalClockMode
        {
            Twelve,
            TwentyFour
        }
    }
}
