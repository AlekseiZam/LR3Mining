using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LR3Mining
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            textBoxLead.Text = "5";
            textBoxBeg.Text = "200000";
            textBoxCount.Text = "200000";
            textBoxBlock.Text = "2";
            textBoxPrev.Text = "00000727854b50bb95c054b39c1fe5c92e5ebcfa4bcb5dc279f56aa96a365e5a";
            textBoxNonce.Text = "0";
            iC = Convert.ToInt64(textBoxCount.Text);
            i1 = Convert.ToInt64(textBoxBeg.Text);
            ShowInterval();
            leadingCount = Convert.ToInt16(textBoxLead.Text);
        }

        public Int64 iC, i1, i2, i3, i4, i5, i6;
        public int leadingCount;

        private void buttonMining_Click(object sender, EventArgs e)
        {
            clearFields();
            Cursor = Cursors.WaitCursor;
            leadingCount = Convert.ToInt16(textBoxLead.Text);
            textBoxNonce.Text = getNonce(leadingCount, i1, i2, 1).ToString();
            textBoxHASH.Text = getBlockHash(textBoxBlock.Text + textBoxNonce.Text + textBoxPrev.Text);
            Cursor = Cursors.Arrow;

            dataGridView1.Rows.Add(dataGridView1.Rows.Count.ToString(), textBoxLead.Text, label12.Text, label13.Text, textBoxNonce.Text, textBoxTick.Text);
            dataGridView2.Rows.Add(dataGridView2.Rows.Count.ToString());
        }

        private void buttonMining2_Click(object sender, EventArgs e)
        {
            clearFields();
            Cursor = Cursors.WaitCursor;
            leadingCount = Convert.ToInt16(textBoxLead.Text);

            Task task1 = new Task(() => getNonce(leadingCount, i3, i4, 1));
            task1.RunSynchronously();

            Task task2 = new Task(() => getNonce(leadingCount, i5, i6, 2));
            task2.RunSynchronously();

            textBoxHASH.Text = getBlockHash(textBoxBlock.Text + textBoxNonce.Text + textBoxPrev.Text);
            textBoxHASH2.Text = getBlockHash(textBoxBlock.Text + textBoxNonce2.Text + textBoxPrev.Text);

            Cursor = Cursors.Arrow;
            dataGridView1.Rows.Add(dataGridView1.Rows.Count.ToString(), textBoxLead.Text, label14.Text, label15.Text, textBoxNonce.Text, textBoxTick.Text);
            dataGridView2.Rows.Add(dataGridView2.Rows.Count.ToString(), textBoxLead.Text, label16.Text, label17.Text, textBoxNonce2.Text, textBoxTick2.Text);
        }

        private void clearFields()
        {
            textBoxTick.Text = "";
            textBoxTick2.Text = "";
            textBoxHASH.Text = "";
            textBoxHASH2.Text = "";
            textBoxNonce.Text = "";
            textBoxNonce2.Text = "";
        }

        private void ShowInterval()
        {
            i2 = i1 + iC;
            i3 = i1;
            i4 = i3 + iC / 2;
            i5 = i4 + 1;
            i6 = i2;
            label12.Text = i1.ToString("### ### ###");
            label13.Text = i2.ToString("### ### ###");
            label14.Text = i3.ToString("### ### ###");
            label15.Text = i4.ToString("### ### ###");
            label16.Text = i5.ToString("### ### ###");
            label17.Text = i6.ToString("### ### ###");
        }

        private string getBlockHash(string block)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(block);
            byte[] hashSHA256 = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA256")).ComputeHash(encodedPassword);
            return BitConverter.ToString(hashSHA256).Replace("-", string.Empty).ToLower();
        }

        public long getNonce(int lCount, Int64 nonceMin, Int64 nonceMax, int tackNum)
        {
            long t1;
            t1 = DateTime.Now.Ticks;
            Int64 nonce = nonceMin;
            string leading = "";
            while(leading != string.Concat(Enumerable.Repeat("0", lCount)) & nonce <= nonceMax)
            {
                nonce++;
                leading = getBlockHash(textBoxBlock.Text + Convert.ToString(nonce) + textBoxPrev.Text).Substring(0, leadingCount);
            }
           
            if (tackNum == 1) textBoxTick.Text = (DateTime.Now.Ticks - t1).ToString("## ### ###");
            else textBoxTick2.Text = (DateTime.Now.Ticks - t1).ToString("## ### ###");

            if (leading == string.Concat(Enumerable.Repeat("0", lCount))){
                if (tackNum == 1) textBoxNonce.Text = nonce.ToString();
                else textBoxNonce2.Text = nonce.ToString();

                return nonce;
            }
            else{
                if (tackNum == 1) textBoxNonce.Text = "0";
                else textBoxNonce2.Text = "0";

                return 0;
            }
        }

        private void textBoxBeg_TextChanged(object sender, EventArgs e)
        {
            i1 = Convert.ToInt64(textBoxBeg.Text);
            ShowInterval();
        }
        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            i1 = Convert.ToInt64(textBoxBeg.Text);
            ShowInterval();
        }

    }
}
