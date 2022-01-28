using MySleeper.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace MySleeper
{
    public class DBContext
    {
        static SQLiteConnection connection;
        static SQLiteCommand command;
        static public bool Connect()
        {
            try
            {
                connection = new SQLiteConnection("Data Source=MySleeper.info;Version=3; FailIfMissing=False");
                connection.Open();
                return true;
            }
            catch (SQLiteException)
            {
                //Console.WriteLine($"Ошибка доступа к базе данных. Исключение: {ex.Message}");
                return false;
            }
        }
        public static SettingsTimers GetSettingsTimers()
        {
            SettingsTimers st = new();
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "select ST_TEXT, ST_TIME, ST_TIMER from SettingsTimers"
                };
                DataTable data = new();
                SQLiteDataAdapter adapter = new(command);
                adapter.Fill(data);
                st.ST_TEXT = data.Rows[0].Field<string>("ST_TEXT");
                st.ST_TIME = data.Rows[0].Field<long>("ST_TIME");
                st.ST_TIMER = data.Rows[0].Field<long>("ST_TIMER");
                adapter.Dispose();
                data.Clear();
                connection.Close();
            }
            
            return st;
        }

        public static SettingsProgramm GetSettingsProgramm()
        {
            SettingsProgramm sp = new();
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "select SP_ID, SP_AUTOSTART from SettingProgramm"
                };
                DataTable data = new();
                SQLiteDataAdapter adapter = new(command);
                adapter.Fill(data);
                sp.SP_ID = data.Rows[0].Field<long>("SP_ID");
                sp.SP_AUTOSTART = data.Rows[0].Field<bool>("SP_AUTOSTART");
                adapter.Dispose();
                data.Clear();
                connection.Close();
            }

            return sp;
        }

        public static List<TypeTrening> GetTypeTrening()
        {
            List<TypeTrening> result = new();
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "select TT_ID, TT_NAME from TypeTrening"
                };
                DataTable data = new();
                SQLiteDataAdapter adapter = new(command);
                adapter.Fill(data);
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {

                        result.Add(new TypeTrening { TT_ID = row.Field<long>("TT_ID"), TT_NAME = row.Field<string>("TT_NAME") });
                    }
                }
                
                adapter.Dispose();
                data.Clear();
                connection.Close();
            }
            return result;
        }

        public static List<SettingsTrenings> GetSettingsTrenings()
        {
            List<SettingsTrenings> result = new();
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "select STR_ID, STR_TYPE, STR_DATE, STR_NUMWORKOUT, STR_NUMREPEAT from SettingsTrenings"
                };
                DataTable data = new();
                SQLiteDataAdapter adapter = new(command);
                adapter.Fill(data);
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {

                        result.Add(new SettingsTrenings { STR_ID = row.Field<long>("STR_ID"), STR_TYPE = row.Field<long>("STR_TYPE"), STR_DATE = row.Field<DateTime>("STR_DATE"), STR_NUMWORKOUT = row.Field<long>("STR_NUMWORKOUT"), STR_NUMREPEAT = row.Field<long>("STR_NUMREPEAT") });
                    }
                }

                adapter.Dispose();
                data.Clear();
                connection.Close();
            }
            return result;
        }

        public static SettingsVisuals GetSettingsVisuals()
        {
            SettingsVisuals sv = new();
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "select SV_FONTCOLOR, SV_BACKCOLOR, SV_BACKGROUNDisEnabled,SV_BACKGROUND,SV_OPACITY,SV_TIMERROTATEIMG from SettingsVisuals"
                };
                DataTable data = new();
                SQLiteDataAdapter adapter = new(command);
                adapter.Fill(data);
                sv.SV_FONTCOLOR = data.Rows[0].Field<string>("SV_FONTCOLOR");
                sv.SV_BACKCOLOR = data.Rows[0].Field<string>("SV_BACKCOLOR");
                sv.SV_BACKGROUNDisEnabled = data.Rows[0].Field<bool>("SV_BACKGROUNDisEnabled");
                sv.SV_BACKGROUND = data.Rows[0].Field<long>("SV_BACKGROUND");
                sv.SV_OPACITY = Convert.ToDouble(data.Rows[0].Field<string>("SV_OPACITY"));
                sv.SV_TIMERROTATEIMG = data.Rows[0].Field<long>("SV_TIMERROTATEIMG");
                adapter.Dispose();
                data.Clear();
                connection.Close();
            }

            return sv;
        }
        public static List<BackGroundImages> GetBackGroundImages()
        {
            List<BackGroundImages> sbg = new();
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "select BG_ID, BG_DATA, BG_isEnable from BackGroundImages"
                };
                DataTable data = new();
                SQLiteDataAdapter adapter = new(command);
                adapter.Fill(data);
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        sbg.Add(new BackGroundImages { BG_ID = row.Field<Int64>("BG_ID"), BG_DATA = row.Field<byte[]>("BG_DATA"), BG_isEnable = row.Field<bool>("BG_isEnable")});
                    }
                }
                adapter.Dispose();
                data.Clear();
                connection.Close();
            }
            return sbg;
        }

        public static void RemoveImg(long id)
        {
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = $"delete from BackGroundImages where BG_ID = {id}"
                };
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void SetSettingsTimers(string Text, int Time, int Timer)
        {
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = $"update SettingsTimers set ST_TEXT = '{Text}', ST_TIME = {Time}, ST_TIMER = {Timer}"
                };
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void SetSettingsVisuals(string FontColor, string BackColor, double Opacity,bool BACKGROUNDisEnabled, int TimerRotaleImages)
        {
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = $"update SettingsVisuals set SV_FONTCOLOR = '{FontColor}', SV_BACKCOLOR = '{BackColor}', SV_OPACITY = '{Opacity}', SV_BACKGROUNDisEnabled = '{BACKGROUNDisEnabled}', SV_TIMERROTATEIMG = {TimerRotaleImages}"
                };
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void SetBackGroundImages(byte[] Text)
        {
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "update BackGroundImages set BG_DATA = @img"
                };
                command.Parameters.Add("@img", DbType.Binary).Value = Text;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void SetInsertBackGroundImages(byte[] Text)
        {
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = "insert into BackGroundImages (BG_DATA, BG_isEnable) VALUES (@img, 1)"
                };
                command.Parameters.Add("@img", DbType.Binary).Value = Text;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void SetSettingsTrenings(long Type, string Date, int NumRepeat, int CountRepeat)
        {
            if (Connect())
            {
                command = new SQLiteCommand(connection)
                {
                    CommandText = $"insert into SettingsTrenings (STR_TYPE, STR_DATE, STR_NUMWORKOUT, STR_NUMREPEAT) VALUES ({Type}, '{Date}', {NumRepeat}, {CountRepeat})"
                };
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
