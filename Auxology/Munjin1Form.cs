using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Auxology
{
    public partial class Munjin1Form : Form
    {
        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        string connStr = "provider = Microsoft.JET.OLEDB.4.0;Data Source={0};";        
        string regday = "";

        public Munjin1Form()
        {
            InitializeComponent();
        }

        private void Munjin1Form_Load(object sender, EventArgs e)
        {
            chartnum_tb.Text = Globalfunction.chart_num;

            regday = Globalfunction.spilitDate(reg_date.Text);            
        }

        private void cb1_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cb1_2.Checked == true)
            {
                tb1_1.Enabled = true;
            }
            else
            {
                tb1_1.Enabled = false;
                tb1_1.Text = "";
            }
        }

        private void cb2_5_CheckedChanged(object sender, EventArgs e)
        {
            if (cb2_5.Checked == true)
            {
                tb2_1.Enabled = true;
            }
            else
            {
                tb2_1.Enabled = false;
                tb2_1.Text = "";
            }
        }

        private void cb4_3_CheckedChanged(object sender, EventArgs e)
        {
            if (cb4_3.Checked == true)
            {
                tb4_1.Enabled = true;
            }
            else
            {
                tb4_1.Enabled = false;
                tb4_1.Text = "";
            }
        }

        private void cb5_3_CheckedChanged(object sender, EventArgs e)
        {
            if (cb5_3.Checked == true)
            {
                tb5_1.Enabled = true;
            }
            else
            {
                tb5_1.Enabled = false;
                tb5_1.Text = "";
            }
        }

        private void cb5_4_CheckedChanged(object sender, EventArgs e)
        {
            if (cb5_4.Checked == true)
            {
                tb5_2.Enabled = true;
                tb5_3.Enabled = true;
                tb5_4.Enabled = true;
            }
            else
            {
                tb5_2.Enabled = false;
                tb5_3.Enabled = false;
                tb5_4.Enabled = false;
                tb5_2.Text = "";
                tb5_3.Text = "";
                tb5_4.Text = "";
            }
        }

        private void cb13_1_CheckedChanged(object sender, EventArgs e)
        {
            if (cb13_1.Checked == true)
            {
                tb13_1.Enabled = true;
                tb13_2.Enabled = true;
            }
            else
            {
                tb13_1.Enabled = false;
                tb13_2.Enabled = false;
                tb13_1.Text = "";
                tb13_2.Text = "";
            }
        }

        private void cb14_4_CheckedChanged(object sender, EventArgs e)
        {
            if (cb14_4.Checked == true)
            {
                cb14_5.Enabled = true;
                cb14_6.Enabled = true;
            }
            else
            {
                cb14_5.Enabled = false;
                cb14_6.Enabled = false;
                cb14_5.Checked = false;
                cb14_6.Checked = false;
            }
        }

        private void cb15_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cb15_2.Checked == true)
            {
                cb15_3.Enabled = true;
            }
            else
            {
                cb15_3.Enabled = false;
                cb15_3.Checked = false;
            }
        }

        private void cb6_2_CheckedChanged(object sender, EventArgs e)
        {
            if (cb6_2.Checked == true)
            {
                tb6_1.Enabled = true;
                cb6_3.Enabled = true;
                cb6_4.Enabled = true;
            }
            else
            {
                tb6_1.Enabled = false;
                cb6_3.Enabled = false;
                cb6_4.Enabled = false;
                tb6_1.Text = "";
                cb6_3.Checked = false;
                cb6_4.Checked = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);

            string field1 = "chart_num, child_name, reg_date, ck1_1, ck1_2, text1_1, ck2_1, ck2_2, ck2_3, ck2_4, ck2_5, text2_1,";
            string field2 = "ck3_1, ck3_2, ck3_3, ck3_4, ck4_1, ck4_2, ck4_3, text4_1, ck5_1, ck5_2, ck5_3, text5_1, ck5_4, text5_2, text5_3, text5_4,";
            string field3 = "ck6_1, ck6_2, text6_1, ck6_3, ck6_4, ck7_1, ck7_2, ck7_3, ck7_4, ck7_5, ck8_1, ck8_2, ck8_3, ck9_1, ck9_2, ck9_3, ck9_4, ck9_5,";
            string field4 = "ck10_1, ck10_2, ck10_3, ck11_1, ck11_2, ck11_3, ck11_4, ck12_1, ck12_2, ck12_3, ck12_4, ck12_5, ck13_1, text13_1, text13_2, ck13_2,";
            string field5 = "ck14_1, ck14_2, ck14_3, ck14_4, ck14_5, ck14_6, ck15_1, ck15_2, ck15_3, ck15_4, ck15_5, text16_1, text16_2, text17_1, text18_1";

            string value1 = "@chart_num, @child_name, @reg_date, @ck1_1, @ck1_2, @text1_1, @ck2_1, @ck2_2, @ck2_3, @ck2_4, @ck2_5, @text2_1,";
            string value2 = "@ck3_1, @ck3_2, @ck3_3, @ck3_4, @ck4_1, @ck4_2, @ck4_3, @text4_1, @ck5_1, @ck5_2, @ck5_3, @text5_1, @ck5_4, @text5_2, @text5_3, @text5_4,";
            string value3 = "@ck6_1, @ck6_2, @text6_1, @ck6_3, @ck6_4, @ck7_1, @ck7_2, @ck7_3, @ck7_4, @ck7_5, @ck8_1, @ck8_2, @ck8_3, @ck9_1, @ck9_2, @ck9_3, @ck9_4, @ck9_5,";
            string value4 = "@ck10_1, @ck10_2, @ck10_3, @ck11_1, @ck11_2, @ck11_3, @ck11_4, @ck12_1, @ck12_2, @ck12_3, @ck12_4, @ck12_5, @ck13_1, @text13_1, @text13_2, @ck13_2,";
            string value5 = "@ck14_1, @ck14_2, @ck14_3, @ck14_4, @ck14_5, @ck14_6, @ck15_1, @ck15_2, @ck15_3, @ck15_4, @ck15_5, @text16_1, @text16_2, @text17_1, @text18_1";

            string field = field1 + field2 + field3 + field4 + field5; 
            string value = value1 + value2 + value3 + value4 + value5;
            string t_name = "munjin1";
            string query = "insert into {0} ({1}) values({2})";

            query = String.Format(query, t_name, field, value);
            gConn.Open();
            OleDbCommand cmd = new OleDbCommand(query, gConn);
            cmd.Parameters.Add("@chart_num", OleDbType.VarWChar).Value = chartnum_tb.Text;
            cmd.Parameters.Add("@child_name", OleDbType.VarWChar).Value = name_tb.Text;
            cmd.Parameters.Add("@reg_date", OleDbType.VarWChar).Value = regday;
            cmd.Parameters.Add("@ck1_1", OleDbType.Boolean).Value = cb1_1.Checked;
            cmd.Parameters.Add("@ck1_2", OleDbType.Boolean).Value = cb1_2.Checked;
            cmd.Parameters.Add("@text1_1", OleDbType.VarWChar).Value = tb1_1.Text;
            cmd.Parameters.Add("@ck2_1", OleDbType.Boolean).Value = cb2_1.Checked;
            cmd.Parameters.Add("@ck2_2", OleDbType.Boolean).Value = cb2_2.Checked;
            cmd.Parameters.Add("@ck2_3", OleDbType.Boolean).Value = cb2_3.Checked;
            cmd.Parameters.Add("@ck2_4", OleDbType.Boolean).Value = cb2_4.Checked;
            cmd.Parameters.Add("@ck2_5", OleDbType.Boolean).Value = cb2_5.Checked;
            cmd.Parameters.Add("@text2_1", OleDbType.VarWChar).Value = tb2_1.Text;
            cmd.Parameters.Add("@ck3_1", OleDbType.Boolean).Value = cb3_1.Checked;
            cmd.Parameters.Add("@ck3_2", OleDbType.Boolean).Value = cb3_2.Checked;
            cmd.Parameters.Add("@ck3_3", OleDbType.Boolean).Value = cb3_3.Checked;
            cmd.Parameters.Add("@ck3_4", OleDbType.Boolean).Value = cb3_4.Checked;
            cmd.Parameters.Add("@ck4_1", OleDbType.Boolean).Value = cb4_1.Checked;
            cmd.Parameters.Add("@ck4_2", OleDbType.Boolean).Value = cb4_2.Checked;
            cmd.Parameters.Add("@ck4_3", OleDbType.Boolean).Value = cb4_3.Checked;
            cmd.Parameters.Add("@text4_1", OleDbType.VarWChar).Value = tb4_1.Text;
            cmd.Parameters.Add("@ck5_1", OleDbType.Boolean).Value = cb5_1.Checked;            
            cmd.Parameters.Add("@ck5_2", OleDbType.Boolean).Value = cb5_2.Checked;
            cmd.Parameters.Add("@ck5_3", OleDbType.Boolean).Value = cb5_3.Checked;
            cmd.Parameters.Add("@text5_1", OleDbType.VarWChar).Value = tb5_1.Text;
            cmd.Parameters.Add("@ck5_4", OleDbType.Boolean).Value = cb5_4.Checked;;
            cmd.Parameters.Add("@text5_2", OleDbType.VarWChar).Value = tb5_2.Text;
            cmd.Parameters.Add("@text5_3", OleDbType.VarWChar).Value = tb5_3.Text;
            cmd.Parameters.Add("@text5_4", OleDbType.VarWChar).Value = tb5_4.Text;
            cmd.Parameters.Add("@ck6_1", OleDbType.Boolean).Value = cb6_1.Checked;
            cmd.Parameters.Add("@ck6_2", OleDbType.Boolean).Value = cb6_2.Checked;
            cmd.Parameters.Add("@text6_1", OleDbType.VarWChar).Value = tb6_1.Text;
            cmd.Parameters.Add("@ck6_3", OleDbType.Boolean).Value = cb6_3.Checked;
            cmd.Parameters.Add("@ck6_4", OleDbType.Boolean).Value = cb6_4.Checked;
            cmd.Parameters.Add("@ck7_1", OleDbType.Boolean).Value = cb7_1.Checked;
            cmd.Parameters.Add("@ck7_2", OleDbType.Boolean).Value = cb7_2.Checked;
            cmd.Parameters.Add("@ck7_3", OleDbType.Boolean).Value = cb7_3.Checked;
            cmd.Parameters.Add("@ck7_4", OleDbType.Boolean).Value = cb7_4.Checked;
            cmd.Parameters.Add("@ck7_5", OleDbType.Boolean).Value = cb7_5.Checked;
            cmd.Parameters.Add("@ck8_1", OleDbType.Boolean).Value = cb8_1.Checked;
            cmd.Parameters.Add("@ck8_2", OleDbType.Boolean).Value = cb8_2.Checked;
            cmd.Parameters.Add("@ck8_3", OleDbType.Boolean).Value = cb8_3.Checked;
            cmd.Parameters.Add("@ck9_1", OleDbType.Boolean).Value = cb9_1.Checked;
            cmd.Parameters.Add("@ck9_2", OleDbType.Boolean).Value = cb9_2.Checked;
            cmd.Parameters.Add("@ck9_3", OleDbType.Boolean).Value = cb9_3.Checked;
            cmd.Parameters.Add("@ck9_4", OleDbType.Boolean).Value = cb9_4.Checked;
            cmd.Parameters.Add("@ck9_5", OleDbType.Boolean).Value = cb9_5.Checked;
            cmd.Parameters.Add("@ck10_1", OleDbType.Boolean).Value = cb10_1.Checked;
            cmd.Parameters.Add("@ck10_2", OleDbType.Boolean).Value = cb10_2.Checked;
            cmd.Parameters.Add("@ck10_3", OleDbType.Boolean).Value = cb10_3.Checked;
            cmd.Parameters.Add("@ck11_1", OleDbType.Boolean).Value = cb11_1.Checked;
            cmd.Parameters.Add("@ck11_2", OleDbType.Boolean).Value = cb11_2.Checked;
            cmd.Parameters.Add("@ck11_3", OleDbType.Boolean).Value = cb11_3.Checked;
            cmd.Parameters.Add("@ck11_4", OleDbType.Boolean).Value = cb11_4.Checked;
            cmd.Parameters.Add("@ck12_1", OleDbType.Boolean).Value = cb12_1.Checked;
            cmd.Parameters.Add("@ck12_2", OleDbType.Boolean).Value = cb12_2.Checked;
            cmd.Parameters.Add("@ck12_3", OleDbType.Boolean).Value = cb12_3.Checked;
            cmd.Parameters.Add("@ck12_4", OleDbType.Boolean).Value = cb12_4.Checked;
            cmd.Parameters.Add("@ck12_5", OleDbType.Boolean).Value = cb12_5.Checked;
            cmd.Parameters.Add("@ck13_1", OleDbType.Boolean).Value = cb13_1.Checked;
            cmd.Parameters.Add("@text13_1", OleDbType.VarWChar).Value = tb13_1.Text;
            cmd.Parameters.Add("@text13_2", OleDbType.VarWChar).Value = tb13_2.Text;
            cmd.Parameters.Add("@ck13_2", OleDbType.Boolean).Value = cb13_2.Checked;
            cmd.Parameters.Add("@ck14_1", OleDbType.Boolean).Value = cb14_1.Checked;
            cmd.Parameters.Add("@ck14_2", OleDbType.Boolean).Value = cb14_2.Checked;
            cmd.Parameters.Add("@ck14_3", OleDbType.Boolean).Value = cb14_3.Checked;
            cmd.Parameters.Add("@ck14_4", OleDbType.Boolean).Value = cb14_4.Checked;
            cmd.Parameters.Add("@ck14_5", OleDbType.Boolean).Value = cb14_5.Checked;
            cmd.Parameters.Add("@ck14_6", OleDbType.Boolean).Value = cb14_6.Checked;
            cmd.Parameters.Add("@ck15_1", OleDbType.Boolean).Value = cb15_1.Checked;
            cmd.Parameters.Add("@ck15_2", OleDbType.Boolean).Value = cb15_2.Checked;
            cmd.Parameters.Add("@ck15_3", OleDbType.Boolean).Value = cb15_3.Checked;
            cmd.Parameters.Add("@ck15_4", OleDbType.Boolean).Value = cb15_4.Checked;
            cmd.Parameters.Add("@ck15_5", OleDbType.Boolean).Value = cb15_5.Checked;
            cmd.Parameters.Add("@text16_1", OleDbType.VarWChar).Value = tb16_1.Text;
            cmd.Parameters.Add("@text16_2", OleDbType.VarWChar).Value = tb16_2.Text;
            cmd.Parameters.Add("@text17_1", OleDbType.VarWChar).Value = tb17_1.Text;
            cmd.Parameters.Add("@text18_1", OleDbType.VarWChar).Value = tb18_1.Text;
            cmd.ExecuteNonQuery();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            ChartSearchForm csf = new ChartSearchForm();
            csf.ShowDialog();
            if (csf.DialogResult == DialogResult.OK)
            {
                connStr = String.Format(connStr, "auxology.mdb");
                OleDbConnection gConn = new OleDbConnection(connStr);
                string query = "select * from munjin1 where chart_num=@chart_num";
                OleDbCommand cmd = new OleDbCommand(query, gConn);
                cmd.Parameters.AddWithValue("@chart_num", Globalfunction.chart_num);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(cmd);
                gConn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    chartnum_tb.Text = dr[0].ToString();
                    name_tb.Text = dr[1].ToString();
                    regday = dr[2].ToString();
                    cb1_1.Checked = (bool)dr[3];
                    cb1_2.Checked = (bool)dr[4];
                    tb1_1.Text = dr[5].ToString();
                    cb2_1.Checked = (bool)dr[6];
                    cb2_2.Checked = (bool)dr[7];
                    cb2_3.Checked = (bool)dr[8];
                    cb2_4.Checked = (bool)dr[9];
                    cb2_5.Checked = (bool)dr[10];
                    tb2_1.Text = dr[11].ToString();
                    cb3_1.Checked = (bool)dr[12];
                    cb3_2.Checked = (bool)dr[13];
                    cb3_3.Checked = (bool)dr[14];
                    cb3_4.Checked = (bool)dr[15];
                    cb4_1.Checked = (bool)dr[16];
                    cb4_2.Checked = (bool)dr[17];
                    cb4_3.Checked = (bool)dr[18];
                    tb4_1.Text = dr[19].ToString();
                    cb5_1.Checked = (bool)dr[20];
                    cb5_2.Checked = (bool)dr[21];
                    cb5_3.Checked = (bool)dr[22];
                    tb5_1.Text = dr[23].ToString();
                    cb5_4.Checked = (bool)dr[24];
                    tb5_2.Text = dr[25].ToString();
                    tb5_3.Text = dr[26].ToString();
                    tb5_4.Text = dr[27].ToString();
                    cb6_1.Checked = (bool)dr[28];
                    cb6_2.Checked = (bool)dr[29];
                    tb6_1.Text = dr[30].ToString();
                    cb6_3.Checked = (bool)dr[31];
                    cb6_4.Checked = (bool)dr[32];
                    cb7_1.Checked = (bool)dr[33];
                    cb7_2.Checked = (bool)dr[34];
                    cb7_3.Checked = (bool)dr[35];
                    cb7_4.Checked = (bool)dr[36];
                    cb7_5.Checked = (bool)dr[37];
                    cb8_1.Checked = (bool)dr[38];
                    cb8_2.Checked = (bool)dr[39];
                    cb8_3.Checked = (bool)dr[40];
                    cb9_1.Checked = (bool)dr[41];
                    cb9_2.Checked = (bool)dr[42];
                    cb9_3.Checked = (bool)dr[43];
                    cb9_4.Checked = (bool)dr[44];
                    cb9_5.Checked = (bool)dr[45];
                    cb10_1.Checked = (bool)dr[46];
                    cb10_2.Checked = (bool)dr[47];
                    cb10_3.Checked = (bool)dr[48];
                    cb11_1.Checked = (bool)dr[49];
                    cb11_2.Checked = (bool)dr[50];
                    cb11_3.Checked = (bool)dr[51];
                    cb11_4.Checked = (bool)dr[52];
                    cb12_1.Checked = (bool)dr[53];
                    cb12_2.Checked = (bool)dr[54];
                    cb12_3.Checked = (bool)dr[55];
                    cb12_4.Checked = (bool)dr[56];
                    cb12_5.Checked = (bool)dr[57];
                    cb13_1.Checked = (bool)dr[58];
                    tb13_1.Text = dr[59].ToString();
                    tb13_2.Text = dr[60].ToString();
                    cb13_2.Checked = (bool)dr[61];
                    cb14_1.Checked = (bool)dr[62];
                    cb14_2.Checked = (bool)dr[63];
                    cb14_3.Checked = (bool)dr[64];
                    cb14_4.Checked = (bool)dr[65];
                    cb14_5.Checked = (bool)dr[66];
                    cb14_6.Checked = (bool)dr[67];
                    cb15_1.Checked = (bool)dr[68];
                    cb15_2.Checked = (bool)dr[69];
                    cb15_3.Checked = (bool)dr[70];
                    cb15_4.Checked = (bool)dr[71];
                    cb15_5.Checked = (bool)dr[72];
                    tb16_1.Text = dr[73].ToString();
                    tb16_2.Text = dr[74].ToString();
                    tb17_1.Text = dr[75].ToString();
                    tb18_1.Text = dr[76].ToString(); 
                }

                regday = Globalfunction.spilitDate(reg_date.Text);
                //birthday = Globalfunction.spilitDate(birth_date.Text);

                dr.Close();
                gConn.Close();
            }
            else
            {
                this.DialogResult = csf.DialogResult;
            }
        }
    }
}
