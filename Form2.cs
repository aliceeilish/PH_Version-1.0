using System.Data;
using System.Data.SqlClient;

namespace PH_Version_1._0
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------------------------------------------------------------------
            
            // --------------------------------------------------------------------------------------------------------------------------------
            comboBox1.Items.Add("Filtrar por nombre");
            comboBox1.Items.Add("Filtrar por código");

            //obtener.valor_busqueda = textBox1.Text;
            //MessageBox.Show(obtener.valor_busqueda);
            //comboBox1.SelectedIndex = 0;

            // --------------------------------------------------------------------------------------------------------------------------------
            string query01;
            conexion n = new conexion();
            //string query = n.funcion_ui();
            // si query cambia, el valor de la tabla también 
            if (comboBox1.SelectedIndex == 0)
            {
                query01 = n.funcion_busqueda();
                MessageBox.Show(query01);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                query01 = n.funcion_ui();
            }
            SqlConnection cadena_conexion = new SqlConnection(@"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=DB_TIENDASDAKA_PROD;Data Source=SERV-SAP\SRVSAP2012");
            SqlDataAdapter adapter = new SqlDataAdapter(@"select ART.ItemCode, ART.ItemName, ART.FirmCode, ART.FirmName, U_DK_GARANTIA, isNull(ART.CodeBars,0), PRE.PriceList1, PRE.ListaName1, PRE.Price1, PRE.PriceList2, PRE.ListaName2, PRE.Price2, ART.Name from
(select A.ItemCode, A.ItemName, A.FirmCode, M.FirmName, A.U_DK_GARANTIA, A.CodeBars, F.name from OITM A 
join OMRC M on A.FirmCode = M.FirmCode
join ITM1 P on A.ItemCode = P.ItemCode
left join [@FAMILIA_PROD] F on f.Code = a.U_Familia
where a.SellItem = 'Y' and SUBSTRING(a.itemcode,1,1) = 'L' and A.OnHand > 0
group by A.ItemCode, A.ItemName, A.FirmCode, M.FirmName, A.U_DK_GARANTIA, A.CodeBars, F.Name) ART
join
(select P1.ItemCode1 as ItemCode, P1.PriceList1, P1.ListaName1, P1.Price1, P2.PriceList2, P2.ListaName2, P2.Price2 from 
(select A.ItemCode ItemCode1, P.PriceList PriceList1, L1.ListName ListaName1, P.Price Price1
from OITM A 
join ITM1 P on A.ItemCode = P.ItemCode
join OPLN L1 on P.PriceList = L1.ListNum
where a.SellItem = 'Y' and SUBSTRING(a.itemcode,1,1) = 'L' and P.PriceList = 7) P1
left join
(select A.ItemCode ItemCode2, P.PriceList PriceList2, L1.ListName ListaName2, P.Price Price2
from OITM A 
join ITM1 P on A.ItemCode = P.ItemCode
join OPLN L1 on P.PriceList = L1.ListNum
where a.SellItem = 'Y' and SUBSTRING(a.itemcode,1,1) = 'L' and P.PriceList = 4) P2
on P1.ItemCode1 = P2.ItemCode2) PRE
on ART.ItemCode = PRE.ItemCode", cadena_conexion);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            this.dataGridView1.Columns["FirmCode"].Visible = false;
            this.dataGridView1.Columns["FirmName"].Visible = false;
            this.dataGridView1.Columns["U_DK_GARANTIA"].Visible = false;
            this.dataGridView1.Columns["Column1"].Visible = false;
            this.dataGridView1.Columns["PriceList1"].Visible = false;
            this.dataGridView1.Columns["PriceList2"].Visible = false;
            this.dataGridView1.Columns["U_DK_GARANTIA"].Visible = false;
            this.dataGridView1.Columns["Price1"].Visible = false;
            this.dataGridView1.Columns["Price2"].Visible = false;
            this.dataGridView1.Columns["Name"].Visible = false;
            this.dataGridView1.Columns["ListaName1"].Visible = false;
            this.dataGridView1.Columns["ListaName2"].Visible = false;
            this.dataGridView1.Columns["ItemName"].Width = 700;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string n,n1 = "";
            Int32 selectedRowCount =
        dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRowCount > 0)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                for (int i = 0; i < selectedRowCount; i++)
                {
                    sb.Append("Fila: ");
                    sb.Append(dataGridView1.SelectedRows[i].Index.ToString());
                    n = dataGridView1.SelectedRows[i].Index.ToString();
                   // n1 = dataGridView1.SelectedRows[i].Index.ToString();
                    conexion conexion = new conexion();
                    conexion.funcion_variable_n(n,n1);
                    sb.Append(Environment.NewLine);
                    //
                    // como capturar varias posiciones de un datagridView
                }

                sb.Append("Total: " + selectedRowCount.ToString());
                //MessageBox.Show(sb.ToString(), "Selected Rows");
            }
            // generar código de barras
            // string valor_xy = valor_x;
            // Es necesario qu ela generación del código de barras se realice despues de la conexión
            BarcodeLib.Barcode codeBars = new BarcodeLib.Barcode();
            codeBars.IncludeLabel = false;
            string valor_x = obtener.valor_obtenido;
            MessageBox.Show(valor_x);
            panel1.BackgroundImage = codeBars.Encode(BarcodeLib.TYPE.CODE128, valor_x, Color.Black, Color.White, 300, 100);

            Image ImageCodeBars = (Image)panel1.BackgroundImage.Clone();
            SaveFileDialog SaveFileDialog = new SaveFileDialog();
            SaveFileDialog.AddExtension = true;
            SaveFileDialog.Filter = "Image PNG (*.png) | *.png";

            if (SaveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageCodeBars.Save(SaveFileDialog.FileName);
                SaveFileDialog.RestoreDirectory = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  conexion n = new conexion();
           // n.funcion_busquedaDinamica();
        }
    }

    class obtener
    {
        public static string valor_obtenido = "NULL";
        //public static string valor_busqueda = "MICROONDA";
    }
}
