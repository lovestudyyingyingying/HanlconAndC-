
using OpcUaHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;
using static System.Windows.Forms.MonthCalendar;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        HWindow hw_home;
        HObject ReadImage, GenRectangle1, ho_ModelContours;
        HTuple hv_ModelID;
        private HTuple hv_Area;
        private HTuple hv_Row;
        private HTuple hv_Column;
        private HTuple hv_HomMat2D,hv_HomMat2DTranslate;
        private HObject ho_ContoursAffineTrans;
        private HTuple hv_Row1;
        private HTuple hv_Column1;
        private HTuple hv_Angle;
        private HTuple hv_Score;
        private HTuple hv_HomMat2DIdentity;
        private HTuple hv_HomMat2DRotate;
        private HObject ho_Rectangle1;
        private HObject ho_ContoursAffineTrans1;

        private void button1_Click(object sender, EventArgs e)
        {
            string fName = string.Empty;
            using (OpenFileDialog open = new OpenFileDialog())
            {
               
                DialogResult result = open.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    fName = open.FileName;
                }
                else
                    return;
                HOperatorSet.ReadImage(out ReadImage, fName);
                HOperatorSet.GetImageSize(ReadImage, out var Imagewidth, out var Imageheight);
                HOperatorSet.SetPart(hw_home, 0, 0, Imagewidth - 1, Imageheight - 1);
                HOperatorSet.DispObj(ReadImage, hw_home);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hw_home = hWindowControl1.HalconWindow;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HOperatorSet.FindShapeModel(ReadImage, hv_ModelID, (new HTuple(-180)).TupleRad()
       , (new HTuple(180)).TupleRad(), 0.5, 0, 0.5, "least_squares", 0, 0.9, out hv_Row1,
       out hv_Column1, out hv_Angle, out hv_Score);
            HOperatorSet.DispObj(ReadImage,hw_home);

            for (int hv_i = 0; hv_i <= (int)((new HTuple(hv_Score.TupleLength())) - 1); hv_i = hv_i + 1)
            {
                HOperatorSet.HomMat2dIdentity(out hv_HomMat2DIdentity);
               
                  //  hv_HomMat2DTranslate.Dispose();
                    HOperatorSet.HomMat2dTranslate(hv_HomMat2DIdentity, hv_Row1.TupleSelect(hv_i),
                        hv_Column1.TupleSelect(hv_i), out hv_HomMat2DTranslate);
                
                    HOperatorSet.HomMat2dRotate(hv_HomMat2DTranslate, hv_Angle.TupleSelect(hv_i),
                        hv_Row1.TupleSelect(hv_i), hv_Column1.TupleSelect(hv_i), out hv_HomMat2DRotate);
                
             
              
                    HOperatorSet.GenRectangle2(out ho_Rectangle1, hv_Row1.TupleSelect(hv_i), hv_Column1.TupleSelect(
                        hv_i), hv_Angle.TupleSelect(hv_i), 30, 30);
               
           
                    HOperatorSet.SetDraw(hw_home, "margin");
             
            
                HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffineTrans1,
                    hv_HomMat2DRotate);
        
      
                    HOperatorSet.DispObj(ho_Rectangle1, hw_home);           
                    HOperatorSet.DispObj(ho_ContoursAffineTrans1, hw_home );
                
            }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            HOperatorSet.ReduceDomain(ReadImage, GenRectangle1, out var imageReduced);
            HOperatorSet.CreateScaledShapeModel(imageReduced, "auto", (new HTuple(-180)).TupleRad()
    , (new HTuple(180)).TupleRad(), "auto", 0.9, 1.1, "auto", "auto", "use_polarity",
    "auto", "auto", out hv_ModelID);
            HOperatorSet.GetShapeModelContours(out ho_ModelContours, hv_ModelID, 1);
            HOperatorSet.AreaCenter(GenRectangle1, out hv_Area, out hv_Row, out hv_Column);
            HOperatorSet.VectorAngleToRigid(0, 0, hv_Area, hv_Row, hv_Column, hv_Area, out hv_HomMat2D);
            HOperatorSet.AffineTransContourXld(ho_ModelContours, out ho_ContoursAffineTrans,
      hv_HomMat2D);
            HOperatorSet.DispObj(ReadImage, hw_home);
            HOperatorSet.DispObj(ho_ContoursAffineTrans, hw_home);
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            HOperatorSet.GenEmptyObj(out GenRectangle1);
            GenRectangle1.Dispose();
            HOperatorSet.DispObj(ReadImage, hw_home);
            HOperatorSet.SetColor(hw_home,"red" );
            HOperatorSet.SetDraw(hw_home, "margin");
            HOperatorSet.DrawRectangle1(hw_home,out var row1,out var col1 , out var row2, out var col2);    
            HOperatorSet.GenRectangle1(out GenRectangle1, row1,col1,row2,col2);
            HOperatorSet.DispObj(GenRectangle1,hw_home);
        }
    }
}
