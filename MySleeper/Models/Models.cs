using System;

namespace MySleeper.Models
{
    public class SettingsVisuals
    {
        public string SV_FONTCOLOR { get; set; }
        public string SV_BACKCOLOR { get; set; }
        public double SV_OPACITY { get; set; }
        public bool SV_BACKGROUNDisEnabled { get; set; }
        public long SV_BACKGROUND { get; set; }
        public long SV_TIMERROTATEIMG { get; set; }
    }

    public class BackGroundImages
    {
        public Int64 BG_ID { get; set; }
        public byte[] BG_DATA { get; set; }
        public bool BG_isEnable { get; set; }
    }

    public class SettingsProgramm
    {
        public long SP_ID { get; set; }
        public bool SP_AUTOSTART { get; set; }
    }

    public class SettingsTimers
    {
        public string ST_TEXT { get; set; }
        public long ST_TIME { get; set; }
        public long ST_TIMER { get; set; }
    }
    public class TypeTrening
    {
        public long TT_ID { get; set; }
        public string TT_NAME { get; set; }
    }

    public class SettingsTrenings
    {
        public long STR_ID;
        public long STR_TYPE;
        public DateTime STR_DATE;
        public long STR_NUMWORKOUT;
        public long STR_NUMREPEAT;
    }

    public class DataGridAddTraning
    {
        public string DateTr1 { get; set; }
        public string TimeTr1 { get; set; }
        public string TypeTr1 { get; set; }
        public long CountW1 { get; set; }
        public long CountR1 { get; set; }
    }
}
