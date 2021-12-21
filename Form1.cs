using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Google.Cloud.TextToSpeech.V1;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var credentialsFilePath = @".\credentials.json";

            var textToSpeechClientBuilder = new TextToSpeechClientBuilder()
            {
                CredentialsPath = credentialsFilePath
            };
            var client = textToSpeechClientBuilder.Build();

            // 読み上げテキストの設定
            SynthesisInput input = new SynthesisInput
            {
                Text = textBox1.Text
            };

            // 音声タイプの設定
            VoiceSelectionParams voice = new VoiceSelectionParams
            {
                Name = "ja-JP-Wavenet-D",
                LanguageCode = "ja-JP",
                SsmlGender = SsmlVoiceGender.Neutral
            };

            // オーディオ出力の設定
            AudioConfig config = new AudioConfig
            {
                AudioEncoding = AudioEncoding.Mp3,
                Pitch = -2.0
            };

            // Text-to-Speech リクエストの生成
            var response = client.SynthesizeSpeech(new SynthesizeSpeechRequest
            {
                Input = input,
                Voice = voice,
                AudioConfig = config
            });

            // Text-to-Speech レスポンス（音声ファイル）の保存
            var fileName = DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".mp3";
            using (Stream output = File.Create(fileName))
            {
                response.AudioContent.WriteTo(output);
                Console.WriteLine($"音声コンテンツを '{fileName}' として保存しました。");

            }
            Process.Start("explorer.exe", Directory.GetCurrentDirectory());
        }
    }
}
