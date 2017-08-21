using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
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

namespace SpeechRecognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer sSynth;
        PromptBuilder pBuilder;
        SpeechRecognitionEngine sRecognize;
        public MainWindow()
        {
            InitializeComponent();
            sSynth = new SpeechSynthesizer();
            pBuilder = new PromptBuilder();
            sRecognize = new SpeechRecognitionEngine();
        }

        private void BtSpeak_Click(object sender, RoutedEventArgs e)
        {
            pBuilder.ClearContent();
            pBuilder.AppendText(getRtbText());
            sSynth.Speak(pBuilder);
        }

        private string getRtbText()
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                richTextBox.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                richTextBox.Document.ContentEnd
             );
            return textRange.Text;
        }

        private void BtStart_Click(object sender, RoutedEventArgs e)
        {
            BtStart.IsEnabled = false;

            BtStart.IsEnabled = true;

            Choices sList = new Choices();
            sList.Add(new string[] { "hello","test","it works","whatever"});
            Grammar gr = new Grammar(new GrammarBuilder(sList));
            try
            {
                sRecognize.RequestRecognizerUpdate();
                sRecognize.LoadGrammar(gr);
                sRecognize.SpeechRecognized += sRecognize_SpeechRecognized ;
                sRecognize.SetInputToDefaultAudioDevice();
                sRecognize.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception)
            {

            }
        }

        private void sRecognize_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Text == "exit")
            {
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("speech recognized:" + e.Result.Text.ToString());
                richTextBox.AppendText(getRtbText() + " " + e.Result.Text.ToString());
            }
        }
    }
}
