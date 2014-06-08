// Worldstar Monitor
// Use: Conglomerates market data and helps one acquire information on the market quickly.
// By: juju
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Resources;
using System.Reflection;

namespace WorldStarMonitor
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        public Form1()
        {
            InitializeComponent();

            //Make box not editable
            textBox9.ReadOnly = true;

            int counter = 0;
            string line;

            // Read the file and display it line by line.
            System.IO.StreamReader file = new System.IO.StreamReader("marketlist.txt");
            while ((line = file.ReadLine()) != null)
            {
                this.listBox1.Items.Add(line.ToString());
                counter++;
            }
            file.Close();
            
            this.listBox5.Items.Clear();

            System.IO.StreamReader filec = new System.IO.StreamReader("coinlist.txt");
            while ((line = filec.ReadLine()) != null)
            {
                this.listBox5.Items.Add(line.ToString());
                counter++;
            }
            filec.Close();

            System.IO.StreamReader filet = new System.IO.StreamReader("totallist.txt");
            while ((line = filet.ReadLine()) != null)
            {
                this.listBox6.Items.Add(line.ToString());
                counter++;
            }
            filet.Close();
           for(int i=0; i<5; i++)
            {
                this.listBox8.Items.Add("0");
            }

            this.comboBox1.SelectedIndex = 0;
            textBox13.ReadOnly = true;
            textBox14.ReadOnly = true;
            textBox26.ReadOnly = true;
            textBox27.ReadOnly = true;
            textBox28.ReadOnly = true;
            //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "N8";
            // Set automatic scrolling 
            //chart2.ChartAreas[0].CursorX.AutoScroll = true;
            //chart2.ChartAreas[0].CursorY.AutoScroll = true;
            //textBox1.DeselectAll();
            //Make my donation box hidden so its a copyable label basically
            textBox9.BorderStyle = 0;
            textBox9.BackColor = this.BackColor;
            textBox9.TabStop = false;
            
            this.label13.Text = "Loading Data";

            this.label12.Text = "0000.00" +" USD";
            this.label2.Text = "0000.00" + " USD";
            
            this.label50.Text = "0";
            this.label51.Text = "0";
            this.label52.Text = "0";
            this.label53.Text = "0";
            this.label54.Text = "0";
            this.label55.Text = "0";
            this.label56.Text = "0";
            this.label57.Text = "0";
            this.label58.Text = "0";

            this.textBox3.Text = Properties.Settings.Default.dwallet;
            this.textBox4.Text = Properties.Settings.Default.dexchange;
            this.textBox10.Text = Properties.Settings.Default.ddonation;

            this.textBox17.Text = Properties.Settings.Default.bwallet;
            this.textBox16.Text = Properties.Settings.Default.bexchange;
            this.textBox15.Text = Properties.Settings.Default.bdonation;

            this.textBox31.Text = Properties.Settings.Default.lwallet;
            this.textBox30.Text = Properties.Settings.Default.lexchange;
            this.textBox29.Text = Properties.Settings.Default.ldonation;

            this.textBox20.Text = Properties.Settings.Default.a1;
            this.textBox19.Text = Properties.Settings.Default.a2;
            this.textBox18.Text = Properties.Settings.Default.a3;
            this.textBox25.Text = Properties.Settings.Default.a4;
            this.textBox23.Text = Properties.Settings.Default.a5;
            this.textBox22.Text = Properties.Settings.Default.a6;
            this.textBox21.Text = Properties.Settings.Default.a7;
            this.textBox24.Text = Properties.Settings.Default.a8;
            this.textBox35.Text = Properties.Settings.Default.a9;
            this.textBox36.Text = Properties.Settings.Default.a10;
            this.textBox37.Text = Properties.Settings.Default.a11;

            this.textBox46.Text = Properties.Settings.Default.mc1;
            this.textBox45.Text = Properties.Settings.Default.mc2;
            this.textBox44.Text = Properties.Settings.Default.mc3;
            this.textBox43.Text = Properties.Settings.Default.mc4;
            this.textBox48.Text = Properties.Settings.Default.mc5;
            this.textBox47.Text = Properties.Settings.Default.mc6;
            this.textBox49.Text = Properties.Settings.Default.mc7;
            this.textBox50.Text = Properties.Settings.Default.mc8;

            this.textBox51.Text = Properties.Settings.Default.mcn1;
            this.textBox52.Text = Properties.Settings.Default.mcn2;
            this.textBox54.Text = Properties.Settings.Default.mcn3;
            this.textBox53.Text = Properties.Settings.Default.mcn4;
            this.textBox56.Text = Properties.Settings.Default.mcn5;
            this.textBox55.Text = Properties.Settings.Default.mcn6;
            this.textBox58.Text = Properties.Settings.Default.mcn7;
            this.textBox57.Text = Properties.Settings.Default.mcn8;

            this.listBox1.SetSelected(0, true);
            //If the textbox for wallet addr is blank put in text
           
            restCalls test = new restCalls
            {
                coinbasevalue = "",
                btcevalue = "",
                time = "",
                label="LTCBTC"
            };
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker1_ProgressChanged);
            // Send argument to our worker thread
            backgroundWorker1.RunWorkerAsync(test);

            //Generate a timer that has an event handler of a tick interval of 10 seconds to update the application
            timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            timer.Interval = (100) * (1);             // Timer will tick evert 10 seconds
            timer.Enabled = true;                       // Enable the timer
            timer.Start();   
        }

        class restCalls
        {
            //Current selected market data return
            public string marketid { get; set; }
            public string label { get; set; }
            public string lasttradeprice { get; set; }
            public string volume { get; set; }
            public string lasttradetime { get; set; }
            public string primaryname { get; set; }
            public string primarycode { get; set; }
            public string secondaryname { get; set; }
            public string secondarycode { get; set; }
            public List<Recenttrade> recenttrades { get; set; }
            public List<Sellorder> sellorders { get; set; }
            public List<Buyorder> buyorders { get; set; }

            //Portfolio Data return
            public string ltcbtclasttradeprice { get; set; }
            public string dogebtclasttradeprice { get; set; }
            public string moonbtclasttradeprice { get; set; }
            public string ftcbtclasttradeprice { get; set; }
            public string frcbtclasttradeprice { get; set; }
            public string cgbbtclasttradeprice { get; set; }
            public string dvcbtclasttradeprice { get; set; }
            public string fortytwobtclasttradeprice { get; set; }
            public string tipsltclasttradeprice { get; set; }
            public string drkbtclasttradeprice { get; set; }
            public string lotbtclasttradeprice { get; set; }
            public string xpmbtclasttradeprice { get; set; }
            public string zetbtclasttradeprice { get; set; }
            public string frkbtclasttradeprice { get; set; }
            public string btbbtclasttradeprice { get; set; }

            //Coinbase and btce USD Btc price
            public string coinbasevalue { get; set; }
            public string btcevalue { get; set; }

            //Blockchain Rest Stuff
            public string totalbc { get; set; } //- https://blockchain.info/q/totalbc
            public string difficulty { get; set; }// - https://blockchain.info/q/getdifficulty
            public string rewardtotal { get; set; } //- https://blockchain.info/q/bcperblock
            public string blockheight { get; set; }// - https://blockchain.info/q/getblockcount
            public string hrprice { get; set; }// - https://blockchain.info/q/24hrprice
            public string hrtransactions { get; set; }// - https://blockchain.info/q/24hrtransactioncount
            public string hrbtcsent { get; set; }// - https://blockchain.info/q/24hrbtcsent
            public string nethashrate { get; set; } //- https://blockchain.info/q/hashrate
            public string marketcap { get; set; } //- https://blockchain.info/q/marketcap
            public string probability { get; set; } //- https://blockchain.info/q/probability
            public string addrbalance { get; set; } //- https://blockchain.info/q/addressbalance/
            //Time the last update occured
            public string time { get; set; }

        }
        public static void cmd_DataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Output from other process");
            Console.WriteLine(e.Data);
            //Form1.textBox61.Text = e.Data.ToString();
        }
        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        // The progress percentage is a property of e
            progressBar1.Value = e.ProgressPercentage;
            this.label59.Text = e.ProgressPercentage + "%";
        }
        public static void cmd_Error(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine("Error from other process");
            Console.WriteLine(e.Data);
        }
        public static void launchminer(string miningcommand)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo = new ProcessStartInfo("cmd.exe", "/c " + miningcommand);
            Process.Start(startInfo);
        }
        public static string InvokeStringMethod(string typeName, string methodName)
        {
            // Get the Type for the class
            Type calledType = Type.GetType(typeName);

            // Invoke the method itself. The string returned by the method winds up in s
            string s = (string)calledType.InvokeMember(
                            methodName,
                            BindingFlags.InvokeMethod | BindingFlags.Public |
                                BindingFlags.Static,
                            null,
                            null,
                            null);

            // Return the string that was returned by the called method.
            return s;
        }
        private string satoshify(string tobesatoshified)
        {
            var y = tobesatoshified;
            return y.ToString();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //
            // e.Argument always contains whatever was sent to the background worker
            // in RunWorkerAsync. We can simply cast it to its original type.
            //
            backgroundWorker1.ReportProgress(1);
            backgroundWorker1.ReportProgress(10);
            restCalls argumentTest = e.Argument as restCalls;
            //else make sure that they enter an address that is atleast the length of a real address
            backgroundWorker1.ReportProgress(15);

            //If the results returned from the function call were not complete or did not return correct data formatting I will have it return null so it wont crash the program trying to reference null DOGE MARKET VARIABLE structures /*
             /*
            string lastltcdogeprice = DOGELTC.@return.markets.DOGE.lasttradeprice;
                string lastltcdogevolume = DOGELTC.@return.markets.DOGE.volume;
                string lastltcdogetime = DOGELTC.@return.markets.DOGE.lasttradetime;
                */
            backgroundWorker1.ReportProgress(20);
            backgroundWorker1.ReportProgress(25);
            WorldStarMonitor.Form1.RootObject allmarketdata = REST_GET("api.php?method=marketdatav2");
            string currentMarketSelected = argumentTest.label;
            if (currentMarketSelected == "42BTC")
            {
                currentMarketSelected = "fortytwoBTC";
            }

            var property = allmarketdata.@return.markets.GetType().GetProperty(currentMarketSelected);
            dynamic market = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            
            argumentTest.label = market.label;
            argumentTest.lasttradeprice = market.lasttradeprice;
            argumentTest.lasttradetime = market.lasttradetime;
            argumentTest.marketid = market.marketid;
            argumentTest.primarycode = market.primarycode;
            argumentTest.primaryname = market.primaryname;
            argumentTest.recenttrades = market.recenttrades;
            argumentTest.secondarycode = market.secondarycode;
            argumentTest.secondaryname = market.secondaryname;
            argumentTest.sellorders = market.sellorders;
            argumentTest.volume = market.volume;
            argumentTest.buyorders = market.buyorders;
            
            //Pass/SET in the lasttradeprice of ALL currencies in the portfolio
            property = allmarketdata.@return.markets.GetType().GetProperty("LTCBTC");
            dynamic marketLTCBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.ltcbtclasttradeprice = marketLTCBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("DOGEBTC");
            dynamic marketDOGEBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.dogebtclasttradeprice = marketDOGEBTC.lasttradeprice;
            /*
            property = allmarketdata.@return.markets.GetType().GetProperty("MOONBTC");
            dynamic marketMOONBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.moonbtclasttradeprice = marketMOONBTC.lasttradeprice;
            */
   
            property = allmarketdata.@return.markets.GetType().GetProperty("FTCBTC");
            dynamic marketFTCBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.ftcbtclasttradeprice = marketFTCBTC.lasttradeprice;
            property = allmarketdata.@return.markets.GetType().GetProperty("FRCBTC");
            dynamic marketFRCBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.frcbtclasttradeprice = marketFRCBTC.lasttradeprice;
            
            property = allmarketdata.@return.markets.GetType().GetProperty("CGBBTC");
            dynamic marketCGBBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.cgbbtclasttradeprice = marketCGBBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("DVCBTC");
            dynamic marketDVCBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.dvcbtclasttradeprice = marketDVCBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("fortytwoBTC");
            dynamic marketfortytwoBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.fortytwobtclasttradeprice = marketfortytwoBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("DRKBTC");
            dynamic marketDRKBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.drkbtclasttradeprice = marketDRKBTC.lasttradeprice;
            
            
            property = allmarketdata.@return.markets.GetType().GetProperty("TIPSLTC");
            dynamic marketTIPSLTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.tipsltclasttradeprice = marketTIPSLTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("LOTBTC");
            dynamic marketLOTBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.lotbtclasttradeprice = marketLOTBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("XPMBTC");
            dynamic marketXPMBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.xpmbtclasttradeprice = marketXPMBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("ZETBTC");
            dynamic marketZETBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.zetbtclasttradeprice = marketZETBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("FRKBTC");
            dynamic marketFRKBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.frkbtclasttradeprice = marketFRKBTC.lasttradeprice;

            property = allmarketdata.@return.markets.GetType().GetProperty("BTBBTC");
            dynamic marketBTBBTC = property.GetMethod.Invoke(allmarketdata.@return.markets, null);
            argumentTest.btbbtclasttradeprice = marketBTBBTC.lasttradeprice;
            backgroundWorker1.ReportProgress(30);

            //If the results returned from the function call were not complete or did not return correct data formatting I will have it return null so it wont crash the program trying to reference null BTC MARKET VARIABLE structures

            /*
             *             public string totalbc { get; set; } //- https://blockchain.info/q/totalbc
            public string difficulty { get; set; }// - https://blockchain.info/q/getdifficulty
            public string rewardtotal { get; set; } //- https://blockchain.info/q/bcperblock
            public string blockheight { get; set; }// - https://blockchain.info/q/getblockcount
            public string hrprice { get; set; }// - https://blockchain.info/q/24hrprice
            public string hrtransactions { get; set; }// - https://blockchain.info/q/24hrtransactioncount
            public string hrbtcsent { get; set; }// - https://blockchain.info/q/24hrbtcsent
            public string nethashrate { get; set; } //- https://blockchain.info/q/hashrate
            public string marketcap { get; set; } //- https://blockchain.info/q/marketcap
            public string probability { get; set; } //- https://blockchain.info/q/probability
            public string addrbalance { get; set; } //- https://blockchain.info/q/addressbalance/
            */
            string nethashrate = REST_GET("/q/hashrate", "https://blockchain.info");
            argumentTest.nethashrate = nethashrate;
            backgroundWorker1.ReportProgress(35);
            string probability = REST_GET("/q/probability", "https://blockchain.info");
            argumentTest.probability = probability;
            backgroundWorker1.ReportProgress(40);
            string hrbtcsent = REST_GET("/q/24hrbtcsent", "https://blockchain.info");
            argumentTest.hrbtcsent = hrbtcsent;
            backgroundWorker1.ReportProgress(45);
            string hrtransactions = REST_GET("/q/24hrtransactioncount", "https://blockchain.info");
            argumentTest.hrtransactions = hrtransactions;
            backgroundWorker1.ReportProgress(50);
            string hrprice = REST_GET("/q/24hrprice", "https://blockchain.info");
            argumentTest.hrprice = hrprice;
            backgroundWorker1.ReportProgress(55);

            string blockheight = REST_GET("/q/getblockcount", "https://blockchain.info");
            argumentTest.blockheight = blockheight;
            backgroundWorker1.ReportProgress(60);
            string rewardtotal = REST_GET("/q/bcperblock", "https://blockchain.info");
            argumentTest.rewardtotal = satoshify(rewardtotal);
            backgroundWorker1.ReportProgress(65);
            string difficulty = REST_GET("/q/getdifficulty", "https://blockchain.info");
            argumentTest.difficulty = difficulty;
            string totalbc = REST_GET("/q/totalbc", "https://blockchain.info");
            argumentTest.totalbc = totalbc;
            backgroundWorker1.ReportProgress(70);
            string marketcap = REST_GET("/q/marketcap", "https://blockchain.info");
            argumentTest.marketcap = marketcap;

            string coinbasevalue = REST_GET_COINBASE("api/v1/prices/spot_rate?currency=USD");

            argumentTest.coinbasevalue = coinbasevalue;
            backgroundWorker1.ReportProgress(75);
            //this.label12.Text = coinbasevalue + " USD";
            backgroundWorker1.ReportProgress(80);
            string btcevalue = REST_GET_BTCE("api/2/btc_usd/ticker/");
            argumentTest.btcevalue = btcevalue;

            backgroundWorker1.ReportProgress(85);
            argumentTest.time = DateTime.Now.ToString();

            //this.label13.Text = DateTime.Now.ToString();
            //https://coinbase.com/api/v1/prices/spot_rate?currency=USD
            /*
             * https://btc-e.com/api/2/btc_usd/ticker/
             * 
             * http://data.mtgox.com/api/2/BTCUSD/money/ticker_fast
             * {"result":"success","data":{"last_local":{"value":"926.40676","value_int":"92640676","display":"$926.41","display_short":"$926.41","currency":"USD"},"last":{"value":"926.40676","value_int":"92640676","display":"$926.41","display_short":"$926.41","currency":"USD"},"last_orig":{"value":"926.40676","value_int":"92640676","display":"$926.41","display_short":"$926.41","currency":"USD"},"last_all":{"value":"926.40676","value_int":"92640676","display":"$926.41","display_short":"$926.41","currency":"USD"},"buy":{"value":"923.00000","value_int":"92300000","display":"$923.00","display_short":"$923.00","currency":"USD"},"sell":{"value":"927.23507","value_int":"92723507","display":"$927.24","display_short":"$927.24","currency":"USD"},"now":"1390166111496010"}}
             *  btce parse
             * {"ticker":{"high":839,"low":803.73798,"avg":821.36899,"vol":7231853.03422,"vol_cur":8804.24766,"last":829.8,"buy":829,"sell":828,"updated":1390165230,"server_time":1390165231}}
            argumentTest.OneValue = 6;
            argumentTest.TwoValue = 3;
            */
            //
            // Now, return the values we generated in this method.
            // ... Use e.Result.
            //
            backgroundWorker1.ReportProgress(90);
            backgroundWorker1.ReportProgress(100);
            e.Result = argumentTest;
            backgroundWorker1.ReportProgress(100);
        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
            // Receive the result from DoWork, and display it.
            //
            restCalls test = e.Result as restCalls;
            //this.Text = test.OneValue.ToString() + " " + test.TwoValue.ToString();

            this.label13.Text = test.time.ToString();

            this.label12.Text = test.coinbasevalue.ToString() + " USD";
            this.label2.Text = test.btcevalue.ToString() + " USD";
            /*
            this.label29.Text = test.primarycode.ToString() + " / " + test.secondarycode.ToString();
           if (checkBox1.Checked==true && textBox39.Text == test.lasttradeprice.ToString())
            {
                Console.WriteLine("Alert Hit: " + test.lasttradeprice.ToString());
                checkBox1.Checked=false;
                notifyIcon1.BalloonTipText = "Price Hit: " + test.lasttradeprice.ToString() + " - " + test.primarycode.ToString() + " / " + test.secondarycode.ToString();
                notifyIcon1.ShowBalloonTip(1000);
            }
            */
            this.textBox13.Text = test.lasttradeprice.ToString();
            this.textBox14.Text = test.volume.ToString();
            this.textBox26.Text = test.lasttradetime.ToString();
            this.textBox27.Text = test.marketid.ToString();
            this.label5.Text = test.primaryname.ToString() + " / " + test.secondaryname.ToString();
            this.textBox28.Text = test.primarycode.ToString() + " / " + test.secondarycode.ToString();
            this.listBox3.Items.Clear(); 
            this.listBox2.Items.Clear();
            this.listBox4.Items.Clear();

            this.label50.Text = test.totalbc.ToString();
            this.label51.Text = test.marketcap.ToString();
            this.label52.Text = test.hrprice.ToString();
            this.label53.Text = test.hrtransactions.ToString();
            this.label54.Text = test.hrbtcsent.ToString();
            this.label55.Text = test.blockheight.ToString();
            this.label56.Text = test.rewardtotal.ToString();
            this.label57.Text = test.difficulty.ToString();
            this.label58.Text = test.probability.ToString();


            string baseDate = DateTime.Now.ToString("hh:mm:ss");
            var x = baseDate;
            var orderscount = 0;

            if (test.buyorders != null)
            {
                orderscount = test.buyorders.Count;
            }
            else 
            {
                 orderscount = 0;
            }

            if (orderscount <= 10)
            {
            }
            if (orderscount > 10)
            {
                orderscount = 10;
            }
            for (int i = 0; i < orderscount; i++)
            {
                this.listBox3.Items.Add("Price: " + test.buyorders[i].price.ToString() + " - Coins: " + test.buyorders[i].quantity.ToString() + " - " + test.secondarycode + " Wall: " + test.buyorders[i].total.ToString());
            }
            for (int i = 0; i < 12; i++)
            {
                this.listBox2.Items.Insert(0,"Price: " + test.sellorders[i].price.ToString() + " - Coins: " + test.sellorders[i].quantity.ToString() + " - " + test.secondarycode + " Wall: " + test.sellorders[i].total.ToString());
            }
            for (int i = 0; i < 8; i++)
            {
                this.listBox4.Items.Add("Price: " + test.recenttrades[i].price.ToString() + " - Coins: " + test.recenttrades[i].quantity.ToString() + " - " + test.secondarycode + " Total: " + test.recenttrades[i].total.ToString());
            }

            // Frist parameter is X-Axis and Second is Collection of Y- Axis



            var y = test.volume.ToString();//test.lastbtcdogeprice.ToString();
            var ly = test.lasttradeprice.ToString();
            //var lby = "1";//test.lastbtcltcprice.ToString();
            double dVal = 0.00000000;

            dVal = double.Parse(y, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            double ddVal = 0.00000000;
            //double dddVal = 0.00000000;
            //dddVal = double.Parse(lby, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            ddVal = double.Parse(ly, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);


           /* if(chart2.Series[0].Points.Count==7)
            {
                chart2.Series[0].Points.RemoveAt(0);
            }
            chart2.Series[0].Points.AddXY(baseDate, dVal);
            */
            /*
            if (chart1.Series[0].Points.Count == 7)
            {
                chart1.Series[0].Points.RemoveAt(0);
            }
            chart1.Series[0].Points.AddXY(baseDate, ddVal);
            */
            //Update portfolio values
            var portbtc=listBox6.Items[0].ToString();
            listBox7.Items[0]=portbtc;
            double ltcdVal = double.Parse(test.ltcbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portltc = (double.Parse(listBox6.Items[1].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * ltcdVal);
            var minusval = (portltc * .0030);
            portltc = portltc - minusval;
            listBox7.Items[1] = portltc.ToString("0.00000000");
            listBox8.Items[0] = ltcdVal;
            
            double dogedVal = double.Parse(test.dogebtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portdoge = (double.Parse(listBox6.Items[2].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * dogedVal);
            minusval = (portdoge * .0030);
            portdoge = portdoge - minusval;
            listBox7.Items[2] = portdoge.ToString("0.00000000");
            listBox8.Items[1] = dogedVal.ToString("0.00000000");

            double drkdVal = double.Parse(test.drkbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portdrk = (double.Parse(listBox6.Items[12].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * drkdVal);
            minusval = (portdrk * .0030);
            portdrk = portdrk - minusval;
            listBox7.Items[12] = portdrk.ToString("0.00000000");
            listBox8.Items[3] = drkdVal.ToString("0.00000000");

            /*
            double moondVal = double.Parse(test.moonbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portmoon = (double.Parse(listBox6.Items[3].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * moondVal);
            minusval = (portmoon * .0030);
            portmoon = portmoon - minusval;
            listBox7.Items[3] = portmoon.ToString("0.00000000");
            */
            double ftcdVal = double.Parse(test.ftcbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portftc = (double.Parse(listBox6.Items[3].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * ftcdVal);
            minusval = (portftc * .0030);
            portftc = portftc - minusval;
            listBox7.Items[3] = portftc.ToString("0.00000000");
            listBox8.Items[4] = ftcdVal.ToString("0.00000000");

            double frcdVal = double.Parse(test.frcbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portfrc = (double.Parse(listBox6.Items[4].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * frcdVal);
            minusval = (portfrc * .0030);
            portfrc = portfrc - minusval;
            listBox7.Items[4] = portfrc.ToString("0.00000000");

            double cgbdVal = double.Parse(test.cgbbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portcgb = (double.Parse(listBox6.Items[5].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * cgbdVal);
            minusval = (portcgb * .0030);
            portcgb = portcgb - minusval;
            listBox7.Items[5] = portcgb.ToString("0.00000000");

            double dvcdVal = double.Parse(test.dvcbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portdvc = (double.Parse(listBox6.Items[6].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * dvcdVal);
            minusval = (portdvc * .0030);
            portdvc = portdvc - minusval;
            listBox7.Items[6] = portdvc.ToString("0.00000000");

            double fortytwodVal = double.Parse(test.fortytwobtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portfortytwo = (double.Parse(listBox6.Items[7].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * fortytwodVal);
            minusval = (portfortytwo * .0030);
            portfortytwo = portfortytwo - minusval;
            listBox7.Items[7] = portfortytwo.ToString("0.00000000");

            double lotdVal = double.Parse(test.lotbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portlot = (double.Parse(listBox6.Items[9].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * lotdVal);
            minusval = (portlot * .0030);
            portlot = portlot - minusval;
            listBox7.Items[9] = portlot.ToString("0.00000000");

            double xpmdVal = double.Parse(test.xpmbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portxpm = (double.Parse(listBox6.Items[10].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * xpmdVal);
            minusval = (portxpm * .0030);
            portxpm = portxpm - minusval;
            listBox7.Items[10] = portxpm.ToString("0.00000000");

            double zetdVal = double.Parse(test.zetbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portzet = (double.Parse(listBox6.Items[11].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * zetdVal);
            minusval = (portzet * .0030);
            portzet = portzet - minusval;
            listBox7.Items[11] = portzet.ToString("0.00000000");
            /*
            double btbdVal = double.Parse(test.btbbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portbtb = (double.Parse(listBox6.Items[18].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * btbdVal);
            minusval = (portbtb * .0030);
            portbtb = portbtb - minusval;
            listBox7.Items[18] = portbtb.ToString("0.00000000");

            double frkdVal = double.Parse(test.frkbtclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var portfrk = (double.Parse(listBox6.Items[15].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * frkdVal);
            minusval = (portfrk * .0030);
            portfrk = portfrk - minusval;
            listBox7.Items[15] = portfrk.ToString("0.00000000");
            */
            double tipsltcdVal = double.Parse(test.tipsltclasttradeprice, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var porttipsltc = (double.Parse(listBox6.Items[8].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture) * tipsltcdVal);
            minusval = (porttipsltc * .0030);
            porttipsltc = porttipsltc - minusval;
            listBox8.Items[2] = tipsltcdVal.ToString("0.00000000");

            var porttipsbtc = (porttipsltc * ltcdVal);
            minusval = (porttipsbtc * .0030);
            porttipsbtc = porttipsbtc - minusval;

            listBox7.Items[8] = porttipsbtc.ToString("0.00000000");

            double finalval=0.0;
            for (int i = 0; i < listBox7.Items.Count; i++)
            {
                finalval += double.Parse(listBox7.Items[i].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture); 
            }
            //double totalportbtc = portltc + (double.Parse(portbtc, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture)) +portcgb + portdoge + portdvc + portfortytwo + portfrc + portfrk + portftc + portlot + portmoon + portxpm + portzet;
            label40.Text = "~" + /*totalportbtc.ToString("0.00000000")*/finalval.ToString("0.00000000") + " BTC " + '\n' + "~"+ (finalval * (double.Parse(test.coinbasevalue, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture))).ToString("0.00") + " USD";

            double[] yValues = { portltc, (double.Parse(portbtc, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture)), portcgb, portdoge, porttipsbtc, portdvc, portfortytwo, portfrc, portftc, portlot/*, portmoon*/, portxpm, portzet, portdrk};
            string[] xValues = { "Litecoin", "Bitcoin", "CryptogenicBullion", "DOGE", "Fedoracoin", "Devcoin" , "42Coin" ,"Freicoin" , "Feathercoin", "Lottocoin" /*, "Mooncoin"*/ , "Primecoin" , "Zetacoin", "Darkcoin"};

            chart3.Series[0].Points.DataBindXY(xValues, yValues);
            chart3.Series[0].ToolTip = "#VALY, #AXISLABEL";
            // Set Doughnut chart type
            chart3.Series[0].ChartType = SeriesChartType.Doughnut;

            // Set labels style
            //chart3.Series[0]["PieLabelStyle"] = "Outside";

            // Set Doughnut radius percentage
            //chart3.Series[0]["DoughnutRadius"] = "30";
             /*
            var btcvestedstr = 1;//this.numericUpDown1.Value.ToString();
            var btcvested = double.Parse(btcvestedstr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var totaldogenow = (btcvested * .0020);
            totaldogenow = btcvested - totaldogenow;
            totaldogenow = totaldogenow / dVal;
            var totalltcnow = (btcvested * .0030);
            totalltcnow = btcvested - totalltcnow;
            totalltcnow=totalltcnow/ dddVal;
            
            var totalltcdogenow = (totalltcnow * .0020);
            totalltcdogenow = totalltcnow-totalltcdogenow;
            totalltcdogenow= totalltcdogenow/ ddVal;


            var btcdogenow = double.Parse(totaldogenow.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            var ltcdogenow = double.Parse(totalltcdogenow.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            if (btcdogenow > ltcdogenow)
            {
                this.textBox30.Text = (btcdogenow - ltcdogenow).ToString();
                this.pictureBox11.Visible = true;
                this.pictureBox12.Visible = false;
            }
            if (ltcdogenow > btcdogenow)
            {
                this.textBox30.Text = (ltcdogenow - btcdogenow).ToString();
                this.pictureBox11.Visible = false;
                this.pictureBox12.Visible = true;
            }
             */          
        }
        //Everytime the timer ticks, populate the boxes
        void timer_Tick(object sender, EventArgs e)
        {
            restCalls test = new restCalls
            {
                coinbasevalue = "▲",
                btcevalue = "▲",
                time = "▲",
                label=listBox1.GetItemText(listBox1.SelectedItem)
            };
            //
            // Send argument to our worker thread
            //
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync(test);
            }
        }
        public string REST_GET(string requestresource, string restclientURL)
        {
            var client = new RestClient(restclientURL);
            var request = new RestRequest(requestresource, Method.GET);
            IRestResponse response = client.Execute(request);
            string toreturn = response.Content;

            if (toreturn == "<html>\r\n<head><title>502 Bad Gateway</title></head>\r\n<body bgcolor=\"white\">\r\n<center><h1>502 Bad Gateway</h1></center>\r\n<hr><center>nginx/1.1.19</center>\r\n</body>\r\n</html>\r\n")
            {
                response = client.Execute(request);
                toreturn = response.Content;
            }
            return toreturn;
        }
        //This uses Dogechains api to gather the balance of a given Doge address
        public string REST_GET_ADDRBALANCE(string requestresource)
        {
            var client = new RestClient("http://dogechain.info/");
            var request = new RestRequest(requestresource, Method.GET);
            IRestResponse response=client.Execute(request);
            string toreturn = response.Content;

            if(toreturn=="<html>\r\n<head><title>502 Bad Gateway</title></head>\r\n<body bgcolor=\"white\">\r\n<center><h1>502 Bad Gateway</h1></center>\r\n<hr><center>nginx/1.1.19</center>\r\n</body>\r\n</html>\r\n")
            {
                response = client.Execute(request);
                toreturn = response.Content;
            }
            return toreturn;
        }
        //This is what populates the UI elements

        
        //Grabs and parses the Coinbase restapi call for current USD price
        public string REST_GET_COINBASE(string requestresource)
        {
            var client = new RestClient("https://coinbase.com/");
            var request = new RestRequest(requestresource, Method.GET);
            IRestResponse response = client.Execute(request);
            string toreturn = response.Content;
            string returnvalue = "";
            if (toreturn[2] !='a')
            {
                response = client.Execute(request);
                toreturn = response.Content;
            }
            //{"amount":"815.81","currency":"USD"}
            //Parse loop
            else
            {
                int i=0;
                    while (toreturn[i] != ':')
                    {
                        i++;
                    }
                        i++;
                        if (toreturn[i] == '"')
                        {
                            string coinbasevalue = "";
                            i++;
                            while (toreturn[i] != '"')
                            {
                                coinbasevalue += toreturn[i];
                                i++;
                            }
                            returnvalue = coinbasevalue;
                        }
            }
            return returnvalue;
        }
        public string REST_GET_BTCE(string requestresource)
        {
            var client = new RestClient("https://btc-e.com/");
            var request = new RestRequest(requestresource, Method.GET);
            IRestResponse response = client.Execute(request);
            string toreturn = response.Content;
            string returnvalue = "";
            if (toreturn[2] !='t')
            {
                response = client.Execute(request);
                toreturn = response.Content;
            }
            //{"ticker":{"high":839,"low":803.73798,"avg":821.36899,"vol":7231853.03422,"vol_cur":8804.24766,"last":829.8,"buy":829,"sell":828,"updated":1390165230,"server_time":1390165231}}
            //Parse loop
            else
            {
                int i = 0;
                while (toreturn[i] != 'a')
                {
                    i++;
                }
                if (toreturn[i] == 'a')
                {
                    i += 5;
                    string btcevalue = "";
                    while (toreturn[i] != ',')
                    {
                        btcevalue += toreturn[i];
                        i++;
                    }
                    returnvalue = btcevalue;
                }
            }
            return returnvalue;
        }
        //This calls Cryptsy and gets market information
        public WorldStarMonitor.Form1.RootObject REST_GET(string requestresource)
        {
            var client = new RestClient("http://pubapi.cryptsy.com/");
            var request = new RestRequest(requestresource, Method.GET);
            IRestResponse response=client.Execute(request);
            string toreturn = response.Content;
            
            while(toreturn[2] != 's' || toreturn=="{\"success\":1,\"return\":false}")
            {
                response = client.Execute(request);
                toreturn = response.Content;
            }
            string v = toreturn.Replace("\\/", "");
            v = v.Replace("42BTC", "fortytwoBTC");
            var Data = JsonConvert.DeserializeObject<RootObject>(v);
            return Data;
        }

        //Autogenerated CODE from a website where you paste in rest call and get structures to slam your data into objects
public class Recenttrade
{
    public string id { get; set; }
    public string time { get; set; }
    public string price { get; set; }
    public string quantity { get; set; }
    public string total { get; set; }
}
public class Sellorder
{
    public string price { get; set; }
    public string quantity { get; set; }
    public string total { get; set; }
}
public class Buyorder
{
    public string price { get; set; }
    public string quantity { get; set; }
    public string total { get; set; }
}
public class BTCUSD
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public object recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}

public class DOGEUSD
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public object recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}

public class DRKUSD
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public object recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}

public class FTCUSD
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public object recenttrades { get; set; }
    public object sellorders { get; set; }
    public object buyorders { get; set; }
}


public class LTCUSD
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public object recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}

public class ADTLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class ANCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class ASCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class AURLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BATLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CGBLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CNCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class COLLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CPRLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CTMLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DBLLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DGCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DIMELTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DMCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DOGELTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DRKLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DVCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ELPLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class EZCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FLOLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FRKLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FSTLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FTCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class GLDLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class GMELTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class IFCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class JKCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class KARMLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MAXLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MECLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MEMLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MOONLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MSTLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class NETLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class NXTLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class PPCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class PXCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class QRKLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class RBBTLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class RDDLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class REDLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class RYCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class SBCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class SXCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class TIPSLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class TIXLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class VRCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class VTCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}
public class WDCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XNCLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XPMLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class YACLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ZEITLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ZETLTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class fortytwoBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ACBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ALFBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class AMCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ANCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ARGBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class AURBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BCXBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BENBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BETBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BQCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BTBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BTEBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BTGBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class BUKBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CACHBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CAPBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CASHBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CATBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CGBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CINNIBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CLRBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CMCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CNCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class COMMBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CRCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class CSCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DEMBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DGBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class DGCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class DIMEBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}

public class DMDBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DOGEBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DRKBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class DVCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class EACBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ELCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class EMC2BTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class EMDBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class EXEBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class EZCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FFCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FLAPBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}


public class FLTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class FRCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class FRKBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class FSTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class FTCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class GDCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class GLCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class GLDBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class GLXBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class HBNBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class HVCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class IFCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class IXCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class JKCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class KDCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class KGCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class LEAFBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class LGDBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class LK7BTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class LKYBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class LOTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public object buyorders { get; set; }
}


public class LTBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class LTCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class LYCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class MAXBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class MECBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class MEOWBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class MINTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MN1BTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MN2BTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MNCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MRYBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class MYRBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class MZCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NANBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NAUTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class NBLBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class NECBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NETBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class NMCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NRBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NRSBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NVCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NXTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class NYANBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class ORBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class OSCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class PHSBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class PointsBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class POTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class PPCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class PTSBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class PXCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class PYCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class QRKBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class RDDBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class RPCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class RYCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class SATBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class SBCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class SMCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class SPABTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class SPTBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class SRCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class STRBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class SXCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class TAGBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class TAKBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class TEKBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class TGCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class TRCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class UNBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class UNOBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class USDeBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class UTCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class VRCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class VTCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class WCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class WDCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XJOBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XLBBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class XPMBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class YACBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}


public class YBCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ZCCBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ZEDBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class ZETBTC
{
    public string marketid { get; set; }
    public string label { get; set; }
    public string lasttradeprice { get; set; }
    public string volume { get; set; }
    public string lasttradetime { get; set; }
    public string primaryname { get; set; }
    public string primarycode { get; set; }
    public string secondaryname { get; set; }
    public string secondarycode { get; set; }
    public List<Recenttrade> recenttrades { get; set; }
    public List<Sellorder> sellorders { get; set; }
    public List<Buyorder> buyorders { get; set; }
}

public class Markets
{
    public BTCUSD BTCUSD { get; set; }
    public DOGEUSD DOGEUSD { get; set; }
    public DRKUSD DRKUSD { get; set; }
    public FTCUSD FTCUSD { get; set; }
    public LTCUSD LTCUSD { get; set; }
    public ADTLTC ADTLTC { get; set; }
    public ANCLTC ANCLTC { get; set; }
    public ASCLTC ASCLTC { get; set; }
    public AURLTC AURLTC { get; set; }
    public BATLTC BATLTC { get; set; }
    public BCLTC BCLTC { get; set; }
    public CGBLTC CGBLTC { get; set; }
    public CNCLTC CNCLTC { get; set; }
    public COLLTC COLLTC { get; set; }
    public CPRLTC CPRLTC { get; set; }
    public CTMLTC CTMLTC { get; set; }
    public DBLLTC DBLLTC { get; set; }
    public DGCLTC DGCLTC { get; set; }
    public DIMELTC DIMELTC { get; set; }
    public DMCLTC DMCLTC { get; set; }
    public DOGELTC DOGELTC { get; set; }
    public DRKLTC DRKLTC { get; set; }
    public DVCLTC DVCLTC { get; set; }
    public ELPLTC ELPLTC { get; set; }
    public EZCLTC EZCLTC { get; set; }
    public FLOLTC FLOLTC { get; set; }
    public FRKLTC FRKLTC { get; set; }
    public FSTLTC FSTLTC { get; set; }
    public FTCLTC FTCLTC { get; set; }
    public GLDLTC GLDLTC { get; set; }
    public GMELTC GMELTC { get; set; }
    public IFCLTC IFCLTC { get; set; }
    public JKCLTC JKCLTC { get; set; }
    public KARMLTC KARMLTC { get; set; }
    public MAXLTC MAXLTC { get; set; }
    public MECLTC MECLTC { get; set; }
    public MEMLTC MEMLTC { get; set; }
    public MOONLTC MOONLTC { get; set; }
    public MSTLTC MSTLTC { get; set; }
    public NETLTC NETLTC { get; set; }
    public NXTLTC NXTLTC { get; set; }
    public PPCLTC PPCLTC { get; set; }
    public PXCLTC PXCLTC { get; set; }
    public QRKLTC QRKLTC { get; set; }
    public RBBTLTC RBBTLTC { get; set; }
    public RDDLTC RDDLTC { get; set; }
    public REDLTC REDLTC { get; set; }
    public RYCLTC RYCLTC { get; set; }
    public SBCLTC SBCLTC { get; set; }
    public SXCLTC SXCLTC { get; set; }
    public TIPSLTC TIPSLTC { get; set; }
    public TIXLTC TIXLTC { get; set; }
    public VRCLTC VRCLTC { get; set; }
    public VTCLTC VTCLTC { get; set; }
    public WDCLTC WDCLTC { get; set; }
    public XCLTC XCLTC { get; set; }
    public XNCLTC XNCLTC { get; set; }
    public XPMLTC XPMLTC { get; set; }
    public YACLTC YACLTC { get; set; }
    public ZEITLTC ZEITLTC { get; set; }
    public ZETLTC ZETLTC { get; set; }
    public fortytwoBTC fortytwoBTC { get; set; }
    public ACBTC ACBTC { get; set; }
    public ALFBTC ALFBTC { get; set; }
    public AMCBTC AMCBTC { get; set; }
    public ANCBTC ANCBTC { get; set; }
    public ARGBTC ARGBTC { get; set; }
    public AURBTC AURBTC { get; set; }
    public BCBTC BCBTC { get; set; }
    public BCXBTC BCXBTC { get; set; }
    public BENBTC BENBTC { get; set; }
    public BETBTC BETBTC { get; set; }
    public BQCBTC BQCBTC { get; set; }
    public BTBBTC BTBBTC { get; set; }
    public BTEBTC BTEBTC { get; set; }
    public BTGBTC BTGBTC { get; set; }
    public BUKBTC BUKBTC { get; set; }
    public CACHBTC CACHBTC { get; set; }
    public CAPBTC CAPBTC { get; set; }
    public CASHBTC CASHBTC { get; set; }
    public CATBTC CATBTC { get; set; }
    public CGBBTC CGBBTC { get; set; }
    public CINNIBTC CINNIBTC { get; set; }
    public CLRBTC CLRBTC { get; set; }
    public CMCBTC CMCBTC { get; set; }
    public CNCBTC CNCBTC { get; set; }
    public COMMBTC COMMBTC { get; set; }
    public CRCBTC CRCBTC { get; set; }
    public CSCBTC CSCBTC { get; set; }
    public DEMBTC DEMBTC { get; set; }
    public DGBBTC DGBBTC { get; set; }
    public DGCBTC DGCBTC { get; set; }
    public DIMEBTC DIMEBTC { get; set; }
    public DMDBTC DMDBTC { get; set; }
    public DOGEBTC DOGEBTC { get; set; }
    public DRKBTC DRKBTC { get; set; }
    public DVCBTC DVCBTC { get; set; }
    public EACBTC EACBTC { get; set; }
    public ELCBTC ELCBTC { get; set; }
    public EMC2BTC EMC2BTC { get; set; }
    public EMDBTC EMDBTC { get; set; }
    public EXEBTC EXEBTC { get; set; }
    public EZCBTC EZCBTC { get; set; }
    public FFCBTC FFCBTC { get; set; }
    public FLAPBTC FLAPBTC { get; set; }
    public FLTBTC FLTBTC { get; set; }
    public FRCBTC FRCBTC { get; set; }
    public FRKBTC FRKBTC { get; set; }
    public FSTBTC FSTBTC { get; set; }
    public FTCBTC FTCBTC { get; set; }
    public GDCBTC GDCBTC { get; set; }
    public GLCBTC GLCBTC { get; set; }
    public GLDBTC GLDBTC { get; set; }
    public GLXBTC GLXBTC { get; set; }
    public HBNBTC HBNBTC { get; set; }
    public HVCBTC HVCBTC { get; set; }
    public IFCBTC IFCBTC { get; set; }
    public IXCBTC IXCBTC { get; set; }
    public JKCBTC JKCBTC { get; set; }
    public KDCBTC KDCBTC { get; set; }
    public KGCBTC KGCBTC { get; set; }
    public LEAFBTC LEAFBTC { get; set; }
    public LGDBTC LGDBTC { get; set; }
    public LK7BTC LK7BTC { get; set; }
    public LKYBTC LKYBTC { get; set; }
    public LOTBTC LOTBTC { get; set; }
    public LTBBTC LTBBTC { get; set; }
    public LTCBTC LTCBTC { get; set; }
    public LYCBTC LYCBTC { get; set; }
    public MAXBTC MAXBTC { get; set; }
    public MECBTC MECBTC { get; set; }
    public MEOWBTC MEOWBTC { get; set; }
    public MINTBTC MINTBTC { get; set; }
    public MN1BTC MN1BTC { get; set; }
    public MN2BTC MN2BTC { get; set; }
    public MNCBTC MNCBTC { get; set; }
    public MRYBTC MRYBTC { get; set; }
    public MYRBTC MYRBTC { get; set; }
    public MZCBTC MZCBTC { get; set; }
    public NANBTC NANBTC { get; set; }
    public NAUTBTC NAUTBTC { get; set; }
    public NBLBTC NBLBTC { get; set; }
    public NECBTC NECBTC { get; set; }
    public NETBTC NETBTC { get; set; }
    public NMCBTC NMCBTC { get; set; }
    public NRBBTC NRBBTC { get; set; }
    public NRSBTC NRSBTC { get; set; }
    public NVCBTC NVCBTC { get; set; }
    public NXTBTC NXTBTC { get; set; }
    public NYANBTC NYANBTC { get; set; }
    public ORBBTC ORBBTC { get; set; }
    public OSCBTC OSCBTC { get; set; }
    public PHSBTC PHSBTC { get; set; }
    public PointsBTC PointsBTC { get; set; }
    public POTBTC POTBTC { get; set; }
    public PPCBTC PPCBTC { get; set; }
    public PTSBTC PTSBTC { get; set; }
    public PXCBTC PXCBTC { get; set; }
    public PYCBTC PYCBTC { get; set; }
    public QRKBTC QRKBTC { get; set; }
    public RDDBTC RDDBTC { get; set; }
    public RPCBTC RPCBTC { get; set; }
    public RYCBTC RYCBTC { get; set; }
    public SATBTC SATBTC { get; set; }
    public SBCBTC SBCBTC { get; set; }
    public SMCBTC SMCBTC { get; set; }
    public SPABTC SPABTC { get; set; }
    public SPTBTC SPTBTC { get; set; }
    public SRCBTC SRCBTC { get; set; }
    public STRBTC STRBTC { get; set; }
    public SXCBTC SXCBTC { get; set; }
    public TAGBTC TAGBTC { get; set; }
    public TAKBTC TAKBTC { get; set; }
    public TEKBTC TEKBTC { get; set; }
    public TGCBTC TGCBTC { get; set; }
    public TRCBTC TRCBTC { get; set; }
    public UNBBTC UNBBTC { get; set; }
    public UNOBTC UNOBTC { get; set; }
    public USDeBTC USDeBTC { get; set; }
    public UTCBTC UTCBTC { get; set; }
    public VRCBTC VRCBTC { get; set; }
    public VTCBTC VTCBTC { get; set; }
    public WCBTC WCBTC { get; set; }
    public WDCBTC WDCBTC { get; set; }
    public XCBTC XCBTC { get; set; }
    public XJOBTC XJOBTC { get; set; }
    public XLBBTC XLBBTC { get; set; }
    public XPMBTC XPMBTC { get; set; }
    public YACBTC YACBTC { get; set; }
    public YBCBTC YBCBTC { get; set; }
    public ZCCBTC ZCCBTC { get; set; }
    public ZEDBTC ZEDBTC { get; set; }
    public ZETBTC ZETBTC { get; set; }
}

public class Return
{
    public Markets markets { get; set; }
}

public class RootObject
{
    public int success { get; set; }
    public Return @return { get; set; }
}
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/132");
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void label3_Click(object sender, EventArgs e)
        {

        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void label12_Click(object sender, EventArgs e)
        {

        }
        private void label14_Click(object sender, EventArgs e)
        {

        }
        private void label15_Click(object sender, EventArgs e)
        {

        }
        private void label16_Click(object sender, EventArgs e)
        {

        }
        private void label21_Click(object sender, EventArgs e)
        {

        }
        private void label19_Click(object sender, EventArgs e)
        {

        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void chart1_Click_1(object sender, EventArgs e)
        {

        }
        private void label11_Click(object sender, EventArgs e)
        {
            Process.Start("http://coinbase.com");
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/135");
        }

        private void label18_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/3");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.coinbase.com");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.reddit.com/r/dogecoin");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://dogechain.info");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://doge-dice.com");
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.bitcointalk.org");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.dogecoin.com");
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/3");
        }

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/132");
        }

        private void linkLabel10_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/135");
        }

        private void label7_Click_1(object sender, EventArgs e)
        {
            Process.Start("http://www.btc-e.com");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //Application.esources["dwallet"] =this.textBox3.Text;
             Properties.Settings.Default.dwallet = this.textBox3.Text;
             Properties.Settings.Default.dexchange = this.textBox4.Text;
             Properties.Settings.Default.ddonation = this.textBox10.Text;

             Properties.Settings.Default.bwallet = this.textBox17.Text;
             Properties.Settings.Default.bexchange = this.textBox16.Text;
             Properties.Settings.Default.bdonation = this.textBox15.Text;

            Properties.Settings.Default.a1 = this.textBox20.Text;
            Properties.Settings.Default.a2 = this.textBox19.Text;
            Properties.Settings.Default.a3 = this.textBox18.Text;
            Properties.Settings.Default.a4 = this.textBox25.Text;
            Properties.Settings.Default.a5 = this.textBox23.Text;
            Properties.Settings.Default.a6 = this.textBox22.Text;
            Properties.Settings.Default.a7 = this.textBox21.Text;
            Properties.Settings.Default.a8 = this.textBox24.Text;
            Properties.Settings.Default.Save();
        }
        private void buildcommand()
        {
            string finalcommand = "";
            string exepath= "\"" + this.textBox40.Text + "\"";
            finalcommand += exepath + " ";
            if (comboBox1.Text == "cgminer")
            //-o http://pool.ABCPool.co:8332 -u workername -p password
            {
                finalcommand += " --scrypt -o ";
                finalcommand += this.textBox42.Text;
                finalcommand += " -u ";
                finalcommand += this.textBox38.Text;
                finalcommand += " -p ";
                finalcommand += this.textBox41.Text;
            }
            if (comboBox1.Text == "cudaminer")
            //C:\Users\time\Downloads\cudaminer-2013-12-18\cudaminer-2013-12-18\x64\cudaminer.exe -o stratum+tcp://teamdoge.com:3333 -O jujuman.jujuwork:soriv
            {
                finalcommand += " -o ";
                finalcommand += this.textBox42.Text;
                finalcommand += " -O ";
                finalcommand += this.textBox38.Text;
                finalcommand += ":";
                finalcommand += this.textBox41.Text;
            }
            if (comboBox1.Text == "minerd")
            //C:\Users\time\Downloads\pooler-cpuminer-2.3.2-win32\minerd.exe --url stratum+tcp://localhost:22555 --userpass u:p
            {
                finalcommand += " --url ";
                finalcommand += this.textBox42.Text;
                finalcommand += " --userpass ";
                finalcommand += this.textBox38.Text;
                finalcommand += ":";
                finalcommand += this.textBox41.Text;
            }
            this.richTextBox1.Text = finalcommand;
        }
        private void linkLabel12_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/135");
        }
        private void linkLabel11_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/132");
        }

        private void linkLabel13_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/3");
        }
        private void linkLabel8_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://virusscan.jotti.org/en");
        }
        private void linkLabel9_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://dogemonitor.com");
        }
        private void linkLabel10_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.cryptsy.com/markets/view/" + this.textBox27.Text);
        }
        private void chart1_Click_2(object sender, EventArgs e)
        {

        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }
        private void label31_Click(object sender, EventArgs e)
        {

        }
        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            /*
            this.chart2.Series[0].Points.Clear();
            this.chart1.Series[0].Points.Clear();
            this.chart1.Enabled = false;
            this.chart1.Enabled = true;
            this.chart2.Enabled = false;
            this.chart2.Enabled = true;
             */
        }
        private void label46_Click(object sender, EventArgs e)
        {

        }
        private void button9_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox48.Text);
        }
        private void textBox48_TextChanged(object sender, EventArgs e)
        {

        }
        private void button4_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox46.Text);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox45.Text);
        }
        private void button6_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox44.Text);
        }
        private void button7_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox43.Text);
        }
        private void button8_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox47.Text);
        }
        private void button10_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox49.Text);
        }
        private void button11_Click(object sender, EventArgs e)
        {
            launchminer(this.textBox50.Text);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            buildcommand();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            if (this.richTextBox1.TextLength > 0)
            {
                launchminer(this.richTextBox1.Text);
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //Application.esources["dwallet"] =this.textBox3.Text;
            Properties.Settings.Default.mc1 = this.textBox46.Text;
            Properties.Settings.Default.mc2 = this.textBox45.Text;
            Properties.Settings.Default.mc3 = this.textBox44.Text;
            Properties.Settings.Default.mc4 = this.textBox43.Text;
            Properties.Settings.Default.mc5 = this.textBox48.Text;
            Properties.Settings.Default.mc6 = this.textBox47.Text;
            Properties.Settings.Default.mc7 = this.textBox49.Text;
            Properties.Settings.Default.mc8 = this.textBox50.Text;

            Properties.Settings.Default.mcn1 = this.textBox51.Text;
            Properties.Settings.Default.mcn2 = this.textBox52.Text;
            Properties.Settings.Default.mcn3 = this.textBox54.Text;
            Properties.Settings.Default.mcn4 = this.textBox53.Text;
            Properties.Settings.Default.mcn5 = this.textBox56.Text;
            Properties.Settings.Default.mcn6 = this.textBox55.Text;
            Properties.Settings.Default.mcn7 = this.textBox58.Text;
            Properties.Settings.Default.mcn8 = this.textBox57.Text;
            Properties.Settings.Default.Save();
        }
        private void label47_Click(object sender, EventArgs e)
        {

        }
        private void label48_Click(object sender, EventArgs e)
        {

        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }
        private void label18_Click_1(object sender, EventArgs e)
        {

        }
        private void label29_Click(object sender, EventArgs e)
        {

        }
        private void linkLabel3_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://just-dice.com");
        }
        private void linkLabel5_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://bitcoinwisdom.com");
        }
        private void linkLabel6_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://winkdex.com/#/");
        }
        private void linkLabel11_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://blockchain.info");
        }
        private void linkLabel12_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://btc-e.com/");
        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void listBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}