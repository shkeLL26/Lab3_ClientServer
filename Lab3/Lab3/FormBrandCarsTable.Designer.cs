namespace Lab3
{
    partial class FormBrandCarsTable
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Model = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HorsePower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RegNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Multimedia = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Airbags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.TBrand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.THorsePower = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TMaxSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TRegNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Wheels = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Brand,
            this.Model,
            this.HorsePower,
            this.MaxSpeed,
            this.RegNumber,
            this.Multimedia,
            this.Airbags});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(825, 158);
            this.dataGridView1.TabIndex = 0;
            // 
            // Brand
            // 
            this.Brand.HeaderText = "Марка";
            this.Brand.Name = "Brand";
            // 
            // Model
            // 
            this.Model.HeaderText = "Модель";
            this.Model.Name = "Model";
            // 
            // HorsePower
            // 
            this.HorsePower.HeaderText = "Мощность (л. с.)";
            this.HorsePower.Name = "HorsePower";
            // 
            // MaxSpeed
            // 
            this.MaxSpeed.HeaderText = "Макс. Скорость";
            this.MaxSpeed.Name = "MaxSpeed";
            // 
            // RegNumber
            // 
            this.RegNumber.HeaderText = "Номер";
            this.RegNumber.Name = "RegNumber";
            // 
            // Multimedia
            // 
            this.Multimedia.HeaderText = "Мультимедиа";
            this.Multimedia.Name = "Multimedia";
            // 
            // Airbags
            // 
            this.Airbags.HeaderText = "Подушки безопасности";
            this.Airbags.Name = "Airbags";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 318);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(825, 29);
            this.progressBar1.TabIndex = 1;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TBrand,
            this.TModel,
            this.THorsePower,
            this.TMaxSpeed,
            this.TRegNumber,
            this.Wheels,
            this.Volume});
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 158);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dataGridView2.Size = new System.Drawing.Size(825, 160);
            this.dataGridView2.TabIndex = 2;
            // 
            // TBrand
            // 
            this.TBrand.HeaderText = "Марка";
            this.TBrand.Name = "TBrand";
            // 
            // TModel
            // 
            this.TModel.HeaderText = "Модель";
            this.TModel.Name = "TModel";
            // 
            // THorsePower
            // 
            this.THorsePower.HeaderText = "Мощность (л. с.)";
            this.THorsePower.Name = "THorsePower";
            // 
            // TMaxSpeed
            // 
            this.TMaxSpeed.HeaderText = "Макс. скорость";
            this.TMaxSpeed.Name = "TMaxSpeed";
            // 
            // TRegNumber
            // 
            this.TRegNumber.HeaderText = "Номер";
            this.TRegNumber.Name = "TRegNumber";
            // 
            // Wheels
            // 
            this.Wheels.HeaderText = "Колёса";
            this.Wheels.Name = "Wheels";
            // 
            // Volume
            // 
            this.Volume.HeaderText = "Объём кузова";
            this.Volume.Name = "Volume";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // FormBrandCarsTable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(825, 347);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "FormBrandCarsTable";
            this.Text = "Список автомобилей";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBrandCarsTable_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Model;
        private System.Windows.Forms.DataGridViewTextBoxColumn HorsePower;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn RegNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Multimedia;
        private System.Windows.Forms.DataGridViewTextBoxColumn Airbags;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn TBrand;
        private System.Windows.Forms.DataGridViewTextBoxColumn TModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn THorsePower;
        private System.Windows.Forms.DataGridViewTextBoxColumn TMaxSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn TRegNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Wheels;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
    }
}