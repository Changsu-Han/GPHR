namespace Auxology
{
    partial class ChartSearchForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.search_tb = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.chart_list = new System.Windows.Forms.DataGridView();
            this.chart_num = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.birth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.chart_list)).BeginInit();
            this.SuspendLayout();
            // 
            // search_tb
            // 
            this.search_tb.Font = new System.Drawing.Font("굴림", 12.75F);
            this.search_tb.Location = new System.Drawing.Point(77, 9);
            this.search_tb.Name = "search_tb";
            this.search_tb.Size = new System.Drawing.Size(153, 27);
            this.search_tb.TabIndex = 0;
            this.search_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.search_tb_KeyDown);
            // 
            // btn_search
            // 
            this.btn_search.Location = new System.Drawing.Point(236, 9);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(75, 27);
            this.btn_search.TabIndex = 1;
            this.btn_search.Text = "검색";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "환자명";
            // 
            // chart_list
            // 
            this.chart_list.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.chart_list.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.chart_list.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("굴림", 11F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.chart_list.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.chart_list.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chart_list.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chart_num,
            this.name,
            this.birth});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.chart_list.DefaultCellStyle = dataGridViewCellStyle2;
            this.chart_list.Location = new System.Drawing.Point(0, 48);
            this.chart_list.Name = "chart_list";
            this.chart_list.RowTemplate.Height = 23;
            this.chart_list.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.chart_list.Size = new System.Drawing.Size(322, 526);
            this.chart_list.TabIndex = 3;
            this.chart_list.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.chart_list_CellDoubleClick);
            // 
            // chart_num
            // 
            this.chart_num.HeaderText = "차트번호";
            this.chart_num.Name = "chart_num";
            // 
            // name
            // 
            this.name.HeaderText = "이름";
            this.name.Name = "name";
            // 
            // birth
            // 
            this.birth.HeaderText = "생년월일";
            this.birth.Name = "birth";
            // 
            // ChartSearchForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(322, 572);
            this.Controls.Add(this.chart_list);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.search_tb);
            this.MaximizeBox = false;
            this.Name = "ChartSearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "차트검색";
            ((System.ComponentModel.ISupportInitialize)(this.chart_list)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox search_tb;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView chart_list;
        private System.Windows.Forms.DataGridViewTextBoxColumn chart_num;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn birth;
    }
}