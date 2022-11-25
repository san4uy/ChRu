using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using urlApp.Models;

namespace urlApp.Services
{
    internal class SoundService
    {
        private string _enco;
        private string _code;
        private MediaPlayer player = new MediaPlayer();
        public string Enco
        {
            get
            {
                return _enco;
            }
            set
            {
                _enco = value;
            }
        }
        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        public string GetPath ()
        {
            string st = AppDomain.CurrentDomain.BaseDirectory;
            /////del
            int f = st.LastIndexOf('\\');
            f = st.Remove(f, 1).LastIndexOf('\\');
            f = st.Remove(f, st.Length - f).LastIndexOf('\\');
            f = st.Remove(f, st.Length - f).LastIndexOf('\\');
            st = st.Remove(f, st.Length - f);
            return st.Insert(st.Length, "\\music.mp3");
        }

        public string GetFolder()
        {
            string st = AppDomain.CurrentDomain.BaseDirectory;
            /////del
            int f = st.LastIndexOf('\\');
            f = st.Remove(f, 1).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            f = st.Remove(f, st.Length-f).LastIndexOf('\\');
            st = st.Remove(f, st.Length-f);
            st += "\\mp3\\";
            return st;
        }
        public void PlayWav()
        {
            
            ushort numchannels = 1;

            uint samplerate = 16000;
            uint numsamples = 16000*10;
            //FileStream f = new FileStream("a.wav", FileMode.Create);
            //BinaryWriter wr = new BinaryWriter(f);

            //wr.Write(Encoding.ASCII.GetBytes("RIFF"));
            //wr.Write(38 + numsamples * numchannels * 2);
            //wr.Write(Encoding.ASCII.GetBytes("WAVE"));
            //wr.Write(Encoding.ASCII.GetBytes("fmt "));
            //wr.Write(18);
            //wr.Write((short)1); // Encoding
            //wr.Write((short)numchannels); // Channels
            //wr.Write((int)(samplerate)); // Sample rate
            //wr.Write((int)(samplerate * 2)); // sampleRate * numChannels * bitsPerSample/8
            //wr.Write((short)(2)); // // Количество байт для одного сэмпла, включая все каналы.
            //wr.Write((int)(16)); // bits per sample //был short, но с интом заработало
            //wr.Write((Encoding.ASCII.GetBytes("data")));
            //wr.Write((int)(numsamples * 2)); // Extra size

            WavHeader wavHeader = new WavHeader(samplerate, numsamples);

            var headerSize = Marshal.SizeOf(wavHeader);
       
            var buffer = new byte[headerSize + wavHeader.Subchunk2Size];

            //fileStream.Read(buffer, 0, headerSize);

            // Чтобы не считывать каждое значение заголовка по отдельности,
            // воспользуемся выделением unmanaged блока памяти
            var headerPtr = Marshal.AllocHGlobal((int)(headerSize + wavHeader.Subchunk2Size));
            // Преобразовываем указатель на блок памяти к нашей структуре
            Marshal.StructureToPtr(wavHeader, headerPtr, true);
            // Копируем считанные байты из файла в выделенный блок памяти
            Marshal.Copy(headerPtr, buffer, 0, headerSize);
            for (int i = 0; i < numsamples; i++)
            {

                //wr.Write((short)(Math.Sin(2 * Math.PI * 4000 * i / samplerate)*5000));



                short tmp =  (short)(Math.Sin(2 * Math.PI * 4000 * i / samplerate)*5000);
                buffer[headerSize + i*2] = (byte)(tmp & 0xFF);
                buffer[headerSize + i*2 + 1] = (byte)((tmp >> 8) & 0xFF);
            }

            // Place the data into a stream
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                // Construct the sound player
                SoundPlayer player = new SoundPlayer(ms);
                player.PlayLooping();
            }
        }
        public void SaveSound(string word, string path, string language)
        {
            string UrlSound = GetUrlSound(word, language);
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate)))
            {
                writer.Write(System.Convert.FromBase64String(UrlSound));
            }
        }

        public string GetUrlSound(string str, string language)
        {
            var URL = "https://translate.google.com/_/TranslateWebserverUi/data/batchexecute?rpcids=jQ1olc&source-path=/&f.sid=6148368410739897732&bl=boq_translate-webserver_20221102.06_p0&hl=ru&soc-app=1&soc-platform=1&soc-device=1&_reqid=1869635&rt=c";
            var prefix = "[[[\"jQ1olc\",\"[\\\"";
            string suffix = "";
            string k = OrderLanguage.Chinese.ToString();

            if (language == OrderLanguage.Chinese.ToString())
            {
                suffix = "\\\",\\\"zh-CN\\\",null,\\\"undefined\\\"]\",null,\"generic\"]]]";
            }
            else if (language == OrderLanguage.English.ToString())
            {
                suffix = "\\\",\\\"en\\\",null,\\\"null\\\"]\",null,\"generic\"]]]";
            }
            else throw new Exception("Erro Enum Langugae");
            string responseInString;
            using (var wb = new WebClient())
            {
                var data = new NameValueCollection();
                data["f.req"] = prefix + str + suffix;
                //data["at"] = "ADiessZ-V9qcF5UZLzb6X_0QfMn-%3A1667665235141";

                var response = wb.UploadValues(URL, "POST", data);
                responseInString = Encoding.UTF8.GetString(response);
            }
            Regex regex = new Regex("(?<=\\[\\\\\").+(?=\\\\\"\\])"); //"(?<=\[\\").+(?=\\"\])"
            MatchCollection matches = regex.Matches(responseInString);
            return matches[0].ToString();
        }
        public void PlaySound(string word, string title)
        {
            string PATH = GetFolder() + title + "\\" + word + ".mp3";
            player.Open(new Uri(PATH, UriKind.RelativeOrAbsolute));
            player.Play();
        }
        public void CheckMp3Folder(ref DataModel dataModel)
        {
            string PATH = GetFolder() + dataModel.Title;
            if (!Directory.Exists(PATH)) Directory.CreateDirectory(PATH);
            string[] files = System.IO.Directory.GetFiles(PATH).Select(f => Path.GetFileNameWithoutExtension(f)).ToArray();
            bool flg = false;
            foreach (var worddata in dataModel.WordDataList)
            {
                for (int x = 0; x < files.Length; x++)
                {
                    if(worddata.WChina == files[x])
                    {
                        flg = true;
                        break;
                    }
                }
                if (!flg)
                {
                    SaveSound(worddata.WChina, PATH + "\\" + worddata.WChina + ".mp3", dataModel.Language);
                }
            }

        }
    }
    [StructLayout(LayoutKind.Sequential)]
    internal class WavHeader
    {   
        public WavHeader(UInt32 samplerate, UInt32 numsamples)
        {
            SampleRate = samplerate;
            NumChannels = 1;
            //ByteRate = SampleRate*2;
            ByteRate = 0xEEEEEEEE;
            ChunkSize =  38 + numsamples * 2;
            Subchunk2Size = numsamples* 2;
        }
        // WAV-формат начинается с RIFF-заголовка:

        // Содержит символы "RIFF" в ASCII кодировке
        // (0x52494646 в big-endian представлении)
        public UInt32 ChunkId = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("RIFF"), 0);

        // 36 + subchunk2Size, или более точно:
        // 4 + (8 + subchunk1Size) + (8 + subchunk2Size)
        // Это оставшийся размер цепочки, начиная с этой позиции.
        // Иначе говоря, это размер файла - 8, то есть,
        // исключены поля chunkId и chunkSize.
        public UInt32 ChunkSize;

        public UInt32 Format = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("WAVE"), 0);         // Содержит символы "WAVE" (0x57415645 в big-endian представлении)
        // Формат "WAVE" состоит из двух подцепочек: "fmt " и "data":
        // Подцепочка "fmt " описывает формат звуковых данных:

        // Содержит символы "fmt "
        // (0x666d7420 в big-endian представлении)
        public UInt32 Subchunk1Id = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("fmt "), 0);
        // Это оставшийся размер подцепочки, начиная с этой позиции.
        public UInt32 Subchunk1Size = 16;
        // Аудио формат, полный список можно получить здесь http://audiocoding.ru/wav_formats.txt
        // Для PCM = 1 (то есть, Линейное квантование).
        // Значения, отличающиеся от 1, обозначают некоторый формат сжатия.
        public UInt16 AudioFormat = 1;

        // Количество каналов. Моно = 1, Стерео = 2 и т.д.
        public UInt16 NumChannels;

        // Частота дискретизации. 8000 Гц, 44100 Гц и т.д.
        public UInt32 SampleRate;

        // sampleRate * numChannels * bitsPerSample/8
        public UInt32 ByteRate;

        // numChannels * bitsPerSample/8
        // Количество байт для одного сэмпла, включая все каналы.
        public UInt16 BlockAlign = 2;

        // Так называемая "глубиная" или точность звучания. 8 бит, 16 бит и т.д.
        public UInt16 BitsPerSample = 16;

        // Подцепочка "data" содержит аудио-данные и их размер.

        // Содержит символы "data"
        // (0x64617461 в big-endian представлении)
        public UInt32 Subchunk2Id = BitConverter.ToUInt32(Encoding.ASCII.GetBytes("data"), 0);

        // numSamples * numChannels * bitsPerSample/8
        // Количество байт в области данных.
        public UInt32 Subchunk2Size;

        // Далее следуют непосредственно Wav данные.
    }
}
