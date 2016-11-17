using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;



using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;
using System.Drawing.Imaging;

namespace Auxology
{
    public partial class PredictForm : Form
    {
        private Otsu ot = new Otsu();
        System.Drawing.Image image;

        string openstrFilename = null;
        string filePath;
        Bitmap myBitmap;            //원본이미지 -> 노이즈제거한 손 외곽선  추출 이미지
        Bitmap edge;                //지골 및 수골의 서브이미지의 외곽선 추출 이미지
        Bitmap gsImage;             //서브이미지의 그레이스케일
        Bitmap resizeImage;          //서브이미지의 사이즈 정규화 
        CannyEdgeDetector filter;
        //지골및 수골의 위치별 서브 이미지 
        private Bitmap CroppedImage1; //요골
        private Bitmap CroppedImage2; //
        private Bitmap CroppedImage3;
        private Bitmap CroppedImage4;
        private Bitmap CroppedImage5;
        private Bitmap CroppedImage6;
        private Bitmap CroppedImage7;
        private Bitmap CroppedImage8;
        private Bitmap CroppedImage9;
        private Bitmap CroppedImage10;
        private Bitmap CroppedImage11;
        private Bitmap CroppedImage12;
        private Bitmap CroppedImage13;

        public PredictForm()
        {
            InitializeComponent();
        }

        private void PredictForm_Load(object sender, EventArgs e)
        {
            chartnum_tb.Text = Globalfunction.chart_num;

            //regday = Globalfunction.spilitDate(reg_date.Text);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            //bright = 0;
            //  value = 10;

            openFileDialog1.Title = "영상파일 열기";
            openFileDialog1.Filter = "All Files(*.*)|*.*| Bitmap File(*.bmp)|*.bmp|GIF File(*.gif)|*.gif|JPEG File(*.jpg)|*.jpg|PNG file(*.png)|*.png|TIFF(*.tif)|*.tif";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openstrFilename = openFileDialog1.FileName;
                image = System.Drawing.Image.FromFile(openstrFilename);
                myBitmap = new Bitmap(image);
                this.xray_preview.Image = myBitmap;// 원 이미지를 담는 PictureBox 개체        
            }

            //임시화일삭제
            string filePath = @"Image\temp.jpg";
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
            }

            /*
             *손바닥 외곽선 추출   
             */
            Bitmap CroppedImage = myBitmap.Clone(new System.Drawing.Rectangle(0, 100, myBitmap.Width, (myBitmap.Height - 100)), myBitmap.PixelFormat); //손바닥          
            int width = 600;
            int height = 600;
            Size resize = new Size(width, height);
            resizeImage = new Bitmap(CroppedImage, resize);

            gsImage = Grayscale.CommonAlgorithms.BT709.Apply(resizeImage);
            filter = new CannyEdgeDetector();
            edge = filter.Apply(gsImage);


            //외곽선 블러링
            Blur hfilter = new Blur();
            // apply the filter
            hfilter.ApplyInPlace(edge);

            ///////////////////////////
            // process image with blob counter
            BlobCounter hblobCounter = new BlobCounter();
            hblobCounter.ProcessImage(edge);
            Blob[] hblobs = hblobCounter.GetObjectsInformation();

            // create convex hull searching algorithm
            GrahamConvexHull hhullFinder = new GrahamConvexHull();

            // lock image to draw on it
            BitmapData hdata = edge.LockBits(new Rectangle(0, 0, edge.Width, edge.Height), ImageLockMode.ReadWrite, edge.PixelFormat);

            // process each blob
            List<IntPoint> hhull = new List<IntPoint> { };
            List<IntPoint> hedgePoints = new List<IntPoint> { };
            int hblobcount = 0;
            int hminX = 0, hmaxX = 700, hminY = 0, hmaxY = 700;

