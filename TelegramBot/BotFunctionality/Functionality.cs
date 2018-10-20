using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Net;
using System.Windows.Forms;
using SimpleJSON;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TelegramBot.BotFunctionality
{
    class Functionality
    {
        static string weatherApiKey = "362b1e92b2762434dbffcc7e86cff720", weatherUrl = "https://api.openweathermap.org/data/2.5/forecast?id=524901&APPID=";
        static WebClient web = new WebClient();

        public static JsonWeather GetWeather()
        {
            string responce = web.DownloadString("https://api.openweathermap.org/data/2.5/weather?q=moscow,ru&units=metric&appid=362b1e92b2762434dbffcc7e86cff720");

            JsonWeather weather = JsonConvert.DeserializeObject<JsonWeather>(responce);
            return weather;
        }
        public class JsonWeather 
        {
            public WeatherMainInfo main;
            public string name;
        }
        public class WeatherMainInfo
        {
            public float temp;
            public float temp_min;
            public float temp_max;
        }
        public class Weather
        {
            public Zero zero;
        }
        public class Zero
        {
            public string description;
        }

        public static FileStream MakeScreenShot() // Сделать скрин
        {
            Bitmap screenShot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics pic = Graphics.FromImage(screenShot);
            pic.CopyFromScreen(0, 0, 0, 0, screenShot.Size);
            screenShot.Save("screenshot.png");
            FileStream scr = File.Open("screenshot.png", FileMode.Open);
            return scr;
        }
    }
}
