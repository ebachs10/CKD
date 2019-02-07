using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwinCAT.Ads;
using System.IO;

namespace Machine
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>

    public partial class Page1 : System.Windows.Controls.Page
    {
        private TcAdsClient tcClient;
        private AdsStream dataStream;
        private BinaryReader binReader;

        private int hEngine;
        private int hDeviceUp;
        private int hDeviceDown;
        private int hSteps;
        private int hCount;
        private int hSwitchNotify;
        private int hSwitchWrite;

        public Page1()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------
        //Wird als erstes beim Starten des Programms aufgerufen
        //Is activated first when the program is started
        //-----------------------------------------------------
        private void Load(object sender, EventArgs e)
        {
            try
            {
                // Eine neue Instanz der Klasse AdsStream erzeugen
                // Create a new instance of the AdsStream class
                dataStream = new AdsStream(31);

                // Eine neue Instanz der Klasse BinaryReader erzeugen
                // Create a new instance of the BinaryReader class
                binReader = new BinaryReader(dataStream);

                // Eine neue Instanz der Klasse TcAdsClient erzeugen
                // Create a new instance of the TcAdsClient class
                tcClient = new TcAdsClient();

                // Verbinden mit lokaler SPS - Laufzeit 1 - Port 801
                // Connecting to local PLC - Runtime 1 - Port 801
                tcClient.Connect(801);
            }
            catch
            {
                MessageBox.Show("Fehler beim Laden");
            }

            try
            {
                // Initialisieren der Überwachung der SPS-Variablen
                // Initializing the monitoring of the PLC variables
                hEngine = tcClient.AddDeviceNotification(".engine", dataStream, 0, 1, AdsTransMode.OnChange, 10, 0, null);
                hDeviceUp = tcClient.AddDeviceNotification(".deviceUp", dataStream, 1, 1, AdsTransMode.OnChange, 10, 0, null);
                hDeviceDown = tcClient.AddDeviceNotification(".deviceDown", dataStream, 2, 1, AdsTransMode.OnChange, 10, 0, null);
                hSteps = tcClient.AddDeviceNotification(".steps", dataStream, 0, 1, AdsTransMode.OnChange, 10, 0, null);
                hCount = tcClient.AddDeviceNotification(".count", dataStream, 4, 2, AdsTransMode.OnChange, 10, 0, null);
                hSwitchNotify = tcClient.AddDeviceNotification(".switch", dataStream, 6, 1, AdsTransMode.OnChange, 10, 0, null);

                // Holen des Handles von "switch" - wird für das Schreiben des Wertes benötigt
                // Getting the handle for "switch" - needed for writing the value
                hSwitchWrite = tcClient.CreateVariableHandle(".switch");

                // Erstellen eines Events für Änderungen an den SPS-Variablen-Werten 
                // Creating an event for changes of the PLC variable values
                tcClient.AdsNotification += new AdsNotificationEventHandler(tcClient_OnNotification);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------
        //wird bei Änderung einer SPS-Variablen aufgerufen
        //is activated when a PLC variable changes
        //------------------------------------------------
        private void tcClient_OnNotification(object sender, AdsNotificationEventArgs e)
        {
            try
            {
                // Setzen der Position von e.DataStream auf die des aktuellen benötigten Wertes
                // Setting the position of e.DataStream to the position of the current needed value
                e.DataStream.Position = e.Offset;

                // Ermittlung welche Variable sich geändert hat
                // Detecting which variable has changed
                if (e.NotificationHandle == hDeviceUp)
                {
                    //Die Farben der Grafiken entsprechened der Variablen anpassen
                    //Adapt colors of graphics according to the variables
                    if (binReader.ReadBoolean() == true)
                    {
                        DeviceUp_LED.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        DeviceUp_LED.Foreground = new SolidColorBrush(Colors.White);
                    }
                }
                else if (e.NotificationHandle == hDeviceDown)
                {
                    if (binReader.ReadBoolean() == true)
                    {
                        DeviceDown_LED.Foreground = new SolidColorBrush(Colors.Red);
                    }
                    else
                    {
                        DeviceDown_LED.Foreground = new SolidColorBrush(Colors.White);
                    }
                }
                else if (e.NotificationHandle == hSteps)
                {
                    // Einstellen der ProgressBar auf den aktuellen Schritt
                    // Setting the ProgressBar to the current step
                    prgSteps.Value = (binReader.ReadByte() * 4);
                }
                else if (e.NotificationHandle == hCount)
                {
                    // Anzeigen des "count"-Werts
                    // Displaying the "count" value
                    lblCount.Content = binReader.ReadUInt16().ToString();
                }
                else if (e.NotificationHandle == hSwitchNotify)
                {
                    // Markieren des korrekten RadioButtons
                    // Checking the correct RadioButton
                    if (binReader.ReadBoolean() == true)
                    {
                        optSpeedFast.IsChecked = true;
                    }
                    else
                    {
                        optSpeedSlow.IsChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------
        //wird aufgerufen, wenn das Feld 'schnell' markiert wird
        //is activated when the 'fast' field is marked
        //------------------------------------------------------
        private void optSpeedFast_Click(object sender, EventArgs e)
        {
            try
            {
                tcClient.WriteAny(hSwitchWrite, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------
        //wird aufgerufen, wenn das Feld 'langsam' markiert wird
        //is activated when the 'slow' field is marked
        //------------------------------------------------------
        private void optSpeedSlow_Click(object sender, EventArgs e)
        {
            try
            {
                tcClient.WriteAny(hSwitchWrite, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //------------------------------------------------------
        // wird beim Beenden des Programms aufgerufen
        // is activated when ending the program
        //------------------------------------------------------
        private void Close(object sender, EventArgs e)
        {
            try
            {
                // Löschen der Notifications und Handles
                // Deleting of the notifications and handles
                tcClient.DeleteDeviceNotification(hEngine);
                tcClient.DeleteDeviceNotification(hDeviceUp);
                tcClient.DeleteDeviceNotification(hDeviceDown);
                tcClient.DeleteDeviceNotification(hSteps);
                tcClient.DeleteDeviceNotification(hCount);
                tcClient.DeleteDeviceNotification(hSwitchNotify);

                tcClient.DeleteVariableHandle(hSwitchWrite);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            tcClient.Dispose();
        }
    }
}