            foreach (Blob blob in hblobs)
            {
                List<IntPoint> leftPoints, rightPoints;
                // get blob's edge points
                hblobCounter.GetBlobsLeftAndRightEdges(blob, out leftPoints, out rightPoints);

                hedgePoints.AddRange(leftPoints);
                hedgePoints.AddRange(rightPoints);

                // blob's convex hull
                hhull = hhullFinder.FindHull(hedgePoints);

                foreach (IntPoint hulls in hhull)
                { // convexhull 최외곽선 추출 
                    if (hblobcount == 0)
                    {
                        hminX = hulls.X; hmaxX = hulls.X;
                        hminY = hulls.Y; hmaxY = hulls.Y;
                    }

                    if (hminX > hulls.X)
                        hminX = hulls.X;
                    else if (hmaxX < hulls.X)
                        hmaxX = hulls.X;

                    if (hminY > hulls.Y)
                        hminY = hulls.Y;
                    else if (hmaxY < hulls.Y)
                        hmaxY = hulls.Y;
                    hblobcount++;
                }
                Drawing.Polygon(hdata, hhull, Color.White);
            }

            edge = edge.Clone(new Rectangle(hminX, hminY, hmaxX - hminX, hmaxY - hminY), myBitmap.PixelFormat);
            this.xray_preview.Image = edge;


            ///////////////////////////////////           
            //수골 및 지골 분할 및 특징 추출
            //손목 : 요골 및 척골 (2곳)
            //손바닥 : 제1,3,5지 중수골 (3곳)
            //손가락 : 제1,3,5지 기절골 및 말절골 (6곳)
            //손가락 : 제3. 5지 중수골 (2곳)
            ///////////////////////////////

            //요골 추출 및 인지 알고리즘 
            CroppedImage1 = myBitmap.Clone(new System.Drawing.Rectangle(270, 620, 250, 180), myBitmap.PixelFormat); //1. 요골

            //이미지 사이즈 정규화 요골 크롭 이미지 250 X 180 -> 125 X 125
            CroppedImage1 = CroppedImage1.Clone(new System.Drawing.Rectangle(10, 0, 230, 150), myBitmap.PixelFormat); 
            width = 125;
            height = 125;
            resize = new Size(width, height);
            resizeImage = new Bitmap(CroppedImage1, resize);

            //전처리 및 특징 추출 루틴 
            //DetectCorners(CroppedImage1);
            gsImage = Grayscale.CommonAlgorithms.BT709.Apply(resizeImage);
            filter = new CannyEdgeDetector();
            edge = filter.Apply(gsImage);

            //외곽선 블러링
            Blur Bfilter = new Blur();
            // apply the filter
            Bfilter.ApplyInPlace(edge);            

            ///////////////////////////
            // process image with blob counter
            BlobCounter blobCounter = new BlobCounter();
            blobCounter.ProcessImage(edge);
            Blob[] blobs = blobCounter.GetObjectsInformation();

            // create convex hull searching algorithm
            GrahamConvexHull hullFinder = new GrahamConvexHull();

            // lock image to draw on it
            BitmapData data = edge.LockBits( new Rectangle(0, 0, edge.Width, edge.Height), ImageLockMode.ReadWrite, edge.PixelFormat);

            // process each blob
            List<IntPoint> hull = new List<IntPoint>{ };
            List<IntPoint> edgePoints = new List<IntPoint> { };
            int blobcount=0;
            int minX=0, maxX=125, minY=0, maxY=125;
            foreach (Blob blob in blobs)
            {
                List<IntPoint> leftPoints, rightPoints;
                // get blob's edge points
                blobCounter.GetBlobsLeftAndRightEdges(blob, out leftPoints, out rightPoints);

                edgePoints.AddRange(leftPoints);
                edgePoints.AddRange(rightPoints);

                // blob's convex hull
                hull = hullFinder.FindHull(edgePoints);

                foreach (IntPoint hulls in hull) { // convexhull 최외곽선 추출 
                    if (blobcount == 0) {
                        minX = hulls.X; maxX = hulls.X;
                        minY = hulls.Y; maxY = hulls.Y;
                    }
                    if (minX > hulls.X)
                        minX = hulls.X;
                    else if (maxX < hulls.X)
                        maxX = hulls.X;

                    if (minY > hulls.Y)
                        minY = hulls.Y;
                    else if (maxY < hulls.Y)
                        maxY = hulls.Y;
                    blobcount++;
                }                
                Drawing.Polygon(data, hull, Color.White);
            }

