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
    public partial class ChartSearchForm : Form
    {
        string connStr = "provider = Microsoft.JET.OLEDB.4.0;Data Source={0};";

        public ChartSearchForm()
        {
            InitializeComponent();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string chartnum = "";
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);
            string query = "select chart_num, child_birth from P_INFO where child_name=@child_name order by chart_num asc" ;
            OleDbCommand cmd = new OleDbCommand(query, gConn);
            cmd.Parameters.AddWithValue("@child_name", search_tb.Text);
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(cmd);
            gConn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                chartnum = dr[0].ToString();
                string[] row = { dr[0].ToString(), search_tb.Text, dr[1].ToString().Substring(0, 10) };
                chart_list.Rows.Add(row);
            }
            

            dr.Close();
            gConn.Close();

        }

        private void chart_list_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {            
            Globalfunction.chart_num = chart_list.Rows[this.chart_list.CurrentCellAddress.Y].Cells[0].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }

        private void search_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_search_Click(sender, e);
            }
        }
    }
}
