using System;

using System.Windows;

using System.Windows.Forms;


using System.Threading;


namespace autoNGROK
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ip;
        string token;
        string webhook;

        public MainWindow()
        {

            Visibility = Visibility.Hidden;

            try
            {
                token = System.IO.File.ReadAllLines("settings.txt")[0].Substring(7);
                webhook = System.IO.File.ReadAllLines("settings.txt")[1].Substring(9);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Pas de fichier 'settings.txt' trouvé!");
                Environment.Exit(0);
            }       

            getIP(token);
            try
            {
                sendWebhook(webhook, "```Nouvelle IP: " + ip + "```", "Informations Serveur");
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Une Erreur est survenue. Ton Lien est-il invalide? As-tu une connection Internet???");
            }

            Environment.Exit(0);
        }

        public static void sendWebhook(string url, string msg, string username)
        {
            http.Post(url, new System.Collections.Specialized.NameValueCollection()
            {
                {
                    "username",
                    username

                },
                {
                    "content",
                    msg
                },


            });
        }
            
        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        public string getIP(string token)
        {
            try
            {
                SendKeys.SendWait("^{ESC}");
                Thread.Sleep(100);
                SendKeys.SendWait("cmd");
                Thread.Sleep(100);
                SendKeys.SendWait("{ENTER}");

                Thread.Sleep(500);

                SendKeys.SendWait("%{TAB}");
                Thread.Sleep(100);
                SendKeys.SendWait("%{TAB}");

                Thread.Sleep(200);

                System.Windows.Clipboard.SetText(@"C:\Windows\System32\ngrok.exe");
                

                SendKeys.SendWait("^v");
                SendKeys.SendWait("{ENTER}");

                Thread.Sleep(200);
                System.Windows.Clipboard.SetText("ngrok authtoken " + token);
                SendKeys.SendWait("^v");

                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(200);
                System.Windows.Clipboard.SetText("ngrok tcp 25565");
                SendKeys.SendWait("^v");
                SendKeys.SendWait("{ENTER}");

                Thread.Sleep(500);

                LeftMouseClick(500, 500);

                for (int i = 0; i < 35; i++)
                {
                    SendKeys.SendWait("+{UP}");
                }

                Thread.Sleep(500);
                SendKeys.SendWait("^c");

                ip = System.Windows.Forms.Clipboard.GetText().Substring(System.Windows.Forms.Clipboard.GetText().IndexOf("tcp://")).Split(char.Parse(" "))[0];
                return ip;
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Un problème est survenu! Vérifie ta Connection Innternet ou la présence de ngrok.exe dans le fichier système32. Assure-toi que tu ne touches pas à ton ordinateur lors du démarrage du serveur.");
                Environment.Exit(0);
                return "Une erreur est survenu.";

            }
            
        }

        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        public static void sendMessageToChannel()
        {

        }


    }
}