            edge = resizeImage.Clone(new System.Drawing.Rectangle(minX, minY, maxX- minX, maxY-minY), myBitmap.PixelFormat);
            CroppedImage1.Save(@"Image\temp.jpg", ImageFormat.Jpeg);
            this.pB_radius.Image = edge;
            /////////////////////////////////////////////////////////////
            

            ////////////////////////
            //척골 
            //////////////
            CroppedImage2 = myBitmap.Clone(new System.Drawing.Rectangle(133, 620, 200, 180), myBitmap.PixelFormat); //2. 척골
                                                                                                                    //이미지 사이즈 정규화 요골 크롭 이미지 250 X 180 -> 125 X 125
            CroppedImage2 = CroppedImage2.Clone(new System.Drawing.Rectangle(0,20, 200, 150), myBitmap.PixelFormat);
            width = 125;
            height = 125;
            resize = new Size(width, height);
            resizeImage = new Bitmap(CroppedImage2, resize);

            //전처리 및 특징 추출 루틴 
            //DetectCorners(CroppedImage1);
            gsImage = Grayscale.CommonAlgorithms.BT709.Apply(resizeImage);
            filter = new CannyEdgeDetector();
            edge = filter.Apply(gsImage);

            //외곽선 블러링
            Bfilter = new Blur();
            // apply the filter
            Bfilter.ApplyInPlace(edge);

            ///////////////////////////
            // process image with blob counter
            blobCounter = new BlobCounter();
            blobCounter.ProcessImage(edge);
            blobs = blobCounter.GetObjectsInformation();

            // create convex hull searching algorithm
            hullFinder = new GrahamConvexHull();

            // lock image to draw on it
            BitmapData data1 = edge.LockBits(new Rectangle(0, 0, edge.Width, edge.Height), ImageLockMode.ReadWrite, edge.PixelFormat);

            // process each blob
            hull = new List<IntPoint> { };
            edgePoints = new List<IntPoint> { };
            blobcount = 0;
            minX = 0; maxX = 125; minY = 0; maxY = 125;
            foreach (Blob blob in blobs)
            {
                List<IntPoint> leftPoints, rightPoints;
                // get blob's edge points
                blobCounter.GetBlobsLeftAndRightEdges(blob, out leftPoints, out rightPoints);

                edgePoints.AddRange(leftPoints);
                edgePoints.AddRange(rightPoints);

                // blob's convex hull
                hull = hullFinder.FindHull(edgePoints);

                foreach (IntPoint hulls in hull)
                { // convexhull 최외곽선 추출 
                    if (blobcount == 0)
                    {
                        minX = hulls.X; maxX = hulls.X;
                        minY = hulls.Y; maxY = hulls.Y;
                    }
                    if (minX > hulls.X)
                        minX = hulls.X;
                    else if (maxX < hulls.X)
                        maxX = hulls.X;

                    if (minY > hulls.Y)
                        minY = hulls.Y;
                    else if (maxY < hulls.Y)
                        maxY = hulls.Y;
                    blobcount++;
                }
                Drawing.Polygon(data1, hull, Color.White);
            }
            Bitmap edge1 = resizeImage.Clone(new System.Drawing.Rectangle(minX, minY, (maxX - minX), (maxY - minY)), myBitmap.PixelFormat);
            this.pB_ulna.Image = edge1;
            ///////////////////////////////////////
            

