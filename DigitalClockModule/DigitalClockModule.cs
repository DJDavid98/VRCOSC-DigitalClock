using VRCOSC.Game.Modules;

namespace DigitalClockModule
{
    public partial class DigitalClockModule : Module
    {
        public override string Title => "Digital Clock";
        public override string Description => "Maps your local time to float parameters for use in a digital clock";
        public override string Author => "DJDavid98";
        public override string Prefab => "VRCOSC-DigitalClock";
        public override ModuleType Type => ModuleType.General;
        protected override TimeSpan DeltaUpdate => TimeSpan.FromMilliseconds(500);

        private const float animationFrameCount = 60f;

        protected override void CreateAttributes()
        {
            CreateSetting(DigitalClockSetting.Mode, "Mode", "Whether the clock should be in 12-hour or 24-hour mode", DigitalClockMode.Twelve);

            CreateParameter<bool>(DigitalClockParameter.Enabled, ParameterMode.Write, "VRCOSC/DigitalClock/Enabled", "Enabled", "Whether this module is attempting to emit values");
            CreateParameter<float>(DigitalClockParameter.Hours, ParameterMode.Write, "VRCOSC/DigitalClock/Hours", "Hours", "The current hour 0-59 mapped to a float");
            CreateParameter<float>(DigitalClockParameter.Minutes, ParameterMode.Write, "VRCOSC/DigitalClock/Minutes", "Minutes", "The current minute 0-59 mapped to a float");
            CreateParameter<float>(DigitalClockParameter.Seconds, ParameterMode.Write, "VRCOSC/DigitalClock/Seconds", "Seconds", "The current second 0-59 mapped to a float");
        }

        protected override void OnModuleStart()
        {
            SendParameter(DigitalClockParameter.Enabled, true);
            base.OnModuleStart();
        }

        protected override void OnModuleUpdate()
        {
            var time = DateTime.Now;

            // Add partial time to next largest unit to ensure timely animation state stransition
            // This helps keep the clock accurate even with slower DeltaUpdate times
            var seconds = time.Second + (time.Millisecond / 1000f);
            var minutes = time.Minute + (seconds / 60f);
            var hours = time.Hour + (minutes / 60f);
            var isTwelveHour = GetSetting<DigitalClockMode>(DigitalClockSetting.Mode) == DigitalClockMode.Twelve;

            float hourNormalised = (isTwelveHour ? hours % 12f : hours) / animationFrameCount;
            var minuteNormalised = minutes / animationFrameCount;
            var secondNormalised = seconds / animationFrameCount;


            SendParameter(DigitalClockParameter.Enabled, true);
            SendParameter(DigitalClockParameter.Hours, RescaleFloat(hourNormalised));
            SendParameter(DigitalClockParameter.Minutes, RescaleFloat(minuteNormalised));
            SendParameter(DigitalClockParameter.Seconds, RescaleFloat(secondNormalised));
        }

        // Converts the float from [0.0;1.0] range to [-0.983;0.983]
        // Avoids reaching -1 or +1 to prevent undesired animation states
        private float RescaleFloat(float input)
        {
            return Math.Clamp(input * 2f - 1f, -0.983f, 0.983f);
        }

        protected override void OnModuleStop()
        {
            SendParameter(DigitalClockParameter.Enabled, false);
            base.OnModuleStop();
        }

        private enum DigitalClockParameter
        {
            Enabled,
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
