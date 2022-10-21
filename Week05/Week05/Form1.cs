using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Week05.MnbServiceReference;
using Week05.Entities;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;

namespace Week05
{
    public partial class Form1 : Form
    {
        BindingList<RateDate> Rates = new BindingList<RateDate>();
        BindingList<string> Currencies = new BindingList<string>();
        public Form1()
        {
            
            

            InitializeComponent();
            dataGridView1.DataSource = Rates;
            comboBox1.DataSource = Currencies;
            GetCurrencies();
            RefreshData();
        }

        private void GetCurrencies()
        {
            var mnbservice = new MNBArfolyamServiceSoapClient();
            var reqcur = new GetCurrenciesRequestBody()
            {
                
            };
           var response= mnbservice.GetCurrencies(reqcur);
            var result = response.GetCurrenciesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);
            XmlNodeList list = xml.SelectNodes("MNBCurrencies/Currencies/Curr");
            int count = list.Count;
            
            
            foreach (XmlElement element in xml.DocumentElement)
            {
                
                for (int i = 0; i < count; i++)
                {
                    var childelement = (XmlElement)element.ChildNodes[i];
                    var currency = childelement.InnerText;
                    Currencies.Add(currency);
                }
            }
            

        }

        private void RefreshData()
        {
            Rates.Clear();
            var mnbService = new MNBArfolyamServiceSoapClient();
           
            var request = new GetExchangeRatesRequestBody()
            {
                
                currencyNames = comboBox1.SelectedItem.ToString(),
                startDate = dtpStart.Value.ToString(),
                endDate = dtpEnd.Value.ToString()
            };
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);
            foreach (XmlElement element in xml.DocumentElement)
            {
                var rate = new RateDate();
                Rates.Add(rate);
                rate.Date = DateTime.Parse(element.GetAttribute("date"));
                var childelement = (XmlElement)element.ChildNodes[0];
                if (childelement is null)
                {
                    continue;
                }
                rate.Currency = childelement.GetAttribute("curr");
                var unit = decimal.Parse(childelement.GetAttribute("unit"));
                var value = decimal.Parse(childelement.InnerText);
                if (unit != 0)
                {
                    rate.Value = value / unit;
                }
            }
            chartRateData.DataSource = Rates;
            var series = chartRateData.Series[0];
            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;
            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dtpEnd_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