            CroppedImage3 = myBitmap.Clone(new System.Drawing.Rectangle(390, 500, 180, 180), myBitmap.PixelFormat); //3. 제1지 중수골
            resizeImage = new Bitmap(CroppedImage3, resize);
            this.pB_Met1.Image = CroppedImage3;

            CroppedImage4 = myBitmap.Clone(new System.Drawing.Rectangle(266, 266, 180, 180), myBitmap.PixelFormat); //4.제3지 기절골/중절골
            resizeImage = new Bitmap(CroppedImage4, resize);
            this.pB_Met3.Image = CroppedImage4;

            CroppedImage5 = myBitmap.Clone(new System.Drawing.Rectangle(75, 335, 180, 180), myBitmap.PixelFormat); //5. 제5지 중수골/기절골
            resizeImage = new Bitmap(CroppedImage5, resize);
            this.pB_Met5.Image = CroppedImage5;

            CroppedImage6 = myBitmap.Clone(new System.Drawing.Rectangle(534, 410, 180, 180), myBitmap.PixelFormat); //6. 제1지 기절골
            resizeImage = new Bitmap(CroppedImage6, resize);
            this.pB_Pph1.Image = CroppedImage6;

            CroppedImage7 = myBitmap.Clone(new System.Drawing.Rectangle(266, 266, 180, 180), myBitmap.PixelFormat); //7. 제3지 기절골/중절골
            resizeImage = new Bitmap(CroppedImage7, resize);
            this.pB_Pph3.Image = CroppedImage7;

            CroppedImage8 = myBitmap.Clone(new System.Drawing.Rectangle(75, 335, 180, 180), myBitmap.PixelFormat); //8. 제5지 중수골/기절골
            resizeImage = new Bitmap(CroppedImage8, resize);
            this.pB_Pph5.Image = CroppedImage8;

            CroppedImage9 = myBitmap.Clone(new System.Drawing.Rectangle(260, 110, 180, 180), myBitmap.PixelFormat); //9. 제3자 중절골
            resizeImage = new Bitmap(CroppedImage9, resize);
            this.pB_Mph3.Image = CroppedImage9;

            CroppedImage10 = myBitmap.Clone(new System.Drawing.Rectangle(0, 250, 180, 180), myBitmap.PixelFormat); //10. 제5지 중절골
            resizeImage = new Bitmap(CroppedImage10, resize);
            this.pB_Mph5.Image = CroppedImage10;

            CroppedImage11 = myBitmap.Clone(new System.Drawing.Rectangle(620, 320, 180, 180), myBitmap.PixelFormat); //11. 제1지 말절골
            resizeImage = new Bitmap(CroppedImage11, resize);
            this.pB_Dph1.Image = CroppedImage11;

            CroppedImage12 = myBitmap.Clone(new System.Drawing.Rectangle(260, 0, 180, 180), myBitmap.PixelFormat); //12. 제3지 말절골
            resizeImage = new Bitmap(CroppedImage12, resize);
            this.pB_Dph3.Image = CroppedImage12;
            
            CroppedImage13 = myBitmap.Clone(new System.Drawing.Rectangle(0, 133, 180, 180), myBitmap.PixelFormat); //13. 제5지 말절골
            resizeImage = new Bitmap(CroppedImage13, resize);
            this.pB_Dph5.Image = CroppedImage13;

