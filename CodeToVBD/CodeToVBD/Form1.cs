using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CodeToVBD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private int topofqueue = 0;
        private Node[,] maingrid;
        private Pair[] queue;
        int[,] links;
        private string m;
        private char[] a;
        private int nodeindex = 0;
        private string drawnode = "";
        private string linknode = "";
        public int size = 80;
        private void del(int start, int end)
        {
            links[start, end] = -100;
        }
        private int skip(int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                if (!(a[i] == '\r' || a[i] == '\n' || a[i] == '\t' || a[i] == ' '))
                {
                    return i;
                }
            }
            return -1;
        }
        private int find(int start, int end, char n)
        {
            if (start <= end)
            {
                for (int i = start; i <= end; i++)
                {
                    if (a[i] == n) return i;
                }
                return -1;
            }
            else
            {
                for (int i = start; i >= end; i--)
                {
                    if (a[i] == n) return i;
                }
                return -1;
            }
        }
        private int match(int start, int end, char n)
        {
            int t = 1;
            int i;
            switch (n)
            {
                case '{':
                    if (start <= end)
                    {
                        for (i = start; i <= end && t > 0; i++)
                        {
                            if (a[i] == '}')t--;
                            else if (a[i] == '{') t++;
                        }
                    }
                    else
                    {
                        for (i = start; i >= end && t > 0; i--)
                        {
                            if (a[i] == '}') t--;
                            else if (a[i] == '{') t++;
                        }
                    }
                    break;
                case '(':
                    if (start <= end)
                    {
                        for (i = start; i <= end && t > 0; i++)
                        {
                            if (a[i] == ')') t--;
                            else if (a[i] == '(') t++;
                        }
                    }
                    else
                    {
                        for (i = start; i >= end && t > 0; i--)
                        {
                            if (a[i] == ')') t--;
                            else if (a[i] == '(') t++;
                        }
                    }
                    break;
                default:
                    return -1;
            }
            return i-1;
        }
        private string append(int start, int end)
        {
            string content = "\"";
            for (int i = start; i <= end; i++)
            {
                if (a[i] == '\r')
                {
                }
                else if (a[i] == '\"')
                {
                    content = content + "\"& Chr(34) &\"";
                }
                else if (a[i] == '\t')
                {
                }
                else if (a[i] == '\n')
                {
                    content = content + "\"& Chr(13) & Chr(10) &\"";
                }
                else content = content + a[i].ToString();
            }
            content += "\"";
            return content;
        }
        private string transform(int r, int c, int s)
        {
            string t = "";
            return t;
        }
        private void UpdateDisplay()
        {
            for (int i = 0; i < topofqueue; i++)
            {
                drawnode = drawnode + maingrid[queue[i].getX(), queue[i].getY()].ToString();
            }
            for (int i = 1; i <= 1000 && i <= nodeindex; i++)
            {
                for (int k = 1; k <= 100 && i <= nodeindex; k++)
                {
                    switch (links[i, k])
                    {
                        case 0:
                            break;
                        default:
                            break;
                        case 1:
                            linknode = linknode + "af.addlink af.nodes(" + i.ToString() + "),af.nodes(" + k.ToString() + ")\r\n";
                            break;
                        case 2:
                            linknode = linknode + "af.addlink af.nodes(" + i.ToString() + "),af.nodes(" + k.ToString() + ")\r\n";
                            linknode = linknode + "af.nodes(" + i.ToString() + ").links(af.nodes(" + i.ToString() + ").links.count).text = \"Y\"\r\n";
                            break;
                        case -1:
                            linknode = linknode + "af.addlink af.nodes(" + i.ToString() + "),af.nodes(" + k.ToString() + ")\r\n";
                            linknode = linknode + "af.nodes(" + i.ToString() + ").links(af.nodes(" + i.ToString() + ").links.count).text = \"N\"\r\n";
                            break;
                    }
                }
            }
            drawnode = drawnode + linknode;
        }
        private void draw(int start, int end, int r, int c, string method)
        {
            if (maingrid[r, c] == null)
            {
                string con = "";
                for (int i = start; i <= end; i++)
                {
                    con += a[i].ToString();
                }
                Node x = new Node(method, con, nodeindex, r, c, size);
                maingrid[r, c] = x;
                Pair tmp = new Pair(r, c);
                queue[topofqueue++] = tmp;
            }
            else
            {
                if (r <= 1000)
                {
                    draw(start, end, r, c+1, method);
                }
                else
                {
                    MessageBox.Show("超出面板！");
                    return;
                }
            }
        }
        private void link(int start, int end, string t)
        {
            if (links[start, end] != -100)
            {
                if (t == "N")
                {
                    links[start, end] = -1;
                }
                else if (t == "Y")
                {
                    links[start, end] = 2;
                }
                else
                {
                    links[start, end] = 1;
                }
            }
        }
        private void link()
        {
            link(nodeindex, nodeindex + 1, "");
        }
        private void link(string t)
        {
            link(nodeindex, nodeindex + 1, t);
        }
        private int linktobtif(int btif)
        {
            int abtif=0;
            for (int i = 1; i <= 100; i++)
            {
                if (links[btif, i] == -1 || links[btif, i] == 2 || links[btif, i] == 1)
                {
                    abtif = i;
                    break;
                }
            }
            return abtif;
        }
        private int solve(int start, int end, string method, int r, int c, int btif, int rif,bool iselse)
        {
            if (start == end) return btif;
            switch (method)
            {
                case "normal":
                    int t1 = 0;
                    int t2 = 0;
                    int t3 = 0;
                    t1 = find(start, end, '{');
                    if (t1 == -1) return btif;
                    t2 = find(t1, start, ')');
                    if (t2 == -1) return btif;
                    for (int i = t1 - 1; i > t2; i--)
                    {
                        if (!(a[i] == '\r' || a[i] == '\t' || a[i] == '\n' || a[i] == ' '))
                        {
                            solve(t1 + 1, end, "normal", r, c, btif, rif,iselse);
                            return btif;
                        }
                    }
                    t3 = find(t2, start, '\n');
                    if (t3 != -1)
                    {
                        nodeindex++;
                        draw(start, t3 - 2, r, c, "predifinition");
                        c++;
                    }
                    nodeindex++;
                    draw(t3 + 1, t2, r, c, "function");
                    r++;
                    link();
                    int t4 = match(t1 + 1, end, '{') - 1;
                    solve(t1 + 1, t4 - 1, "scan", r, c, btif, rif, iselse);
                    del(nodeindex, nodeindex + 1);
                    if (t4 + 3 <= end)
                    {
                        c++;
                        solve(t4 + 3, end, "normal", 1, c, btif, rif, iselse);
                    }
                    break;
                case "scan":
                    if (start == end) return 0;
                    string key = "";
                    int f2 = 0;
                    //for (int i = start; i <= end; i++)
                    //{
                    int f1 = skip(start, end);
                    if (f1 < 0) return btif;
                    if (((a[f1] <= 'Z') && (a[f1] >= 'A')) || ((a[f1] >= 'a') && (a[f1] <= 'z')) || (a[f1] == '_'))
                    {
                        key = key + a[f1].ToString();
                    }
                    for (int i = f1 + 1; i <= end; i++)
                    {
                        if (i == end) return btif;
                        if (((a[i] <= 'Z') && (a[i] >= 'A')) || ((a[i] >= 'a') && (a[i] <= 'z')))
                        {
                            key = key + a[i].ToString();
                        }
                        else { f2 = i; break; }
                    }
                    switch (key)
                    {
                        #region input
                        case "cin":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        case "cout":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        case "scanf":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        case "printf":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        case "fscanf":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        case "fprintf":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        case "getchar":
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "input");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                        #endregion

                        #region if
                        case "if":
                            f1 = find(f1, end, '(');
                            f2 = match(f1 + 1, end, '(');
                            nodeindex++;
                            draw(f1 + 1, f2 - 1, r, c, "if");
                            r++;                            
                            link("Y");
                            f1 = skip(f2 + 1, end);
                            int tmp = nodeindex;
                            if (a[f1] == '{')
                            {
                                f2 = match(f1 + 1, end, '{');
                                rif = tmp;
                                solve(f1 + 1, f2 - 1, "scan", r, c, btif, rif, iselse);
                                int btif2 = nodeindex;
                                r += nodeindex - tmp;
                                f1 = skip(f2 + 1, end);
                                if (f1 == -1) return btif;
                                if (f1 + 4 <= end)
                                {
                                    if (a[f1] == 'e' && a[f1 + 1] == 'l' && a[f1 + 2] == 's' && a[f1 + 3] == 'e' && (a[f1 + 4] == ' ' || a[f1 + 4] == '\t' || a[f1 + 4] == '\n' || a[f1 + 4] == '\r'))
                                    {
                                        f1 = skip(f1 + 4, end);
                                        if (a[f1] == '{')
                                        {
                                            f2 = match(f1 + 1, end, '{');
                                            solve(f2 + 1, end, "scan", r, c, btif, rif, iselse);
                                            if (iselse)
                                            {
                                                link(nodeindex, linktobtif(btif), "");
                                            }
                                            btif = btif2;
                                            link(rif, nodeindex + 1, "N");
                                            del(nodeindex, nodeindex + 1);
                                            iselse = true;
                                            r -= btif2 - rif;
                                            solve(f1 + 1, f2 - 1, "scan", r, c + 1, btif, rif, iselse);
                                            link(nodeindex, linktobtif(btif), "");
                                        }
                                        else
                                        {
                                            for (int i = f1; i <= end; i++)
                                            {
                                                if (a[i] == '{')
                                                {
                                                    int f3 = match(i + 1, end, '{');
                                                    solve(f3 + 1, end, "scan", r, c, btif, rif, iselse);
                                                    if (iselse)
                                                    {
                                                        link(nodeindex, linktobtif(btif), "");
                                                    }
                                                    btif = btif2;
                                                    link(rif, nodeindex + 1, "N");
                                                    del(nodeindex, nodeindex + 1);
                                                    iselse = true;
                                                    r -= btif2 - rif;
                                                    solve(f1, f3, "scan", r, c + 1, btif, rif, iselse);
                                                    link(nodeindex, linktobtif(btif), "");
                                                    break;                                                    
                                                }
                                                else if (a[i] == ';')
                                                {
                                                    solve(i + 1, end, "scan", r, c, btif, rif, iselse);
                                                    if (iselse)
                                                    {
                                                        link(nodeindex, linktobtif(btif), "");
                                                    }
                                                    btif = btif2;
                                                    link(rif, nodeindex + 1, "N");
                                                    del(nodeindex, nodeindex + 1);
                                                    iselse = true;
                                                    r -= btif2 - rif;
                                                    solve(f1, i, "scan", r, c + 1, btif, rif, iselse);
                                                    link(nodeindex, linktobtif(btif), "");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        btif = btif2;
                                        solve(f2 + 1, end, "scan", r, c, btif, rif, iselse);
                                        link(rif, linktobtif(btif), "N");
                                    }
                                }
                                else
                                {
                                    return btif;
                                }
                            }
                            else
                            {
                                f2 = find(f1, end, ';');
                                solve(f1, f2, "scan", r, c, btif, rif, iselse);
                                r++;
                                int btif3 = nodeindex;
                                f1 = skip(f2 + 1, end);
                                if (f1 == -1) return btif;
                                if (f1 + 4 <= end)
                                {
                                    if (a[f1] == 'e' && a[f1 + 1] == 'l' && a[f1 + 2] == 's' && a[f1 + 3] == 'e' && (a[f1 + 4] == ' ' || a[f1 + 4] == '\t' || a[f1 + 4] == '\n' || a[f1 + 4] == '\r'))
                                    {
                                        f1 = skip(f1 + 4, end);
                                        if (a[f1] == '{')
                                        {
                                            f2 = match(f1 + 1, end, '{');
                                            solve(f2 + 1, end, "scan", r, c, btif, rif, iselse);
                                            if (iselse)
                                            {
                                                link(nodeindex, linktobtif(btif), "");
                                            }
                                            btif = btif3;
                                            link(rif, nodeindex + 1, "N");
                                            del(nodeindex, nodeindex + 1);
                                            iselse = true;
                                            r -= btif3 - rif;
                                            solve(f1 + 1, f2 - 1, "scan", r, c + 1, btif, rif, iselse);
                                            link(nodeindex, linktobtif(btif), "");
                                        }
                                        else
                                        {
                                            for (int i = f1; i <= end; i++)
                                            {
                                                if (a[i] == '{')
                                                {
                                                    int f3 = match(i + 1, end, '{');
                                                    solve(f3 + 1, end, "scan", r, c, btif, rif, iselse);
                                                    if (iselse)
                                                    {
                                                        link(nodeindex, linktobtif(btif), "");
                                                    }
                                                    btif = btif3;
                                                    link(rif, nodeindex + 1, "N");
                                                    del(nodeindex, nodeindex + 1);
                                                    iselse = true;
                                                    r -= btif3 - rif;
                                                    solve(f1, f3, "scan", r, c + 1, btif, rif, iselse);
                                                    link(nodeindex, linktobtif(btif), "");
                                                    break;                                                    
                                                }
                                                else if (a[i] == ';')
                                                {
                                                    solve(i + 1, end, "scan", r, c, btif, rif, iselse);
                                                    if (iselse)
                                                    {
                                                        link(nodeindex, linktobtif(btif), "");
                                                    }
                                                    btif = btif3;
                                                    link(rif, nodeindex + 1, "N");
                                                    del(nodeindex, nodeindex + 1);
                                                    iselse = true;
                                                    r -= btif3 - rif;
                                                    solve(f1, i, "scan", r, c + 1, btif, rif, iselse);
                                                    link(nodeindex, linktobtif(btif), "");
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        btif = btif3;
                                        solve(f2 + 1, end, "scan", r, c, btif, rif, iselse);
                                        link(rif, linktobtif(btif), "N");
                                    }
                                }
                                else
                                {
                                    return btif;
                                }
                            }
                            break;
                        #endregion

                        #region for
                        case "for":
                            f1 = skip(f2, end);
                            if (f1 == -1) return btif;
                            if (a[f1] == '(')
                            {
                                f2 = match(f1 + 1, end, '(');
                                int y1, y2;
                                y1 = find(f1, end, ';');
                                y2 = find(y1 + 1, f2, ';');
                                if (y1 - f1 > 1)
                                {
                                    nodeindex++;
                                    draw(f1 + 1, y1 - 1, r, c, "normal");
                                    rif = nodeindex;
                                    link();
                                    r++;
                                }
                                if (y2 - y1 > 1)
                                {
                                    nodeindex++;
                                    draw(y1 + 1, y2 - 1, r, c, "if");
                                    link("Y");
                                    r++;
                                }
                                f1 = skip(f2 + 1, end);
                                if (a[f1] == '{')
                                {
                                    f2 = match(f1 + 1, end, '{');
                                    rif = nodeindex;
                                    solve(f1 + 1, f2 - 1, "scan", r, c, btif, rif, false);
                                    btif = nodeindex;
                                    del(nodeindex, nodeindex + 1);
                                    link(nodeindex, rif, "");
                                }
                                else
                                {
                                    f2 = find(f1 + 1, end, ';');
                                    rif = nodeindex;
                                    solve(f1, f2, "scan", r, c, btif, rif, false);
                                    btif = nodeindex;
                                    del(nodeindex, nodeindex + 1);
                                    link(nodeindex, rif, "");
                                }
                                if (f2 - y2 > 1)
                                {
                                    nodeindex++;
                                    draw(y2 + 1, f2 - 1, r, c, "normal");
                                    r++;
                                }
                                link(rif, linktobtif(btif+1), "N");
                            }
                            solve(f2 + 1, end, "scan", r, c, btif, rif, false);
                            break;
                        #endregion
                        default:
                            f2 = find(f2, end, ';');
                            nodeindex++;
                            draw(f1, f2, r, c, "normal");
                            r++;
                            link();
                            solve(f2 + 3, end, "scan", r, c, btif, rif, iselse);
                            break;
                    }
                    break;
                default:
                    break;
            }
            return btif;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OFD.Filter = "C或C++或TXT文件(*.c;*.cpp;*.txt;)|*.c;*.cpp;*.txt;";
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(OFD.FileName, FileMode.Open);
                StreamReader sr = new StreamReader(fs);
                m = sr.ReadToEnd();
                a = m.ToCharArray();
                sr.Close();
                fs.Close();
            }
            checkBox1.Checked = true;
            label1.Text = "Ready.";
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count;
            count = a.Length;
            size = int.Parse(textBox1.Text);
            maingrid = new Node[1001, 101];
            links = new int[1001, 101];
            queue = new Pair[50001];
            /*for (int i = 0; i <= 1000; i++)
            {
                for (int k = 0; k <= 100; k++)
                {
                    links[i, k] = 0;
                }
            }*/
            solve(0, count - 1, "normal", 1, 1, 1, 1,false);
            UpdateDisplay();
            label1.Text = "Terminated.";
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SFD.Filter = "VBM宏文件(*.vbm;)|*.vbm;";
            SFD.AddExtension = true;
            SFD.Title = "Save";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                FileStream fs2 = new FileStream(SFD.FileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs2);
                sw.Write(drawnode);
                sw.Flush();
                sw.Close();
                fs2.Close();
            }
        }
    }
    public class Pair
    {
        private int x;
        private int y;
        public Pair(int X, int Y)
        {
            x = X;
            y = Y;
        }
        public int getX()
        {
            return x;
        }
        public int getY()
        {
            return y;
        }
        public void setX(int X)
        {
            x = X;
        }
        public void setY(int Y)
        {
            y = Y;
        }
    }
    public class Node
    {
        string myType;
        string myContents;
        int myIndex;
        int myR;
        int myC;
        int mySize;
        public Node(string Type, string Contents, int Index, int R, int C, int Size)
        {
            myType = Type;
            myContents = Contents;
            myIndex = Index;
            myR = R;
            myC = C;
            mySize = Size;
        }
        private string transform(int r, int c, int s)
        {
            int a, x, y, z;
            string tmp = "(";
            x = r * (750 * s / 100 + 1000) + 1000;
            a = (c - 1) * (3000 * s / 100 + 500) + 1000;
            y = 3000 * s / 100;
            z = 1500 * s / 100;
            tmp = tmp + a.ToString() + "," + x.ToString() + "," + y.ToString() + "," + z.ToString() + ")";
            return tmp;
        }
        public override string ToString()
        {
            string t = "";
            char[] tmp = myContents.ToCharArray();
            int c = tmp.Length;
            string content = "\"";
            for (int i = 0; i < c; i++)
            {
                if (tmp[i] == '\r')
                {
                }
                else if (tmp[i] == '\"')
                {
                    content = content + "\"& Chr(34) &\"";
                }
                else if (tmp[i] == '\t')
                {
                }
                else if (tmp[i] == '\n')
                {
                    content = content + "\"& Chr(13) & Chr(10) &\"";
                }
                else content = content + tmp[i].ToString();
            }
            content += "\"";
            //string t = "af.nodes.add \"" + this.transform(myR, myC, mySize) + ".shape = 1\r\naf.nodes(" + myIndex.ToString() + ").text =" + content + "\r\naf.nodes(" + myIndex.ToString() + ").gradient =true\r\naf.nodes(" + myIndex.ToString() + ").fillcolor=14745591\r\naf.nodes(" + myIndex.ToString() + ").gradientcolor=8454112\r\naf.nodes(" + myIndex.ToString() + ").drawcolor=49296\r\n"; ;
            switch (myType)
            {
                case "predifinition":
                    t = "af.nodes.add" + this.transform(myR, myC, mySize) + ".shape = 1\r\naf.nodes(" + myIndex.ToString() + ").text =" + content + "\r\naf.nodes(" + myIndex.ToString() + ").gradient =true\r\naf.nodes(" + myIndex.ToString() + ").fillcolor=14745591\r\naf.nodes(" + myIndex.ToString() + ").gradientcolor=8454112\r\naf.nodes(" + myIndex.ToString() + ").drawcolor=49296\r\n" + "af.nodes(" + myIndex.ToString() + ").drawstyle = 2\r\n";
                    break;
                case "normal":
                    t = "af.nodes.add" + transform(myR, myC, mySize) + ".shape = 1\r\naf.nodes(" + myIndex.ToString() + ").text =" + content + "\r\naf.nodes(" + myIndex.ToString() + ").gradient =true\r\naf.nodes(" + myIndex.ToString() + ").fillcolor=16777152\r\naf.nodes(" + myIndex.ToString() + ").gradientcolor=12648192\r\naf.nodes(" + myIndex.ToString() + ").drawcolor=16760832\r\n";
                    break;
                case "input":
                    t = "af.nodes.add" + transform(myR, myC, mySize) + ".shape = 53\r\naf.nodes(" + myIndex.ToString() + ").text =" + content + "\r\naf.nodes(" + myIndex.ToString() + ").gradient =true\r\naf.nodes(" + myIndex.ToString() + ").fillcolor=16761087\r\naf.nodes(" + myIndex.ToString() + ").gradientcolor=14713087\r\naf.nodes(" + myIndex.ToString() + ").drawcolor=16744672\r\n";
                    break;
                case "if":
                    t = "af.nodes.add" + transform(myR, myC, mySize) + ".shape = 3\r\naf.nodes(" + myIndex.ToString() + ").text =" + content + "\r\naf.nodes(" + myIndex.ToString() + ").gradient =true\r\naf.nodes(" + myIndex.ToString() + ").fillcolor=14743551\r\naf.nodes(" + myIndex.ToString() + ").gradientcolor=57599\r\naf.nodes(" + myIndex.ToString() + ").drawcolor=4254207\r\n";
                    break;
                case "function":
                    t = "af.nodes.add" + transform(myR, myC, mySize) + ".shape = 8\r\naf.nodes(" + myIndex.ToString() + ").text =" + content + "\r\naf.nodes(" + myIndex.ToString() + ").gradient =true\r\naf.nodes(" + myIndex.ToString() + ").fillcolor=16771552\r\naf.nodes(" + myIndex.ToString() + ").gradientcolor=16761024\r\naf.nodes(" + myIndex.ToString() + ").drawcolor=16765120\r\n";
                    break;
            }
            return t;
        }
    }
}