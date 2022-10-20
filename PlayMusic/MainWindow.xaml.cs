using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Un4seen.Bass;

namespace PlayMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int stream;
        int atual = 0;
        string path;
        string[] filesMusic;

        public MainWindow()
        {
            path = @"C:\repo\PlayMusic\PlayMusic\bin\Debug\net6.0-windows\audios";
            string[] arquivos = Directory.GetFiles(path);
            filesMusic = new string[arquivos.Length];

            for (int i = 0; i < arquivos.Length; i++)
            {
                //obtendo somente o nome do arquivo.
                string[] name = arquivos[i].Split(@"\");

                filesMusic[i] = name[name.Length - 1];
            }

            InitializeComponent();

            musicList.ItemsSource = filesMusic;
        }

        private void Button_anterior(object sender, RoutedEventArgs e)
        {
            // free the stream
            Bass.BASS_StreamFree(stream);
            // free BASS
            Bass.BASS_Free();

            bool primeiroItem = atual == 0;
            if (primeiroItem)
            {
                NVAccess.NVDA.Speak("Primeiro item da lista de músicas.");
            }
            else
            {
                string proxima = filesMusic[atual - 1];
                atual = atual - 1;

                NVAccess.NVDA.Speak($"Reprodusindo: {proxima}");
                playFronArray(path + @"\" + proxima);
            }
        }

        private void Button_proxima(object sender, RoutedEventArgs e)
        {
            // free the stream
            Bass.BASS_StreamFree(stream);
            // free BASS
            Bass.BASS_Free();

            bool ultimoItem = atual == filesMusic.Length - 1;
            if (ultimoItem)
            {
                NVAccess.NVDA.Speak("Último item da lista de músicas.");
            }
            else
            {
                string proxima = filesMusic[atual + 1];
                atual = atual + 1;

                NVAccess.NVDA.Speak($"Reprodusindo: {proxima}");
                playFronArray(path + @"\" + proxima);
            }

        }

        private void CheckBox_PausaReprodus(object sender, System.EventArgs e)
        {
            CheckBox AutoClip = sender as CheckBox;

            if (Convert.ToBoolean(AutoClip.IsChecked))
            {
                Bass.BASS_ChannelPause(stream);
            }
            else
            {
                Bass.BASS_ChannelPlay(stream, false);
            }

        }

        private void ListView_SelectionChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var MusicSelected = (CollectionView)CollectionViewSource.GetDefaultView(musicList.SelectedItems);

                // free the stream
                Bass.BASS_StreamFree(stream);
                // free BASS
                Bass.BASS_Free();

                playFronArray(path + @"\" + MusicSelected.CurrentItem.ToString());
            }

        }

        private void playFronArray(string file)
        {
            if (Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                // create a stream channel from a file
                stream = Bass.BASS_StreamCreateFile(file, 0, 0, BASSFlag.BASS_DEFAULT);
                if (stream != 0)
                {
                    // play the stream channel
                    Bass.BASS_ChannelPlay(stream, false);
                }
                else
                {
                    // error creating the stream
                    Console.WriteLine("Stream error: {0}", Bass.BASS_ErrorGetCode());
                }
            }

        }

    }
}