            //edge.UnlockBits(data1);
            //edge.UnlockBits(data);
           
        }

        private void tw3picturInit()  //TW3 이미지박스 초기화
        {
            pB_TW3B.Image = null;
            pB_TW3C.Image = null;
            pB_TW3D.Image = null;
            pB_TW3E.Image = null;
            pB_TW3F.Image = null;
            pB_TW3G.Image = null;
            pB_TW3H.Image = null;
            pB_TW3I.Image = null;

            filePath = Application.StartupPath;
        }

        /// <summary>
        /// TW3 이미지 불러오기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pB_radius_Click(object sender, EventArgs e) //1. 요골
        {
            tw3picturInit();

            const string imageCase = @"Image\temp.jpg";

            const string imageCase1 = @"Image\img\M\1 ra\M-g_ra23.jpg";
            const string imageCase2 = @"Image\img\M\1 ra\M-g_ra30.jpg";
            const string imageCase3 = @"Image\img\M\1 ra\M-g_ra44.jpg";
            const string imageCase4 = @"Image\img\M\1 ra\M-g_ra56.jpg";
            const string imageCase5 = @"Image\img\M\1 ra\M-g_ra78.jpg";
            const string imageCase6 = @"Image\img\M\1 ra\M-g_ra114.jpg";
            const string imageCase7 = @"Image\img\M\1 ra\M-g_ra160.jpg";
            const string imageCase8 = @"Image\img\M\1 ra\M-g_ra218.jpg";

            this.pB_TW3B.Image = System.Drawing.Image.FromFile(imageCase1);
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(imageCase2);
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(imageCase3);
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(imageCase4);
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(imageCase5);
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(imageCase6);
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(imageCase7);
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(imageCase8);

            TemplateMatching(imageCase, imageCase1, lab_B.Name);
            TemplateMatching(imageCase, imageCase2, lab_C.Name);
            TemplateMatching(imageCase, imageCase3, lab_D.Name);
            TemplateMatching(imageCase, imageCase4, lab_E.Name);
            TemplateMatching(imageCase, imageCase5, lab_F.Name);
            TemplateMatching(imageCase, imageCase6, lab_G.Name);
            TemplateMatching(imageCase, imageCase7, lab_H.Name);
            TemplateMatching(imageCase, imageCase8, lab_I.Name);

        }

        private void pB_ulna_Click(object sender, EventArgs e) //2. 척골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\2 ul\M-g_ul30.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\m\2 ul\M-g_ul33.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\m\2 ul\M-g_ul37.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\m\2 ul\M-g_ul45.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\m\2 ul\M-g_ul74.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\m\2 ul\M-g_ul118.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\m\2 ul\M-g_ul173.jpg");
        }

        private void pB_Met1_Click(object sender, EventArgs e)//3. 제1지 중수골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi8.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi12.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi18.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi24.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi31.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi43.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi53.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\3 fi\M-g_fi67.jpg");
        }

        private void pB_Met3_Click(object sender, EventArgs e)  //4. 제3지 중수골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th5.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th8.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th12.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th16.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th23.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th37.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th47.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\4 th\M-g_th53.jpg");

        }

        private void pB_Met5_Click(object sender, EventArgs e) //5. 제5지 중수골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\5 me\M-g_me5.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\5 me\M-g_me47.jpg");

        }

        private void pB_Pph1_Click(object sender, EventArgs e) //6. 제1기절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr9.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr11.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr14.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr20.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr31.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr44.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr56.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\6 pr\M-g_pr67.jpg");

        }

        private void pB_Pph3_Click(object sender, EventArgs e) //7. 제3지 기절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp5.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp7.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp12.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp19.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp27.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp37.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp44.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\7 prp\M-g_prp54.jpg");
        }

        private void pB_Pph5_Click(object sender, EventArgs e)//8. 제3지 기절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\8 fi\M-g_fi4.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\8 fi\M-g_fi44.jpg");

        }

        private void pB_Mph3_Click(object sender, EventArgs e)//9. 제3지 중절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp5.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp8.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp12.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp18.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp27.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp36.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp45.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\9 mp\M-g_mp52.jpg");

        }

        private void pB_Mph5_Click(object sender, EventArgs e)//10.제5지 중절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\10 ff\M-g_ff9.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\10 ff\M-g_ff45.jpg");
        }

        private void pB_Dph1_Click(object sender, EventArgs e)//11.제5지 중절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp7.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp9.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp15.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp22.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp33.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp48.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp51.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\11 dp\M-g_dp68.jpg");
        }

        private void pB_Dph3_Click(object sender, EventArgs e)//12.제5지 중절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt7.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt8.jpg");
            this.pB_TW3D.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt11.jpg");
            this.pB_TW3E.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt15.jpg");
            this.pB_TW3F.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt22.jpg");
            this.pB_TW3G.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt33.jpg");
            this.pB_TW3H.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt37.jpg");
            this.pB_TW3I.Image = System.Drawing.Image.FromFile(@"Image\img\M\12 dpt\M-g_dpt49.jpg");

        }

        private void pB_Dph5_Click(object sender, EventArgs e)//13.제5지 중절골
        {
            tw3picturInit();
            this.pB_TW3B.Image = System.Drawing.Image.FromFile(@"Image\img\M\13 ff\M-g_ff6.jpg");
            this.pB_TW3C.Image = System.Drawing.Image.FromFile(@"Image\img\M\13 ff\M-g_ff37.jpg");

        }

        private void ImageHistogram(Bitmap bmp)  // 이미지 평활화 함수
        {
            HistogramEqualization filter = new HistogramEqualization();
            filter.ApplyInPlace(bmp);
        }

        public void DetectCorners(Bitmap bmp)
        {
            // Load image and create everything you need for drawing

            Graphics graphics = Graphics.FromImage(bmp);
            SolidBrush brush = new SolidBrush(Color.Red);
            Pen pen = new Pen(brush);

            // Create corner detector and have it process the image
            MoravecCornersDetector mcd = new MoravecCornersDetector();
            //SusanCornersDetector scd = new SusanCornersDetector();
            List<IntPoint> corners = mcd.ProcessImage(bmp);

            // Visualization: Draw 3x3 boxes around the corners
            foreach (IntPoint corner in corners)
            {
                graphics.DrawRectangle(pen, corner.X - 1, corner.Y - 1, 3, 3);
            }

            // Display
            pB_radius.Image = bmp;
        }

        private static Size GetSize(Size maxSize, Size size)
        {
            double ratioWidth = (double)maxSize.Width / size.Width;
            double ratioHeight = (double)maxSize.Height / size.Height;
            double ratio = Math.Min(ratioWidth, ratioHeight);
            return new Size((int)Math.Floor(size.Width * ratio), (int)Math.Floor(size.Height * ratio));

        }

        private void TemplateMatching(string tempimage, string twimage, string lableValue)
        {
            const float similarityThreshold = 0.20f;

            ExhaustiveTemplateMatching exhaustiveTemplateMatching = new ExhaustiveTemplateMatching(similarityThreshold);

            Bitmap bitmap1 = new Bitmap(tempimage);
            Bitmap bitmap2 = new Bitmap(twimage);

            Size size = GetSize(bitmap1.Size, bitmap2.Size);
            ResizeBilinear filter = new ResizeBilinear(size.Width, size.Height);
            bitmap2 = filter.Apply(bitmap2);

            TemplateMatch[] matches = exhaustiveTemplateMatching.ProcessImage(bitmap1, bitmap2);
            if (matches.Length <= 0)

                Console.WriteLine("Not even matched");
            else
            {
                float similarity = 0.10f;
                bool isMatch = matches[0].Similarity >= similarity;
                if (lableValue == lab_B.Name)
                {
                    lab_B.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_C.Name)
                {
                    lab_C.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_C.Name)
                {
                    lab_C.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_D.Name)
                {
                    lab_D.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_E.Name)
                {
                    lab_E.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_F.Name)
                {
                    lab_F.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_G.Name)
                {
                    lab_G.Text = matches[0].Similarity.ToString();
                }
                else if (lableValue == lab_H.Name)
                {
                    lab_H.Text = matches[0].Similarity.ToString();
                }
                else
                {
                    lab_I.Text = matches[0].Similarity.ToString();
                }
            }
            bitmap1.Dispose();
        }
    }
}
