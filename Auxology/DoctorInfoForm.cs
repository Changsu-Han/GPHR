using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Data.OleDb;

namespace Auxology
{
    public partial class DoctorInfoForm : Form
    {
        string connStr = "provider = Microsoft.JET.OLEDB.4.0;Data Source={0};";        

        public DoctorInfoForm()
        {
            InitializeComponent();
        }

        private void DoctorInfoForm_Load(object sender, EventArgs e)
        {
            this.Focus();
            
            string sum_pc_lcns = "";
            string[] sub_pc_lcns = new string[6];

            //두자리마다 :를 넣기위한 for문
            for(int i = 0; i<6; i++)
            {
                sub_pc_lcns[i] = Globalfunction.pc_lcns.Substring(i*2,2);
                if (i == 5)
                {
                    sum_pc_lcns += sub_pc_lcns[i];
                }
                else
                {
                    sum_pc_lcns += sub_pc_lcns[i] + ":";
                }
            }
            mac_adrs.Text = sum_pc_lcns;
        }

        private async void btn_confirm_Click(object sender, EventArgs e)
        {
            string e_lcns = Globalfunction.Encrypt(doc_lcns.Text);
            string e_mac = Globalfunction.Encrypt(Globalfunction.pc_lcns);

            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("mt_lcns", e_lcns));
            param.Add(new KeyValuePair<string, string>("mt_mac", e_mac));
            param.Add(new KeyValuePair<string, string>("mt_name", hp_doctor.Text));
            HttpContent content = new FormUrlEncodedContent(param);

            HttpResponseMessage response = await new HttpClient().PostAsync( "http://contentsbox.co.kr"+ "/au/check_lcns.php", content);

            response.EnsureSuccessStatusCode();

            List<Pinfo> result = JsonConvert.DeserializeObject<List<Pinfo>>(await response.Content.ReadAsStringAsync());
            if (result == null)
            {                
                return;
            }
            else
            {
                if (result.Count == 0)
                {
                    MessageBox.Show("입력하신 정보가 일치하지 않습니다.");
                }
                else
                {
                    end_date.Text = result[0].mt_end;
                    save_EndDate(Globalfunction.Encrypt(end_date.Text),e_mac);
                }                
            }
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            string e_lcns = Globalfunction.Encrypt(doc_lcns.Text);
            string e_mac = Globalfunction.Encrypt(Globalfunction.pc_lcns);

            var param = new List<KeyValuePair<string, string>>();
            param.Add(new KeyValuePair<string, string>("mt_lcns", e_lcns));
            param.Add(new KeyValuePair<string, string>("mt_mac", e_mac));
            param.Add(new KeyValuePair<string, string>("mt_name", hp_doctor.Text));
            HttpContent content = new FormUrlEncodedContent(param);

            HttpResponseMessage response = await new HttpClient().PostAsync("http://contentsbox.co.kr" + "/au/insert_info.php", content);

            response.EnsureSuccessStatusCode();
            
            JsonResult result = JsonConvert.DeserializeObject<JsonResult>(await response.Content.ReadAsStringAsync());
            if (result == null)
            {                
                return;
            }
            else
            {
                MessageBox.Show(result.msg);
                save_MsDb(e_lcns, e_mac);
            }
            
        }

        private void save_MsDb(string lcns, string mac)
        {
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);

            string field1 = "h_name, h_pn, hp_adrs, dt_name, dt_lcns, dt_mac";
            string value1 = "@h_name, @h_pn, @hp_adrs, @dt_name, @dt_lcns, @dt_mac";
            string field = field1;
            string value = value1;
            string t_name = "D_INFO";
            string query = "insert into {0} ({1}) values({2})";
            query = String.Format(query, t_name, field, value);
            gConn.Open();
            OleDbCommand cmd = new OleDbCommand(query, gConn);

            cmd.Parameters.Add("@h_name", OleDbType.VarWChar).Value = hp_name.Text;
            cmd.Parameters.Add("@h_pn", OleDbType.VarWChar).Value = hp_hp.Text;
            cmd.Parameters.Add("@hp_adrs", OleDbType.VarWChar).Value = hp_address.Text;
            cmd.Parameters.Add("@dt_name", OleDbType.VarWChar).Value = hp_doctor.Text;
            cmd.Parameters.Add("@dt_lcns", OleDbType.VarWChar).Value = lcns;
            cmd.Parameters.Add("@dt_mac", OleDbType.VarWChar).Value = mac;

            cmd.ExecuteNonQuery();
            gConn.Close();
        }

        private void save_EndDate(string end_date, string mac)
        {
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);
       
            string query = "update D_INFO set dt_end=@dt_end where dt_mac=@dt_mac";
            
            gConn.Open();
            MainForm mf = new MainForm();
            OleDbCommand cmd = new OleDbCommand(query, gConn);
            cmd.Parameters.Add("@dt_end", OleDbType.VarWChar).Value = end_date;
            cmd.Parameters.AddWithValue("@dt_mac", mac);           

            cmd.ExecuteNonQuery();
            gConn.Close();
            
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
        
    }
}
