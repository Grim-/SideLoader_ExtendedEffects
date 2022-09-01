using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SideLoader_ExtendedEffects.Released.Conditions
{
    public class SL_IsWeatherCondition : SL_EffectCondition, ICustomModel
    {
        public Type SLTemplateModel => typeof(SL_IsWeatherCondition);
        public Type GameModel => typeof(IsWeatherCondition);

        public int WeatherType;
        public override void ApplyToComponent<T>(T component)
        {
            IsWeatherCondition comp = component as IsWeatherCondition;
            comp.WeatherType = (SLEX_Weather) WeatherType;
        }

        public override void SerializeEffect<T>(T component)
        {
            IsWeatherCondition comp = component as IsWeatherCondition;
            this.WeatherType = (int)comp.WeatherType;
        }
    }

    public class IsWeatherCondition : EffectCondition
    {
        public SLEX_Weather WeatherType;

        public override bool CheckIsValid(Character _affectedCharacter)
        {
            ExtendedEffects.Instance.DebugLogMessage($"IsWeatherCondition WeatherType : {WeatherType}  result : IsRaining: {WeatherManagerNew.IsRaining} IsSnowing: {WeatherManagerNew.IsSnowing} ");

            switch (WeatherType)
            {
                case SLEX_Weather.CLEAR:
                if (!WeatherManagerNew.IsRaining && !WeatherManagerNew.IsSnowing)
                {
                    return true;
                }
                return false;

                case SLEX_Weather.RAINING:
                return WeatherManagerNew.IsRaining;

                case SLEX_Weather.SNOWING:
                return WeatherManagerNew.IsSnowing;
            }

            return false;
        }
    }

    public enum SLEX_Weather
    {
        CLEAR = 0,
        RAINING = 1,
        SNOWING = 2
    }
}
