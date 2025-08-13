/*!
 * I am Karo  😊👍
 *
 * Contact me:
 *     https://www.karo.link/
 *     https://github.com/iamkaro
 *     https://www.linkedin.com/in/iamkaro
 *
 * Leitner Box  (app)
 * https://github.com/iamkaro/Leitner-Box.git
 * Copyright © 2014 developed.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AllLanguages
{
    public partial class KaroTicket : Form
    {
        #region Karo Company
        //K//----------------------------------------------//
        static string MyURL = "";
        //K//----------------------------------------------//
        class A_S
        {
            public string S1, S2, Talafz, URL;
            public bool EqvNem(A_S i) { if (S2 == i.S2 & Talafz == i.Talafz & URL == i.URL) return true; else return false; }
        }
        class A { public A(int DatLength) { Dat = new A_S[DatLength]; } public A_S[] Dat; public int L = 0;}
        static A_S GetRecord(string s)
        {
            A_S a = new A_S();
            int i = 0; a.S1 = ""; while (s[i] != ';') { a.S1 += s[i]; i += 1; }
            i += 1; a.S2 = ""; while (s[i] != ';') { a.S2 += s[i]; i += 1; }
            i += 1; a.Talafz = ""; while (s[i] != ';') { a.Talafz += s[i]; i += 1; }
            i += 1; a.URL = ""; while (s[i] != ';') { a.URL += s[i]; i += 1; }
            return a;
        }
        static string GetRecord(A_S s) { return s.S1 + ";" + s.S2 + ";" + s.Talafz + ";" + s.URL + ";"; }
        static A[] DataDay;
        void GetData()
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "select * from Ketabs  where Ketab='" + KaroFilName + "';";
            SqlDataReader Sql = cmd.ExecuteReader(); Sql.Read();
            DataDay = new A[(int)Sql["DayN"]]; int i;
            for (i = 0; i < DataDay.Length; i++) { DataDay[i] = new A((int)Sql["LogatN"]); }
            for (i = 0; i < DataDay.Length; i++)
            {
                Sql.Close();
                cmd.CommandText = "select * from Days  where Ketab='" + KaroFilName
                    + "' and Day_i=" + (i + 1).ToString() + ";";
                Sql = cmd.ExecuteReader();
                DataDay[i].L = 0; GetDataT(DataDay[i], Sql);
            }
            Mycon.Close();
        }
        static void GetDataT(A a, SqlDataReader s)
        {
            if (s.Read())
            {
                A_S b = new A_S(); b.S1 = s["S1"].ToString(); b.S2 = s["S2"].ToString();
                b.Talafz = s["Talafuz"].ToString(); b.URL = s["URL"].ToString();
                /////////////////
                GetDataT(a, s);
                /////////////////
                a.Dat[a.L] = b; a.L += 1;
            }
        }
        void SaveDataDay(A_S R, int Day_i)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "insert into Days(Ketab,Day_i,S1,S2,Talafuz,URL) values("
                            + "'" + KaroFilName + "'"
                            + "," + (Day_i + 1).ToString()
                            + ",'" + R.S1 + "'"
                            + ",'" + R.S2 + "'"
                            + ",'" + R.Talafz + "'"
                            + ",'" + R.URL + "'"
                            + ");";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        void EditDataDay(A_S R_Old, A_S R_Now)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "update Days "
                            + "set S1='" + R_Now.S1 + "'"
                            + ",S2='" + R_Now.S2 + "'"
                            + ",Talafuz='" + R_Now.Talafz + "'"
                            + ",URL='" + R_Now.URL + "'"
                            + "where Ketab='" + KaroFilName + "'"
                            + "and S1='" + R_Old.S1 + "';";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        void DeletDataDay(A_S R)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "delete from Days where "
                            + "Ketab='" + KaroFilName + "'"
                            + "and S1='" + R.S1 + "';";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        //K//---------------------------------------------//
        void CreateKtabNow(int Day_n, string Word_n)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "insert into Ketabs(Ketab,DayN,LogatN) values("
                            + "'" + KaroFilName + "'"
                            + "," + Day_n.ToString()
                            + ",'" + Word_n + "'"
                            + ");";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        int EditKtab(string Old_FilName, int Day_n, int Word_n)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "select * from Ketabs where Ketab='" + Old_FilName + "';";
            SqlDataReader Sql = cmd.ExecuteReader(); Sql.Read();
            int i, k, Max = 0, n = (int)Sql["DayN"];
            for (i = 1; i <= Day_n; i++)
            {
                Sql.Close();
                cmd.CommandText = "select * from Days where Ketab='" + KaroFilName + "' and Day_i=" + i.ToString() + ";";
                Sql = cmd.ExecuteReader();
                k = 0; while (Sql.Read()) { k += 1; }
                if (k > Max) { Max = k; }
            }
            Sql.Close();
            if (Word_n < Max) { Mycon.Close(); return (Max); }
            else
            {
                cmd.CommandText = "update Ketabs "
                                + "set Ketab='" + KaroFilName + "'"
                                + ",DayN=" + Day_n.ToString()
                                + ",LogatN=" + Word_n.ToString()
                                + "where Ketab='" + Old_FilName + "';";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update Days "
                                + "set Ketab='" + KaroFilName + "'"
                                + "where Ketab='" + Old_FilName + "';";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update Nets "
                                + "set Ketab='" + KaroFilName + "'"
                                + "where Ketab='" + Old_FilName + "';";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update NetData "
                                + "set Ketab='" + KaroFilName + "'"
                                + "where Ketab='" + Old_FilName + "';";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "update Days " + "set Day_i=0"
                    + "where Ketab='" + KaroFilName + "' and Day_i > " + Day_n.ToString() + ";";
                cmd.ExecuteNonQuery();
                Mycon.Close(); return (-1);
            }
        }
        void DeletKtab()
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "delete from Ketabs where Ketab='" + KaroFilName + "';"; cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from Days where Ketab='" + KaroFilName + "';"; cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from Nets where Ketab='" + KaroFilName + "';"; cmd.ExecuteNonQuery();
            cmd.CommandText = "delete from NetData where Ketab='" + KaroFilName + "';"; cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        //K//---------------------------------------------//
        static void Baygani_Delet_Record(A_S R)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "delete from Days where "
                            + "Ketab='" + KaroFilName + "'"
                            + "and S1='" + R.S1 + "';";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        static void Baygani_Edit_Record(A_S R_Old, A_S R_Now)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "update Days "
                            + "set S1='" + R_Now.S1 + "'"
                            + ",S2='" + R_Now.S2 + "'"
                            + ",Talafuz='" + R_Now.Talafz + "'"
                            + ",URL='" + R_Now.URL + "'"
                            + "where Ketab='" + KaroFilName + "'"
                            + "and S1='" + R_Old.S1 + "';";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        static void Baygani_Save_Record(A_S R)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "insert into Days(Ketab,Day_i,S1,S2,Talafuz,URL) values("
                            + "'" + KaroFilName + "',0"
                            + ",'" + R.S1 + "'"
                            + ",'" + R.S2 + "'"
                            + ",'" + R.Talafz + "'"
                            + ",'" + R.URL + "'"
                            + ");";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        static A RecordExist(A_S R, string Nam)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "select * from Days  where Ketab='" + Nam + "' and S1='" + R.S1 + "';";
            SqlDataReader Sql = cmd.ExecuteReader();
            if (Sql.Read())
            {
                A a = new A(1); a.L = (int)Sql["Day_i"];
                a.Dat[0] = new A_S();
                a.Dat[0].S1 = Sql["S1"].ToString();
                a.Dat[0].S2 = Sql["S2"].ToString();
                a.Dat[0].Talafz = Sql["Talafuz"].ToString();
                a.Dat[0].URL = Sql["URL"].ToString();
                Mycon.Close(); return a;
            }
            else { Mycon.Close(); return (null); }
        }
        //K//----------------------------------------------//
        static void Netw_Delet_Record(A_S R, string Nam)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "delete from NetData where "
                            + "Ketab='" + KaroFilName + "'"
                            + "and Net='" + Nam + "'"
                            + "and S1='" + R.S1 + "';";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        static void Netw_Edit_Record(A_S R_Old, A_S R_Now, string Nam)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "update NetData "
                            + "set S1='" + R_Now.S1 + "'"
                            + ",S2='" + R_Now.S2 + "'"
                            + ",Talafuz='" + R_Now.Talafz + "'"
                            + ",URL='" + R_Now.URL + "'"
                            + "where Ketab='" + KaroFilName + "'"
                            + "and Net='" + Nam + "'"
                            + "and S1='" + R_Old.S1 + "';";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        static void Netw_Save_Record(A_S R, string Nam)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "insert into NetData(Ketab,Net,S1,S2,Talafuz,URL) values("
                            + "'" + KaroFilName + "'"
                            + ",'" + Nam + "'"
                            + ",'" + R.S1 + "'"
                            + ",'" + R.S2 + "'"
                            + ",'" + R.Talafz + "'"
                            + ",'" + R.URL + "'"
                            + ");";
            cmd.ExecuteNonQuery();
            Mycon.Close();
        }
        static A_S RecordExisInNetThisKetab(A_S R, string Nam)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "select * from NetData  where Ketab='"
                            + KaroFilName + "' and Net='" + Nam + "' and S1='" + R.S1 + "';";
            SqlDataReader Sql = cmd.ExecuteReader();
            if (Sql.Read())
            {
                A_S a = new A_S();
                a.S1 = Sql["S1"].ToString();
                a.S2 = Sql["S2"].ToString();
                a.Talafz = Sql["Talafuz"].ToString();
                a.URL = Sql["URL"].ToString();
                Mycon.Close(); return a;
            }
            else { Mycon.Close(); return (null); }
        }
        //K//----------------------------------------------//
        class MyForm : Form
        {
            public delegate void MyEve(); public MyEve Eve_Load;
            public MyForm()
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageBox));
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(497, 272);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.TopMost = true;
                this.BackColor = Color.FromArgb(100, 100, 130);
                this.TransparencyKey = System.Drawing.Color.FromArgb(0, 3, 5);
                this.ResumeLayout(false); this.Opacity = 1;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Icon = KaroSource.KaroTick; this.ShowIcon = this.ShowInTaskbar = false;
                if (Tim == null) { Tim = new Timer(); Tim.Interval = 7; }
                Tim.Tick += TimTick; Tim.Enabled = true; this.Opacity = 0;
            }
            Form KF; static Timer Tim; static bool Tim_B = true;
            public void SetKarForm(Form KarFormSet, double Sel_X, double Sel_Y)
            { KF = KarFormSet; if (KF != null) { this.Size = new Size((int)(KF.Width * Sel_X), (int)(KF.Height * Sel_Y)); } }
            void TimTick(object s, EventArgs e)
            {
                if (Tim_B) { Tim_B = false; }
                else
                {
                    if (KF != null)
                    {
                        this.Location = new Point(KF.Left + (KF.Width / 2) - (this.Width / 2)
                            , KF.Top + (KF.Height / 2) - (this.Height / 2));
                        this.LocationChanged += thisLocationChanged;
                    }
                    ///////////////
                    this.Opacity = 1; Tim.Tick -= TimTick; Tim.Enabled = false;
                    if (Eve_Load != null) Eve_Load();
                }
            }
            void thisLocationChanged(object s, EventArgs e)
            {
                KF.Location = new Point(this.Left + (this.Width / 2) - (KF.Width / 2)
                    , this.Top + (this.Height / 2) - (KF.Height / 2));
            }
        }
        //K//----------------------------------------------//
        class MyPoint { public MyPoint(int x_Set, int y_Set) { x = x_Set; y = y_Set; } public int x = 0, y = 0; }
        class MyLabel : Label
        {
            public MyPoint Loc;
            public MyLabel()
            {
                this.BackColor = System.Drawing.Color.Transparent;
                this.Font = new System.Drawing.Font("", (int)((double)(Font_Heg * this.Height))
                    , ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Regular))), System.Drawing.GraphicsUnit.Pixel, ((byte)(178)));
                this.ForeColor = System.Drawing.Color.White; this.AutoSize = false;
                this.TextAlign = ContentAlignment.MiddleCenter;
                this.SizeChanged += thisSizeChanged; Font_Obj = this;
            }
            bool ThSetColD = true;
            public void SetColorDinamic(MyLabel L)
            {
                if (ThSetColD)
                {
                    this.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180);
                    this.MouseEnter += thisMouseEnter; this.MouseLeave += thisMouseLeave;
                    ThSetColD = false;
                }
                if (L != null) { L.MouseEnter += thisMouseEnter; L.MouseLeave += thisMouseLeave; }
            }
            public void thisMouseEnter(object s, EventArgs e) { this.ForeColor = System.Drawing.Color.White; }
            public void thisMouseLeave(object s, EventArgs e) { this.ForeColor = System.Drawing.Color.FromArgb(180, 180, 180); }
            public void CreateLable()
            {
                int i;
                this.ForeColor = Color.FromArgb(233, 233, 233);
                if (Font_Obj != null) { i = (int)((double)(Font_Heg * Font_Obj.Height)); }
                else { i = (int)((double)(Font_Heg * Font_ObjForm.Height)); }
                if (i <= 7) i = 7;
                this.Font = new System.Drawing.Font("", i
                    , ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)))
                    , System.Drawing.GraphicsUnit.Pixel, ((byte)(178)));
            }
            public void CreataBut()
            {
                ButStartMouseLeave(null, null); this.ForeColor = Color.FromArgb(230, 170, 170);
                this.MouseLeave += ButStartMouseLeave; this.MouseEnter += ButStartMouseEnter;
                ////////////
                int i;
                if (Font_Obj != null) { i = (int)((double)(Font_Heg * Font_Obj.Height)); }
                else { i = (int)((double)(Font_Heg * Font_ObjForm.Height)); }
                if (i <= 7) i = 7;
                this.Font = new System.Drawing.Font("", i
                    , ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic)))
                    , System.Drawing.GraphicsUnit.Pixel, ((byte)(178)));
            }
            void ButStartMouseLeave(object s, EventArgs e) { this.BackColor = Color.FromArgb(20, 20, 20); }
            void ButStartMouseEnter(object s, EventArgs e) { this.BackColor = Color.FromArgb(70, 70, 70); }
            double Font_Heg = 0.70; MyLabel Font_Obj = null; Form Font_ObjForm = null;
            public void SetPersentFont(double Input) { SetPersentFont(Input, this); }
            public void RemovePersentFont(Font F)
            {
                if (Font_ObjForm != null) { Font_ObjForm.SizeChanged -= thisSizeChanged; }
                else { Font_Obj.SizeChanged -= thisSizeChanged; }
                this.Font = F;
            }
            public void SetPersentFont(double Input, MyLabel ObjSet)
            {
                if (Font_ObjForm != null) { Font_ObjForm.SizeChanged -= thisSizeChanged; Font_ObjForm = null; }
                else { Font_Obj.SizeChanged -= thisSizeChanged; }
                Font_Obj = ObjSet; Font_Obj.SizeChanged += thisSizeChanged;
                Font_Heg = Input; if (Font_Heg > 1) Font_Heg = 1; else if (Font_Heg < 0.01) Font_Heg = 0.01;
                thisSizeChanged(null, null);
            }
            public void SetPersentFont(double Input, Form ObjSet)
            {
                if (Font_Obj != null) { Font_Obj.SizeChanged -= thisSizeChanged; Font_Obj = null; }
                else { Font_ObjForm.SizeChanged -= thisSizeChanged; }
                Font_ObjForm = ObjSet; Font_ObjForm.SizeChanged += thisSizeChanged;
                Font_Heg = Input; if (Font_Heg > 1) Font_Heg = 1; else if (Font_Heg < 0.01) Font_Heg = 0.01;
                thisSizeChanged(null, null);
            }
            void thisSizeChanged(object s, EventArgs e)
            {
                int i;
                if (Font_Obj != null) { i = (int)((double)(Font_Heg * Font_Obj.Height)); }
                else { i = (int)((double)(Font_Heg * Font_ObjForm.Height)); }
                if (i <= 7) i = 7;
                this.Font = new System.Drawing.Font("", i, this.Font.Style, System.Drawing.GraphicsUnit.Pixel, ((byte)(178)));
            }
        }
        //K//----------------------------------------------//
        class MyButon : PictureBox
        {
            public MyButon()
            {
                this.BackColor = Color.Transparent;
                this.SizeMode = PictureBoxSizeMode.StretchImage;
                this.MouseEnter += thisMouseEnter;
                this.MouseLeave += thisMouseLeave;
            }
            Bitmap Im1, Im2;
            public void SetImages(Bitmap Im_Leave, Bitmap Im_Enter)
            { Im1 = Im_Leave; Im2 = Im_Enter; this.Image = Im1; }
            void thisMouseEnter(object s, EventArgs e) { this.Image = Im2; }
            void thisMouseLeave(object s, EventArgs e) { this.Image = Im1; }
        }
        //K//----------------------------------------------//
        static int MyPanelDayVal = 0;
        class MyPanelDay : MyLabel
        {
            public byte Val_n = 0;
            public delegate void MyPanelDayEvent(MyPanelDay P);
            MyLabel Item_Name = new MyLabel(), Item_ValMax = new MyLabel(); public MyPanelDayEvent EventClick;
            public MyPanelDay()
            {
                this.Click += T_Click; this.Controls.Add(Item_ValMax); this.Controls.Add(Item_Name);
                this.MinimumSize = new System.Drawing.Size(10, 10);
                this.MaximumSize = new System.Drawing.Size(1000, 500);
                /////////////
                Item_Name.Click += T_Click; Item_Name.AutoSize = true; Item_Name.TextAlign = ContentAlignment.TopLeft;
                Item_Name.SetPersentFont(0.20, this); Item_Name.Text = "Items : ";
                Item_Name.Location = new Point(0, this.Height - Item_Name.Height);
                /////////////
                Item_ValMax.Click += T_Click; Item_ValMax.AutoSize = true; Item_ValMax.TextAlign = ContentAlignment.TopLeft;
                Item_ValMax.SetPersentFont(0.20, this); Item_ValMax.Text = "0";
                Item_ValMax.Location = new Point(Item_Name.Width, this.Height - Item_Name.Height);
                ////////////
                this.SizeChanged += thisSizeChanged;
                this.SetColorDinamic(Item_Name); this.SetColorDinamic(Item_ValMax);
            }
            void T_Click(object s, EventArgs e)
            { if (this.Height == this.MinimumSize.Height) { MyPanelDayVal = Val; if (EventClick != null) EventClick(this); } }
            void thisSizeChanged(object s, EventArgs e)
            {
                Item_Name.Location = new Point(0, this.Height - Item_Name.Height);
                Item_ValMax.Location = new Point(Item_Name.Width, this.Height - Item_Name.Height);
            }
            public void SetItem_ValMax() { if (Val < DataDay.Length) { Item_ValMax.Text = DataDay[Val].L.ToString(); } }
            int Val = 0;
            public int SetVal
            {
                set
                {
                    Val = value;
                    if (DataDay.Length > Val & 0 <= Val)
                    {
                        this.Text = (Val + 1).ToString();
                        if (DataDay[Val] != null)
                            Item_ValMax.Text = DataDay[Val].L.ToString();
                        if (this.Visible == false) this.Visible = true;
                    }
                    else { this.Visible = false; }
                }
                get { return Val; }
            }
        }
        //K//----------------------------------------------//
        class MyPanelDat : MyLabel
        {
            public class TStripBut : ToolStripButton
            {
                public TStripBut()
                {
                    this.Font = new System.Drawing.Font("", 20
                                                      , (System.Drawing.FontStyle.Regular)
                                                      , System.Drawing.GraphicsUnit.Pixel, ((byte)(178)));
                    this.ForeColor = System.Drawing.Color.White;
                    this.BackColor = Color.Transparent;
                }
            }
            //ContextMenuStrip CMS = new ContextMenuStrip();
            TStripBut ButCopy = new TStripBut(), ButMove = new TStripBut();
            public byte Val_n = 0;
            public delegate void MyPanelDatEvent(MyPanelDat P);
            public MyPanelDatEvent EventClick, EventTikYes, EventTikNo, EventDel, EventEdit, EvntCopy, EvntMove;
            MyButon Tik_Yes = new MyButon(), Tik_No = new MyButon(), Del = new MyButon()
                , Edi = new MyButon(), Img = new MyButon(), Talafz = new MyButon();
            //MyButon RitClick = new MyButon();
            public MyPanelDat()
            {
                this.Click += T_Click; this.Controls.Add(Tik_Yes); this.Controls.Add(Tik_No); this.Controls.Add(Del);
                this.Controls.Add(Edi); this.Controls.Add(Img); this.Controls.Add(Talafz); //this.Controls.Add(RitClick);
                this.MinimumSize = new System.Drawing.Size(10, 10);
                this.MaximumSize = new System.Drawing.Size(1000, 500);
                /////////////
                Tik_Yes.SetImages(KaroSource.KaroTikOff, KaroSource.KaroTikOn); Tik_Yes.Click += TikYes_Click;
                /////////////
                Tik_No.SetImages(KaroSource.KaroNoTikOff, KaroSource.KaroNoTikOn); Tik_No.Click += TikNo_Click;
                /////////////
                Del.SetImages(KaroSource.KaroDelOff, KaroSource.KaroDelOn); Del.Click += Del_Click;
                /////////////
                Edi.SetImages(KaroSource.KaroEditOff, KaroSource.KaroEditOn); Edi.Click += Edi_Click;
                /////////////
                Img.SetImages(KaroSource.KaroImgOff, KaroSource.KaroImgOn);
                Img.MouseDown += ImgMouseDown; Img.MouseUp += ImgMouseUp;
                /////////////
                Talafz.SetImages(KaroSource.KaroTalafzOff, KaroSource.KaroTalafzOn);
                Talafz.MouseDown += TalafzMouseDown; Talafz.MouseUp += TalafzMouseUp;
                /////////////
                /*RitClick.SetImages(KaroSource.KaroRitClickOff, KaroSource.KaroRitClickOn);
                RitClick.Click += RitClickClick;*/
                /////////////
                this.SizeChanged += thisSizeChanged; thisSizeChanged(null, null);
                this.MouseDown += thisMouseDown; this.MouseUp += thisMouseUp;
                this.SetColorDinamic(null);
                this.Cursor = Cursors.Hand;
                this.SetPersentFont(0.20, this);
                En_Pr_Event += SetText; En_Pr_Event += TalafzVis; Talafz.Visible = En_Pr;
                ////////////


                /*CMS.Items.Add(ButMove); ButMove.Text = "انتقال به"; CMS.Items.Add("-");
                CMS.Items.Add(ButCopy); ButCopy.Text = "اضافه شود به";
                CMS.BackColor = Color.FromArgb(70, 60, 60); CMS.Width = 130;
                this.ContextMenuStrip = CMS;
                ButMove.Click += ButMoveClick; ButCopy.Click += ButCopyClick;
                */
            }
            void TalafzVis() { Talafz.Visible = En_Pr; }
            void RitClickClick(object s, EventArgs e)
            {
                /*CMS.Show();
                CMS.Left = RitClick.Left + this.Left + this.Parent.Left + this.Parent.Parent.Left
                    + this.Parent.Parent.Parent.Left + this.Parent.Parent.Parent.Parent.Left
                    + this.Parent.Parent.Parent.Parent.Parent.Left - 7;
                CMS.Top = RitClick.Top + this.Top + this.Parent.Top + this.Parent.Parent.Top
                    + this.Parent.Parent.Parent.Top + this.Parent.Parent.Parent.Parent.Top
                    + this.Parent.Parent.Parent.Parent.Parent.Top - 30;*/
            }
            void T_Click(object s, EventArgs e) { if (EventClick != null) EventClick(this); }
            void TikYes_Click(object s, EventArgs e) { if (EventTikYes != null) EventTikYes(this); }
            void TikNo_Click(object s, EventArgs e) { if (EventTikNo != null) EventTikNo(this); }
            void Del_Click(object s, EventArgs e) { if (EventDel != null) EventDel(this); }
            void Edi_Click(object s, EventArgs e) { if (EventEdit != null) EventEdit(this); }
            void ImgMouseDown(object s, EventArgs e) { ImBox.ShowImage(DataDay[MyPanelDayVal].Dat[SetVal].URL); }
            void ImgMouseUp(object s, EventArgs e) { ImBox.EndImage(); }
            void TalafzMouseDown(object s, EventArgs e) { thisMouseEnter(null, null); this.Text = DataDay[MyPanelDayVal].Dat[Val].Talafz; }
            void TalafzMouseUp(object s, EventArgs e) { thisMouseLeave(null, null); this.Text = DataDay[MyPanelDayVal].Dat[Val].S1; }
            void ButMoveClick(object s, EventArgs e) { if (EvntMove != null) EvntMove(this); }
            void ButCopyClick(object s, EventArgs e) { if (EvntCopy != null) EvntCopy(this); }
            void thisSizeChanged(object s, EventArgs e)
            {
                //RitClick.Size =
                Talafz.Size = Img.Size = Edi.Size = Tik_Yes.Size = Tik_No.Size = Del.Size
                    = new Size(this.Height / 5, this.Height / 5);
                Del.Location = new Point(this.Width - Del.Width - 12, this.Height - Del.Height - 1);
                Edi.Location = new Point(Del.Left - Del.Width - 7, Del.Top);
                Tik_No.Location = new Point(Edi.Left - Edi.Width - 7, Edi.Top);
                Tik_Yes.Location = new Point(Tik_No.Left - Del.Width - 7, Del.Top);
                Img.Location = new Point(this.Width - Del.Width - 12, 1);
                Talafz.Location = new Point(Img.Left - 7 - Talafz.Width, 1);
                //RitClick.Location = new Point(12, Del.Top);
            }
            static bool En_Pr = true;
            public static bool EToP
            {
                set { En_Pr = value; if (En_Pr_Event != null)En_Pr_Event(); }
                get { return En_Pr; }
            }
            public delegate void MyEvent();
            public static MyEvent En_Pr_Event;
            public void SetText()
            {
                if (DataDay[MyPanelDayVal].L > Val & 0 <= Val)
                {
                    if (En_Pr) { this.Text = DataDay[MyPanelDayVal].Dat[Val].S1; }
                    else { this.Text = DataDay[MyPanelDayVal].Dat[Val].S2; }
                    if (this.Visible == false) this.Visible = true;
                }
                else { this.Visible = false; }
            }
            void thisMouseDown(object s, EventArgs e)
            {
                if (En_Pr) { this.Text = DataDay[MyPanelDayVal].Dat[Val].S2; }
                else { this.Text = DataDay[MyPanelDayVal].Dat[Val].S1; }
            }
            void thisMouseUp(object s, EventArgs e)
            {
                if (En_Pr) { this.Text = DataDay[MyPanelDayVal].Dat[Val].S1; }
                else { this.Text = DataDay[MyPanelDayVal].Dat[Val].S2; }
            }
            int Val = 0;
            public int SetVal
            {
                set
                {
                    Val = value;
                    if (DataDay[MyPanelDayVal].L > Val & 0 <= Val)
                    {
                        this.Text = DataDay[MyPanelDayVal].Dat[Val].S1;
                        if (this.Visible == false) this.Visible = true;
                    }
                    else { this.Visible = false; }
                }
                get { return Val; }
            }
        }
        //K//----------------------------------------------//
        static double T(double a, double b) { return (double)(a / b); }
        class MyHS : PictureBox
        {
            public delegate void MyEvent();
            public MyEvent EventMove; PictureBox But = new PictureBox();
            double V = 0;
            public MyHS()
            {
                this.Controls.Add(But);
                this.BackColor = Color.Transparent;
                this.SizeMode = PictureBoxSizeMode.StretchImage;
                this.SizeChanged += thisSizeChanged; thisSizeChanged(null, null);
                this.BackColor = Color.FromArgb(100, 100, 100);
                But.BackColor = Color.FromArgb(255, 255, 255);
                But.Left = 1;
                But.MouseDown += ButMouseDown;
                But.MouseMove += ButMouseMove;
                But.MouseUp += ButMouseUp;
                this.MouseDown += thisButMouseDown;
            }
            bool ButMov = false; int y = 0;
            void ButMouseDown(object s, MouseEventArgs e) { y = e.Y; ButMov = true; Val = T(But.Top - 1, (this.Height - 2 - But.Height)); }
            public double Val
            {
                set
                {
                    V = value; if (V > 1) V = 1; else if (V < 0) V = 0;
                    But.Top = 1 + (int)((double)((this.Height - 2 - But.Height) * V));
                    if (EventMove != null) { EventMove(); }
                }
                get { return V; }
            }
            void ButMouseMove(object s, MouseEventArgs e)
            {
                if (ButMov)
                {
                    int t = But.Top + e.Y - y; Val = T(t - 1, (this.Height - 2 - But.Height));
                }
            }
            void ButMouseUp(object s, MouseEventArgs e) { ButMov = false; }
            void thisButMouseDown(object s, MouseEventArgs e)
            { But.Top = e.Y - (But.Height / 2); Val = T(But.Top - 1, (this.Height - 2 - But.Height)); }
            void thisSizeChanged(object s, EventArgs e)
            {
                But.Size = new Size(this.Width - 2, this.Height / 7);
                But.Top = 1 + (int)((double)(Val * (this.Height - 2 - But.Height)));
            }
        }
        //K//----------------------------------------------//
        class MyTextBox : TextBox
        {
            public MyTextBox()
            {
                this.BackColor = System.Drawing.Color.FromArgb(90, 90, 90);
                ////////////////////////////////////////////
                this.Font = new System.Drawing.Font("", 18
                    , ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Regular))), System.Drawing.GraphicsUnit.Pixel, ((byte)(178)));
                this.BorderStyle = BorderStyle.None;
                this.ForeColor = System.Drawing.Color.FromArgb(227, 227, 227);
                this.Location = new System.Drawing.Point(22, 14);
                this.Size = new System.Drawing.Size(53, 34);
                this.TextAlign = HorizontalAlignment.Center;
            }
        }
        class MyPanelTextBox : MyForm
        {
            public MyTextBox S1 = new MyTextBox(), Talafz = new MyTextBox(), S2 = new MyTextBox();
            MyLabel N_S1 = new MyLabel(), N_S2 = new MyLabel(), N_S3 = new MyLabel(); public MyLabel URL = new MyLabel();
            MyLabel ButStart = new MyLabel(), ButCancel = new MyLabel();
            public MyPanelTextBox()
            {
                this.BackColor = Color.FromArgb(70, 70, 100);
                this.Controls.Add(N_S1); this.Controls.Add(N_S2); this.Controls.Add(N_S3); this.Controls.Add(URL);
                this.Controls.Add(S1); this.Controls.Add(Talafz); this.Controls.Add(S2);
                this.Controls.Add(ButStart); this.Controls.Add(ButCancel);
                this.N_S1.AutoSize = true; this.N_S1.TextAlign = ContentAlignment.TopLeft;
                this.N_S2.AutoSize = true; this.N_S2.TextAlign = ContentAlignment.TopLeft;
                this.N_S3.AutoSize = true; this.N_S3.TextAlign = ContentAlignment.TopLeft;
                N_S1.ForeColor = N_S2.ForeColor = N_S3.ForeColor = Color.White;
                //////
                N_S1.CreateLable(); N_S2.CreateLable(); N_S3.CreateLable();
                ////////////
                ButStart.Text = "OK"; ButStart.CreataBut();
                ////////////
                ButCancel.Text = "Cancel"; ButCancel.CreataBut();
                ////////////
                this.N_S1.SetPersentFont(0.12, this); this.N_S2.SetPersentFont(0.12, this); this.N_S3.SetPersentFont(0.12, this);
                S1.KeyDown += SKeyDown; Talafz.KeyDown += SKeyDown; S2.KeyDown += SKeyDown;
                ButStart.Click += ButStartClick; ButCancel.Click += ButCancelClick;
                ////////////
                URL.BackColor = Color.FromArgb(120, 120, 120); URL.ForeColor = Color.FromArgb(150, 250, 150);
                URL.Cursor = Cursors.Hand; URL.TextAlign = ContentAlignment.MiddleLeft; URL.Text = "Image";
                URL.MouseLeave += URLMouseLeave; URL.MouseEnter += URLMouseEnter; URL.Click += URLClick;
                URL.SetPersentFont(0.6);
                this.SizeChanged += thisSizeChanged;
            }
            public A_S GetRecord()
            { A_S a = new A_S(); a.S1 = this.S1.Text; a.S2 = this.S2.Text; a.Talafz = this.Talafz.Text; a.URL = this.URL.Text; return a; }
            string URLFils = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\AllLanguagesImages"; 
            void URLClick(object s, EventArgs e)
            {
                OpenFileDialog F = new OpenFileDialog();
                F.Title = "Select And Open File";
                F.Filter = "Images|*.jpg; *.png; *.gif|icon|*.ico|all|*.*";
                F.Multiselect = false;
                if (System.IO.Directory.Exists(URLFils) == false)
                    System.IO.Directory.CreateDirectory(URLFils);
                F.InitialDirectory = URLFils;
                if (F.ShowDialog() == DialogResult.OK)
                {
                    if (Res(F.FileName))
                    {
                        string q = GetFileNumbr(URLFils + "\\[", "]" + Pasvand(F.FileName));
                        if (q != "OUT !!!") { System.IO.File.Copy(F.FileName, q); }
                        URL.Text = q;
                    }
                    else { URL.Text = F.FileName; }
                }
            }
            string GetFileNumbr(string S1, string S2)
            {
                int i; string q = "";
                for (i = 0; i < 1000000000; i++)
                {
                    q = i.ToString(); while (q.Length < 9) { q = "0" + q; }
                    if (System.IO.File.Exists(S1 + q + S2) == false) return (S1 + q + S2);
                }
                return ("OUT !!!");
            }
            string Pasvand(string S)
            {
                if (S.Length > 5)
                { if (S[S.Length - 4] == '.') { return "." + S[S.Length - 3] + S[S.Length - 2] + S[S.Length - 1]; } }
                return ".jpg";
            }
            bool Res(string s)
            {
                string s1 = URLFils + "\\";
                if (s.Length != (15 + s1.Length)) { return true; }
                int i; for (i = 0; i < s1.Length; i++) if (s1[i] != s[i]) { return true; }
                return false;
            }
            void URLMouseEnter(object s, EventArgs e) { ImBox.ShowImage(URL.Text); }
            void URLMouseLeave(object s, EventArgs e) { ImBox.EndImage(); }
            public string NetNam = "", Lokat = "";
            bool GetSahih()
            {
                MyPanelMasage MyMasage = new MyPanelMasage(); MyMasage.SetKarForm(this, 1, 1);
                MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = false;
                ////////////////////////////////////////////
                if (S1.Text == "") { MyMasage.Payam.Text = "لغت خالی است !!!"; MyMasage.ShowDialog(); return false; }
                else if (NetNam == "")
                {
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon; cmd.CommandText = "select * from Ketabs;";
                    SqlDataReader Sql = cmd.ExecuteReader(); A a; A_S b = GetRecord(); bool Ret = true;
                    if ((a = RecordExist(b, KaroFilName)) != null & Lokat != b.S1)
                    {
                        if (a.Dat[0].EqvNem(b))
                        {
                            MyMasage.Payam.Text = "این لغت در " + ((a.L > 0) ? ("روز " + a.L.ToString()) : "بایگانی")
                              + " به طور کامل موجود است آیا باز می خواهید ثبت شود؟";
                        }
                        else
                        {
                            MyMasage.Payam.Text = "این لغت در " + ((a.L > 0) ? ("روز " + a.L.ToString()) : "بایگانی")
                              + " موجود است ولی بعضی از ویژگی هایش فرق دارند.  آیا باز می خواهید ثبت شود؟.";
                        }
                        MyMasage.ShowDialog();
                        Ret = false;
                    }
                    else
                    {
                        MyMasage.ButCancel.Visible = true; MyMasage.ButCancel.Text = "No";
                        while (Sql.Read())
                        {
                            if (Sql["Ketab"].ToString() != KaroFilName)
                            {
                                if ((a = RecordExist(b, Sql["Ketab"].ToString())) != null)
                                {
                                    if (a.Dat[0].EqvNem(b))
                                    {
                                        MyMasage.Payam.Text = "این لغت در کتاب " + Sql["Ketab"].ToString()
                                                            + ((a.L > 0) ? ("، در روز " + a.L.ToString()) : "، در بایگانی")
                                                            + " به طور کامل موجود است آیا باز می خواهید ثبت شود؟";
                                    }
                                    else
                                    {
                                        MyMasage.Payam.Text = "این لغت در کتاب " + Sql["Ketab"].ToString()
                                                            + ((a.L > 0) ? ("، در روز " + a.L.ToString()) : "، در بایگانی")
                                                            + a.L + " موجود است ولی بعضی از ویژگی هایش فرق دارند.  آیا باز می خواهید ثبت شود؟.";
                                    }
                                    if (MyMasage.ShowDialog() == DialogResult.No) { Ret = false; goto E; }
                                }
                            }
                        }
                    }
                E: ;
                    Mycon.Close();
                    return Ret;
                }
                else
                {
                    A_S b = GetRecord(), a = RecordExisInNetThisKetab(b, NetNam);
                    if (a != null & Lokat != b.S1)
                    {
                        if (a.EqvNem(b)) { MyMasage.Payam.Text = "این لغت در شبکه موجود است"; }
                        else
                        { MyMasage.Payam.Text = "این لغت در شبکه موجود است ولی بعضی از ویژگی هایش فرق دارند. شما می توانیدویژگی های جدید را همان وارد فرمایید."; }
                        MyMasage.ShowDialog();
                        return false;
                    }
                    else { return true; }
                }
            }
            void ButStartClick(object s, EventArgs e) { if (GetSahih()) { this.DialogResult = DialogResult.Yes; this.Close(); } }
            void ButCancelClick(object s, EventArgs e) { this.DialogResult = DialogResult.No; this.Close(); }
            void SKeyDown(object s, KeyEventArgs e)
            { if (e.KeyCode == Keys.Enter) { if (GetSahih()) { this.DialogResult = DialogResult.Yes; this.Close(); } } }
            //K//---------------------------------------------//
            void thisSizeChanged(object s, EventArgs e)
            {
                N_S1.Text = "لغت :"; N_S2.Text = "تلفظ :"; N_S3.Text = "معنی :"; bool RitToLeft = true;
                N_S1.RightToLeft = N_S2.RightToLeft = N_S3.RightToLeft = RitToLeft ? (System.Windows.Forms.RightToLeft.Yes) : (System.Windows.Forms.RightToLeft.No);
                int i = N_S1.Width; if (i < N_S2.Width) i = N_S2.Width; if (i < N_S3.Width) i = N_S3.Width;
                S1.Width = Talafz.Width = S2.Width = this.Width - 30 - i; URL.Height = S1.Height; URL.Width = this.Width - 30;
                if (RitToLeft)
                {
                    S1.Location = new Point(10, 10); Talafz.Location = new Point(10, S1.Height + 20); S2.Location = new Point(10, Talafz.Top + Talafz.Height + 10);
                    N_S1.Location = new Point(S1.Width + 20, 10);
                    N_S2.Location = new Point(Talafz.Width + 20, Talafz.Top);
                    N_S3.Location = new Point(S2.Width + 20, S2.Top);
                }
                else
                {
                    N_S1.Location = new Point(i + 10 - N_S1.Width, 10);
                    N_S2.Location = new Point(i + 10 - N_S2.Width, S1.Height + 20);
                    N_S3.Location = new Point(i + 10 - N_S3.Width, Talafz.Top + Talafz.Height + 10);
                    S1.Location = new Point(N_S1.Width + N_S1.Left + 10, 10);
                    Talafz.Location = new Point(N_S2.Width + N_S2.Left + 10, N_S2.Top);
                    S2.Location = new Point(N_S3.Width + N_S3.Left + 10, N_S3.Top);
                }
                URL.Location = new Point(10, S2.Top + S2.Height + 10);
                ButCancel.Size = ButStart.Size = new Size(this.Width / 3, 30);
                ButStart.Location = new Point((this.Width - (ButStart.Width * 2)) / 3, URL.Top + URL.Height + 15);
                ButCancel.Location = new Point(this.Width - ButStart.Left - ButStart.Width, ButStart.Top);
            }
            public void SetDizin() { thisSizeChanged(null, null); }
        }
        //K//----------------------------------------------//
        class PanelForKtabNow : MyForm
        {
            public MyTextBox S1 = new MyTextBox(), S2 = new MyTextBox(), S3 = new MyTextBox();
            MyLabel N_S1 = new MyLabel(), N_S2 = new MyLabel(), N_S3 = new MyLabel();
            MyLabel ButStart = new MyLabel(), ButCancel = new MyLabel();
            public PanelForKtabNow()
            {
                this.BackColor = Color.FromArgb(70, 70, 100);
                this.Controls.Add(N_S1); this.Controls.Add(N_S2); this.Controls.Add(N_S3);
                this.Controls.Add(S1); this.Controls.Add(S2); this.Controls.Add(S3);
                this.Controls.Add(ButStart); this.Controls.Add(ButCancel);
                this.N_S1.AutoSize = true; this.N_S1.TextAlign = ContentAlignment.TopLeft;
                this.N_S2.AutoSize = true; this.N_S2.TextAlign = ContentAlignment.TopLeft;
                this.N_S3.AutoSize = true; this.N_S3.TextAlign = ContentAlignment.TopLeft;
                N_S1.ForeColor = N_S2.ForeColor = N_S3.ForeColor = Color.White;
                ///////
                N_S1.CreateLable(); N_S2.CreateLable(); N_S3.CreateLable();
                ////////////
                ButStart.Text = "OK"; ButStart.CreataBut();
                ////////////
                ButCancel.Text = "Cancel"; ButCancel.CreataBut();
                ////////////
                this.N_S1.SetPersentFont(0.12, this); this.N_S2.SetPersentFont(0.12, this); this.N_S3.SetPersentFont(0.12, this);
                S1.KeyDown += SKeyDown; S2.KeyDown += SKeyDown; S3.KeyDown += SKeyDown;
                ButStart.Click += ButStartClick; ButCancel.Click += ButCancelClick;
                this.SizeChanged += thisSizeChanged;
            }
            public string KtabOld = "#*:/Kar";
            void R_YesClose()
            {
                MyPanelMasage MyMasage = new MyPanelMasage(); MyMasage.Payam.RightToLeft = RightToLeft.Yes;
                MyMasage.ButCancel.Visible = false; MyMasage.SetKarForm(this, 1, 1);
                ////////////////////////////////////////////
                if (S1.Text == "") { MyMasage.Payam.Text = "نام کتاب نباید خالی باشد."; MyMasage.ShowDialog(); }
                else
                {
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "select * from Ketabs where Ketab='" + S1.Text + "';";
                    SqlDataReader Sql = cmd.ExecuteReader();
                    if (Sql.Read() & (KtabOld != S1.Text))
                    { MyMasage.Payam.Text = "این کتاب قبلا ثبت شده است."; MyMasage.ShowDialog(); }
                    else
                    {
                        int i;
                        for (i = 0; i < S1.Text.Length; i++)
                        {
                            if (S1.Text[i] == '|' | S1.Text[i] == ':' | S1.Text[i] == '/' | S1.Text[i] == '"'
                              | S1.Text[i] == '?' | S1.Text[i] == '*' | S1.Text[i] == '<' | S1.Text[i] == '>'
                              | S1.Text[i] == '.' | S1.Text[i] == '\\')
                            {
                                MyMasage.Payam.Text = "لطفا از حروف    |   :   .   /   \"   ?    *   <    >      \\    استفاده نکنید.";
                                MyMasage.ShowDialog();
                                goto End;
                            }
                        }
                        try
                        {
                            i = int.Parse(S2.Text);
                            if (i < 1) { MyMasage.Payam.Text = "عدد ها باید بزرکتر و مساوی 1 باشند."; MyMasage.ShowDialog(); goto End; }
                            i = int.Parse(S3.Text);
                            if (i < 1) { MyMasage.Payam.Text = "عدد ها باید بزرکتر و مساوی 1 باشند."; MyMasage.ShowDialog(); goto End; }
                            this.DialogResult = DialogResult.Yes; this.Close();
                        }
                        catch
                        {
                            MyMasage.Payam.Text = "لطفا برای تعداد روز ها و تعداد لغات هر روز , عدد وارد کنید";
                            MyMasage.ShowDialog();
                        }
                    End: ;
                    }
                    Mycon.Close();
                }
            }
            void ButStartClick(object s, EventArgs e) { R_YesClose(); }
            void ButCancelClick(object s, EventArgs e) { this.DialogResult = DialogResult.No; this.Close(); }
            void SKeyDown(object s, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { R_YesClose(); } }
            void thisSizeChanged(object s, EventArgs e)
            {
                N_S1.Text = "نام کتاب : "; N_S2.Text = "تعداد روزها : "; N_S3.Text = "تعداد لغت در هر روز : "; bool RitToLeft = true;
                N_S1.RightToLeft = N_S2.RightToLeft = N_S3.RightToLeft = RitToLeft ? (System.Windows.Forms.RightToLeft.Yes) : (System.Windows.Forms.RightToLeft.No);
                int i = N_S1.Width; if (i < N_S2.Width) i = N_S2.Width; if (i < N_S3.Width) i = N_S3.Width;
                S1.Width = S2.Width = S3.Width = this.Width - 30 - i;
                if (RitToLeft)
                {
                    S1.Location = new Point(10, 10); S2.Location = new Point(10, S1.Height + 20); S3.Location = new Point(10, S2.Top + S2.Height + 10);
                    N_S1.Location = new Point(S1.Width + 20, 10); N_S2.Location = new Point(S2.Width + 20, S2.Top);
                    N_S3.Location = new Point(S3.Width + 20, S3.Top);
                }
                else
                {
                    N_S1.Location = new Point(i + 10 - N_S1.Width, 10);
                    N_S2.Location = new Point(i + 10 - N_S2.Width, S1.Height + 20);
                    N_S3.Location = new Point(i + 10 - N_S3.Width, S2.Top + S2.Height + 10);
                    S1.Location = new Point(N_S1.Width + N_S1.Left + 10, 10);
                    S2.Location = new Point(N_S2.Width + N_S2.Left + 10, N_S2.Top);
                    S3.Location = new Point(N_S3.Width + N_S3.Left + 10, N_S3.Top);
                }
                ButCancel.Size = ButStart.Size = new Size(this.Width / 3, 30);
                ButStart.Location = new Point((this.Width - (ButStart.Width * 2)) / 3, this.Height - S3.Height - 17);
                ButCancel.Location = new Point(this.Width - ButStart.Left - ButStart.Width, ButStart.Top);
            }
            public void SetDizin() { thisSizeChanged(null, null); }
        }
        //K//----------------------------------------------//
        class PanelForNetWordNow : MyForm
        {
            public MyTextBox TxtBox = new MyTextBox(); MyLabel Txt = new MyLabel();
            MyLabel ButStart = new MyLabel(), ButCancel = new MyLabel();
            public PanelForNetWordNow()
            {
                this.BackColor = Color.FromArgb(70, 70, 100); this.Controls.Add(Txt); this.Controls.Add(TxtBox);
                this.Controls.Add(ButStart); this.Controls.Add(ButCancel);
                this.Txt.AutoSize = true; this.Txt.TextAlign = ContentAlignment.TopLeft; Txt.ForeColor = Color.White;
                ////////////
                Txt.CreateLable();
                ////////////
                ButStart.Text = "OK"; ButStart.CreataBut();
                ////////////
                ButCancel.Text = "Cancel"; ButCancel.CreataBut();
                ////////////
                TxtBox.KeyDown += SKeyDown; ButStart.Click += ButStartClick;
                ButCancel.Click += ButCancelClick; this.SizeChanged += thisSizeChanged;
            }
            public string KtabOld = "#*:/Kar";
            void R_YesClose()
            {
                MyPanelMasage MyMasage = new MyPanelMasage(); MyMasage.Payam.RightToLeft = RightToLeft.Yes;
                MyMasage.Payam.SetPersentFont(0.2, this);
                MyMasage.ButCancel.Visible = false; MyMasage.SetKarForm(this, 1, 1);
                ////////////////////////////////////////////
                if (TxtBox.Text == "") { MyMasage.Payam.Text = "نام شبکه لغت نباید خالی باشد."; MyMasage.ShowDialog(); }
                else
                {
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "select * from Ketabs where Ketab='" + TxtBox.Text + "';";
                    SqlDataReader Sql = cmd.ExecuteReader();
                    if (Sql.Read() & (KtabOld != TxtBox.Text))
                    { MyMasage.Payam.Text = "این شبکه لغت قبلا ثبت شده است."; MyMasage.ShowDialog(); goto End; }
                    else
                    {
                        int i;
                        for (i = 0; i < TxtBox.Text.Length; i++)
                        {
                            if (TxtBox.Text[i] == '|' | TxtBox.Text[i] == ':' | TxtBox.Text[i] == '/' | TxtBox.Text[i] == '"'
                              | TxtBox.Text[i] == '?' | TxtBox.Text[i] == '*' | TxtBox.Text[i] == '<' | TxtBox.Text[i] == '>'
                              | TxtBox.Text[i] == '.' | TxtBox.Text[i] == '\\')
                            {
                                MyMasage.Payam.Text = "لطفا از حروف    |   :   .   /   \"   ?    *   <    >      \\    استفاده نکنید.";
                                MyMasage.ShowDialog();
                                goto End;
                            }
                        }
                        this.DialogResult = DialogResult.Yes; this.Close();
                    }
                End: ;
                    Mycon.Close();
                }
            }
            void ButStartClick(object s, EventArgs e) { R_YesClose(); }
            void ButCancelClick(object s, EventArgs e) { this.DialogResult = DialogResult.No; this.Close(); }
            void SKeyDown(object s, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { R_YesClose(); } }
            void thisSizeChanged(object s, EventArgs e)
            {
                Txt.Text = "نام شبکه لغت"; bool RitToLeft = true;
                Txt.RightToLeft = RitToLeft ? System.Windows.Forms.RightToLeft.Yes : System.Windows.Forms.RightToLeft.No;
                TxtBox.Width = this.Width - 30 - Txt.Width;
                if (RitToLeft) { TxtBox.Location = new Point(10, 10); Txt.Location = new Point(TxtBox.Width + 20, 10); }
                else { Txt.Location = new Point(10, 10); TxtBox.Location = new Point(Txt.Width + Txt.Left + 10, 10); }
                this.Txt.SetPersentFont(T(TxtBox.Height, this.Height) - 0.03, this);
                ButCancel.Size = ButStart.Size = new Size(this.Width / 3, 30);
                ButStart.Location = new Point((this.Width - (ButStart.Width * 2)) / 3, this.Height - Txt.Height - 17);
                ButCancel.Location = new Point(this.Width - ButStart.Left - ButStart.Width, ButStart.Top);
            }
            public void SetDizin() { thisSizeChanged(null, null); }
        }
        //K//----------------------------------------------//
        class MyPanelMasage : MyForm
        {
            public MyLabel Payam = new MyLabel(), ButCancel = new MyLabel(), ButStart = new MyLabel();
            public MyPanelMasage()
            {
                this.BackColor = Color.FromArgb(60, 60, 90);
                this.Controls.Add(Payam); Payam.Dock = DockStyle.Top;
                this.Controls.Add(ButStart); this.Controls.Add(ButCancel);
                ////////////
                ButStart.Text = "OK"; ButStart.CreataBut();
                ButStart.Click += ButStartClick; ButStart.Visible = true;
                ////////////
                ButCancel.Text = "Cancel"; ButCancel.CreataBut();
                ButCancel.Click += ButCancelClick; ButCancel.Visible = true;
                ////////////
                this.Eve_Load += thisEve_Load; this.Payam.SetPersentFont(0.12, this);
                Payam.ForeColor = Color.FromArgb(230, 230, 230);
            }
            void ButStartClick(object s, EventArgs e) { this.DialogResult = DialogResult.Yes; this.Close(); }
            void ButCancelClick(object s, EventArgs e) { this.DialogResult = DialogResult.No; this.Close(); }
            void thisEve_Load()
            {
                if (ButCancel.Visible)
                {
                    ButCancel.Size = ButStart.Size = new Size(this.Width / 3, 30);
                    ButStart.Location = new Point((this.Width - (ButStart.Width * 2)) / 3, this.Height - ButStart.Height - 7);
                    ButCancel.Location = new Point(this.Width - ButStart.Left - ButStart.Width, this.Height - ButStart.Height - 7);
                }
                else
                {
                    ButStart.Size = new Size(this.Width / 2, 30); ButCancel.Left = this.Width;
                    ButStart.Location = new Point((this.Width / 2) - (ButStart.Width / 2), this.Height - ButStart.Height - 7);
                }
                Payam.Height = ButStart.Top - 7;
            }
        }
        //K//----------------------------------------------//
        class ImageBox : MyForm
        {
            PictureBox ImBox = new PictureBox();
            Timer Tim = new Timer(); MyLabel P = new MyLabel();
            public ImageBox()
            {
                this.Opacity = 0;
                ///////////
                this.Controls.Add(ImBox); ImBox.Dock = DockStyle.Fill; ImBox.BackColor = Color.Transparent;
                ImBox.SizeMode = PictureBoxSizeMode.StretchImage;
                ///////////
                Tim.Tick += TimTick; this.Tim.Interval = 40; Tim.Enabled = false;
                //////////
                ImBox.Controls.Add(P); P.BackColor = Color.Transparent;
                P.ForeColor = Color.FromArgb(250, 230, 230); P.Dock = DockStyle.Top; P.Text = "Empty";
                P.SetPersentFont(0.5);
                Eve_Load += Eve_Loading;
            }
            void Eve_Loading() { Opacity = 0; }
            public void ShowImage(string URL)
            {
                try
                {
                    ImBox.Image = (new Bitmap(URL));
                    P.Height = 0;
                    if (T(ImBox.Image.Width, F.Width) > T(ImBox.Image.Height, F.Height))
                    {
                        this.Size = new Size(F.Width, (F.Width * ImBox.Image.Height) / ImBox.Image.Width);
                    }
                    else
                    {
                        this.Size = new Size((F.Height * ImBox.Image.Width) / ImBox.Image.Height, F.Height);
                    }
                }
                catch { this.Size = new Size(F.Width / 2, P.Height = F.Height / 5); ImBox.Image = null; }
                opn = true; Tim.Enabled = true;
            }
            public void EndImage()
            {
                ImBox.Image = null;
                opn = false; Tim.Enabled = true;
            }
            //K//////////////////////////////
            bool opn = false; public KaroTicket F;
            void TimTick(object s, EventArgs e)
            {
                if (opn)
                {
                    if (this.Opacity < 1) { this.Opacity = ((this.Opacity + 0.07) * 4) / 3; }
                    if (this.Opacity >= 1) { this.Opacity = 1; Tim.Enabled = false; }
                }
                else
                {
                    if (this.Opacity > 0) { this.Opacity = (this.Opacity * 2) / 3; }
                    if (this.Opacity <= 0) { this.Opacity = 0; Tim.Enabled = false; }
                }
                int i;
                if ((Screen.PrimaryScreen.Bounds.Width - F.Left - F.Width) >= (this.Width + 5))
                { this.Top = F.Top; i = F.Left + F.Width - 18; this.Left = i + (int)((double)(25 * this.Opacity)); }
                else if ((Screen.PrimaryScreen.Bounds.Height - F.Top - F.Height) >= (this.Height + 5))
                { this.Left = F.Left; i = F.Top + F.Height - 20; this.Top = i + (int)((double)(25 * this.Opacity)); }
                else if (F.Left >= (this.Width + 5))
                { this.Top = F.Top; i = F.Left - this.Width - 7 + 25; this.Left = i - (int)((double)(25 * this.Opacity)); }
                else if (F.Top >= (this.Height + 5))
                { this.Left = F.Left; i = F.Top - this.Height - 5 + 25; this.Top = i - (int)((double)(25 * this.Opacity)); }
                else
                {
                    int[] a = { (Screen.PrimaryScreen.Bounds.Width - F.Left - F.Width) - (this.Width + 5)
                              , (Screen.PrimaryScreen.Bounds.Height - F.Top - F.Height) - (this.Height + 5)
                              , F.Left - (this.Width + 5), F.Top - (this.Height + 5)};
                    for (i = 0; i < a.Length; i++) if (a[i] < 0) a[i] = -a[i];
                    i = a[0]; if (i > a[1]) i = a[1]; if (i > a[2]) i = a[2]; if (i > a[3]) i = a[3];
                    if (i == a[0]) { this.Top = F.Top; i = F.Left + F.Width - 18; this.Left = i + (int)((double)(25 * this.Opacity)); }
                    else if (i == a[1]) { this.Left = F.Left; i = F.Top + F.Height - 20; this.Top = i + (int)((double)(25 * this.Opacity)); }
                    else if (i == a[2]) { this.Top = F.Top; i = F.Left - this.Width - 7 + 25; this.Left = i - (int)((double)(25 * this.Opacity)); }
                    else { this.Left = F.Left; i = F.Top - this.Height - 5 + 25; this.Top = i - (int)((double)(25 * this.Opacity)); }
                }
            }
        }
        //K//----------------------------------------------//
        class KtaS : MyLabel
        {
            public delegate void KtaSEvent(KtaS P);
            public KtaSEvent EventClick, EventDel, EventEdit;
            MyButon Del = new MyButon(), Edi = new MyButon();
            public KtaS()
            {
                this.Dock = DockStyle.Top; this.SetColorDinamic(null); this.Click += thisLabClick;
                this.Controls.Add(Del); this.Controls.Add(Edi);
                /////////////
                Del.SetImages(KaroSource.KaroDelOff, KaroSource.KaroDelOn); Del.Click += Del_Click;
                /////////////
                Edi.SetImages(KaroSource.KaroEditOff, KaroSource.KaroEditOn); Edi.Click += Edi_Click;
                /////////////
                this.SizeChanged += thisSizeChanged; thisSizeChanged(null, null);
            }
            void thisLabClick(object s, EventArgs e) { if (EventClick != null) { EventClick(this); } }
            void Del_Click(object s, EventArgs e) { if (EventDel != null) EventDel(this); }
            void Edi_Click(object s, EventArgs e) { if (EventEdit != null) EventEdit(this); }
            void thisSizeChanged(object s, EventArgs e)
            {
                Edi.Size = Del.Size = new Size((this.Height * 4) / 5, (this.Height * 4) / 5);
                Del.Location = new Point(this.Width - Del.Width - 8, this.Height - Del.Height - 1);
                Edi.Location = new Point(7, this.Height - Del.Height - 1);
            }
        }
        //K//----------------------------------------------//
        class ThisOUTObj : MyLabel
        {
            //ContextMenuStrip CMS = new ContextMenuStrip();
            public MyPanelDat.TStripBut ButCopy = new MyPanelDat.TStripBut(), ButMove = new MyPanelDat.TStripBut();
            public delegate void MyEve(ThisOUTObj P);
            public MyEve Evnt_Del, Evnt_Edit, EvntCopy, EvntMove; public static A_S[] Dat;
            public MyButon Del = new MyButon(), Edt = new MyButon(), Img = new MyButon(), Talafz = new MyButon();
            //MyButon RitClick = new MyButon();
            int V = 0; public byte Val_n = 0;
            public void SetNet()
            {

                /* CMS.Items.Remove(CMS.Items[0]);
                 CMS.Items.Remove(CMS.Items[0]);
                 ButMove = null;*/
            }
            public ThisOUTObj()
            {
                this.Controls.Add(Del); this.Controls.Add(Edt); this.SizeChanged += thisSizeChanged;
                this.Controls.Add(Img); this.Controls.Add(Talafz);
                //this.Controls.Add(RitClick);
                Del.SetImages(KaroSource.KaroDelOff, KaroSource.KaroDelOn);
                Edt.SetImages(KaroSource.KaroEditOff, KaroSource.KaroEditOn);
                /////////////
                Img.SetImages(KaroSource.KaroImgOff, KaroSource.KaroImgOn);
                Img.MouseDown += ImgMouseDown; Img.MouseUp += ImgMouseUp;
                /////////////
                Talafz.SetImages(KaroSource.KaroTalafzOff, KaroSource.KaroTalafzOn);
                Talafz.MouseDown += TalafzMouseDown; Talafz.MouseUp += TalafzMouseUp; Talafz.Visible = MyPanelDat.EToP;
                //////
                Del.Click += DelClick; Edt.Click += EdtClick;
                /////////////
                /*RitClick.SetImages(KaroSource.KaroRitClickOff, KaroSource.KaroRitClickOn);
                RitClick.Click += RitClickClick;*/
                //////
                this.MouseDown += thisMouseDown; this.MouseUp += thisMouseUp;
                MyPanelDat.En_Pr_Event += MyPanelDatEn_Pr_Event; this.SetPersentFont(0.19, this);
                this.SetColorDinamic(null);
                //////
                thisSizeChanged(null, null);
                ////////////


                /*CMS.Items.Add(ButMove); ButMove.Text = "انتقال به"; CMS.Items.Add("-");
                CMS.Items.Add(ButCopy); ButCopy.Text = "اضافه شود به";
                CMS.BackColor = Color.FromArgb(70, 60, 60); CMS.Width = 130;
                this.ContextMenuStrip = CMS;*/
                ButMove.Click += ButMoveClick; ButCopy.Click += ButCopyClick;
            }
            void RitClickClick(object s, EventArgs e)
            {
                /*CMS.Show();
                CMS.Left = RitClick.Left + this.Left + this.Parent.Left + this.Parent.Parent.Left - 7;
                CMS.Top = RitClick.Top + this.Top + this.Parent.Top + this.Parent.Parent.Top - 30;*/
            }
            void ImgMouseDown(object s, EventArgs e) { ImBox.ShowImage(Dat[Val].URL); }
            void ImgMouseUp(object s, EventArgs e) { ImBox.EndImage(); }
            void TalafzMouseDown(object s, EventArgs e) { thisMouseEnter(null, null); this.Text = Dat[Val].Talafz; }
            void TalafzMouseUp(object s, EventArgs e) { thisMouseLeave(null, null); this.Text = Dat[Val].S1; }
            void DelClick(object s, EventArgs e) { if (Evnt_Del != null) Evnt_Del(this); }
            void EdtClick(object s, EventArgs e) { if (Evnt_Edit != null) Evnt_Edit(this); }
            void ButMoveClick(object s, EventArgs e) { if (EvntMove != null) EvntMove(this); }
            void ButCopyClick(object s, EventArgs e) { if (EvntCopy != null) EvntCopy(this); }
            void MyPanelDatEn_Pr_Event()
            {
                if (V < Dat.Length) { this.Visible = true; if (MyPanelDat.EToP) this.Text = Dat[V].S1; else this.Text = Dat[V].S2; }
                else { this.Visible = false; } Talafz.Visible = MyPanelDat.EToP;
            }
            public int Val
            {
                set
                {
                    V = value;
                    if (V < Dat.Length) { this.Visible = true; if (MyPanelDat.EToP) this.Text = Dat[V].S1; else this.Text = Dat[V].S2; }
                    else { this.Visible = false; }
                }
                get { return V; }
            }
            void thisMouseDown(object s, EventArgs e)
            { if (V < Dat.Length) { if (MyPanelDat.EToP) this.Text = Dat[V].S2; else this.Text = Dat[V].S1; } }
            void thisMouseUp(object s, EventArgs e)
            { if (V < Dat.Length) { if (MyPanelDat.EToP) this.Text = Dat[V].S1; else this.Text = Dat[V].S2; } }
            void thisSizeChanged(object s, EventArgs e)
            {
                //RitClick.Size = 
                Talafz.Size = Img.Size = Del.Size = Edt.Size = new Size(this.Height / 5, this.Height / 5);
                Del.Location = new Point(this.Width - 7 - Del.Width, this.Height - 1 - Del.Height);
                Edt.Location = new Point(Del.Left - 7 - Edt.Width, this.Height - 1 - Del.Height);
                Img.Location = new Point(this.Width - Del.Width - 12, 1);
                Talafz.Location = new Point(Img.Left - 7 - Talafz.Width, 1);
                //RitClick.Location = new Point(12, Del.Top);
            }
        }
        class FormBaygan : MyForm
        {
            MyHS HS = new MyHS(); ThisOUTObj[] Obj = new ThisOUTObj[6];
            public Panel ObjPanel = new Panel(), TopBar = new Panel(), Setin = new Panel();
            MyButon Clos = new MyButon(), Add_Now = new MyButon();
            PictureBox Namebox = new PictureBox(); public MyLabel Nametx = new MyLabel();
            void SetDat()
            {
                SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                int i = 0;
                SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                cmd.CommandText = "select * from Days where Ketab='" + KaroFilName + "' and Day_i=0;";
                SqlDataReader Sql = cmd.ExecuteReader();
                while (Sql.Read()) { i = i + 1; } Sql.Close();
                if (i > 0)
                {
                    A a = new A(i); ThisOUTObj.Dat = a.Dat; a.L = 0;
                    cmd.CommandText = "select * from Days where Ketab='" + KaroFilName + "' and Day_i=0;";
                    Sql = cmd.ExecuteReader(); KaroTicket.GetDataT(a, Sql); Sql.Close();
                    Obj[0].Enabled = true;
                }
                else
                {
                    ThisOUTObj.Dat = new A_S[] { new A_S() }; ThisOUTObj.Dat[0].S1 = "هیچ لغتی موجود نیست";
                    ThisOUTObj.Dat[0].S2 = "هیچ لغتی موجود نیست";
                    Obj[0].Enabled = false;
                }
                Mycon.Close();
            }
            public FormBaygan()
            {
                byte i = 0; for (; i < Obj.Length; i++) { Obj[i] = new ThisOUTObj(); }
                SetDat();
                this.Height = 10; this.Controls.Add(HS); HS.Dock = DockStyle.Right; HS.Width = 23; HS.Val = 0;
                this.Controls.Add(TopBar); TopBar.Dock = DockStyle.Top; TopBar.Height = 30; TopBar.Cursor = Cursors.SizeAll;
                this.Controls.Add(Setin); Setin.Dock = DockStyle.Bottom; Setin.Height = 30;
                Clos.Size = new Size(20, 20); TopBar.Controls.Add(Clos); Clos.Click += ClosClick; Clos.Cursor = Cursors.Arrow;
                Clos.Location = new Point(TopBar.Width - 7 - Clos.Width, (TopBar.Height / 2) - (Clos.Height / 2));
                Clos.SetImages(KaroSource.KaroCloseOff, KaroSource.KaroCloseOn);
                TopBar.Controls.Add(Namebox); Namebox.Size = new System.Drawing.Size(TopBar.Height, TopBar.Height);
                Namebox.Location = new Point(7, 0); Namebox.SizeMode = PictureBoxSizeMode.StretchImage;
                Namebox.Image = KaroSource.KaroBayganiOn; Namebox.Cursor = Cursors.Arrow;
                Namebox.BackColor = Color.Transparent;
                TopBar.Controls.Add(Nametx); Nametx.Size = new System.Drawing.Size(200, TopBar.Height);
                Nametx.Location = new Point(Namebox.Left + Namebox.Width + 7, 0);
                Nametx.TextAlign = ContentAlignment.MiddleLeft; Nametx.Cursor = Cursors.Arrow; Nametx.Text = "بایگانی";
                //////////////////////////////////////
                Setin.Controls.Add(Add_Now); Add_Now.Size = new Size(27, 27);
                Add_Now.Location = new Point((Setin.Width / 2) - (Add_Now.Width / 2), (Setin.Height / 2) - (Add_Now.Height / 2));
                Add_Now.SetImages(KaroSource.KaroAddOff, KaroSource.KaroAddOn);
                Add_Now.Click += Add_Now_Click;
                ///////////
                TopBar.MouseDown += TopBarMouseDown; TopBar.MouseUp += TopBarMouseUp; TopBar.MouseMove += TopBarMouseMove;
                ///////////
                this.Controls.Add(ObjPanel); ObjPanel.Location = new Point(0, TopBar.Height);
                ObjPanel.Size = new Size(this.Width - HS.Width, ((((this.Height - (3 * 3) - TopBar.Height - Setin.Height) / 2)) * 3) + (4 * 3));
                int w = (ObjPanel.Width - 9) / 2, h = (ObjPanel.Height - 12) / 3;
                for (i = 0; i < Obj.Length; i++)
                {
                    ObjPanel.Controls.Add(Obj[i]); Obj[i].Size = new System.Drawing.Size(w, h);
                    Obj[i].Location = new Point(3 + ((3 + w) * (i % 2)), 3 + ((3 + h) * (i / 2)));
                    Obj[i].BackColor = Color.FromArgb(130, 40, 30, 37);
                    Obj[i].Val_n = i; Obj[i].Evnt_Del += But_Delet_Click; Obj[i].Evnt_Edit += But_Edit_Click;
                    Obj[i].Val = i;
                }
                ///////////
                HS.EventMove += HSEventMove; ObjPanel.Top = TopBar.Height; HS.Val = 0;
                this.SizeChanged += thisSizeChanged;
            }
            //K//------------------------------------------------//
            private void But_Delet_Click(ThisOUTObj P)
            {
                MyPanelMasage MyMasage = new MyPanelMasage();
                MyMasage.SetKarForm(this, 0.97, T(this.Height - TopBar.Height - Setin.Height - 30, this.Height));
                ////////////////////////////////////////////
                MyMasage.Payam.Text = "آیا مطمئنید که حذف شود؟"; MyMasage.Payam.RightToLeft = RightToLeft.Yes;
                MyMasage.ButCancel.Visible = true;
                if (MyMasage.ShowDialog() == DialogResult.Yes)
                {
                    Baygani_Delet_Record(ThisOUTObj.Dat[Obj[P.Val_n].Val]); SetDat();
                    if (Obj[5].Visible == false) { HS.Val = 1; }
                    else { for (byte i = 0; i < Obj.Length; i++) Obj[i].Val = Obj[i].Val; }
                }
            }
            private void But_Edit_Click(ThisOUTObj P)
            {
                MyPanelTextBox InputText = new MyPanelTextBox(); InputText.SetDizin();
                InputText.SetKarForm(this, 0.95, T(((this.Height - TopBar.Height - Setin.Height) * 4) / 5, this.Height));
                InputText.Lokat = ThisOUTObj.Dat[P.Val].S1;
                //////////////////////////////////////
                InputText.S1.Text = ThisOUTObj.Dat[P.Val].S1;
                InputText.S2.Text = ThisOUTObj.Dat[P.Val].S2;
                InputText.Talafz.Text = ThisOUTObj.Dat[P.Val].Talafz;
                InputText.URL.Text = ThisOUTObj.Dat[P.Val].URL;
                if (InputText.ShowDialog() == DialogResult.Yes)
                {
                    A_S a = InputText.GetRecord();
                    Baygani_Edit_Record(ThisOUTObj.Dat[P.Val], a); ThisOUTObj.Dat[P.Val] = a; P.Val = P.Val;
                }
            }
            //K//------------------------------------------------//
            void Add_Now_Click(object s, EventArgs e)
            {
                MyPanelTextBox InputText = new MyPanelTextBox(); InputText.SetDizin();
                InputText.SetKarForm(this, 0.95, T(((this.Height - TopBar.Height - Setin.Height) * 4) / 5, this.Height));
                //////////////////////////////////////
                InputText.Talafz.Text = ""; InputText.S2.Text = ""; InputText.S1.Text = ""; InputText.URL.Text = "Image";
                if (InputText.ShowDialog() == DialogResult.Yes) { Baygani_Save_Record(InputText.GetRecord()); SetDat(); HS.Val = 1; }
            }
            //K//------------------------------------------------//
            void HSEventMove()
            {
                int t = TopBar.Height - (int)((double)(HS.Val *
                    (((ThisOUTObj.Dat.Length / 2) + (((ThisOUTObj.Dat.Length % 2) > 0) ? 1 : 0) - 2) * (Obj[0].Height + 3))));
                if (t > TopBar.Height) t = TopBar.Height;
                int i = 0, j = (3 + Obj[0].Height);
                while (t < (TopBar.Height - j)) { t += j; i += 1; }
                i = i * 2;
                for (j = 0; j < Obj.Length; j++) { Obj[j].Val = i + j; }
                ObjPanel.Top = t;
            }
            int x = -7, y = -7;
            void TopBarMouseDown(object s, MouseEventArgs e) { x = e.X; y = e.Y; }
            void TopBarMouseMove(object s, MouseEventArgs e) { if (x > 0) { this.Left += e.X - x; this.Top += e.Y - y; } }
            void TopBarMouseUp(object s, MouseEventArgs e) { x = -7; y = -7; }
            void ClosClick(object s, EventArgs e) { this.Close(); }
            void thisSizeChanged(object s, EventArgs e)
            {
                ObjPanel.Size = new Size(this.Width - HS.Width, ((((this.Height - (3 * 3) - TopBar.Height - Setin.Height) / 2)) * 3) + (4 * 3));
                ObjPanel.Location = new Point(0, TopBar.Height); int w = (ObjPanel.Width - 9) / 2, h = (ObjPanel.Height - 12) / 3; byte i;
                for (i = 0; i < Obj.Length; i++)
                {
                    Obj[i].Size = new System.Drawing.Size(w, h);
                    Obj[i].Location = new Point(3 + ((3 + w) * (i % 2)), 3 + ((3 + h) * (i / 2)));
                    Obj[i].Val = i;
                }
                Clos.Location = new Point(TopBar.Width - 7 - Clos.Width, (TopBar.Height / 2) - (Clos.Height / 2));
                Add_Now.Location = new Point((Setin.Width / 2) - (Add_Now.Width / 2), (Setin.Height / 2) - (Add_Now.Height / 2));
            }
        }
        //K//----------------------------------------------//
        class FormNetwrk : MyForm
        {
            MyHS HS = new MyHS(); ThisOUTObj[] Obj = new ThisOUTObj[6]; MyLabel NameNet = new MyLabel(), NumberNet = new MyLabel();
            public Panel ObjPanel = new Panel(), TopBar = new Panel(), Setin = new Panel();
            MyButon Clos = new MyButon(), Add_Net = new MyButon(), Rit = new MyButon(), Lft = new MyButon()
                , Edit_Net = new MyButon(), Del_Net = new MyButon(), Add_Now = new MyButon();
            PictureBox Namebox = new PictureBox(); public MyLabel Nametx = new MyLabel();
            void SetDat(string Nam)
            {
                SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                cmd.CommandText = "select * from Nets where Ketab='" + KaroFilName + "' and Net='" + Nam + "';";
                SqlDataReader Sql = cmd.ExecuteReader();
                if (Sql.Read())
                {
                    Sql.Close(); int i = 0;
                    cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "select * from NetData where Ketab='" + KaroFilName + "' and Net='" + Nam + "';";
                    Sql = cmd.ExecuteReader(); while (Sql.Read()) { i = i + 1; } Sql.Close();
                    if (i > 0)
                    {
                        A a = new A(i); ThisOUTObj.Dat = a.Dat; a.L = 0;
                        cmd.CommandText = "select * from NetData where Ketab='" + KaroFilName + "' and Net='" + Nam + "';";
                        Sql = cmd.ExecuteReader(); KaroTicket.GetDataT(a, Sql); Sql.Close();
                        Obj[0].Enabled = true;
                    }
                    else
                    {
                        ThisOUTObj.Dat = new A_S[] { new A_S() };
                        ThisOUTObj.Dat[0].S1 = "هیچ لغتی مو جود نیست"; ThisOUTObj.Dat[0].S2 = "هیچ لغتی مو جود نیست";
                        Obj[0].Enabled = false;
                    }
                }
                else { Sql.Close(); ThisOUTObj.Dat = new A_S[] { }; }
                Mycon.Close();
            }
            string[] Names; int Names_i = 0;
            public FormNetwrk()
            {
                byte i; for (i = 0; i < Obj.Length; i++) { Obj[i] = new ThisOUTObj(); }
                SetNets();
                this.Height = 10; this.Controls.Add(HS); HS.Dock = DockStyle.Right; HS.Width = 23; HS.Val = 0;
                this.Controls.Add(NameNet); NameNet.Dock = DockStyle.Top; NameNet.Height = this.Height / 6;
                NameNet.SetPersentFont(0.5);
                NameNet.BackColor = Color.FromArgb(130, 40, 30, 37);
                this.Controls.Add(TopBar); TopBar.Dock = DockStyle.Top; TopBar.Height = 30; TopBar.Cursor = Cursors.SizeAll;
                this.Controls.Add(Setin); Setin.Dock = DockStyle.Bottom; Setin.Height = 30;
                Clos.Size = new Size(20, 20); TopBar.Controls.Add(Clos); Clos.Click += ClosClick; Clos.Cursor = Cursors.Arrow;
                Clos.Location = new Point(TopBar.Width - 7 - Clos.Width, (TopBar.Height / 2) - (Clos.Height / 2));
                Clos.SetImages(KaroSource.KaroCloseOff, KaroSource.KaroCloseOn);
                TopBar.Controls.Add(Namebox); Namebox.Size = new System.Drawing.Size(TopBar.Height, TopBar.Height);
                Namebox.Location = new Point(7, 0); Namebox.SizeMode = PictureBoxSizeMode.StretchImage;
                Namebox.Image = KaroSource.KaroNetWrkOn; Namebox.Cursor = Cursors.Arrow;
                Namebox.BackColor = Color.Transparent;
                TopBar.Controls.Add(Nametx); Nametx.Size = new System.Drawing.Size(200, TopBar.Height);
                Nametx.Location = new Point(Namebox.Left + Namebox.Width + 7, 0);
                Nametx.TextAlign = ContentAlignment.MiddleLeft; Nametx.Cursor = Cursors.Arrow; Nametx.Text = "شبکه سازی لغات";
                //////////////////////////////////////
                Setin.Controls.Add(Add_Net); Add_Net.Size = new Size(27, 27);
                Add_Net.Location = new Point(12, (Setin.Height / 2) - (Add_Net.Height / 2));
                Add_Net.SetImages(KaroSource.KaroAddNetOff, KaroSource.KaroAddNetOn);
                Add_Net.Click += Add_NetClick;
                //////////////////////////////////////
                Setin.Controls.Add(Lft); Lft.Size = new Size(27, 27);
                Lft.Location = new Point((Setin.Width / 2) - Lft.Width - 67, (Setin.Height / 2) - (Lft.Height / 2));
                Lft.SetImages(KaroSource.KaroNetLftOff, KaroSource.KaroNetLftOn);
                Lft.Click += Lft_Click;
                /////////// 
                Setin.Controls.Add(NumberNet); NumberNet.Size = new Size(100, 30); NumberNet.SetPersentFont(0.7);
                NumberNet.Location = new Point((Setin.Width / 2) - (NumberNet.Width / 2), (Setin.Height / 2) - (NumberNet.Height / 2));
                ///////////
                Setin.Controls.Add(Rit); Rit.Size = new Size(27, 27);
                Rit.Location = new Point((Setin.Width / 2) + Rit.Width + 40, (Setin.Height / 2) - (Rit.Height / 2));
                Rit.SetImages(KaroSource.KaroNetRitOff, KaroSource.KaroNetRitOn);
                Rit.Click += Rit_Click;
                ///////////
                Setin.Controls.Add(Add_Now); Add_Now.Size = new Size(27, 27);
                Add_Now.Location = new Point(Add_Net.Left + Add_Net.Width + 12, (Setin.Height / 2) - (Add_Now.Height / 2));
                Add_Now.SetImages(KaroSource.KaroAddOff, KaroSource.KaroAddOn);
                Add_Now.Click += Add_Now_Click;
                //////////////////////////////////////
                TopBar.MouseDown += TopBarMouseDown; TopBar.MouseUp += TopBarMouseUp; TopBar.MouseMove += TopBarMouseMove;
                ////////////////////
                NameNet.Controls.Add(Edit_Net); Edit_Net.Size = new Size(27, 27);
                Edit_Net.Location = new Point(7, NameNet.Height - 7 - Edit_Net.Height);
                Edit_Net.SetImages(KaroSource.KaroEditOff, KaroSource.KaroEditOn);
                Edit_Net.Click += Edit_Net_Click;
                ///////////
                NameNet.Controls.Add(Del_Net); Del_Net.Size = new Size(27, 27);
                Del_Net.Location = new Point(NameNet.Width - 8 - Del_Net.Width, NameNet.Height - 7 - Del_Net.Height);
                Del_Net.SetImages(KaroSource.KaroDelOff, KaroSource.KaroDelOn);
                Del_Net.Click += Del_Net_Click;
                ////////////////////
                this.Controls.Add(ObjPanel); ObjPanel.Location = new Point(0, TopBar.Height + NameNet.Height);
                ObjPanel.Size = new Size(this.Width - HS.Width
                    , (((this.Height - (3 * 3) - NameNet.Height - TopBar.Height - Setin.Height) / 2) * 3) + (4 * 3));
                int w = (ObjPanel.Width - 9) / 2, h = (ObjPanel.Height - 12) / 3;
                for (i = 0; i < Obj.Length; i++)
                {
                    ObjPanel.Controls.Add(Obj[i]); Obj[i].Size = new System.Drawing.Size(w, h);
                    Obj[i].Location = new Point(3 + ((3 + w) * (i % 2)), 3 + ((3 + h) * (i / 2)));
                    Obj[i].BackColor = Color.FromArgb(130, 40, 30, 37);
                    Obj[i].Val_n = i; Obj[i].Evnt_Del += But_Delet_Click; Obj[i].Evnt_Edit += But_Edit_Click;
                    Obj[i].Val = i;
                    Obj[i].SetNet();
                }
                ///////////
                HS.EventMove += HSEventMove; HS.Val = 0; this.SizeChanged += thisSizeChanged;
            }
            //K//------------------------------------------------//
            private void But_Delet_Click(ThisOUTObj P)
            {
                MyPanelMasage MyMasage = new MyPanelMasage();
                MyMasage.SetKarForm(this, 0.97, T(this.Height - TopBar.Height - Setin.Height - 30, this.Height));
                ////////////////////////////////////////////
                MyMasage.Payam.Text = "آیا مطمئنید که حذف شود؟"; MyMasage.Payam.RightToLeft = RightToLeft.Yes;
                MyMasage.ButCancel.Visible = true;
                if (MyMasage.ShowDialog() == DialogResult.Yes)
                {
                    if (Names_i >= Names.Length) Names_i = Names.Length - 1; Netw_Delet_Record(ThisOUTObj.Dat[P.Val], Names[Names_i]);
                    if (Names.Length > 0) { SetDat(Names[Names_i]); } else { SetDat("$#Karo:*#$"); }
                    if (Obj[5].Visible == false) { HS.Val = 1; } else { for (byte i = 0; i < Obj.Length; i++) Obj[i].Val = Obj[i].Val; }
                }
            }
            private void But_Edit_Click(ThisOUTObj P)
            {
                MyPanelTextBox InputText = new MyPanelTextBox(); InputText.SetDizin();
                InputText.NetNam = Names[Names_i];
                InputText.Lokat = ThisOUTObj.Dat[P.Val].S1;
                InputText.SetKarForm(this, 0.95, T(((this.Height - TopBar.Height - Setin.Height) * 4) / 5, this.Height));
                //////////////////////////////////////
                InputText.S2.Text = ThisOUTObj.Dat[P.Val].S2;
                InputText.S1.Text = ThisOUTObj.Dat[P.Val].S1;
                InputText.Talafz.Text = ThisOUTObj.Dat[P.Val].Talafz;
                InputText.URL.Text = ThisOUTObj.Dat[P.Val].URL;
                if (InputText.ShowDialog() == DialogResult.Yes)
                { A_S a = InputText.GetRecord(); Baygani_Edit_Record(ThisOUTObj.Dat[P.Val], a); ThisOUTObj.Dat[P.Val] = a; P.Val = P.Val; }
            }
            void Add_Now_Click(object s, EventArgs e)
            {
                MyPanelTextBox InputText = new MyPanelTextBox(); InputText.SetDizin();
                InputText.NetNam = Names[Names_i];
                InputText.SetKarForm(this, 0.95, T(((this.Height - TopBar.Height - Setin.Height) * 4) / 5, this.Height));
                //////////////////////////////////////
                InputText.Talafz.Text = ""; InputText.S2.Text = ""; InputText.S1.Text = "";
                InputText.URL.Text = "Image";
                if (InputText.ShowDialog() == DialogResult.Yes)
                {
                    if (Names_i >= Names.Length) Names_i = Names.Length - 1; Netw_Save_Record(InputText.GetRecord(), Names[Names_i]);
                    if (Names.Length > 0) { SetDat(Names[Names_i]); } else { SetDat("$#Karo:*#$"); }
                    HS.Val = 1;
                }
            }
            //K//------------------------------------------------//
            void SetNets()
            {
                SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                cmd.CommandText = "select * from Nets where Ketab='" + KaroFilName + "';"; SqlDataReader Sql = cmd.ExecuteReader();
                Names_i = 0; while (Sql.Read()) { Names_i += 1; } Sql.Close();
                Names = new string[Names_i]; Names_i = 0;
                cmd.CommandText = "select * from Nets where Ketab='" + KaroFilName + "';"; Sql = cmd.ExecuteReader();
                SetNetsT(Sql); Sql.Close(); Names_i = 0;
                if (Names.Length > 0)
                {
                    SetDat(Names[0]); NameNet.Text = Names[Names_i];
                    Del_Net.Visible = Edit_Net.Visible = Add_Now.Visible = true;
                    NumberNet.Text = (Names_i + 1).ToString() + " of " + Names.Length;
                }
                else
                {
                    SetDat("$#Karo:*#$"); NameNet.Text = "هیچ شبکه ای موجود نیست";
                    Del_Net.Visible = Edit_Net.Visible = Add_Now.Visible = false;
                    NumberNet.Text = "0 of 0";
                }
                HS.Val = 0;
                Mycon.Close();
            }
            void SetNetsT(SqlDataReader s)
            {
                if (s.Read())
                {
                    string st = s["Net"].ToString();
                    /////////////////
                    SetNetsT(s);
                    /////////////////
                    Names[Names_i] = st; Names_i += 1;
                }
            }
            void Add_NetClick(object s, EventArgs e)
            {
                PanelForNetWordNow F = new PanelForNetWordNow();
                F.SetKarForm(this, 0.95, T((this.ObjPanel.Height * 2) / 5, this.Height));
                if (F.ShowDialog() == DialogResult.Yes)
                {
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "insert into Nets(Ketab,Net) values('" + KaroFilName + "','" + F.TxtBox.Text + "');";
                    cmd.ExecuteNonQuery(); SetNets();
                    Mycon.Close();
                }
            }
            void Edit_Net_Click(object s, EventArgs e)
            {
                PanelForNetWordNow F = new PanelForNetWordNow();
                F.SetKarForm(this, 0.95, T((this.ObjPanel.Height * 2) / 5, this.Height));
                F.TxtBox.Text = NameNet.Text;
                if (F.ShowDialog() == DialogResult.Yes)
                {
                    if (NameNet.Text != F.TxtBox.Text)
                    {
                        SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                        SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                        cmd.CommandText = "update Nets Set Net='" + F.TxtBox.Text + "'"
                            + "where Ketab='" + KaroFilName + "' and Net='" + NameNet.Text + "';";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = "update NetData Set Net='" + F.TxtBox.Text + "'"
                            + "where Ketab='" + KaroFilName + "' and Net='" + NameNet.Text + "';";
                        cmd.ExecuteNonQuery();
                        NameNet.Text = F.TxtBox.Text;
                        Mycon.Close();
                    }
                }
            }
            void Del_Net_Click(object s, EventArgs e)
            {
                MyPanelMasage MyMasage = new MyPanelMasage();
                MyMasage.SetKarForm(this, 0.97, T(this.ObjPanel.Height - 30, this.Height));
                ////////////////////////////////////////////
                MyMasage.Payam.Text = "با حذف شبکه همه ی اطلاعات آن حذف می شود. آیا مطمئید?";
                MyMasage.Payam.RightToLeft = RightToLeft.Yes;
                if (MyMasage.ShowDialog() == DialogResult.Yes)
                {
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "delete from Nets where Ketab='" + KaroFilName + "' and Net='" + NameNet.Text + "';";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "delete from NetData where Ketab='" + KaroFilName + "' and Net='" + NameNet.Text + "';";
                    cmd.ExecuteNonQuery();
                    SetNets();
                    Mycon.Close();
                }
            }
            void Rit_Click(object s, EventArgs e)
            {
                if (Names_i < (Names.Length - 1))
                { Names_i += 1; SetDat(Names[Names_i]); NameNet.Text = Names[Names_i]; NumberNet.Text = (Names_i + 1).ToString() + " of " + Names.Length; HS.Val = 0; }
            }
            void Lft_Click(object s, EventArgs e)
            {
                if (Names_i > 0)
                { Names_i -= 1; SetDat(Names[Names_i]); NameNet.Text = Names[Names_i]; NumberNet.Text = (Names_i + 1).ToString() + " of " + Names.Length; HS.Val = 0; }
            }
            //K//------------------------------------------------//
            void HSEventMove()
            {
                int a = (TopBar.Height + NameNet.Height);
                int t = a - (int)((double)(HS.Val *
                    (((ThisOUTObj.Dat.Length / 2) + (((ThisOUTObj.Dat.Length % 2) > 0) ? 1 : 0) - 2) * (Obj[0].Height + 3))));
                if (t > a) t = a; int i = 0, j = (3 + Obj[0].Height);
                while (t < (a - j)) { t += j; i += 1; }
                i = i * 2; for (j = 0; j < Obj.Length; j++) { Obj[j].Val = i + j; }
                ObjPanel.Top = t;
            }
            int x = -7, y = -7;
            void TopBarMouseDown(object s, MouseEventArgs e) { x = e.X; y = e.Y; }
            void TopBarMouseMove(object s, MouseEventArgs e) { if (x > 0) { this.Left += e.X - x; this.Top += e.Y - y; } }
            void TopBarMouseUp(object s, MouseEventArgs e) { x = -7; y = -7; }
            void ClosClick(object s, EventArgs e) { this.Close(); }
            void thisSizeChanged(object s, EventArgs e)
            {
                NameNet.Height = this.Height / 6;
                ObjPanel.Location = new Point(0, TopBar.Height + NameNet.Height);
                ObjPanel.Size = new Size(this.Width - HS.Width
                    , (((this.Height - (3 * 3) - NameNet.Height - TopBar.Height - Setin.Height) / 2) * 3) + (4 * 3));
                int w = (ObjPanel.Width - 9) / 2, h = (ObjPanel.Height - 12) / 3; byte i;
                for (i = 0; i < Obj.Length; i++)
                {
                    Obj[i].Size = new System.Drawing.Size(w, h);
                    Obj[i].Location = new Point(3 + ((3 + w) * (i % 2)), 3 + ((3 + h) * (i / 2)));
                    Obj[i].Val = i;
                }
                Clos.Location = new Point(TopBar.Width - 7 - Clos.Width, (TopBar.Height / 2) - (Clos.Height / 2));
                Add_Net.Location = new Point(12, (Setin.Height / 2) - (Add_Net.Height / 2));
                Lft.Location = new Point((Setin.Width / 2) - Lft.Width - 67, (Setin.Height / 2) - (Lft.Height / 2));
                NumberNet.Location = new Point((Setin.Width / 2) - (NumberNet.Width / 2), (Setin.Height / 2) - (NumberNet.Height / 2));
                Rit.Location = new Point((Setin.Width / 2) + Rit.Width + 40, (Setin.Height / 2) - (Rit.Height / 2));
                Add_Now.Location = new Point(Add_Net.Left + Add_Net.Width + 12, (Setin.Height / 2) - (Add_Now.Height / 2));
                Edit_Net.Location = new Point(12, NameNet.Height - 7 - Edit_Net.Height);
                Del_Net.Location = new Point(NameNet.Width - 13 - Del_Net.Width, NameNet.Height - 7 - Del_Net.Height);
            }
        }
        //K//----------------------------------------------//
        class FormCombo : MyForm
        {
            class Obj : MyLabel
            {
                public int V = 0;
                public MyEvnt EvntClick;
                public delegate void MyEvnt(Obj O);
                public Obj()
                {
                    this.Click += thisClick;
                    this.SetColorDinamic(null);
                }
                void thisClick(object s, EventArgs e) { if (EvntClick != null) EvntClick(this); }
            }
            Obj[] Objs = new Obj[7000];
            public int Objs_i = 0;
            MyLabel ButCancel = new MyLabel(); public MyLabel ButBack = new MyLabel();
            public MyLabel TitleBox = new MyLabel();
            MyHS Hs = new MyHS(); Panel PanelBotom = new Panel(), NmaishPanel = new Panel(), Nmaish = new Panel();
            public FormCombo()
            {
                this.Controls.Add(NmaishPanel); NmaishPanel.Dock = DockStyle.Fill;
                NmaishPanel.BackColor = Color.FromArgb(30, 30, 40);
                this.Controls.Add(Hs); Hs.Dock = DockStyle.Right; Hs.Width = 25; Hs.EventMove += HsEventMove;
                this.Controls.Add(PanelBotom); PanelBotom.Dock = DockStyle.Bottom; PanelBotom.Height = 33;
                this.Controls.Add(TitleBox); TitleBox.Dock = DockStyle.Top; TitleBox.Height = 35;
                TitleBox.SetPersentFont(0.57);
                TitleBox.ForeColor = Color.FromArgb(255, 220, 230);
                TitleBox.TextAlign = ContentAlignment.MiddleLeft;
                ////////////
                PanelBotom.Controls.Add(ButCancel); ButCancel.Size = new Size(70, 27); ButCancel.Location = new Point(10, 3);
                ButCancel.CreataBut(); ButCancel.Text = "لغو"; ButCancel.Click += ButCancelClick;
                ////////////
                PanelBotom.Controls.Add(ButBack); ButBack.Size = new Size(70, 27);
                ButBack.Location = new Point(ButCancel.Width + ButCancel.Left + 7, 3);
                ButBack.CreataBut(); ButBack.Text = "بازگشت"; ButBack.Click += ButBackClick;
                ////////////
                NmaishPanel.Controls.Add(Nmaish); Nmaish.Width = NmaishPanel.Width; Nmaish.Height = 1;
                Nmaish.Location = new Point(0, 0);
                ///////////   
                this.SizeChanged += thisSizeChanged;
            }
            void thisSizeChanged(object s, EventArgs e) { Nmaish.Width = NmaishPanel.Width; }
            void HsEventMove()
            {
                if (Nmaish.Height > NmaishPanel.Height)
                { Nmaish.Top = (int)((NmaishPanel.Height - Nmaish.Height) * Hs.Val); }
                else { Nmaish.Location = new Point(0, 0); }
            }
            public void SetName(string[] st) { for (int i = 0; i < st.Length; i++) { SetName(st[i]); } }
            public void SetName()
            {
                Nmaish.Height = Objs_i * 30; Hs.Val = 0;
                for (; Objs_i < Objs.Length; Objs_i++)
                    if (Objs[Objs_i] != null) { Objs[Objs_i].Height = 0; }
                    else { goto E; }
            E: ;
            }
            public void SetName(string st)
            {
                if (Objs[Objs_i] == null)
                {
                    Objs[Objs_i] = new Obj(); Nmaish.Controls.Add(Objs[Objs_i]);
                    Objs[Objs_i].Dock = DockStyle.Bottom; Objs[Objs_i].Height = 30;
                    Objs[Objs_i].EvntClick += ObjsEvntClick;
                    Objs[Objs_i].V = Objs_i;
                }
                else { Objs[Objs_i].Height = 30; }
                Objs[Objs_i].Text = new string(st.ToCharArray());
                Objs_i += 1;
            }
            public int Resolt_i = 0;
            public string GetRisolt() { return Objs[Resolt_i].Text; }
            void ObjsEvntClick(Obj o) { Resolt_i = o.V; this.DialogResult = DialogResult.Yes; this.Close(); }
            void ButCancelClick(object s, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
            void ButBackClick(object s, EventArgs e) { this.DialogResult = DialogResult.No; this.Close(); }
        }
        #endregion
        //K//-------------------------------------------------------------//
        public KaroTicket() { InitializeComponent(); }
        //K//-------------------------------------------------------------//
        Random R = new Random(); static ImageBox ImBox = new ImageBox();
        MyButon Close_Box = new MyButon(), KaroRest = new MyButon(), KaroInfo = new MyButon(), But_KtabNow = new MyButon(), But_Home = new MyButon()
            , AddTiket = new MyButon(), ETOP = new MyButon(), Bagan = new MyButon(), Netwrk = new MyButon();
        MyPanelDay[] PanelDay = new MyPanelDay[16]; MyPanelDat[] PanelDat = new MyPanelDat[16];
        MyHS HSDay = new MyHS(), HSDat = new MyHS(); MyLabel NumberDay = new MyLabel(); MyHS MenoHS = new MyHS(); KtaS[] MenoObj;
        //K//-------------------------------------------------------------//
        private void KaroTicket_Load(object sender, EventArgs e)
        {
            Bitmap I;
            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\DateBase.mdf")
                & System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate.mdf"))
            {
                TitleBox.Controls.Add(Close_Box); TitleBox.Controls.Add(KaroRest); TitleBox.Controls.Add(KaroInfo);
                SetingBox.Controls.Add(But_Home); SetingBox.Controls.Add(AddTiket); SetingBox.Controls.Add(NumberDay);
                SetingBox.Controls.Add(ETOP); SetingBox.Controls.Add(Bagan); SetingBox.Controls.Add(Netwrk);
                ////////////////////////////////////////////
                LogoBox.Image = KaroSource.KaroTickImg;
                this.Icon = KaroSource.KaroTick;
                ////////////////////////////////////////////
                this.Size = new System.Drawing.Size(600, 300); TitleBox.Height = 33;
                KaroMeno.Size = KaroMeno.MaximumSize = KaroHome.Size = new System.Drawing.Size(this.Width, this.Height - TitleBox.Height);
                SetingBox.Height = 33;
                LogoBox.Size = new Size(TitleBox.Height - 3, TitleBox.Height - 3);
                LogoBox.Location = new Point(2, 2);
                KaroInfo.Size = KaroRest.Size = Close_Box.Size = new Size(20, 20);
                Close_Box.Top = (TitleBox.Height / 2) - (Close_Box.Height / 2); Close_Box.Left = TitleBox.Width - 20 - Close_Box.Top;
                KaroRest.Location = new Point(Close_Box.Left - KaroRest.Width - 10, Close_Box.Top);
                KaroInfo.Location = new Point(KaroRest.Left - KaroInfo.Width - 10, Close_Box.Top);
                Close_Box.Cursor = Cursors.Arrow; Close_Box.Click += ClosBox_Click;
                KaroRest.Cursor = Cursors.Arrow; KaroRest.Click += KaroRest_Click;
                KaroInfo.Cursor = Cursors.Arrow; KaroInfo.Click += KaroInfo_Click;
                NameBox.Location = new Point(LogoBox.Left + LogoBox.Width + 3, (TitleBox.Height / 2) - (NameBox.Height / 2));
                Ktab.Location = new Point(NameBox.Left + NameBox.Width + 3, NameBox.Top);
                Close_Box.SetImages(KaroSource.KaroCloseOff, KaroSource.KaroCloseOn);
                KaroRest.SetImages(KaroSource.KaroRestOff, KaroSource.KaroRestOn);
                KaroInfo.SetImages(KaroSource.KaroInfoOff, KaroSource.KaroInfoOn);
                Panel_Dat.Height = 0; Panel_DataDay.Height = NmaishBox.Height;
                Panel_Dat.MaximumSize = new System.Drawing.Size(NmaishBox.Width, NmaishBox.Height);
                //////////////////////////////////////
                Panel_DataDay.Controls.Add(HSDay); Panel_Dat.Controls.Add(HSDat);
                HSDay.Size = HSDat.Size = new System.Drawing.Size(23, NmaishBox.Height - 2);
                HSDay.Location = new Point(NmaishBox.Width - 26, 1); HSDat.Location = new Point(NmaishBox.Width - 26, 1);
                ////////////////////////////////////////
                int w = (NmaishBox.Width - 15 - 30) / 4, h = (NmaishBox.Height - 12) / 3; byte i;
                Panel_D_Dat.Size = Panel_D_Day.Size = new System.Drawing.Size(NmaishBox.Width - HSDay.Width - 6, 3 + ((h + 3) * 4));
                Panel_D_Day.Location = Panel_D_Dat.Location = new Point(3, 0);
                for (i = 0; i < PanelDay.Length; i++)
                {
                    PanelDay[i] = new MyPanelDay();
                    Panel_D_Day.Controls.Add(PanelDay[i]);
                    PanelDay[i].BackColor = Color.FromArgb(130, 40, 30, 37);
                    PanelDay[i].Size = new Size(w, h);
                    PanelDay[i].Location = new Point(((3 + w) * (i % 4)), 3 + ((3 + h) * (i / 4)));
                    PanelDay[i].Val_n = i;
                    PanelDay[i].Loc = new MyPoint(PanelDay[i].Left, PanelDay[i].Top);
                    PanelDay[i].MinimumSize = new Size(w, h);
                    PanelDay[i].MaximumSize = new Size(Panel_D_Day.Width + 25 - 20, NmaishBox.Height - 30);
                    PanelDay[i].EventClick += PanelDayEventClick;
                    ///////////
                    PanelDat[i] = new MyPanelDat();
                    Panel_D_Dat.Controls.Add(PanelDat[i]);
                    PanelDat[i].BackColor = Color.FromArgb(130, 40, 30, 37);
                    PanelDat[i].Size = new Size(w, h);
                    PanelDat[i].Location = new Point(((3 + w) * (i % 4)), 3 + ((3 + h) * (i / 4)));
                    PanelDat[i].Loc = new MyPoint(PanelDat[i].Left, PanelDat[i].Top);
                    PanelDat[i].MinimumSize = new Size(w, h);
                    PanelDat[i].MaximumSize = new Size(Panel_D_Day.Width + 25 - 20, NmaishBox.Height - 30);
                    PanelDat[i].EventClick += PanelDatEventClick;
                    PanelDat[i].Val_n = i;
                    PanelDat[i].EventTikYes += But_Tik_Click;
                    PanelDat[i].EventTikNo += But_NotTik_Click;
                    PanelDat[i].EventDel += But_Delet_Click;
                    PanelDat[i].EventEdit += But_Edit_Click;
                    PanelDat[i].EvntMove += PanelDatEvntMove;
                    PanelDat[i].EvntCopy += PanelDatEvntCopy;
                }
                //////////////////////////////////////
                I = new Bitmap(2, 2);
                {
                    for (i = (byte)(I.Width); i > 0; i--)
                    {
                        I.SetPixel(i - 1, I.Width - i, Color.FromArgb(100, 30, 10, 10));
                    }
                }
                TitleBox.BackgroundImage = I;
                TitleBox.BackColor = Color.FromArgb(100, 100, 150);
                ////////////
                I = new Bitmap(5, 7);
                I.SetPixel(1, 2, Color.FromArgb(60, 150, 150, 150));
                I.SetPixel(0, 3, Color.FromArgb(40, 150, 150, 150));
                I.SetPixel(3, 6, Color.FromArgb(30, 150, 150, 150));
                Panel_D_Day.BackgroundImage = Panel_D_Dat.BackgroundImage = KaroMeno.BackgroundImage
                    = NmaishBox.BackgroundImage = I;
                KaroMeno.BackColor = Panel_D_Day.BackColor = Panel_D_Dat.BackColor = NmaishBox.BackColor
                    = Color.FromArgb(50, 50, 70);
                ////////////
                I = new Bitmap(17, SetingBox.Height);
                {
                    int a, b;
                    for (i = 0; i < I.Width; i++)
                    {
                        a = 1 + R.Next(I.Height - 3 - (9)); b = a + ((9)); if (b >= I.Height) b = I.Height - 2;
                        for (; a < b; a++) { I.SetPixel(i, a, Color.FromArgb(23, 250, 250, 250)); }
                        ///////
                        I.SetPixel(i, 0, Color.FromArgb(130, 60, 60));
                        I.SetPixel(i, I.Height - 1, Color.FromArgb(190, 70, 70));
                    }
                }
                SetingMeno.BackgroundImage = SetingBox.BackgroundImage = I;
                SetingMeno.BackColor = SetingBox.BackColor = Color.FromArgb(30, 30, 47);
                //////////////////////////////////////
                Bagan.Size = Netwrk.Size = ETOP.Size = AddTiket.Size = But_Home.Size = new Size(27, 27);
                But_Home.Location = new Point(7, 3);
                AddTiket.Location = new Point((SetingBox.Width / 2) - (AddTiket.Width / 2), NmaishBox.Height);
                ETOP.Location = new Point((SetingBox.Width / 2) + (AddTiket.Width / 2) + 10, NmaishBox.Height);
                Netwrk.Location = new Point(SetingBox.Width - 7 - Netwrk.Width, 3);
                Bagan.Location = new Point(Netwrk.Left - 7 - Bagan.Width, 3);
                //////////////////////////////////////
                But_Home.SetImages(KaroSource.KaroHomeOff, KaroSource.KaroHomeOn);
                AddTiket.SetImages(KaroSource.KaroAddOff, KaroSource.KaroAddOn);
                ETOP.SetImages(KaroSource.KaroEnTOPrOff, KaroSource.KaroEnTOPrOn);
                Bagan.SetImages(KaroSource.KaroBayganiOff, KaroSource.KaroBayganiOn);
                Netwrk.SetImages(KaroSource.KaroNetWrkOff, KaroSource.KaroNetWrkOn);
                //////////////////////////////////////
                But_Home.Click += But_Home_Click; AddTiket.Click += But_Add_Click;
                HSDay.EventMove += HSDayEventMove; HSDat.EventMove += HSDatEventMove;
                ETOP.Click += ETOPClick; Bagan.Click += BaganClick; Netwrk.Click += NetwrkClick;
                //////////////////////////////////////
                NumberDay.Size = new System.Drawing.Size(SetingBox.Width / 7, 27);
                NumberDay.Location = new Point(But_Home.Width + 15, 3);
                NumberDay.BackColor = Color.FromArgb(30, 20, 25);
                NumberDay.ForeColor = Color.White;
                //////////////////////////////////////
                ImBox.F = this; ImBox.Size = this.Size; ImBox.Owner = this; ImBox.Show();
                ////////////////////////////////////////////
                SetingMeno.Height = 33;
                KaroMeno.Controls.Add(MenoHS); MenoHS.Size = new System.Drawing.Size(23, KaroMeno.Height - SetingMeno.Height);
                MenoHS.Location = new Point(KaroMeno.Width - MenoHS.Width - 1, 0);
                if (!Close_Box.Visible) { Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End; }
                MenoNmaishPanel.Size = new System.Drawing.Size(KaroMeno.Width - MenoHS.Width, KaroMeno.Height);
                ////////////////////////
                try
                {
                    SqlConnection Mycon; SqlCommand cmd; SqlDataReader Sql;
                    string St = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\AllLanguages"; 
                    if (!(System.IO.Directory.Exists(St)))
                    {
                        System.IO.Directory.CreateDirectory(St);
                        System.IO.File.Copy(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate.mdf", St + @"\KaroDate.mdf");
                    }
                    else if (!(System.IO.File.Exists(St + @"\KaroDate.mdf")))
                    { System.IO.File.Copy(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate.mdf", St + @"\KaroDate.mdf"); }
                    Mycon = new SqlConnection(@"
                                Data Source=.\SQLEXPRESS;  
                                AttachDbFilename=" + St + @"\KaroDate.mdf;
                                Integrated Security=True;
                                User Instance=True
                    ");
                    Mycon.Open();
                    cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "select * from MyDat  where MyKey='URL';";
                    Sql = cmd.ExecuteReader(); Sql.Read();
                    St = Sql["MyVal"].ToString(); Sql.Close();
                    if (St == "#")
                    {
                        {
                            MyPanelMasage MyMasage = new MyPanelMasage();
                            MyMasage.Size = new Size(700, 250);
                            ////////////
                            {
                                I = new Bitmap(19, 17);
                                int l, a, b;
                                for (l = 0; l < I.Width; l++)
                                {
                                    a = R.Next(I.Height - (2)); b = a + ((2)); if (b >= I.Height) b = I.Height - 1;
                                    for (; a < b; a++)
                                    { I.SetPixel(l, a, Color.FromArgb(50 + R.Next(10), 100 + R.Next(130), 100 + R.Next(130), 100 + R.Next(130))); }
                                }
                                MyMasage.BackgroundImageLayout = ImageLayout.Stretch;
                                MyMasage.BackgroundImage = I;
                                I = new Bitmap(MyMasage.Width, 1);
                                Graphics.FromImage(I).Clear(Color.FromArgb(230, 70, 70));
                                MyMasage.Payam.ImageAlign = ContentAlignment.BottomCenter; MyMasage.Payam.Image = I;
                                MyMasage.BackColor = Color.FromArgb(37, 37, 57);
                            }
                            ////////////////////////////////////
                            MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = true;
                            MyMasage.StartPosition = FormStartPosition.CenterScreen;
                            MyMasage.Payam.SetPersentFont(0.115);
                            MyMasage.Payam.Text = "شما به هیچ پایگاه داده ای وصل نیستید.";
                            MyMasage.ButStart.Text = "درست کردن";
                            MyMasage.ButCancel.Text = "انتخاب";
                            if (MyMasage.ShowDialog() == DialogResult.Yes)
                            {
                                FolderBrowserDialog F = new FolderBrowserDialog();
                            S1: ;
                                if (F.ShowDialog() == DialogResult.OK)
                                {
                                    St = ""; while (System.IO.File.Exists(F.SelectedPath + @"\DateBase" + St + ".mdf")) { St += "Now"; }
                                    try
                                    {
                                        System.IO.File.Copy(System.IO.Directory.GetCurrentDirectory() + @"\DateBase.mdf"
                                            , F.SelectedPath + @"\DateBase" + St + ".mdf");
                                        St = F.SelectedPath + @"\DateBase" + St + ".mdf";
                                    }
                                    catch { goto S1; }
                                }
                                else { Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End; }
                            }
                            else
                            {
                                OpenFileDialog F = new OpenFileDialog();
                                F.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                                F.Filter = "DataSorse|*.mdf;"; F.Multiselect = false;
                                if (F.ShowDialog() == DialogResult.OK) { St = F.FileName; }
                                else { Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End; }
                            }
                        }
                        cmd.CommandText = "update MyDat set MyVal='" + St + "' where MyKey='URL';"; cmd.ExecuteNonQuery();
                    }
                    MyURL = @"
                                Data Source=.\SQLEXPRESS;  
                                AttachDbFilename=" + St + @";
                                Integrated Security=True;
                                User Instance=True
                            ";
                    {
                        MyPanelMasage MyMasage = new MyPanelMasage();
                        MyMasage.Size = new Size(700, 250);
                        ////////////
                        {
                            I = new Bitmap(19, 17);
                            int l, a, b;
                            for (l = 0; l < I.Width; l++)
                            {
                                a = R.Next(I.Height - (2)); b = a + ((2)); if (b >= I.Height) b = I.Height - 1;
                                for (; a < b; a++)
                                { I.SetPixel(l, a, Color.FromArgb(50 + R.Next(10), 100 + R.Next(130), 100 + R.Next(130), 100 + R.Next(130))); }
                            }
                            MyMasage.BackgroundImageLayout = ImageLayout.Stretch;
                            MyMasage.BackgroundImage = I;
                            I = new Bitmap(MyMasage.Width, 1);
                            Graphics.FromImage(I).Clear(Color.FromArgb(230, 70, 70));
                            MyMasage.Payam.ImageAlign = ContentAlignment.BottomCenter; MyMasage.Payam.Image = I;
                            MyMasage.BackColor = Color.FromArgb(37, 37, 57);
                        }
                        MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = false;
                        MyMasage.StartPosition = FormStartPosition.CenterScreen;
                        MyMasage.Payam.SetPersentFont(0.115); MyMasage.ButStart.Text = "OK";
                        ////////////////////////////////////
                        SqlConnection co; SqlCommand cm; SqlDataReader dr; i = 0;
                    Start: ;
                        try
                        {
                            co = new SqlConnection(MyURL);
                            co.Open();
                            cm = new SqlCommand(); cm.Connection = co;
                            try
                            {
                                cm.CommandText = "select * from Days;";
                                dr = cm.ExecuteReader(); dr.Read(); dr.Close();
                                cm.CommandText = "select * from Ketabs;";
                                dr = cm.ExecuteReader(); dr.Read(); dr.Close();
                            }
                            catch
                            {
                                cmd.CommandText = "update MyDat set MyVal='#' where MyKey='URL';"; cmd.ExecuteNonQuery();
                                MyMasage.Payam.Text = "پایگاه داده معتبر نمی باشد. لطفا نرم افزار را دوباره ران کنید."; MyMasage.ShowDialog();
                                St = "#"; Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End;
                            }
                            co.Close();
                        }
                        catch
                        {
                            try
                            {
                                if (i == 0)
                                {
                                    if (System.IO.File.Exists(St))
                                    {
                                        string ur = string.Concat(St.ToCharArray(0, St.Length - 4)) + "_log.ldf";
                                        System.IO.File.Copy(System.IO.Directory.GetCurrentDirectory() + @"\DateBase_log.ldf", ur);
                                        i += 1; goto Start;
                                    }
                                    else
                                    {
                                        cmd.CommandText = "update MyDat set MyVal='#' where MyKey='URL';"; cmd.ExecuteNonQuery();
                                        MyMasage.Payam.Text = "پایگاه داده از بین رفته است. لطفا نرم افزار را دوباره ران کنید.";
                                        MyMasage.ShowDialog();
                                        St = "#"; Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End;
                                    }
                                }
                                else
                                {
                                    cmd.CommandText = "update MyDat set MyVal='#' where MyKey='URL';"; cmd.ExecuteNonQuery();
                                    System.IO.File.Delete(string.Concat(St.ToCharArray(0, St.Length - 4)) + "_log.ldf");
                                    MyMasage.Payam.Text = "این نرم افزار کامل نصب نشده است یا پایگاه داده مشکل دارد. لطفا نرم افزار را دوباره ران کنید اگر درست نشد آن را از اول نصب کنید.";
                                    MyMasage.ShowDialog();
                                    St = "#"; Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End;
                                }
                            }
                            catch
                            {
                                cmd.CommandText = "update MyDat set MyVal='#' where MyKey='URL';"; cmd.ExecuteNonQuery();
                                MyMasage.Payam.Text = "این نرم افزار کامل نصب نشده است یا پایگاه داده مشکل دارد. لطفا نرم افزار را دوباره ران کنید اگر درست نشد آن را از اول نصب کنید.";
                                MyMasage.ShowDialog();
                                St = "#"; Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End;
                            }
                        }
                    }
                    Mycon.Close();
                }
                catch
                {
                    MyPanelMasage MyMasage = new MyPanelMasage();
                    MyMasage.Size = new Size(700, 250);
                    ////////////
                    {
                        I = new Bitmap(19, 17);
                        int l, a, b;
                        for (l = 0; l < I.Width; l++)
                        {
                            a = R.Next(I.Height - (2)); b = a + ((2)); if (b >= I.Height) b = I.Height - 1;
                            for (; a < b; a++)
                            { I.SetPixel(l, a, Color.FromArgb(50 + R.Next(10), 100 + R.Next(130), 100 + R.Next(130), 100 + R.Next(130))); }
                        }
                        MyMasage.BackgroundImageLayout = ImageLayout.Stretch;
                        MyMasage.BackgroundImage = I;
                        I = new Bitmap(MyMasage.Width, 1);
                        Graphics.FromImage(I).Clear(Color.FromArgb(230, 70, 70));
                        MyMasage.Payam.ImageAlign = ContentAlignment.BottomCenter; MyMasage.Payam.Image = I;
                        MyMasage.BackColor = Color.FromArgb(37, 37, 57);
                    }
                    ////////////////////////////////////
                    MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = false;
                    MyMasage.StartPosition = FormStartPosition.CenterScreen;
                    MyMasage.Payam.SetPersentFont(0.115);
                    try
                    {
                        if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldf"))
                        {
                            if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldfKaro"))
                            { System.IO.File.Delete(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldf"); }
                            else
                            {
                                System.IO.File.Move(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldf"
                                    , System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldfKaro");
                            }
                        }
                        else if (System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldfKaro"))
                        {
                            System.IO.File.Move(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldfKaro"
                                    , System.IO.Directory.GetCurrentDirectory() + @"\KaroDate_log.ldf");
                        }
                    }
                    catch { }
                    MyMasage.Payam.Text = "نرم افزار مشکل دارد. لطفا نرم افزار را چند بار باز و بسته فرمایید اگر درست نشد آن را از اول نصب کنید!";
                    MyMasage.ButStart.Text = "OK"; MyMasage.ShowDialog();
                    Tim.Tick += ClosBox_Click; Tim.Enabled = true; goto End;
                }
                ////////////////////////
                MenoObj = new KtaS[6]; int k;
                for (k = MenoObj.Length - 1; k >= 0; k--)
                {
                    MenoObj[k] = new KtaS(); MenoNmaishPanel.Controls.Add(MenoObj[k]);
                    if ((k % 2) == 0) MenoObj[k].BackColor = Color.FromArgb(130, 50, 30, 40);
                    else MenoObj[k].BackColor = Color.FromArgb(130, 30, 50, 40);
                    MenoObj[k].Height = (KaroMeno.Height - SetingMeno.Height) / 5; MenoObj[k].EventClick += MenoObjEventClick;
                    MenoObj[k].EventEdit += MenoObjEventEdit; MenoObj[k].EventDel += MenoObjEventDel;
                }
                MenoNmaishPanel.Height = MenoObj[0].Height * 6;
                MenoNmaishPanel.Location = new Point(0, 0);
                MenoHS.EventMove += MenoHSEventMove;
                Ktab.ForeColor = Color.FromArgb(235, 150, 130); Ktab.Text = "Home";
                SetingMeno.Controls.Add(But_KtabNow); But_KtabNow.SetImages(KaroSource.KaroAddKtabOff, KaroSource.KaroAddKtabOn);
                But_KtabNow.Size = AddTiket.Size;
                But_KtabNow.Location = new Point((SetingMeno.Width / 2) - (But_KtabNow.Width / 2), (SetingMeno.Height / 2) - (But_KtabNow.Height / 2));
                But_KtabNow.Click += But_KtabNowClick;
                ///////////////////////
                SetData_S(); MenoHS.Val = 0;
                TimStart.Tick += TimStart_Tick; TimStart.Enabled = false;
            End: ;
            }
            else if (!(System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\DateBase.mdf")
                & System.IO.File.Exists(System.IO.Directory.GetCurrentDirectory() + @"\KaroDate.mdf")))
            {
                MyPanelMasage MyMasage = new MyPanelMasage();
                MyMasage.Size = new Size(700, 250);
                ////////////
                {
                    I = new Bitmap(19, 17);
                    int l, a, b;
                    for (l = 0; l < I.Width; l++)
                    {
                        a = R.Next(I.Height - (2)); b = a + ((2)); if (b >= I.Height) b = I.Height - 1;
                        for (; a < b; a++)
                        { I.SetPixel(l, a, Color.FromArgb(50 + R.Next(10), 100 + R.Next(130), 100 + R.Next(130), 100 + R.Next(130))); }
                    }
                    MyMasage.BackgroundImageLayout = ImageLayout.Stretch;
                    MyMasage.BackgroundImage = I;
                    I = new Bitmap(MyMasage.Width, 1);
                    Graphics.FromImage(I).Clear(Color.FromArgb(230, 70, 70));
                    MyMasage.Payam.ImageAlign = ContentAlignment.BottomCenter; MyMasage.Payam.Image = I;
                    MyMasage.BackColor = Color.FromArgb(37, 37, 57);
                }
                ////////////////////////////////////
                MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = false;
                MyMasage.StartPosition = FormStartPosition.CenterScreen; MyMasage.Payam.SetPersentFont(0.115);
                MyMasage.Payam.Text = "فایل های این نرم افزار از بین رفته اند. لتفا آن را دوباره نصب کنید.";
                MyMasage.ButStart.Text = "OK"; MyMasage.ShowDialog();
                Tim.Tick += ClosBox_Click; Tim.Enabled = true;
            }
        }
        static bool Right_Text(string s, char c) { int i; for (i = 0; i < s.Length; i++) if (s[i] == c) { return false; } return true; }
        //K//-------------------------------------------------------------------//
        void BaganClick(object sender, EventArgs e)
        {
            MyPanelDat.EToP = true;
            FormBaygan Bagn = new FormBaygan();
            Bagn.TopBar.BackgroundImage = TitleBox.BackgroundImage;
            Bagn.ObjPanel.BackgroundImage = Bagn.BackgroundImage = NmaishBox.BackgroundImage;
            Bagn.ObjPanel.BackColor = Bagn.BackColor = NmaishBox.BackColor;
            Bagn.TopBar.BackColor = TitleBox.BackColor; Bagn.Setin.BackgroundImage = SetingBox.BackgroundImage;
            Bagn.Setin.Height = SetingBox.Height; Bagn.Setin.BackColor = SetingBox.BackColor;
            Bagn.Size = this.Size; Bagn.Nametx.Font = NameBox.Font; Bagn.SetKarForm(this, 1, 1);
            Bagn.ShowDialog();
        }
        void NetwrkClick(object sender, EventArgs e)
        {
            MyPanelDat.EToP = true;
            FormNetwrk Bagn = new FormNetwrk();
            Bagn.TopBar.BackgroundImage = TitleBox.BackgroundImage;
            Bagn.ObjPanel.BackgroundImage = Bagn.BackgroundImage = NmaishBox.BackgroundImage;
            Bagn.ObjPanel.BackColor = Bagn.BackColor = NmaishBox.BackColor;
            Bagn.TopBar.BackColor = TitleBox.BackColor; Bagn.Setin.BackgroundImage = SetingBox.BackgroundImage;
            Bagn.Setin.Height = SetingBox.Height; Bagn.Setin.BackColor = SetingBox.BackColor;
            Bagn.Size = this.Size; Bagn.Nametx.Font = NameBox.Font; Bagn.SetKarForm(this, 1, 1);
            Bagn.ShowDialog();
        }
        //K//-------------------------------------------------------------------//
        string[] Data_S; static string KaroFilName = "";
        void SetData_S()
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "select * from Ketabs;"; SqlDataReader Sql = cmd.ExecuteReader();
            int k = 0; while (Sql.Read()) { k += 1; } Sql.Close();
            cmd.CommandText = "select * from Ketabs;"; Sql = cmd.ExecuteReader();
            Data_S = new string[k]; k = SetData_ST(Sql); Sql.Close();
            Mycon.Close();
        }
        int SetData_ST(SqlDataReader s)
        {
            if (s.Read())
            {
                string st = s["Ketab"].ToString();
                /////////////////
                int i = SetData_ST(s);
                /////////////////
                Data_S[i] = st; return (i + 1);
            }
            else { return 0; }
        }
        void MenoHSEventMove()
        {
            int i, j, t = -(int)((double)((MenoHS.Val * (Data_S.Length - 5)) * MenoObj[0].Height));
            if (t > 0) t = 0; j = -(t / MenoObj[0].Height);
            for (i = 0; i < MenoObj.Length; i++)
            {
                if (Data_S.Length > (j + i)) { MenoObj[i].Text = Data_S[j + i]; MenoObj[i].Visible = true; }
                else { MenoObj[i].Visible = false; }
            }
            t = t % MenoObj[0].Height; MenoNmaishPanel.Top = t;
        }
        void MenoObjEventClick(KtaS L)
        {
            KaroFilName = L.Text; Tim.Tick += MenoTimOpen; Ktab.Text = "Home \\  " + L.Text;
            Tim.Enabled = true; GetData(); HSDay.Val = 0;
        }
        void MenoObjEventEdit(KtaS L)
        {
            SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
            PanelForKtabNow F_KtabNow = new PanelForKtabNow();
            F_KtabNow.SetKarForm(this, 0.95, T((this.NmaishBox.Height * 4) / 5, this.Height)); F_KtabNow.S1.Text = L.Text;
            SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
            cmd.CommandText = "select * from Ketabs where Ketab='" + L.Text + "';"; SqlDataReader Sql = cmd.ExecuteReader();
            if (Sql.Read()) { F_KtabNow.S2.Text = Sql["DayN"].ToString(); F_KtabNow.S3.Text = Sql["LogatN"].ToString(); }
            Sql.Close(); 
            F_KtabNow.KtabOld = L.Text;
            Mycon.Close();
            if (F_KtabNow.ShowDialog() == DialogResult.Yes)
            {
                KaroFilName = F_KtabNow.S1.Text;
                if (EditKtab(L.Text, int.Parse(F_KtabNow.S2.Text), int.Parse(F_KtabNow.S3.Text)) < 0)
                { SetData_S(); MenoHS.Val = MenoHS.Val; }
                else
                {
                    MyPanelMasage MyMasage = new MyPanelMasage();
                    MyMasage.Payam.RightToLeft = RightToLeft.Yes;
                    MyMasage.SetKarForm(this, 0.97, T(this.NmaishBox.Height - 30, this.Height));
                    MyMasage.Payam.Text = "متاسفم. تعداد لغات یکی از روز ها از این تعداد لغات تعین شده برای هر روز، بیشتر است!";
                    MyMasage.ButStart.Text = "بستن"; MyMasage.ShowDialog();
                }
            }
        }
        void MenoObjEventDel(KtaS L)
        {
            MyPanelMasage MyMasage = new MyPanelMasage();
            ////////////////////////////////////////////
            MyMasage.Payam.Text = "باحذف کتاب همه ی اطلاعات مربوط به آن حذف می شوند. آیا مطمئنيد؟";
            MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = true;
            MyMasage.SetKarForm(this, 0.97, T(this.NmaishBox.Height - 30, this.Height));
            if (MyMasage.ShowDialog() == DialogResult.Yes)
            { KaroFilName = L.Text; DeletKtab(); SetData_S(); MenoHS.Val = MenoHS.Val; }
        }
        void MenoTimOpen(object s, EventArgs e)
        {
            if (KaroMeno.Height > 0) { KaroMeno.Height = (KaroMeno.Height * 3 / 4); }
            else { Tim.Tick -= MenoTimOpen; Tim.Enabled = false; }
        }
        void MenoTimClose(object s, EventArgs e)
        {
            if (KaroMeno.Height < KaroMeno.MaximumSize.Height) { KaroMeno.Height = ((KaroMeno.Height + 1) * 5 / 4); }
            else { Tim.Tick -= MenoTimClose; Tim.Enabled = false; }
        }
        void But_KtabNowClick(object s, EventArgs e)
        {
            PanelForKtabNow F_KtabNow = new PanelForKtabNow();
            F_KtabNow.SetKarForm(this, 0.95, T((this.NmaishBox.Height * 4) / 5, this.Height)); F_KtabNow.S1.Text = "";
            F_KtabNow.S2.Text = "7"; F_KtabNow.S3.Text = "100";
            if (F_KtabNow.ShowDialog() == DialogResult.Yes)
            {
                KaroFilName = F_KtabNow.S1.Text; CreateKtabNow(int.Parse(F_KtabNow.S2.Text), F_KtabNow.S3.Text);
                SetData_S(); MenoHS.Val = 0;
            }
        }
        //K//-------------------------------------------------------------------//
        void HSDayEventMove()
        {
            int t = (int)((double)(-(HSDay.Val * (((DataDay.Length / 4)
                + (((DataDay.Length % 4) > 0) ? 1 : 0) - 3) * (3 + PanelDay[0].MinimumSize.Height)))));
            if (t > 0) t = 0;
            int i = 0, j = (3 + PanelDay[0].MinimumSize.Height);
            while (t < -j) { t += j; i += 1; }
            i = i * 4;
            for (j = 0; j < PanelDay.Length; j++) { PanelDay[j].SetVal = i + j; }
            Panel_D_Day.Top = t;
        }
        void HSDatEventMove()
        {
            int t = -(int)((double)(HSDat.Val *
                (((DataDay[MyPanelDayVal].L / 4) + (((DataDay[MyPanelDayVal].L % 4) > 0) ? 1 : 0) - 3) * (PanelDat[0].MinimumSize.Height + 3))));
            if (t > 0) t = 0;
            int i = 0, j = (3 + PanelDat[0].MinimumSize.Height);
            while (t < -j) { t += j; i += 1; }
            i = i * 4;
            for (j = 0; j < PanelDat.Length; j++) { PanelDat[j].SetVal = i + j; }
            Panel_D_Dat.Top = t;
        }
        //K//-------------------------------------------------------------------//
        int x = -1, y = -1; bool MoveOn = false;
        private void TitleBox_MouseDown(object sender, MouseEventArgs e) { MoveOn = true; x = e.X; y = e.Y; }
        private void TitleBox_MouseMove(object sender, MouseEventArgs e)
        { if (MoveOn) { this.Left += e.X - x; this.Top += e.Y - y; } }
        private void TitleBox_MouseUp(object sender, MouseEventArgs e) { MoveOn = false; }
        private void ClosBox_Click(object sender, EventArgs e)
        {
            /*
            int k = 1;
        H: ;
            Karoo(k);
            if (k < 30) { k = k + 1; goto H; }
            //*/
            Application.Exit();
        }
        private void KaroInfo_Click(object sender, EventArgs e)
        {
            MyPanelMasage MyMasage = new MyPanelMasage();
            MyMasage.SetKarForm(this, 0.97, T(this.NmaishBox.Height - 30, this.Height));
            ////////////////////////////////////////////
            PictureBox P = new PictureBox(); MyMasage.Payam.Text = "";
            MyMasage.Payam.Controls.Add(P); P.Dock = DockStyle.Fill;
            P.SizeMode = PictureBoxSizeMode.Zoom; P.Image = KaroSource.Karo;
            MyMasage.ButCancel.Visible = false; MyMasage.ShowDialog();
        }
        //K//-------------------------------------------------------------------//
        Timer TimStart = new Timer();
        private void KaroRest_Click(object sender, EventArgs e) { TimStart.Enabled = true; }
        private void TimStart_Tick(object sender, EventArgs e) { But_Home_Click(null, null); }
        //K//-------------------------------------------------------------------//
        void Karoo(int k)
        {
            if (System.IO.File.Exists(@"D:\Output_FF\I" + k.ToString() + ".png"))
            {
                string[] Nam = { "Home", "Add", "Edit", "Tik", "NoTik", "Save", "Del", "EnTOPr", "Img", "Talafz", "Rest", "NetWrk", "Baygani"
                                   , "AddKtab","AddNet","NetLft","NetRit" ,"RitClick","Info"};
                Bitmap I = new Bitmap(@"D:\Output_FF\I" + k.ToString() + ".png"), O, OO, T = new Bitmap(1, 1);
                O = new Bitmap(I.Width, I.Height); OO = new Bitmap(I.Width, I.Height); int i, j;
                for (i = 0; i < I.Width; i++)
                    for (j = 0; j < I.Height; j++)
                        if  //(I.GetPixel(i, j).A > 200)//
                            (I.GetPixel(i, j).R > 30 | I.GetPixel(i, j).G > 30 | I.GetPixel(i, j).B > 30)
                        //(I.GetPixel(i, j) != T.GetPixel(0, 0))
                        {
                            /*
                            O.SetPixel(i, j, Color.FromArgb(255 - 70 + (int)(70 * Math.Sin((j * 3.14159) / I.Height))
                                , (I.GetPixel(i, j).R * 3) / 5, (I.GetPixel(i, j).G * 3) / 5, (I.GetPixel(i, j).B * 3) / 5));
                            OO.SetPixel(i, j, Color.FromArgb(255 - 70 + (int)(70 * Math.Sin((j * 3.14159) / I.Height)), I.GetPixel(i, j)));
                             */
                            //*
                            O.SetPixel(i, j, Color.FromArgb(I.GetPixel(i, j).R / 2, I.GetPixel(i, j).G / 2, I.GetPixel(i, j).B / 2));
                            OO.SetPixel(i, j, I.GetPixel(i, j));
                            //*/
                            if (O.GetPixel(i, j).R < 3 & O.GetPixel(i, j).G < 3 & O.GetPixel(i, j).B < 3)
                                O.SetPixel(i, j, T.GetPixel(0, 0));
                            if (OO.GetPixel(i, j).R < 3 & OO.GetPixel(i, j).G < 3 & OO.GetPixel(i, j).B < 3)
                                OO.SetPixel(i, j, T.GetPixel(0, 0));
                        }
                        else
                        {
                            // O.SetPixel(i, j, Color.FromArgb(70 - (int)(50 * Math.Sin((j * 3.14159) / I.Height)), 0, 0));
                        }
                O.Save(@"D:\Output_FF\Karo" + Nam[k - 1] + "Off.png");
                OO.Save(@"D:\Output_FF\Karo" + Nam[k - 1] + "On.png");
            }
        }
        //K//-------------------------------------------------------------------//
        byte Tim_Day_Val = 0, Tim_Dat_Val = 0; byte TimDatON = 0;
        void PanelDayEventClick(MyPanelDay P)
        {
            if (Tim.Enabled == false)
            {
                Tim.Tick += Tim_Open_DataDay; Tim_Day_Val = P.Val_n; Tim.Enabled = true;
                if (Panel_Dat.Height == 0) { Panel_D_Dat.Top = 0; }
                Panel_D_Day.Width += 25; HSDay.Top = NmaishBox.Height;
                if (TimDatON == 0) TimDatON = 1;
                AddTiket.Top = 3;
                NumberDay.Text = (P.SetVal + 1).ToString();
                HSDat.Val = 0;
                ETOP.Top = 3;
            }
        }
        void PanelDatEventClick(MyPanelDat P)
        {
            if (Tim.Enabled == false)
            {
                if (P.Height == P.MinimumSize.Height)
                {
                    Tim_Dat_Val = P.Val_n; Tim.Tick += Tim_Open_DataDat; if (TimDatON == 1) TimDatON = 2; Tim.Enabled = true;
                    Panel_D_Dat.Width += 25; HSDat.Top = NmaishBox.Height;
                    AddTiket.Top = NmaishBox.Height;
                }
            }
        }
        //K//-------------------------------------------------------------------//
        private void Tim_Open_DataDay(object sender, EventArgs e)
        {
            if (PanelDay[Tim_Day_Val].Height == PanelDay[Tim_Day_Val].MinimumSize.Height)
            {
                byte i;
                for (i = 0; i < PanelDay.Length; i++)
                    if (PanelDay[i].Val_n != Tim_Day_Val) { PanelDay[i].Left = NmaishBox.Width; }
            }
            if (PanelDay[Tim_Day_Val].Height < PanelDay[Tim_Day_Val].MaximumSize.Height)
            { PanelDay[Tim_Day_Val].Height = (PanelDay[Tim_Day_Val].Height * 4) / 3; }
            if (PanelDay[Tim_Day_Val].Width < PanelDay[Tim_Day_Val].MaximumSize.Width)
            { PanelDay[Tim_Day_Val].Width = (PanelDay[Tim_Day_Val].Width * 4) / 3; }
            int j = (10 - PanelDay[Tim_Day_Val].Left); if (j < 0) j = -j;
            if (j > 2) { PanelDay[Tim_Day_Val].Left += (10 - PanelDay[Tim_Day_Val].Left) / 3; }
            else { PanelDay[Tim_Day_Val].Left = 10; }
            j = (15 - Panel_D_Day.Top - PanelDay[Tim_Day_Val].Top); if (j < 0) j = -j;
            if (j > 2) { PanelDay[Tim_Day_Val].Top += (15 - Panel_D_Day.Top - PanelDay[Tim_Day_Val].Top) / 3; }
            else { PanelDay[Tim_Day_Val].Top = 15 - Panel_D_Day.Top; }
            if (
                (PanelDay[Tim_Day_Val].Height == PanelDay[Tim_Day_Val].MaximumSize.Height)
              & (PanelDay[Tim_Day_Val].Width == PanelDay[Tim_Day_Val].MaximumSize.Width)
              & (PanelDay[Tim_Day_Val].Left == 10) & (PanelDay[Tim_Day_Val].Top == (15 - Panel_D_Day.Top))
               )
            {
                Tim.Tick -= Tim_Open_DataDay; Tim.Tick += Tim_Open_DataDay_Fin;
            }
        }
        private void Tim_Open_DataDay_Fin(object sender, EventArgs e)
        {
            if (Panel_Dat.Height < Panel_Dat.MaximumSize.Height)
            {
                Panel_Dat.Height = ((Panel_Dat.Height + 1) * 6) / 5;
            }
            else { Tim.Tick -= Tim_Open_DataDay_Fin; Tim.Enabled = false; }
        }
        //K//-------------------------------------------------------------------//
        private void Tim_Open_DataDat(object sender, EventArgs e)
        {
            if (PanelDat[Tim_Dat_Val].Height == PanelDat[Tim_Dat_Val].MinimumSize.Height)
            {
                byte i;
                for (i = 0; i < PanelDat.Length; i++)
                    if (PanelDat[i].Val_n != Tim_Dat_Val) { PanelDat[i].Left = NmaishBox.Width; }
            }
            if (PanelDat[Tim_Dat_Val].Height < PanelDat[Tim_Dat_Val].MaximumSize.Height)
            { PanelDat[Tim_Dat_Val].Height = (PanelDat[Tim_Dat_Val].Height * 4) / 3; }
            if (PanelDat[Tim_Dat_Val].Width < PanelDat[Tim_Dat_Val].MaximumSize.Width)
            { PanelDat[Tim_Dat_Val].Width = (PanelDat[Tim_Dat_Val].Width * 4) / 3; }
            int j = (10 - PanelDat[Tim_Dat_Val].Left); if (j < 0) j = -j;
            if (j > 2) { PanelDat[Tim_Dat_Val].Left += (10 - PanelDat[Tim_Dat_Val].Left) / 3; }
            else { PanelDat[Tim_Dat_Val].Left = 10; }
            j = (15 - Panel_D_Dat.Top - PanelDat[Tim_Dat_Val].Top); if (j < 0) j = -j;
            if (j > 2) { PanelDat[Tim_Dat_Val].Top += (15 - Panel_D_Dat.Top - PanelDat[Tim_Dat_Val].Top) / 3; }
            else { PanelDat[Tim_Dat_Val].Top = 15 - Panel_D_Dat.Top; }
            if (
                (PanelDat[Tim_Dat_Val].Height == PanelDat[Tim_Dat_Val].MaximumSize.Height)
              & (PanelDat[Tim_Dat_Val].Width == PanelDat[Tim_Dat_Val].MaximumSize.Width)
              & (PanelDat[Tim_Dat_Val].Left == 10) & (PanelDat[Tim_Dat_Val].Top == (15 - Panel_D_Dat.Top))
               )
            {
                Tim.Tick -= Tim_Open_DataDat; Tim.Enabled = false;
            }
        }
        //K//-------------------------------------------------------------------//
        private void But_Home_Click(object sender, EventArgs e)
        {
            if (Tim.Enabled == false)
            {
                if (TimDatON >= 2)
                {
                    AddTiket.Top = 3; Tim.Tick += Tim_Close_DataDat;
                    if (TimDatON == 3)
                    {
                        HSDat.Val = T(((PanelDat[Tim_Dat_Val].SetVal - PanelDat[Tim_Dat_Val].Val_n) / 4) + 1
                            , ((DataDay[MyPanelDayVal].L - 1) / 4) + (((DataDay[MyPanelDayVal].L - 1) % 4) == 0 ? 0 : 1) - 3);
                        if (PanelDat[Tim_Dat_Val].Val_n < 4) Panel_D_Dat.Top = 0;
                    }
                    TimDatON = 1; Tim.Enabled = true;
                }
                else if (TimDatON == 1)
                {
                    NumberDay.Text = ""; AddTiket.Top = NmaishBox.Height; Tim.Tick += Tim_Close_DataDay; TimDatON = 0;
                    Tim.Enabled = true; ETOP.Top = NmaishBox.Height;
                }
                else
                {
                    Tim.Tick += MenoTimClose; Tim.Enabled = true;
                    Ktab.Text = "Home"; TimStart.Enabled = false;
                }
            }
        }
        private void Tim_Close_DataDay(object sender, EventArgs e)
        {
            if (Panel_Dat.Height > 0)
            {
                Panel_Dat.Height = (Panel_Dat.Height * 4) / 5;
            }
            else { Tim.Tick -= Tim_Close_DataDay; Tim.Tick += Tim_Close_DataDay_Fin; }
        }
        private void Tim_Close_DataDay_Fin(object sender, EventArgs e)
        {
            if (PanelDay[Tim_Day_Val].Height > PanelDay[Tim_Day_Val].MinimumSize.Height)
            { PanelDay[Tim_Day_Val].Height = (PanelDay[Tim_Day_Val].Height * 2) / 3; }
            if (PanelDay[Tim_Day_Val].Width > PanelDay[Tim_Day_Val].MinimumSize.Width)
            { PanelDay[Tim_Day_Val].Width = (PanelDay[Tim_Day_Val].Width * 2) / 3; }
            int j = (PanelDay[Tim_Day_Val].Loc.x - PanelDay[Tim_Day_Val].Left); if (j < 0) j = -j;
            if (j > 2) { PanelDay[Tim_Day_Val].Left += (PanelDay[Tim_Day_Val].Loc.x - PanelDay[Tim_Day_Val].Left) / 3; }
            else { PanelDay[Tim_Day_Val].Left = PanelDay[Tim_Day_Val].Loc.x; }
            j = (PanelDay[Tim_Day_Val].Loc.y - PanelDay[Tim_Day_Val].Top); if (j < 0) j = -j;
            if (j > 2) { PanelDay[Tim_Day_Val].Top += (PanelDay[Tim_Day_Val].Loc.y - PanelDay[Tim_Day_Val].Top) / 3; }
            else { PanelDay[Tim_Day_Val].Top = PanelDay[Tim_Day_Val].Loc.y; }
            if (
                (PanelDay[Tim_Day_Val].Height == PanelDay[Tim_Day_Val].MinimumSize.Height)
              & (PanelDay[Tim_Day_Val].Width == PanelDay[Tim_Day_Val].MinimumSize.Width)
              & (PanelDay[Tim_Day_Val].Left == PanelDay[Tim_Day_Val].Loc.x) & (PanelDay[Tim_Day_Val].Top == (PanelDay[Tim_Day_Val].Loc.y))
               )
            {
                int i;
                for (i = 0; i < PanelDay.Length; i++)
                { PanelDay[i].Left = PanelDay[i].Loc.x; }
                Panel_D_Day.Width -= 25; HSDay.Top = 1;
                Tim.Tick -= Tim_Close_DataDay_Fin; Tim.Enabled = false;
            }
        }
        private void Tim_Close_DataDat(object sender, EventArgs e)
        {
            if (PanelDat[Tim_Dat_Val].Height > PanelDat[Tim_Dat_Val].MinimumSize.Height)
            { PanelDat[Tim_Dat_Val].Height = (PanelDat[Tim_Dat_Val].Height * 2) / 3; }
            if (PanelDat[Tim_Dat_Val].Width > PanelDat[Tim_Dat_Val].MinimumSize.Width)
            { PanelDat[Tim_Dat_Val].Width = (PanelDat[Tim_Dat_Val].Width * 2) / 3; }
            int j = (PanelDat[Tim_Dat_Val].Loc.x - PanelDat[Tim_Dat_Val].Left); if (j < 0) j = -j;
            if (j > 2) { PanelDat[Tim_Dat_Val].Left += (PanelDat[Tim_Dat_Val].Loc.x - PanelDat[Tim_Dat_Val].Left) / 3; }
            else { PanelDat[Tim_Dat_Val].Left = PanelDat[Tim_Dat_Val].Loc.x; }
            j = (PanelDat[Tim_Dat_Val].Loc.y - PanelDat[Tim_Dat_Val].Top); if (j < 0) j = -j;
            if (j > 2) { PanelDat[Tim_Dat_Val].Top += (PanelDat[Tim_Dat_Val].Loc.y - PanelDat[Tim_Dat_Val].Top) / 3; }
            else { PanelDat[Tim_Dat_Val].Top = PanelDat[Tim_Dat_Val].Loc.y; }
            if (
                (PanelDat[Tim_Dat_Val].Height == PanelDat[Tim_Dat_Val].MinimumSize.Height)
              & (PanelDat[Tim_Dat_Val].Width == PanelDat[Tim_Dat_Val].MinimumSize.Width)
              & (PanelDat[Tim_Dat_Val].Left == PanelDat[Tim_Dat_Val].Loc.x) & (PanelDat[Tim_Dat_Val].Top == (PanelDat[Tim_Dat_Val].Loc.y))
               )
            {
                byte i;
                for (i = 0; i < PanelDat.Length; i++)
                { PanelDat[i].Left = PanelDat[i].Loc.x; }
                Panel_D_Dat.Width -= 25; HSDat.Top = 1;
                Tim.Tick -= Tim_Close_DataDat; Tim.Enabled = false;
                if (PanelDat[13].SetVal > DataDay[MyPanelDayVal].L) { HSDat.Val = 1; }
            }
        }
        //K//--------------------------------------------------------------//
        byte Dat_n = 0;
        private void But_Tik_Click(MyPanelDat P)
        {
            int i = (MyPanelDayVal + 1);
            if (DataDay.Length > i)
            {
                if (DataDay[i].L < DataDay[i].Dat.Length)
                {
                    DeletDataDay(DataDay[MyPanelDayVal].Dat[P.SetVal]);
                    SaveDataDay(DataDay[MyPanelDayVal].Dat[P.SetVal], i);
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "select * from Days  where Ketab='" + KaroFilName
                        + "' and Day_i=" + (i + 1).ToString() + ";";
                    SqlDataReader Sql = cmd.ExecuteReader();
                    DataDay[i].L = 0; GetDataT(DataDay[i], Sql); Sql.Close();
                    Mycon.Close();
                    DataDay[MyPanelDayVal].Dat[P.SetVal] = null; DataDay[MyPanelDayVal].L -= 1;
                    for (i = P.SetVal; i < DataDay[MyPanelDayVal].L; i++)
                    { DataDay[MyPanelDayVal].Dat[i] = DataDay[MyPanelDayVal].Dat[i + 1]; }
                    DataDay[MyPanelDayVal].Dat[DataDay[MyPanelDayVal].L] = null;
                    for (i = P.Val_n; i < PanelDat.Length; i++) { PanelDat[i].SetText(); }
                    PanelDay[Tim_Day_Val].SetItem_ValMax();
                    if ((Tim_Day_Val + 1) < PanelDay.Length) { PanelDay[(Tim_Day_Val + 1)].SetItem_ValMax(); }
                    if (PanelDat[13].SetVal != 13)
                    { if (PanelDat[13].SetVal >= DataDay[MyPanelDayVal].L & TimDatON == 1) { HSDat.Val = 1; } }
                    if (P.Visible == false & TimDatON == 2) { But_Home_Click(null, null); }
                }
                else
                {
                    MyPanelMasage MyMasage = new MyPanelMasage();
                    MyMasage.SetKarForm(this, 0.97, T(this.NmaishBox.Height - 30, this.Height));
                    ////////////////////////////////////////////
                    MyMasage.Payam.Text = "لیست روز بعدی شما پر شده است.";
                    MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = false;
                    MyMasage.ShowDialog();
                }
            }
            else
            {
                DeletDataDay(DataDay[MyPanelDayVal].Dat[P.SetVal]);
                Baygani_Save_Record(DataDay[MyPanelDayVal].Dat[P.SetVal]);
                DataDay[MyPanelDayVal].Dat[P.SetVal] = null; DataDay[MyPanelDayVal].L -= 1;
                for (i = P.SetVal; i < DataDay[MyPanelDayVal].L; i++)
                { DataDay[MyPanelDayVal].Dat[i] = DataDay[MyPanelDayVal].Dat[i + 1]; }
                DataDay[MyPanelDayVal].Dat[DataDay[MyPanelDayVal].L] = null;
                for (i = P.Val_n; i < PanelDat.Length; i++) { PanelDat[i].SetText(); }
                PanelDay[Tim_Day_Val].SetItem_ValMax();
                if ((Tim_Day_Val + 1) < PanelDay.Length) { PanelDay[(Tim_Day_Val + 1)].SetItem_ValMax(); }
                if (PanelDat[13].SetVal != 13)
                { if (PanelDat[13].SetVal >= DataDay[MyPanelDayVal].L & TimDatON == 1) { HSDat.Val = 1; } }
                if (P.Visible == false & TimDatON == 2) { But_Home_Click(null, null); }
            }
        }
        private void But_NotTik_Click(MyPanelDat P)
        {
            if (TimDatON >= 2 & Tim.Enabled == false)
            {
                Dat_n = (byte)(P.Val_n + 1);
                if (Dat_n < 16)
                {
                    if (PanelDat[Dat_n].Visible)
                    { Tim.Tick += Tim_NoTik; Tim.Enabled = true; Dat_n = P.Val_n; }
                    else { But_Home_Click(null, null); }
                }
                else
                {
                    if (DataDay[MyPanelDayVal].L > (P.SetVal + 1))
                    { Tim.Tick += Tim_NoTik; Tim.Enabled = true; Dat_n = P.Val_n; }
                    else { But_Home_Click(null, null); }
                }
                TimDatON = 3;
            }
        }
        void Tim_NoTik(object s, EventArgs e)
        {
            if (Dat_n < 15)
            {
                if (PanelDat[Dat_n].Left > -(NmaishBox.Width) & PanelDat[Dat_n].Width != PanelDat[Dat_n].MinimumSize.Width)
                {
                    int j = (-(NmaishBox.Width) - PanelDat[Dat_n].Left); if (j < 0) j = -j;
                    if (j > 2) { PanelDat[Dat_n].Left += (-(NmaishBox.Width) - PanelDat[Dat_n].Left) / 2; }
                    else { PanelDat[Dat_n].Left = -(NmaishBox.Width); }
                }
                else if (PanelDat[Dat_n].Width != PanelDat[Dat_n].MinimumSize.Width)
                {
                    PanelDat[Dat_n].Size = new Size(PanelDat[Dat_n].MinimumSize.Width, PanelDat[Dat_n].MinimumSize.Height);
                    PanelDat[Dat_n].Location = new Point(NmaishBox.Width, PanelDat[Dat_n].Loc.y);

                    PanelDat[Dat_n + 1].Size = new Size(PanelDat[Dat_n].MaximumSize.Width, PanelDat[Dat_n].MaximumSize.Height);
                    PanelDat[Dat_n + 1].Location = new Point(NmaishBox.Width, 15 - Panel_D_Dat.Top);
                }
                else if (PanelDat[Dat_n + 1].Left > 10)
                {
                    int j = (10 - PanelDat[Dat_n + 1].Left); if (j < 0) j = -j;
                    if (j > 2) { PanelDat[Dat_n + 1].Left += (10 - PanelDat[Dat_n + 1].Left) / 2; }
                    else { PanelDat[Dat_n + 1].Left = 10; }
                }
                else
                {
                    PanelDat[Dat_n + 1].Left = 10; Tim.Tick -= Tim_NoTik; Tim.Enabled = false;
                    Tim_Dat_Val = (byte)(Dat_n + 1);
                }
            }
            else
            {
                if (PanelDat[Dat_n].Left > -(NmaishBox.Width) & PanelDat[Dat_n].Width != PanelDat[Dat_n].MinimumSize.Width)
                {
                    int j = (-(NmaishBox.Width) - PanelDat[Dat_n].Left); if (j < 0) j = -j;
                    if (j > 2) { PanelDat[Dat_n].Left += (-(NmaishBox.Width) - PanelDat[Dat_n].Left) / 2; }
                    else { PanelDat[Dat_n].Left = -(NmaishBox.Width); }
                }
                else if (PanelDat[Dat_n].Width != PanelDat[Dat_n].MinimumSize.Width)
                {
                    PanelDat[Dat_n].Size = new Size(PanelDat[Dat_n].MinimumSize.Width, PanelDat[Dat_n].MinimumSize.Height);
                    PanelDat[Dat_n].Location = new Point(NmaishBox.Width, PanelDat[Dat_n].Loc.y);

                    for (byte i = 0; i < PanelDat.Length; i++) { PanelDat[i].SetVal = PanelDat[i].SetVal + 4; }
                    PanelDat[Dat_n - 3].Size = new Size(PanelDat[Dat_n].MaximumSize.Width, PanelDat[Dat_n].MaximumSize.Height);
                    PanelDat[Dat_n - 3].Location = new Point(NmaishBox.Width, 15 - Panel_D_Dat.Top);
                }
                else if (PanelDat[Dat_n - 3].Left > 10)
                {
                    int j = (10 - PanelDat[Dat_n - 3].Left); if (j < 0) j = -j;
                    if (j > 2) { PanelDat[Dat_n - 3].Left += (10 - PanelDat[Dat_n - 3].Left) / 2; }
                    else { PanelDat[Dat_n - 3].Left = 10; }
                }
                else
                {
                    PanelDat[Dat_n - 3].Left = 10; Tim.Tick -= Tim_NoTik; Tim.Enabled = false;
                    Tim_Dat_Val = (byte)(Dat_n - 3);
                }
            }
        }
        private void But_Delet_Click(MyPanelDat P)
        {
            MyPanelMasage MyMasage = new MyPanelMasage();
            ////////////////////////////////////////////
            MyMasage.Payam.Text = "آیا مطمئنید که حذف شود؟"; MyMasage.Payam.RightToLeft = RightToLeft.Yes;
            MyMasage.ButCancel.Visible = true;
            MyMasage.SetKarForm(this, 0.97, T(this.NmaishBox.Height - 30, this.Height));
            if (MyMasage.ShowDialog() == DialogResult.Yes)
            {
                int i;
                DeletDataDay(DataDay[MyPanelDayVal].Dat[P.SetVal]);
                DataDay[MyPanelDayVal].Dat[P.SetVal] = null; DataDay[MyPanelDayVal].L -= 1;
                for (i = P.SetVal; i < DataDay[MyPanelDayVal].L; i++)
                { DataDay[MyPanelDayVal].Dat[i] = DataDay[MyPanelDayVal].Dat[i + 1]; }
                DataDay[MyPanelDayVal].Dat[DataDay[MyPanelDayVal].L] = null;
                for (i = P.Val_n; i < PanelDat.Length; i++) { PanelDat[i].SetText(); }
                PanelDay[Tim_Day_Val].SetItem_ValMax();
                if (PanelDat[13].SetVal != 13)
                { if (PanelDat[13].SetVal >= DataDay[MyPanelDayVal].L & TimDatON == 1) { HSDat.Val = 1; } }
                if (P.Visible == false & TimDatON == 2) { But_Home_Click(null, null); }
            }
        }
        //K//-------------------------------------------------------------------//
        private void But_Edit_Click(MyPanelDat P)
        {
            MyPanelTextBox InputText = new MyPanelTextBox();
            InputText.Lokat = DataDay[MyPanelDayVal].Dat[P.SetVal].S1;
            InputText.SetDizin(); InputText.SetKarForm(this, 0.95, T((this.NmaishBox.Height * 4) / 5, this.Height));
            //////////////////////////////////////
            InputText.S2.Text = DataDay[MyPanelDayVal].Dat[P.SetVal].S2;
            InputText.S1.Text = DataDay[MyPanelDayVal].Dat[P.SetVal].S1;
            InputText.Talafz.Text = DataDay[MyPanelDayVal].Dat[P.SetVal].Talafz;
            InputText.URL.Text = DataDay[MyPanelDayVal].Dat[P.SetVal].URL;
            if (InputText.ShowDialog() == DialogResult.Yes)
            {
                EditDataDay(DataDay[MyPanelDayVal].Dat[P.Val_n], InputText.GetRecord());
                DataDay[MyPanelDayVal].Dat[P.Val_n] = null;
                DataDay[MyPanelDayVal].Dat[P.Val_n] = InputText.GetRecord();
                PanelDat[P.Val_n].SetText();
            }
        }
        //K//--------------------------------------------------------------//
        private void But_Add_Click(object sender, EventArgs e)
        {
            if (DataDay[MyPanelDayVal].L < DataDay[MyPanelDayVal].Dat.Length)
            {
                MyPanelTextBox InputText = new MyPanelTextBox(); InputText.SetDizin();
                InputText.SetKarForm(this, 0.95, T((this.NmaishBox.Height * 4) / 5, this.Height));
                //////////////////////////////////////
                InputText.Talafz.Text = ""; InputText.S2.Text = ""; InputText.S1.Text = "";
                InputText.URL.Text = "Image";
                if (InputText.ShowDialog() == DialogResult.Yes)
                {
                    SqlConnection Mycon = new SqlConnection(MyURL); Mycon.Open();
                    int i = MyPanelDayVal;
                    SaveDataDay(InputText.GetRecord(), i);
                    SqlCommand cmd = new SqlCommand(); cmd.Connection = Mycon;
                    cmd.CommandText = "select * from Days  where Ketab='" + KaroFilName
                        + "' and Day_i=" + (i + 1).ToString() + ";";
                    SqlDataReader Sql = cmd.ExecuteReader();
                    DataDay[i].L = 0; GetDataT(DataDay[i], Sql); Sql.Close();
                    HSDat.Val = 0; PanelDay[Tim_Day_Val].SetItem_ValMax();
                    Mycon.Close();
                }
            }
            else
            {
                MyPanelMasage MyMasage = new MyPanelMasage();
                MyMasage.SetKarForm(this, 0.97, T(this.NmaishBox.Height - 30, this.Height));
                ////////////////////////////////////////////
                MyMasage.Payam.Text = "لیست امروز شما پر شده است.";
                MyMasage.Payam.RightToLeft = RightToLeft.Yes; MyMasage.ButCancel.Visible = false;
                MyMasage.ShowDialog();
            }
        }
        void ETOPClick(object sender, EventArgs e) { if (MyPanelDat.EToP) { MyPanelDat.EToP = false; } else { MyPanelDat.EToP = true; } }
        //K//--------------------------------------------------------------//
        void PanelDatEvntMove(MyPanelDat P) { }
        void PanelDatEvntCopy(MyPanelDat P) { }
    }
}