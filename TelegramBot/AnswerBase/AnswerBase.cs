using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Threading;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using ApiAiSDK;
using ApiAiSDK.Model;
using Google.Apis.Dialogflow.v2;

namespace TelegramBot.AnswerBase
{
    class AnswerBase
    {
        
        public static Responce FindAnswer(Telegram.Bot.Types.Message msg)
        {
            if (msg.Text.IndexOf("привет", StringComparison.OrdinalIgnoreCase) >= 0) return new Responce("Привет", AnswerType.TypesMsg.onlyTxt);
            else if (msg.Text.IndexOf("погода", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                BotFunctionality.Functionality.JsonWeather responcedWeather = BotFunctionality.Functionality.GetWeather();
                return new Responce($"сейчас в Москве {responcedWeather.main.temp}°C. Максимальная температура воздуха: {responcedWeather.main.temp_max}°C. " +
                    $"Минимальная: {responcedWeather.main.temp_min}°C.", AnswerType.TypesMsg.withName);
            }
            else if (msg.Text.IndexOf("время", StringComparison.OrdinalIgnoreCase) >= 0) return new Responce("Дата и время сервера: " + DateTime.UtcNow.ToString(), 
                AnswerType.TypesMsg.onlyTxt);
            else if (msg.Text.IndexOf("как зовут", StringComparison.OrdinalIgnoreCase) >= 0) return new Responce("Меня зовут бот Тет", AnswerType.TypesMsg.onlyTxt);
            else if (msg.Text.IndexOf("/screenshot 1234", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                object resp = BotFunctionality.Functionality.MakeScreenShot();
                return new Responce(resp, AnswerType.TypesMsg.photo);
            }
            else
            {
                try
                {
                    var request = Form1.ai.TextRequest(msg.Text.ToString());
                    string txt = request.Result.Fulfillment.Speech;
                    if (txt == "") txt = "Я не понимаю тебя:(";
                    return new Responce(txt, AnswerType.TypesMsg.onlyTxt);
                }
                catch (Exception e)
                {
                    return new Responce($"Не понимаю тебя:( {e.Message}", AnswerType.TypesMsg.onlyTxt);
                }
            }
        }
        public class Responce
        {
            public object responce;
            public AnswerType.TypesMsg type;
            public Responce(object res, AnswerType.TypesMsg t)
            {
                responce = res;
                type = t;
            }
        }
    }
}
