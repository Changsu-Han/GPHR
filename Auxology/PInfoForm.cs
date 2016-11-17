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
    public partial class PInfoForm : Form
    {
        string connStr = "provider = Microsoft.JET.OLEDB.4.0;Data Source={0};";
        string birthday = "";
        string regday = "";

        public PInfoForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                xrayimage.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }

        private void PInfoForm_Load(object sender, EventArgs e)
        {   
            int chart;
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);
            DataSet myDataSet = new DataSet();
            string query = @"select top 1 chart_num from p_info order by chart_num desc";
            OleDbCommand cmd = new OleDbCommand(query, gConn);            
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(cmd);
            gConn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {   
                chart = Convert.ToInt16(dr[0].ToString())+1;
                Globalfunction.chart_num = chart.ToString();
                
            }
            if (Globalfunction.chart_num == "")
            {
                Globalfunction.chart_num = "1";
            }
            chartnum_tb.Text = Globalfunction.chart_num;
            dr.Close();
            gConn.Close();

            regday = Globalfunction.spilitDate(reg_date.Text);         
        }

        private void btn_reg_Click(object sender, EventArgs e)
        {
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);

            
            string grade = radioBtn_Check("cd");
            

            string field1 = "chart_num, doctor_name, reg_date, parent_name, parent_sex";
            string field2 = ",child_name, child_sex, child_birth, man_age, man_month, address_1, address_2,address_3, grade_1, grade_2, p_num1, p_num2, p_num3, hp_num1, hp_num2, hp_num3, homep_num1, homep_num2, homep_num3";
            string field3 = ",child_height, child_weight, child_bmi, past_height, past_weight, past_bmi";
            string field4 = ",father_name, father_height, mother_name, mother_height";

            string value1 = "@chart_num, @doctor_name, @reg_date, @parent_name, @parent_sex";
            string value2 = ",@child_name, @child_sex, @child_birth,  @man_age, @man_month, @address_1, @address_2, @address_3, @grade_1, @grade_2, @p_num1, @p_num2, @p_num3, @hp_num1, @hp_num2, @hp_num3, @homep_num1, @homep_num2, @homep_num3";
            string value3 = ",@child_height, @child_weight, @child_bmi, @past_height, @past_weight, @past_bmi";
            string value4 = ",@father_name, @father_height, @mother_name, @mother_height";

            string field = field1 + field2 + field3 + field4;
            string value = value1 + value2 + value3 + value4;
            string t_name = "P_INFO";
            string query = "insert into {0} ({1}) values({2})";

            query = String.Format(query, t_name, field, value);
            gConn.Open();
            OleDbCommand cmd = new OleDbCommand(query, gConn);

            cmd.Parameters.Add("@chart_num", OleDbType.VarWChar).Value = chartnum_tb.Text;
            cmd.Parameters.Add("@doctor_name", OleDbType.VarWChar).Value = dt_cb.Text;
            cmd.Parameters.Add("@reg_date", OleDbType.VarWChar).Value = regday;
            cmd.Parameters.Add("@parent_name", OleDbType.VarWChar).Value = parent_tb.Text;
            cmd.Parameters.Add("@parent_sex", OleDbType.VarWChar).Value = parent_cb.Text;

            cmd.Parameters.Add("@child_name", OleDbType.VarWChar).Value = patient_tb.Text;
            cmd.Parameters.Add("@child_sex", OleDbType.VarWChar).Value = patient_cb.Text;
            cmd.Parameters.Add("@child_birth", OleDbType.VarWChar).Value = birthday;
            cmd.Parameters.Add("@man_age", OleDbType.VarWChar).Value = man_tb1.Text;
            cmd.Parameters.Add("@man_month", OleDbType.VarWChar).Value = man_tb2.Text;
            cmd.Parameters.Add("@address_1", OleDbType.VarWChar).Value = adrs_tb1.Text;
            cmd.Parameters.Add("@address_2", OleDbType.VarWChar).Value = adrs_tb2.Text;
            cmd.Parameters.Add("@address_3", OleDbType.VarWChar).Value = adrs_tb3.Text;
            cmd.Parameters.Add("@grade_1", OleDbType.VarWChar).Value = grade;
            cmd.Parameters.Add("@grade_2", OleDbType.VarWChar).Value = gr_tb1.Text;
            cmd.Parameters.Add("@p_num1", OleDbType.VarWChar).Value = pn_tb1.Text;
            cmd.Parameters.Add("@p_num2", OleDbType.VarWChar).Value = pn_tb2.Text;
            cmd.Parameters.Add("@p_num3", OleDbType.VarWChar).Value = pn_tb3.Text;
            cmd.Parameters.Add("@hp_num1", OleDbType.VarWChar).Value = hp_tb1.Text;
            cmd.Parameters.Add("@hp_num2", OleDbType.VarWChar).Value = hp_tb2.Text;
            cmd.Parameters.Add("@hp_num3", OleDbType.VarWChar).Value = hp_tb3.Text;
            cmd.Parameters.Add("@homep_num1", OleDbType.VarWChar).Value = hpn_tb1.Text;
            cmd.Parameters.Add("@homep_num2", OleDbType.VarWChar).Value = hpn_tb2.Text;
            cmd.Parameters.Add("@homep_num3", OleDbType.VarWChar).Value = hpn_tb3.Text;

            cmd.Parameters.Add("@child_height", OleDbType.VarWChar).Value = h_tb.Text;
            cmd.Parameters.Add("@child_weight", OleDbType.VarWChar).Value = w_tb.Text;
            cmd.Parameters.Add("@child_bmi", OleDbType.VarWChar).Value = bmi_tb.Text;
            cmd.Parameters.Add("@past_height", OleDbType.VarWChar).Value = ph_tb.Text;
            cmd.Parameters.Add("@past_weight", OleDbType.VarWChar).Value = pw_tb.Text;
            cmd.Parameters.Add("@past_bmi", OleDbType.VarWChar).Value = pbmi_tb.Text;

            cmd.Parameters.Add("@father_name", OleDbType.VarWChar).Value = fname_tb.Text;
            cmd.Parameters.Add("@father_height", OleDbType.VarWChar).Value = fh_tb.Text;
            cmd.Parameters.Add("@mother_name", OleDbType.VarWChar).Value = mname_tb.Text;
            cmd.Parameters.Add("@mother_height", OleDbType.VarWChar).Value = mh_tb.Text;
            cmd.ExecuteNonQuery();
        }

        private string radioBtn_Check(string str)
        {
            string grade = "";
            //클라이언트->db
            if (str == "cd")
            {
                if (gr_rb1.Checked == true)
                {
                    grade = "초";
                }
                else if (gr_rb2.Checked == true)
                {
                    grade = "중";
                }
                else
                    grade = "고";
            }
            //db->클라이언트
            else
            {
                if (str == "초")
                {
                    gr_rb1.Checked = true;                   
                }
                if (str == "중")
                {
                    gr_rb2.Checked = true;
                }
                if (str == "고")
                {
                    gr_rb3.Checked = true;
                }
            }
            

            return grade;
        }

        private void Man_Age_Cal(string birth, string reg)
        {
            int manage = 0;
            int rm_bm = 0;
            int rd_bd = 0;
            int ryear = Convert.ToInt16(reg.Substring(0,4));
            int byear = Convert.ToInt16(birth.Substring(0, 4));
            int rmonth = Convert.ToInt16(reg.Substring(5, 2));
            int bmonth = Convert.ToInt16(birth.Substring(5, 2));
            int rday = Convert.ToInt16(reg.Substring(8, 2));
            int bday = Convert.ToInt16(birth.Substring(8, 2));

            manage = ryear - byear;
            rm_bm = rmonth - bmonth;
            rd_bd = rday - bday;
            if (rm_bm < 0)
            {
                manage = manage - 1;
                if (rd_bd < 0)
                {
                    rm_bm = 12 + rm_bm - 1;
                }
                else
                {
                    rm_bm = 12 + rm_bm;
                }

                man_tb1.Text = manage.ToString();
                man_tb2.Text = rm_bm.ToString();
            }
            else if (rm_bm == 0)
            {
                if (rd_bd < 0)
                {
                    manage = manage - 1;
                    rm_bm = 11;
                }
                else
                {
                    rm_bm = 0;
                }

                man_tb1.Text = manage.ToString();
                man_tb2.Text = rm_bm.ToString();
            }
            else
            {
                if (rd_bd < 0)
                    rm_bm = rm_bm - 1;
                man_tb1.Text = manage.ToString();
                man_tb2.Text = rm_bm.ToString();
            }

        }

        private void reg_date_CloseUp(object sender, EventArgs e)
        {            
            regday = Globalfunction.spilitDate(reg_date.Text);            
        }

        private void birth_date_CloseUp(object sender, EventArgs e)
        {
            birthday = Globalfunction.spilitDate(birth_date.Text);             
            Man_Age_Cal(birthday, regday);
        }

        private void btn_search1_Click(object sender, EventArgs e)
        {
            ChartSearchForm csf = new ChartSearchForm();
            csf.ShowDialog();
            if (csf.DialogResult == DialogResult.OK)
            {
                connStr = String.Format(connStr, "auxology.mdb");
                OleDbConnection gConn = new OleDbConnection(connStr);
                string query = "select * from P_INFO where chart_num=@chart_num";
                OleDbCommand cmd = new OleDbCommand(query, gConn);
                cmd.Parameters.AddWithValue("@chart_num", Globalfunction.chart_num);
                OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(cmd);
                gConn.Open();
                OleDbDataReader dr = cmd.ExecuteReader();
                string gr_rb = "gr_rd";
                while (dr.Read())
                {
                    chartnum_tb.Text = dr[0].ToString();
                    dt_cb.Text = dr[1].ToString();
                    reg_date.Text = dr[2].ToString(); 
                    parent_tb.Text = dr[3].ToString(); 
                    parent_cb.Text = dr[4].ToString();

                    patient_tb.Text = dr[5].ToString();
                    patient_cb.Text = dr[6].ToString();
                    birth_date.Text = dr[7].ToString();
                    man_tb1.Text = dr[8].ToString();
                    man_tb2.Text = dr[9].ToString();
                    adrs_tb1.Text = dr[10].ToString();
                    adrs_tb2.Text = dr[11].ToString();
                    adrs_tb3.Text = dr[12].ToString();
                    gr_rb = radioBtn_Check(dr[13].ToString());
                    gr_tb1.Text = dr[14].ToString();
                    pn_tb1.Text = dr[15].ToString();
                    pn_tb2.Text = dr[16].ToString();
                    pn_tb3.Text = dr[17].ToString();
                    hp_tb1.Text = dr[18].ToString();
                    hp_tb2.Text = dr[19].ToString();
                    hp_tb3.Text = dr[20].ToString();
                    hpn_tb1.Text = dr[21].ToString();
                    hpn_tb2.Text = dr[22].ToString();
                    hpn_tb3.Text = dr[23].ToString();

                    h_tb.Text = dr[24].ToString();
                    w_tb.Text = dr[25].ToString();
                    bmi_tb.Text = dr[26].ToString();
                    ph_tb.Text = dr[27].ToString();
                    pw_tb.Text = dr[28].ToString();
                    pbmi_tb.Text = dr[29].ToString();

                    fname_tb.Text = dr[30].ToString();
                    fh_tb.Text = dr[31].ToString();
                    mname_tb.Text = dr[32].ToString();
                    mh_tb.Text = dr[33].ToString();
                }

                regday = Globalfunction.spilitDate(reg_date.Text);
                birthday = Globalfunction.spilitDate(birth_date.Text);

                dr.Close();
                gConn.Close();
            }
            else
            {
                this.DialogResult = csf.DialogResult;
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            chartnum_tb.Text = Globalfunction.chart_num;
            dt_cb.Text = "";
            reg_date.Text = "";
            parent_tb.Text = "";
            parent_cb.Text = "";

            patient_tb.Text = "";
            patient_cb.Text = "";
            birth_date.Text = "";
            man_tb1.Text = "";
            man_tb2.Text = "";
            adrs_tb1.Text = "";
            adrs_tb2.Text = "";
            adrs_tb3.Text = "";
            gr_tb1.Text = "";
            pn_tb1.Text = "";
            pn_tb2.Text = "";
            pn_tb3.Text = "";
            hp_tb1.Text = "";
            hp_tb2.Text = "";
            hp_tb3.Text = "";
            hpn_tb1.Text = "";
            hpn_tb2.Text = "";
            hpn_tb3.Text = "";

            h_tb.Text = "";
            w_tb.Text = "";
            bmi_tb.Text = "";
            ph_tb.Text = "";
            pw_tb.Text = "";
            pbmi_tb.Text = "";

            fname_tb.Text = "";
            fh_tb.Text = "";
            mname_tb.Text = "";
            mh_tb.Text = "";
        }
    }
}
