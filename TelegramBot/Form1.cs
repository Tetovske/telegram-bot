using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Drawing;
using System.Threading;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using System.IO;
using ApiAiSDK;
using ApiAiSDK.Model;
using Google.Apis.Dialogflow;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    public partial class Form1 : Form
    {
        private string dialogeFlowAPI = "37dd216cc86047d282318730c81db3a7";
        public static ApiAi ai;
        public static string path = @"C:\Users\Tet\Documents\VisualStudioprojects\TelegramBot\TelegramBot\AudioData\noga.wav";

        private string botApi = "558073501:AAEoPo8ovqD8vfAh2oJfBRFM5YsMeGVH0I4";
        TelegramBotClient bot;
        public delegate void Function();
        WebClient web;
        public static int currentPage;
        public static ReplyKeyboardMarkup userKeyboard;

        public enum Page : int
        {
            main,
            other
        }

        public Form1()
        {
            InitializeComponent();
            Initialization();
        }
        void Initialization()
        {
            web = new WebClient();
            AIConfiguration aiConfig = new AIConfiguration(dialogeFlowAPI, SupportedLanguage.Russian);
            ai = new ApiAi(aiConfig);
        }
        private async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Telegram.Bot.Types.Message msg = e.Message;
            await CheckMessage(msg);
        }

        private async void button_click(object sender, EventArgs e) // Запуск бота
        {
            start_button.Text = "Запускаю бота";
            start_button.ForeColor = Color.DarkOrange;
            start_button.Enabled = false;
            
            bot = new TelegramBotClient(botApi);
            bot.OnMessage += Bot_OnMessage;
            bot.StartReceiving();
            currentPage = (int)Page.main;
            userKeyboard = new ReplyKeyboardMarkup(new[]
            {
                new[]
                {
                    new KeyboardButton("Погода") { },
                    new KeyboardButton("Время")
                },
                new[]
                {
                    new KeyboardButton("Новости") { },
                    new KeyboardButton("Дата и время")
                }
            });

            start_button.Text = "Бот запущен";
            start_button.ForeColor = Color.Green;
        }

        private async Task CheckMessage(Telegram.Bot.Types.Message msg) // Проверяем тип сообщения (текстовое, стикер...)
        {
            AnswerType ans;
            //string answ;
            if(msg.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                ans = Answers.TextMessageRecieved(msg) as AnswerType;
                await SendMessage(ans);
            }
            else if(msg.Type == Telegram.Bot.Types.Enums.MessageType.Voice)
            {
                //SendMessage(ans);
            }

        }
        private async Task SendMessage(AnswerType ans)
        {
            switch(ans.type)
            {
                case AnswerType.TypesMsg.withName:
                    await bot.SendChatActionAsync(ans.msg.Chat.Id, ChatAction.Typing);
                    await Task.Delay(500);
                    await bot.SendTextMessageAsync(ans.msg.Chat.Id, $"{ans.msg.From.FirstName}, {ans.responce.ToString()}");
                    infoBox.Invoke(new Action(() =>
                    {
                        infoBox.Text += "[" + DateTime.UtcNow + "] " + ans.msg.From.FirstName + " " + ans.msg.From.LastName + " : " + ans.msg.Text + ans.msg.Location +"\r" + "\n" + ans.msg.From.Username;
                    }));
                    break;
                case AnswerType.TypesMsg.onlyTxt:
                    await bot.SendChatActionAsync(ans.msg.Chat.Id, ChatAction.Typing);
                    await Task.Delay(2000);
                    await bot.SendTextMessageAsync(ans.msg.Chat.Id, ans.responce.ToString());
                    infoBox.Invoke(new Action(() =>
                    {
                        infoBox.Text += "[" + DateTime.UtcNow + "] " + ans.msg.From.FirstName + " " + ans.msg.From.LastName + " : " + ans.msg.Text + ans.msg.Location + "\r" + "\n";
                    }));
                    break;
                case AnswerType.TypesMsg.photo:
                    FileStream sendPhoto = (FileStream)ans.responce;
                    await bot.SendPhotoAsync(ans.msg.Chat.Id, sendPhoto);
                    break;
            }
        }
    }
}
