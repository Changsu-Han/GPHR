using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using OpenCvSharp;
using AForge;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
namespace Auxology
{
    public partial class MainForm : Form
    {
        string connStr = "provider = Microsoft.JET.OLEDB.4.0;Data Source={0};";
        PInfoForm pinfo = new PInfoForm();//개인정보등록폼
        Munjin1Form munjin1 = new Munjin1Form();//문진표1폼
        PredictForm predict_form = new PredictForm();//신장예측폼
        
        public MainForm()
        {
            InitializeComponent();
        }

        private void 문진표1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new_Form(2);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Boolean access = check_lcns();
            if (access)
            {
                pinfo.MdiParent = this;
                pinfo.Show();
                panel1.Controls.Add(pinfo);
            }
            else
            {
                DoctorInfoForm di_Form = new DoctorInfoForm();//병원 정보입력폼
                di_Form.Owner = this;
                di_Form.ShowDialog();
                if (di_Form.DialogResult == DialogResult.OK)
                {
                    pinfo.MdiParent = this;
                    pinfo.Show();
                    panel1.Controls.Add(pinfo);
                }
                else
                {
                    this.DialogResult = di_Form.DialogResult;
                }
            }
            
            IplImage src = Cv.LoadImage("test4.jpg");
            
            IplImage dst = new IplImage(600,600, BitDepth.U8, 3);
            IplImage dst2 = new IplImage(600, 600, BitDepth.U8, 1);
            Cv.Resize(src,dst);
            IplImage result = new IplImage(600,600, BitDepth.U8, 3);
            IplImage result2 = new IplImage(600,600, BitDepth.U8, 3);
            Cv.CvtColor(dst, dst2, ColorConversion.BgrToGray);
            Cv.Threshold(dst2, dst2, 20, 255, ThresholdType.Binary);
            Cv.Smooth(dst2, dst2, SmoothType.Gaussian);
            Cv.NamedWindow("nonthresh");
            Cv.ShowImage("nonthresh", dst2);

            CvSeq<CvPoint> contours,contours2;
            CvMemStorage storage = new CvMemStorage();
            Cv.FindContours(dst2, storage, out contours, CvContour.SizeOf, ContourRetrieval.External, ContourChain.ApproxSimple);
            
            contours = Cv.ApproxPoly(contours, CvContour.SizeOf, storage, ApproxPolyMethod.DP, 3, true);
            contours2 = contours;

            Cv.DrawContours(result, contours, CvColor.Green,CvColor.Red,3);
            Cv.NamedWindow("img12");
            Cv.ShowImage("img12", result);

            CvSeq<CvPoint> first_contour;
            int i;
            int contour_max = 0;
            
            for (first_contour = contours; contours != null; contours = contours.HNext)
            {                
                if (contour_max < contours.Total)
                {
                    contour_max = contours.Total;
                }             
            }

            CvPoint[] ptseq = new CvPoint[contour_max];
            
            for (first_contour = contours2; contours2 != null; contours2 = contours2.HNext)
            {
                if (contours2.Total == contour_max)
                {
                    for (i = 0; i < contours2.Total; i++)
                    {
                        CvPoint? pt = Cv.GetSeqElem<CvPoint>(contours2, i);
                        ptseq[i] = new CvPoint { X = pt.Value.X, Y = pt.Value.Y };
                    }
                }
            }
            

            CvPoint[] hull;
            Cv.ConvexHull2(ptseq, out hull, ConvexHullOrientation.Counterclockwise);
            CvPoint pt0 = hull[hull.Length - 1];
            foreach(CvPoint pt in hull)
            {
                Cv.Line(dst, pt0, pt, CvColor.Green);
                pt0 = pt;
            }
            
            
            Cv.NamedWindow("img");
            Cv.ShowImage("img",dst);

            Cv.WaitKey();
            

        }

        private void Form1_Resize(object sender, EventArgs e)
        {/*
            if(this.WindowState == FormWindowState.Maximized)
            {
                pinfo.Hide();
                pinfo.Form_Maximized();
                pinfo.Show();
                pinfo.WindowState = FormWindowState.Maximized;
                
            }
            else
            {
                pinfo.Hide();
                pinfo.Form_Minimized();
                pinfo.Show();
            }*/
        }

        private void 신상정보ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new_Form(1);
        }

        private void 최대신장예측ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new_Form(3);
        }

        private void 코드관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //new_Form(3);
        }

        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();   
        }
        public void new_Form(int num)
        {
            if (num == 1)
            {
                panel1.Controls.Clear();
                pinfo.MdiParent = this;
                pinfo.Show();
                panel1.Controls.Add(pinfo);
            }
            if (num == 2)
            {
                panel1.Controls.Clear();
                munjin1.MdiParent = this;
                munjin1.Show();
                panel1.Controls.Add(munjin1);
            }
            if (num == 3)
            {
                panel1.Controls.Clear();
                predict_form.MdiParent = this;
                predict_form.Show();
                panel1.Controls.Add(predict_form);
            }
        }
        private Boolean check_lcns()
        {
            Boolean access_flag;
            string dt_name = "";
            connStr = String.Format(connStr, "auxology.mdb");
            OleDbConnection gConn = new OleDbConnection(connStr);
            DataSet myDataSet = new DataSet();
            string e_lcns = Globalfunction.Encrypt(Globalfunction.pc_lcns);
            string query = @"select dt_name from d_info where dt_mac=@dt_mac";
            
            OleDbCommand cmd = new OleDbCommand(query, gConn);
            cmd.Parameters.AddWithValue("@dt_mac", e_lcns);
            OleDbDataAdapter myDataAdapter = new OleDbDataAdapter(cmd);
            gConn.Open();
            OleDbDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                dt_name = dr[0].ToString();
            }
            
            if (dt_name != "")            
                access_flag = true;            
            else
                access_flag = false;
            dr.Close();
            gConn.Close();

            return access_flag;

        }        
    }
}
